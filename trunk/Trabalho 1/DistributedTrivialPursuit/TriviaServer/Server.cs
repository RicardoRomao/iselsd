using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Configuration;
using Proxy;
using System.Runtime.Remoting.Messaging;
using System.Net.Sockets;
using System.Runtime.Remoting.Lifetime;

namespace TriviaServer
{
    public sealed class Server : MarshalByRefObject, IRingServer, IZoneServer
    {
        private readonly Guid _guid;
        private readonly IDictionary<String, List<IExpert>> _expertList;
        private readonly NameValueCollection _serverRing;
        private IRingServer _nextServer;
        private Int32 _nextServerIndex;

        private readonly string configFile = ConfigurationManager.AppSettings["serverConfigFile"];
		private readonly object monitor = new Object();

        public Server()
        {
            _guid = Guid.NewGuid();
            _nextServerIndex = 0;

            _expertList = new Dictionary<String, List<IExpert>>();
            _serverRing = ConfigurationManager.AppSettings;

            RemotingConfiguration.Configure(configFile, false);
            WellKnownClientTypeEntry et = new WellKnownClientTypeEntry(typeof(IRingServer), _serverRing.Get(_nextServerIndex));
            _nextServer = (IRingServer)Activator.GetObject(et.ObjectType, et.ObjectUrl);
            ITriviaSponsor sponsor = _nextServer.getSponsor();
            ILease lease = (ILease)RemotingServices.GetLifetimeService((MarshalByRefObject)_nextServer);
            lease.Register(sponsor);
        }

        private void FowardRegistration(Guid guid, String theme, IExpert expert)
        {
            try
            {
                //First try the FastPath
                _nextServer.Register(guid, theme, expert);
            }
            catch (SocketException ex) {
				_nextServerIndex = (_nextServerIndex + 1) % _serverRing.Count;
				WellKnownClientTypeEntry et = new WellKnownClientTypeEntry(typeof(IRingServer), _serverRing.Get(_nextServerIndex));
				_nextServer = (IRingServer)Activator.GetObject(et.ObjectType, et.ObjectUrl);
                ITriviaSponsor sponsor = _nextServer.getSponsor();
                ILease lease = (ILease)RemotingServices.GetLifetimeService((MarshalByRefObject)_nextServer);
                lease.Register(sponsor);
				FowardRegistration(guid, theme, expert);
            }
        }

		private void EndRegistration(IAsyncResult result)
		{
			AsyncResult asyncResult = (AsyncResult)result;
			var invoker = (Action<Guid, String, IExpert>)asyncResult.AsyncDelegate;
			invoker.EndInvoke(result);
		}

        private void FowardUnregistration(Guid guid, String theme, IExpert expert)
        {
            try
            {
                //First try the FastPath
                _nextServer.UnRegister(guid, theme, expert);
            }
            catch (SocketException ex)
            {
				_nextServerIndex = (_nextServerIndex + 1) % _serverRing.Count;
				WellKnownClientTypeEntry et = new WellKnownClientTypeEntry(typeof(IRingServer), _serverRing.Get(_nextServerIndex));
				_nextServer = (IRingServer)Activator.GetObject(et.ObjectType, et.ObjectUrl);
                ITriviaSponsor sponsor = _nextServer.getSponsor();
                ILease lease = (ILease)RemotingServices.GetLifetimeService((MarshalByRefObject)_nextServer);
                lease.Register(sponsor);
				FowardUnregistration(guid, theme, expert);
            }
        }

		/*
		 * Este método tem a mesma implementação que o EndRegistration
		 * no entanto mantive os dois separados para o caso de futuramente
		 * ser necessário processamento diferente
		 */
		private void EndUnregistration(IAsyncResult result)
		{
			AsyncResult asyncResult = (AsyncResult)result;
			var invoker = (Action<Guid, String, IExpert>)asyncResult.AsyncDelegate;
			invoker.EndInvoke(result);
		}

        #region IRingServer Members

        public void Register(Guid guid, string theme, IExpert expert)
        {
            //Check if is the initial sender of the registration
            //If so the process ends here, if not, it must register the expert and foward the registration
            if (!_guid.Equals(guid))
            {
				lock (monitor)
				{
					//Check if the theme exists in the Dictionary
					//If not, create a new entrance in the dictionary
					if (!_expertList.Keys.Contains(theme))
						_expertList.Add(theme, new List<IExpert>());

					//Safely add the expert in the corresponding list
					_expertList[theme].Add(expert);					
				}

				//Foward the registration to the other ring servers asynchronously
				Action<Guid, string, IExpert> asyncRegister = new Action<Guid, string, IExpert>(FowardRegistration);
				asyncRegister.BeginInvoke(guid, theme, expert, EndRegistration, null);
            }
        }

        public void UnRegister(Guid guid, string theme, IExpert expert)
        {
            //Check if is the initial sender of the registration
            //If so the process ends here, if not, it must unregister the expert and foward the registration
            if (!_guid.Equals(guid))
            {
				lock (monitor)
				{
					//Check if the theme exists in the Dictionary
					if (_expertList.Keys.Contains(theme))
					{
						_expertList[theme].Remove(expert);
						//If there is no more experts of this theme, remove the theme
						if (_expertList[theme].Count == 0)
							_expertList.Remove(theme);
					}					
				}

				//Foward the unregistration to the other ring servers asynchronously
				Action<Guid, string, IExpert> asyncUnregister = new Action<Guid, string, IExpert>(FowardUnregistration);
				asyncUnregister.BeginInvoke(guid, theme, expert, EndUnregistration, null);
            }
        }

        #endregion

        #region IZoneServer Members

        public void Register(string theme, IExpert expert)
        {
			lock (monitor)
			{
				//Check if the theme exists in the Dictionary
				//If not, create a new entrance in the dictionary
				if (!_expertList.Keys.Contains(theme))
					_expertList.Add(theme, new List<IExpert>());

				//Safely add the expert in the corresponding list
				_expertList[theme].Add(expert);
				Console.WriteLine("Registered expert for {0}", theme);
			}

			//Foward the registration to the other ring servers asynchronously
			//so the client don't have to wait
			Action<Guid, string, IExpert> asyncRegister = new Action<Guid,string,IExpert>(FowardRegistration);
			asyncRegister.BeginInvoke(_guid, theme, expert, EndRegistration, null);
        }

        public void UnRegister(string theme, IExpert expert)
        {
			lock (monitor)
			{
				//Check if the theme exists in the Dictionary
				if (_expertList.Keys.Contains(theme))
				{
					_expertList[theme].Remove(expert);
					//If there is no more experts of this theme, remove the theme
					if (_expertList[theme].Count == 0)
						_expertList.Remove(theme);
				}
			}

			//Foward the unregistration to the other ring servers asynchronously
			//so the client don't have to wait
			Action<Guid, string, IExpert> asyncUnregister = new Action<Guid, string, IExpert>(FowardUnregistration);
			asyncUnregister.BeginInvoke(_guid, theme, expert, EndUnregistration, null);
        }

        public List<IExpert> getExpertList(string theme)
        {
			lock (monitor)
			{
				if (_expertList.Keys.Contains(theme))
					return _expertList[theme];				
			}
            return null;
        }

        public void NotifyClientFault(string theme, IExpert expert)
        {
            UnRegister(theme, expert);
        }

        public ITriviaSponsor getSponsor()
        {
            return new TriviaSponsor();
        }

        public override object InitializeLifetimeService()
        {
            ILease lease = (ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = lease.RenewOnCallTime = TimeSpan.FromSeconds(30);
            }
            return lease;

        }

        #endregion
    }
}
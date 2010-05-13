using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using Proxy;
using System.Runtime.Remoting.Messaging;
using System.Net.Sockets;
using System.Runtime.Remoting.Lifetime;
using System.Configuration;

namespace TriviaServer
{
    public sealed class Server : MarshalByRefObject, IRingServer, IZoneServer
    {
        private readonly string _uId;
        private readonly string _ownUrl;
        private readonly IDictionary<String, List<IExpert>> _expertList;
        private readonly NameValueCollection _serverRing;
        private IRingServer _nextServer;
        private ITriviaSponsor _sponsor;
        private Int32 _nextServerIndex;

        private readonly object monitor = new Object();

        #region Public parameterless constructor

        public Server()
        {
            _uId = ConfigurationManager.AppSettings["serverId"];

            _expertList = new Dictionary<String, List<IExpert>>();
            _serverRing = (NameValueCollection)ConfigurationManager.GetSection("RingServers");

            for (int i = 0; i < _serverRing.Keys.Count; i++)
            {
                if (_serverRing.Keys[i].Equals(_uId))
                {
                    _nextServerIndex = i;
                    _ownUrl = _serverRing[i];
                }
            }
        }

        #endregion

        private void SetNextServer()
        {
             _nextServerIndex = (_nextServerIndex + 1) % _serverRing.Count; 

            Console.WriteLine("[{0}] - Trying to connect to server {1}", _uId, _serverRing[_nextServerIndex]);
            WellKnownClientTypeEntry et = new WellKnownClientTypeEntry(typeof(IRingServer), _serverRing.Get(_nextServerIndex));
            _nextServer = (IRingServer)Activator.GetObject(et.ObjectType, et.ObjectUrl);
            SetSponsor();

        }

        #region Sponsor management

        private void SetSponsor()
        {
            Func<ITriviaSponsor> setSponsor = new Func<ITriviaSponsor>(_nextServer.getSponsor);
            setSponsor.BeginInvoke(OnSetSponsorComplete, null);

        }

        private void OnSetSponsorComplete(IAsyncResult resp)
        {
            AsyncResult res = (AsyncResult)resp;

            Func<ITriviaSponsor> setSponsorAction = (Func<ITriviaSponsor>)res.AsyncDelegate;
            try
            {
                _sponsor = setSponsorAction.EndInvoke(resp);
                ILease lease = (ILease)RemotingServices.GetLifetimeService((MarshalByRefObject)_nextServer);
                lease.Register(_sponsor);
            }
            catch (SocketException)
            {
                Console.WriteLine("Unable to get Sponsor for next Ring Server.");
            }
        }

        #endregion

        #region Private methods for communication with other ring servers

        private void FowardRegistration(string uId, string theme, IExpert expert)
        {
            try
            {
                Console.WriteLine("[{0}] - Forwarding registration to {1} server...", _uId, _serverRing[_nextServerIndex]);

                //First try the FastPath
                if (_nextServer == null || _nextServer.GetId().Equals(_uId))
                    SetNextServer();
                _nextServer.Register(uId, theme, expert);
            }
            catch (SocketException)
            {
                SetNextServer();
                FowardRegistration(uId, theme, expert);
            }
        }

        private void EndRegistration(IAsyncResult result)
        {
            AsyncResult asyncResult = (AsyncResult)result;
            var invoker = (Action<string, string, IExpert>)asyncResult.AsyncDelegate;
            invoker.EndInvoke(result);
        }

        private void FowardUnregistration(string uId, string theme, IExpert expert)
        {
            try
            {
                //First try the FastPath
                if (_nextServer == null || _nextServer.GetId().Equals(_uId))
                    SetNextServer();
                Console.WriteLine("[{0}] - Forwarding unregistration to {1} server...", _uId, _serverRing[_nextServerIndex]);
                _nextServer.UnRegister(uId, theme, expert);
            }
            catch (SocketException)
            {
                SetNextServer();
                FowardUnregistration(uId, theme, expert);
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
            var invoker = (Action<string, string, IExpert>)asyncResult.AsyncDelegate;
            invoker.EndInvoke(result);
        }

        #endregion

        #region IRingServer Members

        public string GetId() { return _uId; }

        public void Register(string uId, string theme, IExpert expert)
        {
            Console.WriteLine("[{0}] - Remote registration of expert on {1} sent by server {2}", _uId, theme, uId);
            //Check if is the initial sender of the registration
            //If so the process ends here, if not, it must register the expert and foward the registration
            if (!_uId.Equals(uId))
            {
                lock (monitor)
                {
                    //Check if the theme exists in the Dictionary
                    //If not, create a new entrance in the dictionary
                    if (!_expertList.Keys.Contains(theme))
                        _expertList.Add(theme, new List<IExpert>());
                    Console.WriteLine("[{0}] - Registering remote expert on {1} sent by server {2}", _uId, theme, uId);

                    //Safely add the expert in the corresponding list
                    _expertList[theme].Add(expert);
                }

                //Foward the registration to the other ring servers asynchronously
                Action<string, string, IExpert> asyncRegister = new Action<string, string, IExpert>(FowardRegistration);
                asyncRegister.BeginInvoke(uId, theme, expert, EndRegistration, null);
            }
        }

        public void UnRegister(string uId, string theme, IExpert expert)
        {
            Console.WriteLine("[{0}] - Remote unregistration of expert on {1} sent by server {2}", _uId, theme, uId);
            //Check if is the initial sender of the registration
            //If so the process ends here, if not, it must unregister the expert and foward the registration
            if (!_uId.Equals(uId))
            {
                lock (monitor)
                {
                    Console.WriteLine("[{0}] - Unregistering remote expert on {1} sent by server {2}", _uId, theme, uId);
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
                Action<string, string, IExpert> asyncUnregister = new Action<string, string, IExpert>(FowardUnregistration);
                asyncUnregister.BeginInvoke(uId, theme, expert, EndUnregistration, null);
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
                Console.WriteLine("[{0}] - Registered expert for {1}", _uId, theme);
            }

            //Foward the registration to the other ring servers asynchronously
            //so the client don't have to wait
            Action<string, string, IExpert> asyncRegister = new Action<string, string, IExpert>(FowardRegistration);
            asyncRegister.BeginInvoke(_uId, theme, expert, EndRegistration, null);
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
                    Console.WriteLine("[{0}] - Unregistered expert for {1}", _uId, theme);

                }
            }

            //Foward the unregistration to the other ring servers asynchronously
            //so the client don't have to wait
            Action<string, string, IExpert> asyncUnregister = new Action<string, string, IExpert>(FowardUnregistration);
            asyncUnregister.BeginInvoke(_uId, theme, expert, EndUnregistration, null);
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
            Console.WriteLine("[{0}] - Received client warning regarding expert availability on {0}!", _uId, theme);
            UnRegister(theme, expert);
        }

        public ITriviaSponsor getSponsor()
        {
            Console.WriteLine("[{0}] - Received sponsor request.", _uId);
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
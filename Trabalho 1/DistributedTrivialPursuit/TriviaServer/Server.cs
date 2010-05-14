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

        private readonly IDictionary<String, List<IExpert>> _expertList;
        private readonly NameValueCollection _serverRing;

        private ITriviaSponsor _serverSponsor;
        private const double RENEW_TIME = 60;

        private IRingServer _nextServer;
        private Int32 _nextServerIndex;
        private Int32 _baseIndex;

        private readonly object monitor = new Object();

        #region Public parameterless constructor
        public Server()
        {
            _expertList = new Dictionary<String, List<IExpert>>();
            _serverRing = (NameValueCollection)ConfigurationManager.GetSection("RingServers");

            _uId = ConfigurationManager.AppSettings["serverId"];

            for (int i = 0; i < _serverRing.Keys.Count; i++)
            {
                if (_serverRing.Keys[i].Equals(_uId))
                {
                    _baseIndex = _nextServerIndex = i;
                }
            }
            SetNextServer();
        }
        #endregion

        #region Destructor for releasing resources namely unregister next server sponsor
        public ~Server()
        {
            if (_nextServer != null && _serverSponsor != null)
                ClearSponsor();
            if (_expertList != null){
                foreach(String theme in _expertList.Keys){
                    if (_expertList[theme]!= null)
                        _expertList[theme].Clear();
                }
                _expertList.Clear();
            }
        }
        #endregion

        #region Private methods
        private void SetNextServer()
        {
            lock (monitor)
            {
                _nextServerIndex = (_nextServerIndex + 1) % _serverRing.Count;
                if (_nextServerIndex != _baseIndex)
                {
                    Console.WriteLine("[{0}] - NEXT SERVER: {1}", _uId, _serverRing[_nextServerIndex]);
                    WellKnownClientTypeEntry et = new WellKnownClientTypeEntry(typeof(IRingServer), _serverRing.Get(_nextServerIndex));
                    _nextServer = (IRingServer)Activator.GetObject(et.ObjectType, et.ObjectUrl);
                    SetSponsor();
                }
                else
                {
                    _nextServer = null;
                }
            }
        }

        private void EnqueueExpert(string theme, IExpert expert)
        {
            lock (monitor)
            {
                //Check if the theme exists in the Dictionary
                //If not, create a new entrance in the dictionary
                if (!_expertList.Keys.Contains(theme))
                    _expertList.Add(theme, new List<IExpert>());

                _expertList[theme].Add(expert);
                Console.WriteLine("[{0}] - REGISTER: {1}", _uId, theme);
            }
        }

        private void DequeueExpert(string theme, IExpert expert)
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
                    Console.WriteLine("[{0}] - UNREGISTER: {1}", _uId, theme);
                }
            }
        }
        #endregion

        #region Sponsor management
        private void SetSponsor()
        {
            Console.WriteLine("[{0}] - SENDING SPONSOR REQUEST.",_uId);
            Func<ITriviaSponsor> setSponsor = new Func<ITriviaSponsor>(_nextServer.GetSponsor);
            setSponsor.BeginInvoke(OnSetSponsorComplete, null);

        }

        private void OnSetSponsorComplete(IAsyncResult resp)
        {
            AsyncResult res = (AsyncResult)resp;

            Func<ITriviaSponsor> setSponsorAction = (Func<ITriviaSponsor>)res.AsyncDelegate;
            try
            {
                _serverSponsor = setSponsorAction.EndInvoke(resp);
                Console.WriteLine(_serverSponsor == null);
                ILease lease = (ILease)RemotingServices.GetLifetimeService((MarshalByRefObject)_nextServer);
                lease.Register(_serverSponsor);
            }
            catch (SocketException)
            {
                Console.WriteLine("[{0}] - NO SPONSOR RECEIVED.",_uId);
            }
        }

        private void ClearSponsor()
        {
            Console.WriteLine("[{0}] - CANCELING NEXT SERVER SPONSOR.", _uId);
            ILease lease = (ILease)RemotingServices.GetLifetimeService((MarshalByRefObject)_nextServer);
            lease.Register(_serverSponsor);
        }
        #endregion

        #region Private methods for communication with other ring servers
        private void FowardRegistration(string uId, string theme, IExpert expert)
        {
                try
                {
                    Console.WriteLine("[{0}] - FORWARD REGISTER: {1}", _uId, _serverRing[_nextServerIndex]);
                    _nextServer.Register(uId, theme, expert);
                }
                catch (SocketException)
                {
                    SetNextServer();
                    FowardRegistration(uId, theme, expert);
                }
                catch (NullReferenceException)
                {
                    SetNextServer();
                    Console.WriteLine("[{0}] - FORWARD REGISTER STOPED.", _uId);
                }
        }

        private void FowardUnregistration(string uId, string theme, IExpert expert)
        {
                try
                {
                    Console.WriteLine("[{0}] - FORWARD UNREGISTER: {1}", _uId, _serverRing[_nextServerIndex]);
                    _nextServer.UnRegister(uId, theme, expert);
                }
                catch (SocketException)
                {
                    SetNextServer();
                    FowardUnregistration(uId, theme, expert);
                }
                catch (NullReferenceException)
                {
                    SetNextServer();
                    Console.WriteLine("[{0}] - FORWARD REGISTER STOPED.", _uId);
                }
        }

        private void EndForwarding(IAsyncResult result)
        {
            AsyncResult asyncResult = (AsyncResult)result;
            var invoker = (Action<string, string, IExpert>)asyncResult.AsyncDelegate;
            invoker.EndInvoke(result);
        }
        #endregion

        #region IRingServer Members
        public void Register(string uId, string theme, IExpert expert)
        {
            Console.WriteLine("[{0}] - Remote REGISTER: {1} - {2}", _uId, theme, uId);
            //Check if is the initial sender of the registration
            //If so the process ends here, if not, it must register the expert and foward the registration
            if (!_uId.Equals(uId))
            {
                Console.WriteLine("[{0}] - Asking REGISTER: {1} - {2}", _uId, theme, uId);
                EnqueueExpert(theme, expert);

                //Foward the registration to the other ring servers asynchronously
                Action<string, string, IExpert> asyncRegister = new Action<string, string, IExpert>(FowardRegistration);
                asyncRegister.BeginInvoke(uId, theme, expert, EndForwarding, null);
            }
        }

        public void UnRegister(string uId, string theme, IExpert expert)
        {
            Console.WriteLine("[{0}] - Remote UNREGISTER: {1} - {2}", _uId, theme, uId);
            //Check if is the initial sender of the registration
            //If so the process ends here, if not, it must unregister the expert and foward the registration
            if (!_uId.Equals(uId))
            {

                Console.WriteLine("[{0}] - Asking UNREGISTER: {1} - {2}", _uId, theme, uId);
                DequeueExpert(theme, expert);

                //Foward the unregistration to the other ring servers asynchronously
                Action<string, string, IExpert> asyncUnregister = new Action<string, string, IExpert>(FowardUnregistration);
                asyncUnregister.BeginInvoke(uId, theme, expert, EndForwarding, null);
            }
        }
        #endregion

        #region IZoneServer Members
        public void Register(string theme, IExpert expert)
        {
            EnqueueExpert(theme, expert);
            //Foward the registration to the other ring servers asynchronously
            Action<string, string, IExpert> asyncRegister = new Action<string, string, IExpert>(FowardRegistration);
            asyncRegister.BeginInvoke(_uId, theme, expert, EndForwarding, null);
        }

        public void UnRegister(string theme, IExpert expert)
        {

            DequeueExpert(theme, expert);
            //Foward the unregistration to the other ring servers asynchronously
            Action<string, string, IExpert> asyncUnregister = new Action<string, string, IExpert>(FowardUnregistration);
            asyncUnregister.BeginInvoke(_uId, theme, expert, EndForwarding, null);
        }

        public List<IExpert> GetExpertList(string theme)
        {
            ILease lease =(ILease)this.GetLifetimeService();
            Console.WriteLine("Tempo de Lease corrente {0}\n",
                lease.CurrentLeaseTime);

            lock (monitor)
            {
                if (_expertList.Keys.Contains(theme))
                    return _expertList[theme];
            }
            return null;
        }

        public void NotifyClientFault(string theme, IExpert expert)
        {
            Console.WriteLine("[{0}] - CLIENT WARNING: {1}!", _uId, theme);
            UnRegister(theme, expert);
        }

        public ITriviaSponsor GetSponsor()
        {
            Console.WriteLine("[{0}] - RECEIVED SPONSOR REQUEST", _uId);
            return new ServerSponsor(_uId,RENEW_TIME);
        }
        #endregion

        public override object InitializeLifetimeService()
        {
            ILease lease = (ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromSeconds(20);
                lease.RenewOnCallTime = TimeSpan.FromSeconds(10);
                lease.SponsorshipTimeout = TimeSpan.FromSeconds(10);
            }
            return lease;
        }

    }
}
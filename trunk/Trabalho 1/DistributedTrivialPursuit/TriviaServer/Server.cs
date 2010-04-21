using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Configuration;
using Proxy;

namespace TriviaServer
{
    public sealed class Server : MarshalByRefObject, IRingServer, IZoneServer
    {
        private readonly Guid _guid;
        private readonly IDictionary<String, List<IExpert>> _expertList;
        private readonly NameValueCollection _serverRing;
        private IRingServer _nextServer;
        private Int32 _nextServerIndex;

        public Server()
        {
            _guid = new Guid();
            _expertList = new Dictionary<String, List<IExpert>>();
            _serverRing = ConfigurationManager.AppSettings;
            _nextServerIndex = 0;
            WellKnownClientTypeEntry et = new WellKnownClientTypeEntry(typeof(IRingServer), _serverRing.Get(_nextServerIndex));
            _nextServer = (IRingServer)Activator.GetObject(et.ObjectType, et.ObjectUrl);
        }

        //Provavelmente estou a complicar!!! mas não me ocorreu mais nada!!!
        private void FowardRegistration(Guid guid, String theme, IExpert expert)
        {
            try
            {
                //First try the FastPath
                _nextServer.Register(guid, theme, expert);
            }
            catch (RemotingException) {
                bool send = false;
                Int32 index = _nextServerIndex + 1;
                //Must try to sent it to all servers in the list until the foward has been sent or, reaches is own id again
                while (!send && (index % _serverRing.Count) != _nextServerIndex)
                {
                    WellKnownClientTypeEntry et = new WellKnownClientTypeEntry(typeof(IRingServer), _serverRing.Get(index));
                    _nextServer = (IRingServer)Activator.GetObject(et.ObjectType, et.ObjectUrl);
                    try
                    {
                        _nextServer.Register(guid, theme, expert);
                        send = true;
                        _nextServerIndex = index;
                    }
                    catch (RemotingException) {
                        index++;
                    }
                }
            }
        }
        private void FowardUnregistration(Guid guid, String theme, IExpert expert)
        {
            try
            {
                //First try the FastPath
                _nextServer.UnRegister(guid, theme, expert);
            }
            catch (RemotingException)
            {
                bool send = false;
                Int32 index = _nextServerIndex + 1;
                //Must try to sent it to all servers in the list until the foward has been sent or, reaches is own id again
                while (!send && (index % _serverRing.Count) != _nextServerIndex)
                {
                    WellKnownClientTypeEntry et = new WellKnownClientTypeEntry(typeof(IRingServer), _serverRing.Get(index));
                    _nextServer = (IRingServer)Activator.GetObject(et.ObjectType, et.ObjectUrl);
                    try
                    {
                        _nextServer.UnRegister(guid, theme, expert);
                        send = true;
                        _nextServerIndex = index;
                    }
                    catch (RemotingException)
                    {
                        index++;
                    }
                }
            }
        }
        

        #region IRingServer Members

        public void Register(Guid guid, string theme, IExpert expert)
        {
            //TODO
            //Concurrency worries

            //Check if is the initial sender of the registration
            //If so the process ends here, if not, it must register the expert and foward the registration
            if (!_guid.Equals(guid))
            {
                //Check if the theme exists in the Dictionary
                //If not, create a new entrance in the dictionary
                if (!_expertList.Keys.Contains(theme))
                    _expertList.Add(theme, new List<IExpert>());

                //Safely add the expert in the corresponding list
                _expertList[theme].Add(expert);

                //Foward the registration
                FowardRegistration(guid, theme, expert);
            }
        }

        public void UnRegister(Guid guid, string theme, IExpert expert)
        {
            //TODO
            //Concurrency worries

            //Check if is the initial sender of the registration
            //If so the process ends here, if not, it must unregister the expert and foward the registration
            if (!_guid.Equals(guid))
            {
                //Check if the theme exists in the Dictionary
                if (_expertList.Keys.Contains(theme))
                    _expertList[theme].Remove(expert);

                //Foward the unregistration
                FowardUnregistration(guid, theme, expert);
            }
        }

        #endregion

        #region IZoneServer Members

        public void Register(string theme, IExpert expert)
        {
            //TODO
            //Concurrency worries

            //Check if the theme exists in the Dictionary
            //If not, create a new entrance in the dictionary
            if (!_expertList.Keys.Contains(theme))
                _expertList.Add(theme, new List<IExpert>());

            //Safely add the expert in the corresponding list
            _expertList[theme].Add(expert);

            //Foward the registration
            FowardRegistration(_guid, theme, expert);
        }

        public void UnRegister(string theme, IExpert expert)
        {
            //TODO
            //Concurrency worries

            //Check if the theme exists in the Dictionary
            if (_expertList.Keys.Contains(theme))
                _expertList[theme].Remove(expert);

            //Foward the unregistration
            FowardUnregistration(_guid, theme, expert);
        }

        public List<IExpert> getExpertList(string theme)
        {
            //TODO
            //Concurrency worries

            if (_expertList.Keys.Contains(theme))
                return _expertList[theme];
            return null;
        }

        public void NotifyClientFault(string theme, IExpert expert)
        {
            //TODO
            //Concurrency worries

            UnRegister(theme, expert);
        }

        #endregion
    }
}

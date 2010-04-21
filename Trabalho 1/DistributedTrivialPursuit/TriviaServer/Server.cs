using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using Proxy;

namespace TriviaServer
{
    public sealed class Server : MarshalByRefObject, IRingServer, IZoneServer
    {
        private readonly Guid _guid;
        private readonly IDictionary<String, List<IExpert>> _expertList;

        private IRingServer _nextServer;

        public Server()
        {
            _guid = new Guid();
            _expertList = new Dictionary<String, List<IExpert>>();

            SetNextServer();
        }

        private void SetNextServer()
        {
            //TODO
            //_nextServer = obter informação do appConfig
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

                try
                {
                    //Foward the registration
                    _nextServer.Register(guid, theme, expert);
                }
                catch (RemotingException)
                {
                    SetNextServer();
                    if (_nextServer != null)
                        _nextServer.Register(guid, theme, expert);
                }
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
                _nextServer.UnRegister(guid, theme, expert);
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
            _nextServer.Register(_guid, theme, expert);
        }

        public void UnRegister(string theme, IExpert expert)
        {
            //TODO
            //Concurrency worries

            //Check if the theme exists in the Dictionary
            if (_expertList.Keys.Contains(theme))
                _expertList[theme].Remove(expert);

            //Foward the unregistration
            _nextServer.UnRegister(_guid, theme, expert);
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

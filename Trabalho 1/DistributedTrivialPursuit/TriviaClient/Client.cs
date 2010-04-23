using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proxy;
using System.Runtime.Remoting.Messaging;

namespace TriviaClient
{
    public class Client : MarshalByRefObject, IClient, IZoneClient
    {
        public event EventHandler OnQuestionAnswered;
        public event EventHandler OnQuestionAnsweredByExpert;

        private readonly Dictionary<String, IExpert> _themeExpert;

        private readonly IZoneServer _server;

        public Client()
        {
            _server = null;
            _themeExpert = new Dictionary<string, IExpert>();
        }


        #region IClient Members

        public List<IExpert> GetExperts()
        {
            return _themeExpert.Values.ToList();
        }

        public IExpert GetExpert(string theme)
        {
            if (_themeExpert.ContainsKey(theme))
                return _themeExpert[theme];
            return null;
        }

        public List<string> GetThemes()
        {
            return _themeExpert.Keys.ToList();
        }

        public IExpert CreateExpert(string theme)
        {
            IExpert expert = new Expert(theme);
            _themeExpert.Add(theme, expert);
            return expert;
        }

        public List<IExpert> CreateExperts(List<string> themes)
        {
            List<IExpert> ret = new List<IExpert>();
            IExpert temp;
            foreach (string t in themes)
            {
                temp = new Expert(t);
                ret.Add(temp);
                _themeExpert.Add(t, temp);
            }
            return ret;
        }

        public void RegisterExpert(string theme)
        {
            _server.Register(theme, _themeExpert[theme]);
        }

        public void UnregisterExpert(string theme)
        {
            _server.UnRegister(theme, _themeExpert[theme]);
        }

        #endregion

        #region IZoneClient Members

        public void ReceiveAnswer(IAsyncResult result)
        {
            AsyncResult iaR2 = (AsyncResult)result;
            String ret = ((Func<String>)iaR2.AsyncDelegate).EndInvoke(result);
        }

        #endregion
    }
}

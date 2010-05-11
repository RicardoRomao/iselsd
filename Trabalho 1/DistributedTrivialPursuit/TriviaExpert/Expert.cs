using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proxy;
using TriviaModel;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Configuration;

namespace TriviaClient
{
    public class Expert : MarshalByRefObject, IExpert
    {
        private readonly IRepository _data;
        private readonly String _theme;

        public Expert(String theme)
        {
            _theme = theme;
            _data = XMLRepository.GetInstance(ConfigurationManager.AppSettings["DataSource"]);
        }

		public String Theme
		{
			get
			{
				return _theme;
			}
		}

        #region IExpert Members

		public event QuestionHandler OnQuestionAnswered;

        public string Ask(List<String> keyWords)
        {
            String answer = _data.GetAnswer(keyWords, _theme);
            if (!String.IsNullOrEmpty(answer) && OnQuestionAnswered != null)
				OnQuestionAnswered(this, String.Format("{0}: {1}", _theme, answer));
            return answer;
        }

        public IAsyncResult BeginAsk(List<String> keyWords, AsyncCallback callback, object state)
        {
            return new Func<List<String>, string>(Ask).BeginInvoke(keyWords, callback, state);
        }

        public string EndAsk(IAsyncResult iaR)
        {
            AsyncResult iaR2 = (AsyncResult)iaR;
            return ((Func<List<String>, string>)(iaR2.AsyncDelegate)).EndInvoke(iaR);
        }

        #endregion
    }
}
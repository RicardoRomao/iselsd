using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proxy;
using TriviaModel;
using System.Threading;
using System.Runtime.Remoting.Messaging;

namespace TriviaClient
{
    public class Expert : MarshalByRefObject, IExpert
    {
        private readonly IRepository _data;
        private readonly String _theme;
        private readonly AsyncCallback _answerCallback;

        public Expert(AsyncCallback answerCallback, String theme)
        {
            _answerCallback = answerCallback;
            _theme = theme;
            _data = null;
            //TODO: IRepository instantiation
        }

        #region IExpert Members

        public event EventHandler OnQuestionAnswered;

        public string Ask(List<String> keyWords)
        {
            String answer = _data.GetAnswer(keyWords, _theme);
            if (answer != null && OnQuestionAnswered.GetInvocationList() != null)
                OnQuestionAnswered.Invoke(this, null);
            return answer;
        }

        public IAsyncResult BeginAsk(
                    List<String> keyWords, object state)
        {
            return new Func<List<String>, string>(Ask).BeginInvoke(
                keyWords, _answerCallback, state
            );
        }

        public string EndAsk(IAsyncResult iaR)
        {
            AsyncResult iaR2 = (AsyncResult)iaR;
            return ((Func<List<String>, string>)(iaR2.AsyncDelegate)).EndInvoke(iaR);
        }

        #endregion

    }
}

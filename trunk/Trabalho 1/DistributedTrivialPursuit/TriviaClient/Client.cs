using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proxy;

namespace TriviaClient
{
    public class Client : MarshalByRefObject, IClient
    {
        public event EventHandler OnQuestionAnswered;
        public event EventHandler OnQuestionAnsweredByExpert;

        #region IClient Members

        public void ReceiveAnswer(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proxy;

namespace TriviaClient
{
    public class Client : MarshalByRefObject, IClient
    {
        #region IClient Members

        public void ReceiveAnswer(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proxy;

namespace TriviaClient
{
    public class Expert : MarshalByRefObject, IExpert
    {
        #region IExpert Members

        public event EventHandler OnQuestionAnswered;

        public string Ask(string question)
        {
            throw new NotImplementedException();            
        }

        #endregion
    }
}

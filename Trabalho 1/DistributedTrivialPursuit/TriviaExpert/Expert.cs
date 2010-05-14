using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proxy;
using TriviaModel;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Configuration;
using System.Runtime.Remoting.Lifetime;

namespace TriviaExpert
{
    public class Expert : MarshalByRefObject, IExpert
    {
        private readonly IRepository _data;
        private readonly String _theme;
        private const double RENEW_TIME = 60;

        public Expert(String theme)
        {
            _theme = theme;
            _data = XMLRepository.GetInstance(ConfigurationManager.AppSettings["DataSource"]);
        }

        #region IExpert Members

        public event QuestionHandler OnQuestionAnswered;

        public string Ask(List<String> keyWords)
        {
            String answer = _data.GetAnswer(keyWords, _theme);
            if (!String.IsNullOrEmpty(answer) && OnQuestionAnswered != null)
            {
                ILease lease = (ILease)this.GetLifetimeService();
                    

                OnQuestionAnswered(this,
                                   string.Join(",", keyWords.ToArray()),
                                   answer + ":" + lease.CurrentLeaseTime);
            }
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

        public String GetTheme()
        {
            return _theme;
        }

        public ITriviaSponsor GetSponsor()
        {
            return new ExpertSponsor(RENEW_TIME);
        }
        #endregion

        public override object InitializeLifetimeService()
        {
            ILease lease = (ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromSeconds(60);
                lease.SponsorshipTimeout = TimeSpan.FromSeconds(10);
                lease.RenewOnCallTime = TimeSpan.FromSeconds(30);
            }
            return lease;
        }
    }
}
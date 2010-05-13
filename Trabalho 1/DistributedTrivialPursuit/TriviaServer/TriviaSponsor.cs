using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Lifetime;
using Proxy;

namespace TriviaServer
{
    public class TriviaSponsor : MarshalByRefObject, ITriviaSponsor
    {
        private volatile bool _toRenew = true;

        #region ITriviaSponsor Members

        public void setNotRenew()
        {
            _toRenew = false;
        }

        #endregion

        #region ISponsor Members

        public TimeSpan Renewal(ILease lease)
        {
            Console.WriteLine("Sponsor being called....");
            if (_toRenew)
                return TimeSpan.FromSeconds(30);
            else
                return TimeSpan.Zero;
        }

        #endregion
    }
}

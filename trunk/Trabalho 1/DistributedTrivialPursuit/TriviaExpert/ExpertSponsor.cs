using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Lifetime;
using Proxy;

namespace TriviaExpert
{
    public class ExpertSponsor : MarshalByRefObject, ITriviaSponsor
    {
        private volatile bool _toRenew = true;
        private readonly double _renewVal;

        public ExpertSponsor(double renewVal) { _renewVal = renewVal; }

        #region ITriviaSponsor Members

        public void setNotRenew()
        {
            _toRenew = false;
        }

        #endregion

        #region ISponsor Members

        public TimeSpan Renewal(ILease lease)
        {
            if (_toRenew)
                return TimeSpan.FromSeconds(_renewVal);
            else
                return TimeSpan.Zero;
        }

        #endregion
    }
}

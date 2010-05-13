using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Lifetime;
using Proxy;

namespace TriviaServer
{
    public class ServerSponsor : MarshalByRefObject, ITriviaSponsor
    {
        private volatile bool _toRenew = true;
        private readonly string _entity;
        private readonly double _renewVal;

        public ServerSponsor(string entity, double renewVal)
        {
            _entity = entity;
            _renewVal = renewVal;
        }

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
            {
                Console.WriteLine("[{0}'s Sponsor] - CALL MADE: Renewing {1}s."
                            , _entity, _renewVal);
                return TimeSpan.FromSeconds(_renewVal);
            }
            else
            {
                Console.WriteLine("[{0}'s Sponsor] - CALL MADE: Not Renewing.");
                return TimeSpan.Zero;
            }
        }

        #endregion
    }
}

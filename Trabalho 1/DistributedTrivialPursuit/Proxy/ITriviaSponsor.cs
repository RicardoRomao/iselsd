using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Lifetime;

namespace Proxy
{
    public interface ITriviaSponsor : ISponsor
    {
        void setNotRenew();
    }
}

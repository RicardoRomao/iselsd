using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy
{
    public interface IRingServer
    {
        void Register(Guid guid, string theme, IExpert expert);
        void UnRegister(Guid guid, string theme, IExpert expert);
        ITriviaSponsor getSponsor();
    }
}
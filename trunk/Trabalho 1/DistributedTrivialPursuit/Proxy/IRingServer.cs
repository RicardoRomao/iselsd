using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy
{
    public interface IRingServer
    {
        string GetId();
        void Register(string uId, string theme, IExpert expert);
        void UnRegister(string uId, string theme, IExpert expert);
        ITriviaSponsor getSponsor();

    }
}
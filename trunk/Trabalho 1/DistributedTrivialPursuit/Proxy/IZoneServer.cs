using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy
{
    public interface IZoneServer
    {
        void Register(String theme, IExpert expert);
        void UnRegister(String theme, IExpert expert);
        List<IExpert> getExpertList(String theme);
        void NotifyClientFault(String theme, IExpert expert);
    }
}

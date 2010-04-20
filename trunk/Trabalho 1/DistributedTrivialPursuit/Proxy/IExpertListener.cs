using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy
{
    public interface IExpertListener
    {
        void Notify(IExpert sender, EventArgs args);
    }
}

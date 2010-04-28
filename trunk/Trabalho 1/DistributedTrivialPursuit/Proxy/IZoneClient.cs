using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy
{
    public interface IZoneClient
    {
        void ReceiveAnswer(IAsyncResult result);
    }
}
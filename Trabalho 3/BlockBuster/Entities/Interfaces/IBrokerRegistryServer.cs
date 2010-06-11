using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public interface IBrokerRegistryServer
    {
        void AddCinema(string name, string url);
        void RemoveCinema(string name);
        Dictionary<string, string> GetCinemas();
    }
}

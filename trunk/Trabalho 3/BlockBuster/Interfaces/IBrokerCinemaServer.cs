using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interfaces
{
    public interface IBrokerCinemaServer
    {
        void AddCinema(string name, string url);
        void RemoveCinema(string name);
        Dictionary<string, string> GetCinemas();
    }
}

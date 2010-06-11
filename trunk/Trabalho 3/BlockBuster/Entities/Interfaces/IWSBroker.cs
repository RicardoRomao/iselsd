using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public interface IWSBroker
    {
        List<CinemaRegistry> GetCinemas();
        void RegisterCinema(string name, string url);
        void UnregisterCinema(string name);
    }
}

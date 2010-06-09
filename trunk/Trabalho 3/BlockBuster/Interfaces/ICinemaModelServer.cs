using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interfaces
{
    public interface ICinemaModelServer
    {
        Guid AddReservation(String name, String sessionId, int seats);
        bool RemoveReservation(Guid code);
        int GetTotalReservations(String sessionId);
    }
}

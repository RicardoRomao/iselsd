using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public interface ICinemaReservationServer
    {
        Guid AddReservation(String name, String sessionId, int seats);
        bool RemoveReservation(Guid code);
        int GetTotalReservations(String sessionId);
    }
}

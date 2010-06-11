using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace Entities
{
    public class Reservation
    {
        public Guid Code { get; private set; }
        public int Seats { get; private set; }
        public String Name { get; set; }
        public String SessionId { get; set; }

        public Reservation() { }
    }
}

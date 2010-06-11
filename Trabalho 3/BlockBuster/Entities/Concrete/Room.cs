using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace Entities
{
    public class Room
    {
        public int Num { get; set; }
        public int Capacity { get; set; }

        public Room() { }

        public Room(int num, int capacity)
        {
            Num = num;
            Capacity = capacity;
        }
    }
}

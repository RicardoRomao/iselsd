using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace Entities
{
    public class CinemaRegistry
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public CinemaRegistry() { }

        public CinemaRegistry(string name, string url)
        {
            Name = name;
            Url = url;
        }
    }
}

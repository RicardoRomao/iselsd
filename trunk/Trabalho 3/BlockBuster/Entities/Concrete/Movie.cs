using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public String Desc { get; set; }
        public List<MovieSession> Sessions { get; set; }

        public Movie() { }

        public Movie(int id, String title, String desc)
        {
            Id = id;
            Title = title;
            Desc = desc;
        }

        public void AddSession(MovieSession session)
        {
            if (Sessions == null) Sessions = new List<MovieSession>();
            Sessions.Add(session);
        }
    }
}

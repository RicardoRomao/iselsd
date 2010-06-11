using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace Entities
{
    public interface IWSCinema
    {
        List<Movie> GetMovies();
        List<Movie> GetMoviesByTitle(List<string> keyWords);
        List<Movie> GetMoviesByPeriod(DateTime start, DateTime end);
        Guid AddReservation(string name, string sessionId, int seats);
        bool RemoveReservation(Guid code);
    }
}

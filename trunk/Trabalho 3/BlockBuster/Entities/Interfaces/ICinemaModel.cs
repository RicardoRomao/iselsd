using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities.Interfaces
{
    public interface ICinemaModel
    {
        IEnumerable<Movie> GetMovies();
        IEnumerable<Movie> GetMovies(List<String> keywords);
        IEnumerable<Movie> GetMovies(DateTime startTime, DateTime endTime);
        int GetRoomCapacity(String sessionId);
    }
}

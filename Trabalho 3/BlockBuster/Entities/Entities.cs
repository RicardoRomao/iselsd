using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class Movie
    {
        public int Id { get; private set; }
        public String Title { get; private set; }
        public String Desc { get; private set; }
        public List<MovieSession> Sessions { get; private set; }

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

    public class MovieSession
    {
        public String Id { get; private set; }
        public DateTime StartTime { get; private set; }
        public int MovieId { get; private set; }
        public int RoomId { get; private set; }
        public Room Room { get; private set; }

        public MovieSession(String id, DateTime startTime, int movieId, int roomId)
        {
            Id = id;
            StartTime = startTime;
            MovieId = movieId;
            RoomId = roomId;
        }

        public void SetRoom(Room room) { Room = room; }
    }

    public class Room
    {
        public int Num { get; private set; }
        public int Capacity { get; private set; }

        public Room(int num, int capacity)
        {
            Num = num;
            Capacity = capacity;
        }
    }

    public class Reservation
    {
        public Guid Code { get; private set; }
        public int Seats { get; private set; }
        public String Name { get; set; }
        MovieSession _session;
    }

    public class TheaterRoom
    {
        class Reserves
        {
            int _total = 0;
            public int Total { get; set; }
            List<Reservation> _reservations;

            public void Add(Reservation res)
            {
                if (_reservations == null) _reservations = new List<Reservation>();
                _reservations.Add(res);
                _total += res.Seats;
            }

            public void Remove(Guid code)
            {
                Reservation r = _reservations.First(res => res.Code == code);
                _total -= r.Seats;
                _reservations.Remove(r);
            }
        }

        int _capacity;
        Dictionary<String, Reserves> _attendance =
            new Dictionary<String, Reserves>();

        public TheaterRoom(int capacity) { _capacity = capacity; }

        public bool AddReservation(String sessionCode, Reservation res)
        {
            if (_attendance.ContainsKey(sessionCode))
            {
                if (_attendance[sessionCode] == null)
                    _attendance[sessionCode] = new Reserves();
                if ((_attendance[sessionCode].Total + res.Seats) >= _capacity)
                    return false;
                _attendance[sessionCode].Add(res);
                return true;
            }
            return false;
        }

        public bool RemoveReservation(String sessionCode, Guid code)
        {
            if (_attendance.ContainsKey(sessionCode))
            {
                if (_attendance[sessionCode] == null)
                    return false;
                _attendance[sessionCode].Remove(code);
                return true;
            }
            return false;
        }
    }

}

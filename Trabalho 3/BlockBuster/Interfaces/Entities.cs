using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interfaces
{

    public class MovieSession
    {
        String _code;
        DateTime _startTime;

        public MovieSession(String code, DateTime startTime)
        {
            _code = code;
            _startTime = startTime;
        }
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
            new Dictionary<String,Reserves>();

        public TheaterRoom(int capacity) { _capacity = capacity; }

        public bool AddReservation(String sessionCode, Reservation res) {
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

    public class Reservation
    {
        public Guid Code { get; private set; }
        public int Seats { get; private set; }
        public String Name { get; set; }
        MovieSession _session;
        Movie _movie;
    }

    public class Movie
    {
        String _title;
        String _desc;
        List<MovieSession> _appearances;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using System.Configuration;
using System.Xml.Linq;

namespace Model
{
    public sealed class CinemaModel
    {
        static Object _monitor = new Object();
        static readonly string _source = ConfigurationSettings.AppSettings["datasource"];

        private Dictionary<int, Movie> _movies;
        private Dictionary<int, Room> _rooms;
        private Dictionary<string, MovieSession> _sessions;

        private static readonly CinemaModel _current = new CinemaModel();

        public static CinemaModel Current { get { return _current; } }

        private CinemaModel() { Init(); }

        #region Private Operations
        private void Init()
        {
            lock (_monitor)
            {
                XDocument doc = XDocument.Load(_source, LoadOptions.None);
                LoadRooms(doc).LoadSessions(doc).LoadMovies(doc);
            }

            foreach (MovieSession s in _sessions.Values)
            {
                s.SetRoom(_rooms[s.RoomId]); // Sets session room
                _movies[s.MovieId].AddSession(s); // Adds session to movie
            }
        }

        private CinemaModel LoadRooms(XDocument doc)
        {
            _rooms = new Dictionary<int, Room>();

            IEnumerable<XElement> roomsRoot =
                            from r in doc.Root.Elements().Where(x => x.Name == "rooms")
                            select r;

            foreach (XElement r in roomsRoot.Descendants())
            {
                _rooms.Add(Int32.Parse(r.Attribute("id").Value),
                        new Room(Int32.Parse(r.Attribute("id").Value),
                                Int32.Parse(r.Attribute("capacity").Value)
                        )
                );
            }
            return this;
        }

        private CinemaModel LoadSessions(XDocument doc)
        {
            _sessions = new Dictionary<string, MovieSession>();

            IEnumerable<XElement> sessionsRoot =
                            from r in doc.Root.Elements().Where(x => x.Name == "sessions")
                            select r;

            foreach (XElement s in sessionsRoot.Descendants())
            {
                _sessions.Add(s.Attribute("id").Value,
                        new MovieSession(s.Attribute("id").Value,
                                DateTime.Parse(s.Attribute("startTime").Value),
                                Int32.Parse(s.Attribute("movieId").Value),
                                Int32.Parse(s.Attribute("roomId").Value)
                        )
                );
            }
            return this;
        }

        private CinemaModel LoadMovies(XDocument doc)
        {
            _movies = new Dictionary<int, Movie>();

            IEnumerable<XElement> moviesRoot =
                            from m in doc.Root.Elements().Where(x => x.Name == "movies")
                            select m;

            foreach (XElement m in moviesRoot.Descendants())
            {
                _movies.Add(Int32.Parse(m.Attribute("id").Value),
                        new Movie(Int32.Parse(m.Attribute("id").Value),
                                m.Attribute("title").Value,
                                m.Value)
                );
            }
            return this;
        }
        #endregion

        #region Public Operations
        //Returns movies that contain at least one of the keywords provided
        public IEnumerable<Movie> GetMovies(List<String> keywords)
        {
            Init();
            if (keywords == null) return _movies.Values;
            keywords = keywords.ConvertAll(s => s.ToUpper());
            return _movies.Values.Where(m => m.Title.ToUpper().Split(' ').Any(
                        t => keywords.Contains(t)));
        }

        //Returns all movies appearing in this cinema
        public IEnumerable<Movie> GetMovies() {
            Init();
            return GetMovies(null); 
        }

        //Returns movies that have sessions within the given period
        public IEnumerable<Movie> GetMovies(DateTime startTime, DateTime endTime)
        {
            Init();
            return _movies.Values.Where(m =>
                    m.Sessions != null && m.Sessions.Any(s =>
                        s.StartTime.CompareTo(startTime) >= 0 &&
                        s.StartTime.CompareTo(endTime) <= 0
                    )
            );
        }

        //Returns the total number of seats of a room by session id
        public int GetRoomCapacity(String sessionId)
        {
            if (_sessions.ContainsKey(sessionId))
                return _sessions[sessionId].Room.Capacity;
            return -1;
        }
        #endregion
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Configuration;
using System.Xml.Linq;

namespace CinemaModel
{
    public class CinemaModel
    {
        static readonly string _source = ConfigurationSettings.AppSettings["datasource"];
        private Dictionary<int, Movie> _movies;
        private Dictionary<int, Room> _rooms;
        private Dictionary<string, MovieSession> _sessions;

        private static CinemaModel current;

        private CinemaModel() { Init(); }

        public static CinemaModel Current
        {
            get
            {
                if (current == null)
                    current = new CinemaModel();
                return current;
            }
        }

        private void Init()
        {
            XDocument doc = XDocument.Load(_source, LoadOptions.None);

            loadRooms(doc).loadSessions(doc).loadMovies(doc);

            foreach (MovieSession s in _sessions.Values)
            {
                s.SetRoom(_rooms[s.RoomId]); // Sets session room
                _movies[s.MovieId].AddSession(s); // Adds session to movie
            }
        }

        private CinemaModel loadRooms(XDocument doc)
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

        private CinemaModel loadSessions(XDocument doc)
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

        private CinemaModel loadMovies(XDocument doc)
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

        //Returns a movie by id
        private Movie getMovie(int id) {
            if (_movies.ContainsKey(id))
                return _movies[id];
            return null;
        }

        //Returns movies that contain at least one of the keywords provided
        private IEnumerable<Movie> getMovies(List<String> keywords) {
            return _movies.Values.Where(m => keywords.Contains(m.Title));
        }

        //Returns movies that have sessions within the given period
        private IEnumerable<Movie> getMovies(DateTime startTime, DateTime endTime)
        {
            return _movies.Values.Where(m =>
                    m.Sessions.Where(s =>
                        s.StartTime >= startTime && s.StartTime <= endTime
                    ).Count() > 0
            );
        }

    }
}

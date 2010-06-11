using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace Entities
{
    public class MovieSession
    {
        public String Id { get; set; }
        public DateTime StartTime { get; set; }
        public int MovieId { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public MovieSession() { }

        public MovieSession(String id, DateTime startTime, int movieId, int roomId)
        {
            Id = id;
            StartTime = startTime;
            MovieId = movieId;
            RoomId = roomId;
        }

        public void SetRoom(Room room) { Room = room; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientFormsApplication
{
    public class SessionInfo
    {
        public string SessionID { get; set; }
        public DateTime StartTime { get; set; }
        public string Cinema { get; set; }
        public string MovieTitle { get; set; }
        public string Name { get; set; }
        public Guid Code { get; set; }
        public int Seats { get; set; }
		public int Expires { get; set; }
    }

}

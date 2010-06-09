using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Entities;
using Model;
using System.Runtime.Remoting;
using System.Configuration;
using Interfaces;

namespace WSCinema
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://sd.isel.pt/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CinemaService : System.Web.Services.WebService
    {

        private static ICinemaModelServer _server;

        private static ICinemaModelServer Server
        {
            get
            {
                if (_server == null)
                {
                    WellKnownClientTypeEntry[] entries =
                        RemotingConfiguration.GetRegisteredWellKnownClientTypes();
                    _server = (ICinemaModelServer)Activator.GetObject(
                        entries[0].ObjectType, entries[0].ObjectUrl);
                }
                return _server;
            }
        }

        static CinemaService()
        {
            RemotingConfiguration.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, false);
        }

        [WebMethod]
        public List<Movie> GetMovies()
        {
            return CinemaModel.Current.GetMovies().ToList();
        }

        [WebMethod]
        public List<Movie> GetMoviesByTitle(List<string> keyWords)
        {
            return CinemaModel.Current.GetMovies(keyWords).ToList();
        }

        [WebMethod]
        public List<Movie> GetMoviesByPeriod(DateTime start, DateTime end)
        {
            return CinemaModel.Current.GetMovies(start, end).ToList();
        }

        [WebMethod]
        public Guid AddReservation(string name, string sessionId, int seats)
        {
            int roomCapacity = CinemaModel.Current.GetRoomCapacity(sessionId);
            int sessionReservations = Server.GetTotalReservations(sessionId);

            if ((sessionReservations + seats <= roomCapacity) && roomCapacity > 0)
            {
                return Server.AddReservation(name, sessionId, seats);
            }
            return Guid.Empty;
        }

        [WebMethod]
        public bool RemoveReservation(Guid code)
        {
            return Server.RemoveReservation(code);
        }
    }
}

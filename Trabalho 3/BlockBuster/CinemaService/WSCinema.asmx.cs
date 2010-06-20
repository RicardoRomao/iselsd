using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Model;
using System.Runtime.Remoting;
using Entities;
using System.Web.Services.Protocols;
using System.Xml;
using System.Net.Sockets;
using Headers;

namespace WSCinema
{
    /// <summary>
    /// BlockBusterCinema web service that provides information on 
    /// movies currently showing and reservations capability
    /// </summary>
    [WebService(Namespace = "http://sd.deetc.isel.pt/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WSCinema : System.Web.Services.WebService, IWSCinema
    {
        private readonly Object _monitor = new Object();
        private ICinemaReservationServer _server;
        public ReservationHeader _reservationHeader = new ReservationHeader();

        private new ICinemaReservationServer Server
        {
            get
            {
                lock (_monitor)
                {
                    if (_server == null)
                    {
                        WellKnownClientTypeEntry[] entries =
                            RemotingConfiguration.GetRegisteredWellKnownClientTypes();
                        _server = (ICinemaReservationServer)Activator.GetObject(
                            entries[0].ObjectType, entries[0].ObjectUrl);
                    }
                }
                return _server;
            }
        }

        static WSCinema()
        {
            RemotingConfiguration.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, false);

        }

        #region Movies/Sessions Operations
        [WebMethod(Description = "Returns all movies in the cinema")]
        public List<Movie> GetMovies()
        {
            return CinemaModel.Current.GetMovies().ToList();
        }

        [WebMethod(Description = "Returns all movies whose titles match the keywords")]
        public List<Movie> GetMoviesByTitle(List<string> keyWords)
        {
            return CinemaModel.Current.GetMovies(keyWords).ToList();
        }

        [WebMethod(Description = "Returns all movies with sessions within the period")]
        public List<Movie> GetMoviesByPeriod(DateTime start, DateTime end)
        {
            return CinemaModel.Current.GetMovies(start, end).ToList();
        }
        #endregion

        #region Reservations Operations
        [WebMethod(Description = "Adds a reservation to a movie session.\n" +
            "Returns an empty Guid if no more seats are available.\n" +
            "Throws SoapException if sessionId is invalid or " +
            "reservation server is down.")]
		[SoapHeader("_reservationHeader", Direction = SoapHeaderDirection.Out)]
        public Guid AddReservation(string name, string sessionId, int seats)
        {
			Guid res = Guid.Empty;
            int roomCapacity = CinemaModel.Current.GetRoomCapacity(sessionId);
            if (roomCapacity == -1)
                throw new SoapException("SessionID not Valid",
                    SoapException.ServerFaultCode, "BBCinema",
                    GetSoapExceptionDesc("Invalid session id entered in AddReservation operation"));
            try
            {
                int sessionReservations = Server.GetTotalReservations(sessionId);
                if ((sessionReservations + seats) <= roomCapacity && roomCapacity > 0)
                res = Server.AddReservation(name, sessionId, seats);
                if (!res.Equals(Guid.Empty))
                {
					_reservationHeader.Days = (new Random()).Next(1, 3);
                }
                return res;
            }
            catch (SocketException)
            {
                throw new SoapException("Reservation Server down",
                       SoapException.ServerFaultCode, "BBCinema",
                       GetSoapExceptionDesc("No reservation capabilities possible."));
            }
        }

        [WebMethod(Description = "Removes the reservation with the given Guid.\n" +
            "Throws SoapException if reservation server is down.")]
        public bool RemoveReservation(Guid code)
        {
            try
            {
                return Server.RemoveReservation(code);
            }
            catch (SocketException)
            {
                throw new SoapException("Reservation Server down",
                       SoapException.ServerFaultCode, "BBCinema",
                       GetSoapExceptionDesc("No reservation capabilities possible."));

            }
        }
        #endregion

        private XmlNode GetSoapExceptionDesc(string content)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode node = doc.CreateNode(XmlNodeType.Element, SoapException.DetailElementName.Name, SoapException.DetailElementName.Namespace);
            XmlNode child = doc.CreateNode(XmlNodeType.Element, "error", "http://sd.deetc.isel.pt/");
            child.InnerText = "BBCinemaService:" + content;
            node.AppendChild(child);
            return node;
        }
    }
}

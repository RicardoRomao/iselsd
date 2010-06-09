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
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Xml;
using System.Net;
using System.Net.Sockets;

namespace WSCinema
{
    /// <summary>
    /// BlockBusterCinema web service providing information on 
    /// movies currently showing and reservations capability
    /// </summary>
    [WebService(Namespace = "http://sd.isel.pt/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class CinemaService : System.Web.Services.WebService
    {

        private static ICinemaModelServer _server;

        private new static ICinemaModelServer Server
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
            "Returns an empty Guid if no more seats are available.")]
        public Guid AddReservation(string name, string sessionId, int seats)
        {
            try
            {
                int roomCapacity = CinemaModel.Current.GetRoomCapacity(sessionId);
                if (roomCapacity == -1)
                    throw new SoapException("SessionID not Valid",
                        SoapException.ServerFaultCode, "BBCinema",
                        GetSoapExceptionDesc("Invalid session id entered in AddReservation operation"));

                int sessionReservations = Server.GetTotalReservations(sessionId);

                if ((sessionReservations + seats) <= roomCapacity && roomCapacity > 0)
                    return Server.AddReservation(name, sessionId, seats);
            }
            catch (SocketException)
            {
                throw new SoapException("Reservation Server down",
                       SoapException.ServerFaultCode, "BBCinema",
                       GetSoapExceptionDesc("No reservation capabilities possible."));
            }
            return Guid.Empty;
        }

        [WebMethod(Description = "Removes the reservation with the given Guid")]
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
            XmlNode child = doc.CreateNode(XmlNodeType.Element, "error", "http://sd.isel.pt/");
            child.InnerText = "BBCinemaService:" + content;
            node.AppendChild(child);
            return node;
        }
    }
}

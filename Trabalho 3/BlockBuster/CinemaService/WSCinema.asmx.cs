using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Entities;
using Model;

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
        public bool AddReservation(Reservation res)
        {


            throw new NotImplementedException();
        }
    }
}

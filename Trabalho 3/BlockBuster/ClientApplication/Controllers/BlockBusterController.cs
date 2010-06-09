using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using ClientApplication.BlockBusterCinema;

namespace ClientApplication.Controllers
{
    public class BlockBusterController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListMovies()
        {
            CinemaService service = new CinemaService();
            List<Movie> movies = service.GetMovies().ToList();
            return View(movies);
        }

        public ActionResult ListMoviesByTitle()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ListMoviesByTitle(string keywords)
        {
            CinemaService service = new CinemaService();
            List<Movie> movies = service.GetMoviesByTitle(keywords.Split(' ')).ToList();
            return View(movies);
        }

        public ActionResult ListMoviesByPeriod()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ListMoviesByPeriod(DateTime start, DateTime end)
        {
            CinemaService service = new CinemaService();
            List<Movie> movies = service.GetMoviesByPeriod(start, end).ToList();
            return View(movies);
        }
    }
}

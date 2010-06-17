using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientFormsApplication.BBBroker;
using ClientFormsApplication.BBCinema;
using System.Windows.Forms;
using System.Web.Services.Protocols;

namespace ClientFormsApplication
{
    class Coordinator : ICoordinator
    {
        class CinemaInfo
        {
            public String Url { get; set; }
            public List<Movie> Movies { get; set; }
        }

        private readonly WSBroker _broker = new WSBroker();
        private readonly WSCinema _cinema = new WSCinema();
        private readonly Dictionary<string, CinemaInfo> _registry =
                new Dictionary<string, CinemaInfo>();

        public Coordinator()
        {
            _cinema.GetMoviesCompleted += Cinema_GetMoviesCompleted;
            _cinema.GetMoviesByTitleCompleted += Cinema_GetMoviesByTitleCompleted;
            _cinema.GetMoviesByPeriodCompleted += Cinema_GetMoviesByPeriodCompleted;
            _cinema.AddReservationCompleted += Cinema_AddReservationCompleted;
            _cinema.RemoveReservationCompleted += Cinema_RemoveReservationCompleted;

            _broker.GetCinemasCompleted += Broker_GetCinemasCompleted;
            _broker.GetCinemasAsync();
        }

        #region ICoordinator Members
        public event ReceivedMovieListDelegate MoviesReceived;
        public event ErrorOccuredDelegate ErrorOccured;
        public event ReservationProcessedDelegate AddReservationProcessed;
        public event ReservationProcessedDelegate RemoveReservationProcessed;

        public void GetMovies()
        {
            foreach (KeyValuePair<string, CinemaInfo> kv in _registry)
            {
                _cinema.Url = kv.Value.Url;
                _cinema.GetMoviesAsync(kv.Key);
            }
        }

        public void GetMovies(String keywords)
        {
            foreach (KeyValuePair<string, CinemaInfo> kv in _registry)
            {
                _cinema.Url = kv.Value.Url;
                _cinema.GetMoviesByTitleAsync(keywords.Split(' '), kv.Key);
            }
        }

        public void GetMovies(DateTime start, DateTime end)
        {
            foreach (KeyValuePair<string, CinemaInfo> kv in _registry)
            {
                _cinema.Url = kv.Value.Url;
                _cinema.GetMoviesByPeriodAsync(start, end, kv.Key);
            }
        }

        public void SendReservation(SessionInfo resInfo)
        {
            _cinema.Url = _registry[resInfo.Cinema].Url;
            _cinema.AddReservationAsync(
                resInfo.Name,
                resInfo.SessionID,
                resInfo.Seats,
                resInfo
            );
        }

        public void RemoveReservation(SessionInfo resInfo)
        {
            _cinema.Url = _registry[resInfo.Cinema].Url;
            _cinema.RemoveReservationAsync(resInfo.Code, resInfo);
        }
        #endregion

        #region Broker Async Callbacks
        void Broker_GetCinemasCompleted(object sender, GetCinemasCompletedEventArgs e)
        {
            try
            {
                List<CinemaRegistry> cinemas = e.Result.ToList();
                foreach (CinemaRegistry c in cinemas)
                {
                    _registry.Add(c.Name, new CinemaInfo { Url = c.Url });
                }
            }
            catch (Exception ex)
            {
                if (ErrorOccured != null)
                {
                    ErrorOccured("BBBroker - Registry server is down.");
                }
            }

        }
        #endregion

        #region Cinema Async Callbacks
        void Cinema_GetMoviesCompleted(object sender, GetMoviesCompletedEventArgs e)
        {
            string name = (string)e.UserState;
            if (MoviesReceived != null)
            {
                MoviesReceived(name, e.Result.ToList());
            }
        }

        void Cinema_GetMoviesByTitleCompleted(object sender, GetMoviesByTitleCompletedEventArgs e)
        {
            string name = (string)e.UserState;
            if (MoviesReceived != null)
            {
                MoviesReceived(name, e.Result.ToList());
            }
        }

        void Cinema_GetMoviesByPeriodCompleted(object sender, GetMoviesByPeriodCompletedEventArgs e)
        {
            string name = (string)e.UserState;
            if (MoviesReceived != null)
            {
                MoviesReceived(name, e.Result.ToList());
            }
        }

        void Cinema_AddReservationCompleted(object sender, AddReservationCompletedEventArgs e)
        {
            SessionInfo resInfo = (SessionInfo)e.UserState;
            try
            {
                resInfo.Code = e.Result;
                if (AddReservationProcessed != null)
                    AddReservationProcessed(resInfo);
            }
            catch (Exception)
            {
                if (ErrorOccured != null)
                {
                    ErrorOccured(resInfo.Cinema + " - Reservation server is down.");
                }
            }
        }

        void Cinema_RemoveReservationCompleted(object sender, RemoveReservationCompletedEventArgs e)
        {
            SessionInfo resInfo = (SessionInfo)e.UserState;
            try
            {
                if (RemoveReservationProcessed != null)
                    RemoveReservationProcessed(resInfo);
            }
            catch (Exception)
            {
                if (ErrorOccured != null)
                {
                    ErrorOccured(resInfo.Cinema + " - Reservation server is down.");
                }
            }
        }
        #endregion

    }
}

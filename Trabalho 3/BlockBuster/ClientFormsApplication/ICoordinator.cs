using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientFormsApplication.BBCinema;

namespace ClientFormsApplication
{
    delegate void ReceivedMovieListDelegate(string cinemaName, List<Movie> movies);
    delegate void ErrorOccuredDelegate(string message);
    delegate void ReservationProcessedDelegate(SessionInfo resInfo);

    interface ICoordinator
    {
        event ReceivedMovieListDelegate MoviesReceived;
        event ErrorOccuredDelegate ErrorOccured;
        event ReservationProcessedDelegate AddReservationProcessed;
        event ReservationProcessedDelegate RemoveReservationProcessed;

        void GetMovies();
        void GetMovies(String keywords);
        void GetMovies(DateTime start, DateTime end);
        void SendReservation(SessionInfo resInfo);
        void RemoveReservation(SessionInfo resInfo);
    }
}

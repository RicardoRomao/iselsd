using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Tester.BlockBusterCinema;
using System.Threading;

namespace Tester
{


    class Program
    {
        public static ManualResetEvent mre = new ManualResetEvent(false);

        public static void GetMoviesCompleted(object sender, GetMoviesCompletedEventArgs state)
        {
            List<Movie> movies = state.Result.ToList();
            foreach (Movie m in movies)
                Console.WriteLine("Id: {0}\nTitle: {1}\nDesc: {2}\n\n",
                    m.Id, m.Title, m.Desc);
            mre.Set();
        }

        static void Main(string[] args)
        {
            CinemaService cinema = new CinemaService();

            cinema.GetMoviesCompleted += GetMoviesCompleted;

            //List<Movie> movies = cinema.GetMovies().ToList();
            cinema.GetMoviesAsync();
            mre.WaitOne();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Runtime.Remoting;

namespace CinemaReservationServer
{
    
    class ServerApp{

        public static void Main(String[] args)
        {
            Console.WriteLine(".: {0} :.",ConfigurationManager.AppSettings["serverName"]);
            RemotingConfiguration.Configure(
                AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                false
            );
            Server ps = new Server();
            Console.WriteLine("Server started ...");
            Console.WriteLine("Press Enter to stop server.");
            Console.ReadLine();
        }

    }
}

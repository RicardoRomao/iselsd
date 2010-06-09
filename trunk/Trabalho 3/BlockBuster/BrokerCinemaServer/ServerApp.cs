using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;

namespace BrokerCinemaServer
{
    class ServerApp
    {
        public static void Main(String[] args)
        {
            Console.WriteLine(".: Starting Broker Cinema Server :.");
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

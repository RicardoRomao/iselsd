using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Configuration;

namespace TriviaServer
{
    public class ServerApp
    {
        private static readonly string configFile = ConfigurationManager.AppSettings["serverConfigFile"];

        public static void Main(String[] args)
        {
            RemotingConfiguration.Configure(configFile, false);
            Server ps = new Server();
            Console.WriteLine("Server started ...");
            Console.WriteLine("Press Enter to stop server.");
            Console.ReadLine();
        }
    }
}
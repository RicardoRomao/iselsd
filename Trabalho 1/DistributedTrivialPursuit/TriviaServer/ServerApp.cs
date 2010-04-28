using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;

namespace TriviaServer
{
    public class ServerApp
    {
        private static readonly String configFile = "TriviaServer.exe.config";

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
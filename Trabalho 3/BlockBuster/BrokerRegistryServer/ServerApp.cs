using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Threading;

namespace BrokerRegistryServer
{
    class ServerApp
    {
        public static void Worker(object i){
            KeyValuePair<int, Server> s = (KeyValuePair<int, Server>)i;
            s.Value.AddCinema("Cinema" + s.Key, "http://localhost/xpto"+s.Key);
        }

        public static void DoTest(Server ps)
        {
            for (int i = 0; i < 20; i++)
            {
                new Thread(Worker).Start(new KeyValuePair<int, Server>(i, ps));
            }
        }

        public static void Main(String[] args)
        {
            Console.WriteLine(".: Starting Broker Registry Server :.");
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

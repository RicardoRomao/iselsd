using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Runtime.Remoting;

namespace TriviaClient
{
    class ClientApp
    {

        private static readonly string configFile = ConfigurationManager.AppSettings["clientConfigFile"];

        [STAThread]
        static void Main()
        {
            RemotingConfiguration.Configure(configFile, false);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TriviaClientForm());
        }
    }
}

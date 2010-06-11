using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Runtime.Remoting;
using System.Xml;
using System.Web.Services.Protocols;
using System.Net.Sockets;
using Entities;

namespace BrokerService
{
    /// <summary>
    /// BlockBusterBroker web service that provides information on 
    /// BlockBusterCinemas client services endpoint
    /// </summary>
    [WebService(Namespace = "http://sd.deetc.isel.pt/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WSBroker : WebService, IWSBroker
    {
        private readonly Object _monitor = new Object();
        private IBrokerRegistryServer _server;

        private new IBrokerRegistryServer Server
        {
            get
            {
                lock (_monitor)
                {
                    if (_server == null)
                    {
                        WellKnownClientTypeEntry[] entries =
                            RemotingConfiguration.GetRegisteredWellKnownClientTypes();
                        _server = (IBrokerRegistryServer)Activator.GetObject(
                            entries[0].ObjectType, entries[0].ObjectUrl);
                    }
                }
                return _server;
            }
        }

        static WSBroker()
        {
            RemotingConfiguration.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, false);
        }

        [WebMethod(Description = "Returns all registered cinemas in Broker registry.")]
        public List<CinemaRegistry> GetCinemas()
        {
            List<CinemaRegistry> ret = new List<CinemaRegistry>();
            try
            {
                foreach (KeyValuePair<string, string> kv in Server.GetCinemas())
                {
                    ret.Add(new CinemaRegistry(kv.Key, kv.Value));
                }
            }
            catch (SocketException)
            {
                throw new SoapException("Cinema Registry Server down",
                       SoapException.ServerFaultCode, "BBBroker",
                       GetSoapExceptionDesc("No registration capabilities possible."));
            }
            return ret;

        }

        [WebMethod(Description = "Register a BBCinema and it's webservice endpoint.")]
        public void RegisterCinema(string name, string url)
        {
            try
            {
                Server.AddCinema(name, url);
            }
            catch (SocketException)
            {
                throw new SoapException("Cinema Registry Server down",
                       SoapException.ServerFaultCode, "BBBroker",
                       GetSoapExceptionDesc("No registration capabilities possible."));
            }
        }

        [WebMethod(Description = "Unregisters a BBCinema with the given name.")]
        public void UnregisterCinema(string name)
        {
            try
            {
                Server.RemoveCinema(name);
            }
            catch (SocketException)
            {
                throw new SoapException("Cinema Registry Server down",
                       SoapException.ServerFaultCode, "BBBroker",
                       GetSoapExceptionDesc("No registration capabilities possible."));
            }
        }

        private XmlNode GetSoapExceptionDesc(string content)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode node = doc.CreateNode(XmlNodeType.Element, SoapException.DetailElementName.Name, SoapException.DetailElementName.Namespace);
            XmlNode child = doc.CreateNode(XmlNodeType.Element, "error", "http://sd.deetc.isel.pt/");
            child.InnerText = "BBBrokerService:" + content;
            node.AppendChild(child);
            return node;
        }
    }
}
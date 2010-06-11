using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Linq;
using System.Threading;
using Entities;

namespace BrokerRegistryServer
{
    public sealed class Server : MarshalByRefObject, IBrokerRegistryServer
    {
        static Object _monitor = new Object();
        static readonly string _source = ConfigurationSettings.AppSettings["datasource"];

        public Server() { }

        #region IBrokerRegistryServer Members

        public void AddCinema(string name, string url)
        {
            lock (_monitor)
            {
                XDocument doc = XDocument.Load(_source, LoadOptions.None);
                doc.Root.Add(new XElement("cinema",
                    new XElement[] {
                    new XElement("name", name),new XElement("url", url) 
                }
                ));
                doc.Save(_source, SaveOptions.None);
            }
        }

        public void RemoveCinema(string name)
        {
            lock (_monitor)
            {
                XDocument doc = XDocument.Load(_source, LoadOptions.None);
                if (doc.Root.HasElements)
                {
                    doc.Root.Descendants("cinema").Where(e =>
                        e.Descendants("name").First().Value.Equals(name)
                    ).Remove();
                    doc.Save(_source, SaveOptions.None);
                }
            }
        }

        public Dictionary<string, string> GetCinemas()
        {
            Dictionary<string, string> lista = new Dictionary<string, string>();
            lock (_monitor)
            {
                XDocument doc = XDocument.Load(_source, LoadOptions.None);
                if (doc.Root.HasElements)
                {
                    doc.Root.Descendants("cinema").ToList().ForEach(
                        delegate(XElement c)
                        {
                            lista.Add(c.Descendants("name").First().Value,
                                c.Descendants("url").First().Value);
                        }
                    );
                }
            }
            return lista;
        }

        #endregion
    }
}

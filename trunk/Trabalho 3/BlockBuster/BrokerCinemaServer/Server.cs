using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;
using System.Configuration;
using System.Xml.Linq;

namespace BrokerCinemaServer
{
    public class Server : MarshalByRefObject, IBrokerCinemaServer
    {

        static readonly string _source = ConfigurationSettings.AppSettings["datasource"];

        public Server() { }

        #region IBrokerCinemaServer Members

        public void AddCinema(string name, string url)
        {
            XDocument doc = XDocument.Load(_source, LoadOptions.None);
            doc.Root.Add(new XElement("cinema",
                new XElement[] {
                    new XElement("name", name),new XElement("url", url) 
                }
            ));
            doc.Save(_source, SaveOptions.None);
        }

        public void RemoveCinema(string name)
        {
            XDocument doc = XDocument.Load(_source, LoadOptions.None);
            doc.Root.Descendants().Where(e =>
                e.Element("name").Value.Equals(name)
            ).Remove();
            doc.Save(_source, SaveOptions.None);
        }

        public Dictionary<string, string> GetCinemas()
        {
            XDocument doc = XDocument.Load(_source, LoadOptions.None);

            Dictionary<string, string> lista = new Dictionary<string, string>();

            doc.Root.Descendants().ToList().ForEach(
                delegate(XElement c)
                {
                    lista.Add(c.Attribute("name").Value, 
                        c.Attribute("url").Value);
                }
            );
            return lista;
        }

        #endregion
    }
}

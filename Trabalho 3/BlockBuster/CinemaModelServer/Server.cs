using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Interfaces;
using System.Configuration;
using System.Xml.Linq;

namespace CinemaModelServer 
{
    public class Server : MarshalByRefObject, ICinemaModelServer
    {

        static readonly string _source = ConfigurationSettings.AppSettings["datasource"];

        #region ICinemaModelServer Members

        public Guid AddReservation(string name, string sessionId, int seats)
        {
            XDocument doc = XDocument.Load(_source, LoadOptions.None);
            Guid guid = new Guid();
            doc.Root.Add(BuildReservation(name, sessionId, seats, guid));
            doc.Save(_source);
            return guid;
        }

        public bool RemoveReservation(Guid code)
        {
            XDocument doc = XDocument.Load(_source, LoadOptions.None);
            foreach (XElement e in doc.Root.Descendants()){
                if (e.Attribute("code").Value.Equals(code.ToString()))
                {
                    e.Remove();
                    doc.Save(_source);
                    return true;
                }
            }
            return false;
        }

        #endregion

        private XElement BuildReservation(string name, string sessionId, int seats, Guid guid)
        {
            XElement ret = new XElement("reservation");
            ret.SetAttributeValue("code", guid);
            ret.SetAttributeValue("name", name);
            ret.SetAttributeValue("sessionId", sessionId);
            ret.SetAttributeValue("seats", seats);
            return ret;
        }

    }
}

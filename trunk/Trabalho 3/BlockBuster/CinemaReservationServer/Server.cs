using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Configuration;
using System.Xml.Linq;
using Entities;

namespace CinemaReservationServer
{
    public sealed class Server : MarshalByRefObject, ICinemaReservationServer
    {
        static Object _monitor = new Object();
        static readonly string _source = ConfigurationSettings.AppSettings["dataSource"];

        public Server() { }

        XElement BuildReservation(string name, string sessionId, int seats, Guid guid)
        {
            XElement ret = new XElement("reservation");
            ret.SetAttributeValue("code", guid);
            ret.SetAttributeValue("name", name);
            ret.SetAttributeValue("sessionId", sessionId);
            ret.SetAttributeValue("seats", seats);
            return ret;
        }

        #region ICinemaModelServer Members

        public Guid AddReservation(string name, string sessionId, int seats)
        {
            Guid guid = Guid.NewGuid();
            lock (_monitor)
            {
                XDocument doc = XDocument.Load(_source, LoadOptions.None);
                doc.Root.Add(BuildReservation(name, sessionId, seats, guid));
                doc.Save(_source);
            }
            return guid;
        }

        public bool RemoveReservation(Guid code)
        {
            lock (_monitor)
            {
                XDocument doc = XDocument.Load(_source, LoadOptions.None);

                var res = doc.Root.Descendants().Where(e =>
                    e.Attribute("code").Value.Equals(code.ToString())
                );
                if (res.ToList().Count > 0)
                {
                    res.Remove();
                    doc.Save(_source);
                    return true;
                }
                return false;
            }
        }

        public int GetTotalReservations(string sessionId)
        {
            int total = 0;
            lock (_monitor)
            {
                XDocument doc = XDocument.Load(_source, LoadOptions.None);
                foreach (XElement e in doc.Root.Descendants().Where
                            (e => e.Attribute("sessionId").Value.Equals(sessionId))
                        )
                    total += Int32.Parse(e.Attribute("seats").Value);
            }
            return total;
        }

        #endregion
    }
}

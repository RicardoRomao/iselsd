using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrokerModel;
using System.Xml.Linq;
using System.Configuration;

namespace BrokerDAL
{
	public class BrokerContext
	{
		private static readonly BrokerContext _instance = new BrokerContext();

		private BrokerContext()
		{
		}

		public static BrokerContext GetInstance()
		{
			return _instance;
		}

		public void AddCinema(CinemaSvc cinema)
		{
			string ds = ConfigurationSettings.AppSettings["datasource"];
			XDocument doc = XDocument.Load(ds, LoadOptions.None);
			XElement root = doc.Root;
			root.Add(new XElement("cinema", new XElement[]{new XElement("name", cinema.Name), new XElement("url", cinema.Url)}));
			doc.Save(ds, SaveOptions.None);
		}

		public void RemoveCinema(string name)
		{
			string ds = ConfigurationSettings.AppSettings["datasource"];
			XDocument doc = XDocument.Load(ds, LoadOptions.None);
			XElement root = doc.Root;
			var elem = (from e in root.Elements()
					   where e.Element("name").Value.Equals(name)
					   select e).FirstOrDefault();
			if (elem == null)
			{
				return;
			}
			elem.Remove();
			doc.Save(ds, SaveOptions.None);
		}

		public CinemaSvc[] GetCinemas()
		{
			string ds = ConfigurationSettings.AppSettings["datasource"];
			XDocument doc = XDocument.Load(ds, LoadOptions.None);
			XElement root = doc.Root;

			var cinemas = (from c in root.Elements()
						  select c).ToList();

			CinemaSvc[] lista = new CinemaSvc[cinemas.Count];
			int idx = 0;
			cinemas.ForEach(delegate(XElement cinema) {
				CinemaSvc c = new CinemaSvc();
				c.Name = cinema.Element("name").Value;
				c.Url = cinema.Element("url").Value;
				lista[idx++] = c;
				//lista[lista.Count(cn => cn != null)] = c;
			});

			return lista;
		}
	}
}
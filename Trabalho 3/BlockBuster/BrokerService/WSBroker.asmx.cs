using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BrokerModel;
using BrokerDAL;

namespace BrokerService
{
	[WebService(Namespace = "http://sd.deetc.isel.pt/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	public class WSBroker : WebService
	{
		[WebMethod()]
		public CinemaSvc[] GetCinemas()
		{
			return BrokerContext.GetInstance().GetCinemas();
		}

		[WebMethod()]
		public void RegisterCinema(string name, string url)
		{
			CinemaSvc cinema = new CinemaSvc();
			cinema.Name = name;
			cinema.Url = url;

			BrokerContext.GetInstance().AddCinema(cinema);
		}

		[WebMethod()]
		public void UnregisterCinema(string name)
		{
			BrokerContext.GetInstance().RemoveCinema(name);
		}
	}
}
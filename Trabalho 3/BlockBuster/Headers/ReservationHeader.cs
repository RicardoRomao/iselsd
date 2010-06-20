using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;

namespace Headers
{
	public class ReservationHeader : SoapHeader
	{
		public int Days;
	}
}
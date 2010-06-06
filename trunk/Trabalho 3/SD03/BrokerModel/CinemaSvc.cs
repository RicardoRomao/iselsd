using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrokerModel
{
	public class CinemaSvc
	{
		private string _name;
		private string _url;

		public CinemaSvc()
		{
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public string Url
		{
			get
			{
				return _url;
			}
			set
			{
				_url = value;
			}
		}
	}
}
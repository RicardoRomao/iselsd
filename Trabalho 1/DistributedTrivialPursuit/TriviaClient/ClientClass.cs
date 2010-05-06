using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using Proxy;

namespace TriviaClient
{
	public delegate void ErrorHandler(String message);

	public class ClientClass
	{
		public event ErrorHandler OnError;
		public event QuestionHandler OnQuestionAnswered;

		private readonly object monitor = new object();
		private readonly IZoneServer _server;
		private readonly String _config;
		private List<Expert> _myExperts;
		private Dictionary<String, List<IExpert>> _ringExperts;
		private int _nrQuestions;

		public ClientClass(String config)
		{
			_config = config;
			RemotingConfiguration.Configure(_config, false);
			WellKnownClientTypeEntry[] entries = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
			_server = (IZoneServer)Activator.GetObject(entries[0].ObjectType, entries[0].ObjectUrl);
		}

		public void AddExpert(String theme)
		{
			lock (monitor)
			{
				if (_myExperts == null)
				{
					_myExperts = new List<Expert>();
				}
				Expert newExpert = new Expert(theme);
				newExpert.OnQuestionAnswered += OnExpertQuestionAnswered;
				_myExperts.Add(newExpert);
			}
		}

		private void OnExpertQuestionAnswered(IExpert sender, string answer)
		{
			if (OnQuestionAnswered != null)
			{
				OnQuestionAnswered(sender, answer);
			}
		}

		public void RegisterAll()
		{
			lock (monitor)
			{
			}
		}
	}
}
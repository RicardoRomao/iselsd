using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using Proxy;
using System.Runtime.Remoting.Messaging;
using System.Net.Sockets;
using System.Runtime.Remoting.Lifetime;

namespace TriviaClient
{
	public delegate void ErrorHandler(String message);
	public delegate void ThemeHandler(String theme);
	public delegate void ResponseHandler(Int32 questionNumber, String answer);

	public class ClientClass
	{
		public event ErrorHandler OnError;
		public event QuestionHandler OnQuestionAnswered;
		public event ThemeHandler OnExpertsGetComplete;
		public event ResponseHandler OnAnswerReceived;

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
			_nrQuestions = 0;
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

		private void OnRegisterComplete(IAsyncResult resp)
		{
			AsyncResult res = (AsyncResult)resp;
			Action<String, IExpert> registerAction = (Action<String, IExpert>)res.AsyncDelegate;
			try
			{
				registerAction.EndInvoke(resp);
			}
			catch (SocketException ex)
			{
				if (OnError != null)
				{
					OnError(String.Format("Register failed for theme {0}.", (String)resp.AsyncState));
				}
			}
		}

		public void RegisterAll()
		{
			lock (monitor)
			{
				// Begin registering the experts
				Action<String, IExpert> registerAction = new Action<string, IExpert>(_server.Register);
				foreach (Expert expert in _myExperts)
				{
					registerAction.BeginInvoke(expert.Theme, expert, OnRegisterComplete, expert.Theme);
				}
			}
		}

		private void OnExpertsGet(IAsyncResult resp)
		{
			String theme = null;
			AsyncResult res = (AsyncResult)resp;
			Func<String, List<IExpert>> del = (Func<String, List<IExpert>>)res.AsyncDelegate;
			try
			{
				List<IExpert> peritos = del.EndInvoke(resp);
				if (peritos != null && peritos.Count > 0)
				{
					lock (monitor)
					{
						if (_ringExperts == null)
						{
							_ringExperts = new Dictionary<string, List<IExpert>>();
						}
						_ringExperts.Add((String)resp.AsyncState, peritos);
					}
					theme = (String)resp.AsyncState;
				}
			}
			catch (SocketException ex)
			{
				if (OnError != null)
				{
					OnError("Can't get experts");
				}
			}
			finally
			{
				if (OnExpertsGetComplete != null)
				{
					OnExpertsGetComplete(theme);
				}
			}
		}

		public void GetExperts(String theme)
		{
			Func<String, List<IExpert>> getThemeExperts = new Func<string, List<IExpert>>(_server.getExpertList);
			getThemeExperts.BeginInvoke(theme, OnExpertsGet, theme);
		}

		private void OnAskEnd(IAsyncResult resp)
		{
			AsyncResult res = (AsyncResult)resp;
			Func<List<String>, String> askQuestion = (Func<List<String>, String>)res.AsyncDelegate;
			QuestionState state = null;
			try
			{
				String answer = askQuestion.EndInvoke(resp);
				state = (QuestionState)resp.AsyncState;
				if (OnAnswerReceived != null)
				{
					OnAnswerReceived(int.Parse(state.Index), answer);
				}
			}
			catch (SocketException ex)
			{
				if (OnError != null)
				{
					OnError("Can't contact expert");
				}
				new Action<String, IExpert>(_server.NotifyClientFault).BeginInvoke(state.Theme, state.Expert, OnNotifyComplete, null);
			}
		}

		private void OnNotifyComplete(IAsyncResult resp)
		{
			AsyncResult res = (AsyncResult)resp;
			Action<String, IExpert> notifyCall = (Action<String, IExpert>)res.AsyncDelegate;
			try
			{
				notifyCall.EndInvoke(resp);
			}
			catch (SocketException ex)
			{
				if (OnError != null)
				{
					OnError("Can't notify zone server");
				}
			}
		}

		public void Ask(String theme, List<String> keywords)
		{
			List<IExpert> geniuses = null;
			lock (monitor)
			{
				geniuses = _ringExperts[theme];
				_nrQuestions++;
			}
			foreach (IExpert perito in geniuses)
			{
				Func<List<String>, String> askQuestion = new Func<List<String>, String>(perito.Ask);
				askQuestion.BeginInvoke(keywords, OnAskEnd, new QuestionState() { Index = _nrQuestions.ToString(), Theme = theme, Expert = perito });
			}
		}

		private void OnUnregisterComplete(IAsyncResult resp)
		{
			AsyncResult res = (AsyncResult)resp;
			Action<String, IExpert> unregisterAction = (Action<String, IExpert>)res.AsyncDelegate;
			try
			{
				unregisterAction.EndInvoke(resp);
			}
			catch (SocketException ex)
			{
				if (OnError != null)
				{
					OnError(String.Format("Unregister failed for theme {0}.", (String)resp.AsyncState));
				}
			}
		}

		public void UnregisterAll()
		{
			lock (monitor)
			{
				foreach (Expert expert in _myExperts)
				{
					Action<String, IExpert> unregister = new Action<string, IExpert>(_server.UnRegister);
					unregister.BeginInvoke(expert.Theme, expert, OnUnregisterComplete, null);
				}
				_myExperts.Clear();
			}
		}
	}
}
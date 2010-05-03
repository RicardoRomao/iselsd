using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting;
using Proxy;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;

namespace TriviaClient
{
	public partial class TriviaClientForm : Form
	{
		private IZoneServer _server;
		private List<Expert> _myExperts;
		private Dictionary<String, List<IExpert>> experts;
		private int _nrQuestions;

		public TriviaClientForm()
		{
			InitializeComponent();
			InitRemote();
		}

		private void InitRemote()
		{
			RemotingConfiguration.Configure("TriviaClient.exe.config", false);
			WellKnownClientTypeEntry[] entries = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
			_server = (IZoneServer)Activator.GetObject(entries[0].ObjectType, entries[0].ObjectUrl);
			experts = new Dictionary<string, List<IExpert>>();
			_myExperts = new List<Expert>();
			Expert exp = new Expert("Desporto");
			exp.OnQuestionAnswered += OnExpertQuestionAnswered;
			_myExperts.Add(exp);
			exp = new Expert("Tecnologia");
			exp.OnQuestionAnswered += OnExpertQuestionAnswered;
			_myExperts.Add(exp);

			// Begin registering the experts
			Action<String, IExpert> registerAction = new Action<string,IExpert>(_server.Register);
			for (int i = 0; i < _myExperts.Count; i++)
			{
				// É enviado o indice do expert para no caso de registar-mos muitos experts
				// o utilizador apenas ser informado no callback, em caso de falha, no ultimo expert
				registerAction.BeginInvoke(_myExperts[i].Theme, _myExperts[i], OnRegisterComplete, (i+1));
			}
		}

		private void OnExpertQuestionAnswered(IExpert sender, String answer)
		{
			rtbAnswers.AppendText(answer);
		}

		private void OnRegisterComplete(IAsyncResult resp) {
			AsyncResult res = (AsyncResult)resp;
			Action<String, IExpert> registerAction = (Action<String, IExpert>)res.AsyncDelegate;
			try
			{
				registerAction.EndInvoke(resp);
			}
			catch (SocketException ex)
			{
				if ((int)resp.AsyncState == _myExperts.Count)
				{
					MessageBox.Show("Zone Server Unavailable\nRegister Failed", "Trivia Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}

		private void OnExpertsReceived(IAsyncResult resp)
		{
			AsyncResult res = (AsyncResult)resp;
			Func<String, List<IExpert>> del = (Func<String, List<IExpert>>)res.AsyncDelegate;
			try
			{
				List<IExpert> peritos = del.EndInvoke(resp);
				experts.Add((String)resp.AsyncState, peritos);
				// Está escrito na pedra, actualizações nos controlos apenas
				// na thread criadora
				this.Invoke(new MethodInvoker(delegate() {
					txtQuestion.Enabled = true;
					btnAsk.Enabled = true;
					if (!lstThemes.Items.Contains(resp.AsyncState))
						lstThemes.Items.Add((String)resp.AsyncState);
				}));
			}
			catch (SocketException ex)
			{
				MessageBox.Show("Zone Server Unavailable\nCan't get experts", "Trivia Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void OnQuestionAnswered(IAsyncResult resp)
		{
			AsyncResult res = (AsyncResult)resp;
			Func<List<String>, String> askQuestion = (Func<List<String>, String>)res.AsyncDelegate;
			QuestionState state = null;
			try
			{
				String answer = askQuestion.EndInvoke(resp);
				state = (QuestionState)resp.AsyncState;
				this.Invoke(new MethodInvoker(delegate() {
					rtbQuestions.AppendText(String.Format("{0}: {1}\n", state.Index, answer));
				}));
			}
			catch (SocketException ex)
			{
				new Action<String, IExpert>(_server.NotifyClientFault).BeginInvoke(state.Theme, state.Expert, null, null);
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
				MessageBox.Show("Zone Server Unavailable\nCan't notify zone server", "Trivia Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void btnGetTheme_Click(object sender, EventArgs e)
		{
			Func<String, List<IExpert>> getThemeExperts = new Func<string, List<IExpert>>(_server.getExpertList);
			getThemeExperts.BeginInvoke(txtTheme.Text, OnExpertsReceived, txtTheme.Text);
		}

		private void btnAsk_Click(object sender, EventArgs e)
		{
			List<IExpert> geniuses = experts[lstThemes.SelectedItem.ToString()];
			_nrQuestions++;
			rtbQuestions.AppendText(String.Format("Question {0}: {1}", _nrQuestions.ToString(), txtQuestion.Text));
			List<String> keywords = new List<String>(txtQuestion.Text.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries));
			foreach (IExpert perito in geniuses)
			{
				Func<List<String>, String> askQuestion = new Func<List<String>, String>(perito.Ask);
				askQuestion.BeginInvoke(keywords, OnQuestionAnswered, new QuestionState() { Index = _nrQuestions.ToString(), Theme = lstThemes.SelectedItem.ToString(), Expert = perito });
			}
		}
	}
}
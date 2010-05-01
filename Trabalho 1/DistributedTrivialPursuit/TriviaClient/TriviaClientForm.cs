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
			_myExperts.Add(exp);
			exp = new Expert("Tecnologia");
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

		private void btnGetTheme_Click(object sender, EventArgs e)
		{
			Func<String, List<IExpert>> getThemeExperts = new Func<string, List<IExpert>>(_server.getExpertList);
			getThemeExperts.BeginInvoke(txtTheme.Text, OnExpertsReceived, txtTheme.Text);
		}
	}
}
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
			try
			{
				Expert exp = new Expert("Desporto");
				_myExperts.Add(exp);
				_server.Register("Desporto", exp);
				exp = new Expert("Tecnologia");
				_myExperts.Add(exp);
				_server.Register("Tecnologia", exp);
			}
			catch (SocketException ex)
			{
				MessageBox.Show("Zone Server Unavailable", "Trivia Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
				_myExperts.Clear();
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
			}
			catch (Exception ex)
			{
				MessageBox.Show("Zone Server Unavailable", "Trivia Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void btnGetTheme_Click(object sender, EventArgs e)
		{
			Func<String, List<IExpert>> getThemeExperts = new Func<string, List<IExpert>>(_server.getExpertList);
			getThemeExperts.BeginInvoke(txtTheme.Text, OnExpertsReceived, txtTheme.Text);
		}
	}
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Configuration;
using Proxy;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using TriviaModel;

namespace TriviaClient
{
	public partial class TriviaClientForm : Form
	{
		private Client _client;

		public TriviaClientForm()
		{
			InitializeComponent();
			InitRemote();
		}

		private void InitRemote()
		{
			_client = new Client();
			_client.OnError += OnError;
			_client.OnExpertsGetComplete += OnExpertsReceived;
			_client.OnAnswerReceived += OnQuestionAnswered;
			
            IRepository xrep = XMLRepository.GetInstance(ConfigurationManager.AppSettings["DataSource"]);
			List<String> themes = xrep.GetThemes();
			
            foreach (string theme in themes)
			{
				_client.AddExpert(theme);
			}
			
            _client.OnQuestionAnswered += OnExpertQuestionAnswered;
			_client.RegisterAll();
		}

		private void OnError(String message)
		{
			MessageBox.Show(message, "Trivia Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void OnExpertQuestionAnswered(IExpert sender, String answer)
		{
			this.Invoke(new MethodInvoker(delegate()
			{
				rtbAnswers.AppendText(String.Format("{0}\n", answer));
			}));
		}

		private void OnExpertsReceived(String theme)
		{
			this.Invoke(new MethodInvoker(delegate()
			{
				txtTheme.Text = "";
				txtTheme.Enabled = true;
				btnGetTheme.Enabled = true;
				if (String.IsNullOrEmpty(theme))
				{
					MessageBox.Show(String.Format("Experts unavailable for theme {0}", theme), "Trivia Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					txtQuestion.Enabled = true;
					btnAsk.Enabled = true;
					if (!lstThemes.Items.Contains(theme))
						lstThemes.Items.Add(theme);
				}
			}));
		}

		private void OnQuestionAnswered(int questionNumber, String answer)
		{
			this.Invoke(new MethodInvoker(delegate()
			{
				rtbQuestions.AppendText(String.Format("{0}: {1}\n", questionNumber, answer));
			}));
		}

		private void btnGetTheme_Click(object sender, EventArgs e)
		{
			btnGetTheme.Enabled = false;
			txtTheme.Enabled = false;
			_client.GetExperts(txtTheme.Text);
		}

		private void btnAsk_Click(object sender, EventArgs e)
		{
            if (lstThemes.SelectedItems.Count > 0)
            {
                rtbQuestions.AppendText(String.Format("Question {0}\n", txtQuestion.Text));
                List<String> keywords = new List<String>(txtQuestion.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                _client.Ask(lstThemes.SelectedItem.ToString(), keywords);
                txtQuestion.Text = "";
            }
		}

		private void TriviaClientForm_OnClosing(object sender, FormClosingEventArgs e)
		{
			_client.UnregisterAll();
		}
	}
}
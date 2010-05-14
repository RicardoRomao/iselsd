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

namespace TriviaExpert
{
    public partial class TriviaClientForm : Form
    {
        private IClient _client;

        public TriviaClientForm()
        {
            InitializeComponent();
        }

        private void Connect()
        {
            _client = new Client();
            _client.OnError += OnError;
            _client.OnExpertsGetComplete += OnExpertsReceived;
            _client.OnAnswerReceived += OnQuestionAnswered;
            try
            {
                _client.Connect();
                RegisterThemes();
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                btnAsk.Enabled = true;
                btnGetTheme.Enabled = true;
                lblServerURL.Text = "Connected to: " + _client.GetServerUrl();
            }
            catch (SocketException)
            {
                OnError("Connection to server was not possible.");
                _client = null;
            }
        }

        private void Disconnect()
        {
            if (_client != null && _client.IsConnected())
            {
                _client.UnregisterAll();
                _client.Disconnect();
            }

            lstThemes.Items.Clear();
            rtbQuestions.Clear();
            rtbAnswers.Clear();

            btnAsk.Enabled = false;
            btnGetTheme.Enabled = false;
            btnDisconnect.Enabled = false;
            btnConnect.Enabled = true;

            lblServerURL.Text = "Not connected...";
        }

        private void RegisterThemes()
        {
            IRepository xrep = XMLRepository.GetInstance(ConfigurationManager.AppSettings["DataSource"]);
            List<String> themes = xrep.GetThemes();
            foreach (string theme in themes)
            {
                _client.AddLocalExpert(theme);
            }
            _client.OnQuestionAnswered += OnExpertQuestionAnswered;
            _client.RegisterAll();
        }

        private void OnError(String message)
        {
            MessageBox.Show(
                message,
                "Trivia Client",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void OnExpertQuestionAnswered(IExpert sender, String keyWords, String answer)
        {
            this.Invoke(new MethodInvoker(delegate()
            {
                rtbAnswers.AppendText(String.Format("{0}\n" +
                                                    "Keywords:{1}\n" +
                                                    "Answer:{2}\n",
                                                    sender.GetTheme(),
                                                    keyWords,
                                                    answer)
                );
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
                        OnError(String.Format
                            ("Experts unavailable for theme {0}", theme)
                        );
                    }
                    else
                    {
                        txtQuestion.Enabled = true;
                        btnAsk.Enabled = true;
                        if (!lstThemes.Items.Contains(theme))
                            lstThemes.Items.Add(theme);
                    }
                }
            ));
        }

        private void OnQuestionAnswered(int questionNumber, String answer)
        {
            this.Invoke(new MethodInvoker(delegate()
                {
                    rtbQuestions.AppendText(
                        String.Format("{0}: {1}\n", questionNumber, answer));
                }
            ));
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
                rtbQuestions.AppendText(
                    String.Format("Question {0}: {1}\n",
                        _client.GetQuestionCount() + 1, txtQuestion.Text)
                );
                List<String> keywords = new List<String>(
                    txtQuestion.Text.Split(
                        new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries
                    )
                );
                _client.Ask(lstThemes.SelectedItem.ToString(), keywords);
                txtQuestion.Text = "";
            }
            else
                OnError("Target theme must be selected.");
        }

        private void TriviaClientForm_OnClosing(object sender, FormClosingEventArgs e)
        { Disconnect(); }

        private void btnConnect_Click(object sender, EventArgs e)
        { Connect(); }

        private void btnDisconnect_Click(object sender, EventArgs e)
        { Disconnect(); }
    }
}
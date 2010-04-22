using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TriviaClient
{
    public partial class TriviaForm : Form
    {
        //delegate to receive the answeres given by the client experts
        private delegate void updExpertsTextBox(String msg);
        //delegate to receive the answeres given to the client
        private delegate void updClientTextBox(String msg);

        private readonly Client _client = new Client();

        public TriviaForm()
        {
            InitializeComponent();
            Init();
            
        }

        private void Init()
        {
            //Registering Client Events
            _client.OnQuestionAnswered += WriteAnswerToClient;
            _client.OnQuestionAnsweredByExpert += WriteAnswerByExpert;
        }

        //Callback to update answeres given by client Experts
        public void WriteAnswerByExpert(Object source, EventArgs e)
        {            
            //Need to be changed
            if (this.InvokeRequired)
            {
                updExpertsTextBox utb = new updExpertsTextBox(updExpertAnswer);

                utb.BeginInvoke(e.ToString(), null, null);
            }
            else
            {
                this.rtbExpertAnswer.AppendText(e.ToString());
            }
        }
        //Method to update the answeres given by client -- Thread Confinement
        private void updExpertAnswer(String msg)
        {
            this.rtbExpertAnswer.AppendText(msg);
        }

        //CallBack to update answeres given to the client
        public void WriteAnswerToClient(Object source, EventArgs e)
        {
            //Need to be changed
            if (this.InvokeRequired)
            {
                updClientTextBox utb = new updClientTextBox(updAnswerToClient);
                utb.BeginInvoke(e.ToString(), null, null);
            }
            else
            {
                this.rtbClientQuestions.AppendText(e.ToString());
            }
        }
        //Method to update the answeres given to the client -- Thread Confinement
        private void updAnswerToClient(String msg)
        {
            this.rtbClientQuestions.AppendText(msg);
        }
        
    }
}

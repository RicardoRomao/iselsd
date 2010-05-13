﻿namespace TriviaExpert
{
	partial class TriviaClientForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.rtbQuestions = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rtbAnswers = new System.Windows.Forms.RichTextBox();
            this.txtTheme = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGetTheme = new System.Windows.Forms.Button();
            this.txtQuestion = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnAsk = new System.Windows.Forms.Button();
            this.lstThemes = new System.Windows.Forms.ListBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.lblServerURL = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "My Questions";
            // 
            // rtbQuestions
            // 
            this.rtbQuestions.Location = new System.Drawing.Point(12, 47);
            this.rtbQuestions.Name = "rtbQuestions";
            this.rtbQuestions.ReadOnly = true;
            this.rtbQuestions.Size = new System.Drawing.Size(454, 122);
            this.rtbQuestions.TabIndex = 1;
            this.rtbQuestions.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "My Experts Answers";
            // 
            // rtbAnswers
            // 
            this.rtbAnswers.Location = new System.Drawing.Point(12, 197);
            this.rtbAnswers.Name = "rtbAnswers";
            this.rtbAnswers.ReadOnly = true;
            this.rtbAnswers.Size = new System.Drawing.Size(454, 122);
            this.rtbAnswers.TabIndex = 3;
            this.rtbAnswers.Text = "";
            // 
            // txtTheme
            // 
            this.txtTheme.Location = new System.Drawing.Point(12, 340);
            this.txtTheme.Name = "txtTheme";
            this.txtTheme.Size = new System.Drawing.Size(373, 20);
            this.txtTheme.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 324);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Theme";
            // 
            // btnGetTheme
            // 
            this.btnGetTheme.Enabled = false;
            this.btnGetTheme.Location = new System.Drawing.Point(391, 340);
            this.btnGetTheme.Name = "btnGetTheme";
            this.btnGetTheme.Size = new System.Drawing.Size(75, 23);
            this.btnGetTheme.TabIndex = 6;
            this.btnGetTheme.Text = "Get";
            this.btnGetTheme.UseVisualStyleBackColor = true;
            this.btnGetTheme.Click += new System.EventHandler(this.btnGetTheme_Click);
            // 
            // txtQuestion
            // 
            this.txtQuestion.Enabled = false;
            this.txtQuestion.Location = new System.Drawing.Point(12, 442);
            this.txtQuestion.Name = "txtQuestion";
            this.txtQuestion.Size = new System.Drawing.Size(373, 20);
            this.txtQuestion.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 426);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Question";
            // 
            // btnAsk
            // 
            this.btnAsk.Enabled = false;
            this.btnAsk.Location = new System.Drawing.Point(391, 440);
            this.btnAsk.Name = "btnAsk";
            this.btnAsk.Size = new System.Drawing.Size(75, 23);
            this.btnAsk.TabIndex = 9;
            this.btnAsk.Text = "Ask";
            this.btnAsk.UseVisualStyleBackColor = true;
            this.btnAsk.Click += new System.EventHandler(this.btnAsk_Click);
            // 
            // lstThemes
            // 
            this.lstThemes.FormattingEnabled = true;
            this.lstThemes.Location = new System.Drawing.Point(12, 366);
            this.lstThemes.Name = "lstThemes";
            this.lstThemes.Size = new System.Drawing.Size(454, 56);
            this.lstThemes.TabIndex = 10;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(391, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 11;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Location = new System.Drawing.Point(310, 12);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 12;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // lblServerURL
            // 
            this.lblServerURL.AutoSize = true;
            this.lblServerURL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblServerURL.Location = new System.Drawing.Point(12, 468);
            this.lblServerURL.Name = "lblServerURL";
            this.lblServerURL.Size = new System.Drawing.Size(87, 13);
            this.lblServerURL.TabIndex = 13;
            this.lblServerURL.Text = "Not connected...";
            // 
            // TriviaClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 490);
            this.Controls.Add(this.lblServerURL);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.lstThemes);
            this.Controls.Add(this.btnAsk);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtQuestion);
            this.Controls.Add(this.btnGetTheme);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTheme);
            this.Controls.Add(this.rtbAnswers);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rtbQuestions);
            this.Controls.Add(this.label1);
            this.Name = "TriviaClientForm";
            this.Text = "TriviaClientForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TriviaClientForm_OnClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RichTextBox rtbQuestions;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RichTextBox rtbAnswers;
		private System.Windows.Forms.TextBox txtTheme;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnGetTheme;
		private System.Windows.Forms.TextBox txtQuestion;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnAsk;
		private System.Windows.Forms.ListBox lstThemes;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label lblServerURL;
	}
}
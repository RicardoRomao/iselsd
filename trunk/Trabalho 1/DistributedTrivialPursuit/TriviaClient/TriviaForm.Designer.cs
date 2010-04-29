namespace TriviaClient
{
    partial class TriviaForm
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
			this.rtbClientQuestions = new System.Windows.Forms.RichTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtQuestion = new System.Windows.Forms.TextBox();
			this.btnAsk = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbTheme = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.rtbExpertAnswer = new System.Windows.Forms.RichTextBox();
			this.btnGetExperts = new System.Windows.Forms.Button();
			this.txtTheme = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// rtbClientQuestions
			// 
			this.rtbClientQuestions.Location = new System.Drawing.Point(12, 29);
			this.rtbClientQuestions.Name = "rtbClientQuestions";
			this.rtbClientQuestions.ReadOnly = true;
			this.rtbClientQuestions.Size = new System.Drawing.Size(252, 210);
			this.rtbClientQuestions.TabIndex = 0;
			this.rtbClientQuestions.Text = "";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(154, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Your Questions, Trivia Answers";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 295);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Your question";
			// 
			// txtQuestion
			// 
			this.txtQuestion.Location = new System.Drawing.Point(12, 311);
			this.txtQuestion.Multiline = true;
			this.txtQuestion.Name = "txtQuestion";
			this.txtQuestion.Size = new System.Drawing.Size(252, 50);
			this.txtQuestion.TabIndex = 3;
			// 
			// btnAsk
			// 
			this.btnAsk.Enabled = false;
			this.btnAsk.Location = new System.Drawing.Point(12, 367);
			this.btnAsk.Name = "btnAsk";
			this.btnAsk.Size = new System.Drawing.Size(252, 23);
			this.btnAsk.TabIndex = 4;
			this.btnAsk.Text = "Send Question";
			this.btnAsk.UseVisualStyleBackColor = true;
			this.btnAsk.Click += new System.EventHandler(this.btnAsk_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 256);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(84, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Choose a theme";
			// 
			// cmbTheme
			// 
			this.cmbTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTheme.FormattingEnabled = true;
			this.cmbTheme.Location = new System.Drawing.Point(270, 248);
			this.cmbTheme.Name = "cmbTheme";
			this.cmbTheme.Size = new System.Drawing.Size(23, 21);
			this.cmbTheme.TabIndex = 6;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(296, 13);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(178, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "Questions answered by your Experts";
			// 
			// rtbExpertAnswer
			// 
			this.rtbExpertAnswer.Location = new System.Drawing.Point(299, 29);
			this.rtbExpertAnswer.Name = "rtbExpertAnswer";
			this.rtbExpertAnswer.ReadOnly = true;
			this.rtbExpertAnswer.Size = new System.Drawing.Size(252, 361);
			this.rtbExpertAnswer.TabIndex = 8;
			this.rtbExpertAnswer.Text = "";
			// 
			// btnGetExperts
			// 
			this.btnGetExperts.Location = new System.Drawing.Point(189, 271);
			this.btnGetExperts.Name = "btnGetExperts";
			this.btnGetExperts.Size = new System.Drawing.Size(75, 23);
			this.btnGetExperts.TabIndex = 9;
			this.btnGetExperts.Text = "Get";
			this.btnGetExperts.UseVisualStyleBackColor = true;
			this.btnGetExperts.Click += new System.EventHandler(this.btnGetExperts_Click);
			// 
			// txtTheme
			// 
			this.txtTheme.Location = new System.Drawing.Point(12, 272);
			this.txtTheme.Name = "txtTheme";
			this.txtTheme.Size = new System.Drawing.Size(171, 20);
			this.txtTheme.TabIndex = 10;
			// 
			// TriviaForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(564, 404);
			this.Controls.Add(this.txtTheme);
			this.Controls.Add(this.btnGetExperts);
			this.Controls.Add(this.rtbExpertAnswer);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cmbTheme);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnAsk);
			this.Controls.Add(this.txtQuestion);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.rtbClientQuestions);
			this.Name = "TriviaForm";
			this.Text = "Trivial";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbClientQuestions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtQuestion;
        private System.Windows.Forms.Button btnAsk;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbTheme;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox rtbExpertAnswer;
		private System.Windows.Forms.Button btnGetExperts;
		private System.Windows.Forms.TextBox txtTheme;
    }
}
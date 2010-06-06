namespace CinemaApplication
{
	partial class MainForm
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
			this.ActionStatus = new System.Windows.Forms.StatusStrip();
			this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.SearchContainer = new System.Windows.Forms.GroupBox();
			this.NomeLabel = new System.Windows.Forms.Label();
			this.txtFilme = new System.Windows.Forms.TextBox();
			this.ActionStatus.SuspendLayout();
			this.SearchContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// ActionStatus
			// 
			this.ActionStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
			this.ActionStatus.Location = new System.Drawing.Point(0, 430);
			this.ActionStatus.Name = "ActionStatus";
			this.ActionStatus.Size = new System.Drawing.Size(634, 22);
			this.ActionStatus.SizingGrip = false;
			this.ActionStatus.TabIndex = 0;
			// 
			// StatusLabel
			// 
			this.StatusLabel.Name = "StatusLabel";
			this.StatusLabel.Size = new System.Drawing.Size(0, 17);
			// 
			// SearchContainer
			// 
			this.SearchContainer.Controls.Add(this.txtFilme);
			this.SearchContainer.Controls.Add(this.NomeLabel);
			this.SearchContainer.Location = new System.Drawing.Point(12, 12);
			this.SearchContainer.Name = "SearchContainer";
			this.SearchContainer.Size = new System.Drawing.Size(610, 160);
			this.SearchContainer.TabIndex = 1;
			this.SearchContainer.TabStop = false;
			this.SearchContainer.Text = "Pesquisas";
			// 
			// NomeLabel
			// 
			this.NomeLabel.AutoSize = true;
			this.NomeLabel.Location = new System.Drawing.Point(6, 22);
			this.NomeLabel.Name = "NomeLabel";
			this.NomeLabel.Size = new System.Drawing.Size(34, 13);
			this.NomeLabel.TabIndex = 0;
			this.NomeLabel.Text = "Filme:";
			// 
			// txtFilme
			// 
			this.txtFilme.Location = new System.Drawing.Point(46, 19);
			this.txtFilme.Name = "txtFilme";
			this.txtFilme.Size = new System.Drawing.Size(220, 20);
			this.txtFilme.TabIndex = 1;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(634, 452);
			this.Controls.Add(this.SearchContainer);
			this.Controls.Add(this.ActionStatus);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Cinemas Info";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ActionStatus.ResumeLayout(false);
			this.ActionStatus.PerformLayout();
			this.SearchContainer.ResumeLayout(false);
			this.SearchContainer.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip ActionStatus;
		private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
		private System.Windows.Forms.GroupBox SearchContainer;
		private System.Windows.Forms.TextBox txtFilme;
		private System.Windows.Forms.Label NomeLabel;
	}
}


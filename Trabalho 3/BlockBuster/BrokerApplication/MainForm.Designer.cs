namespace BrokerApplication
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
			this.lblNome = new System.Windows.Forms.Label();
			this.txtNome = new System.Windows.Forms.TextBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.txtUrl = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lstCinemas = new System.Windows.Forms.ListBox();
			this.lblCinemas = new System.Windows.Forms.Label();
			this.btnRemove = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblNome
			// 
			this.lblNome.AutoSize = true;
			this.lblNome.Location = new System.Drawing.Point(12, 9);
			this.lblNome.Name = "lblNome";
			this.lblNome.Size = new System.Drawing.Size(35, 13);
			this.lblNome.TabIndex = 0;
			this.lblNome.Text = "Nome";
			// 
			// txtNome
			// 
			this.txtNome.Location = new System.Drawing.Point(15, 25);
			this.txtNome.Name = "txtNome";
			this.txtNome.Size = new System.Drawing.Size(206, 20);
			this.txtNome.TabIndex = 1;
			// 
			// btnAdd
			// 
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(227, 23);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 2;
			this.btnAdd.Text = "Adicionar";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// txtUrl
			// 
			this.txtUrl.Location = new System.Drawing.Point(15, 64);
			this.txtUrl.Name = "txtUrl";
			this.txtUrl.Size = new System.Drawing.Size(206, 20);
			this.txtUrl.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(20, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Url";
			// 
			// lstCinemas
			// 
			this.lstCinemas.FormattingEnabled = true;
			this.lstCinemas.Location = new System.Drawing.Point(12, 131);
			this.lstCinemas.Name = "lstCinemas";
			this.lstCinemas.Size = new System.Drawing.Size(209, 69);
			this.lstCinemas.TabIndex = 5;
			// 
			// lblCinemas
			// 
			this.lblCinemas.AutoSize = true;
			this.lblCinemas.Location = new System.Drawing.Point(12, 115);
			this.lblCinemas.Name = "lblCinemas";
			this.lblCinemas.Size = new System.Drawing.Size(47, 13);
			this.lblCinemas.TabIndex = 6;
			this.lblCinemas.Text = "Cinemas";
			// 
			// btnRemove
			// 
			this.btnRemove.Enabled = false;
			this.btnRemove.Location = new System.Drawing.Point(227, 131);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(75, 23);
			this.btnRemove.TabIndex = 7;
			this.btnRemove.Text = "Remover";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(314, 212);
			this.Controls.Add(this.btnRemove);
			this.Controls.Add(this.lblCinemas);
			this.Controls.Add(this.lstCinemas);
			this.Controls.Add(this.txtUrl);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.txtNome);
			this.Controls.Add(this.lblNome);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Gestão de Cinemas";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblNome;
		private System.Windows.Forms.TextBox txtNome;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.TextBox txtUrl;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox lstCinemas;
		private System.Windows.Forms.Label lblCinemas;
		private System.Windows.Forms.Button btnRemove;
	}
}


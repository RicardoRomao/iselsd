namespace ClientFormsApplication
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
            this.dateStart = new System.Windows.Forms.DateTimePicker();
            this.dateEnd = new System.Windows.Forms.DateTimePicker();
            this.lblDateStart = new System.Windows.Forms.Label();
            this.lblDateEnd = new System.Windows.Forms.Label();
            this.btnSendPeriod = new System.Windows.Forms.Button();
            this.tabQuery = new System.Windows.Forms.TabControl();
            this.tabAll = new System.Windows.Forms.TabPage();
            this.btnSendAll = new System.Windows.Forms.Button();
            this.tabTitle = new System.Windows.Forms.TabPage();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.btnSendTitle = new System.Windows.Forms.Button();
            this.tabPeriod = new System.Windows.Forms.TabPage();
            this.treeMovies = new System.Windows.Forms.TreeView();
            this.lblMovies = new System.Windows.Forms.Label();
            this.btnAddReservation = new System.Windows.Forms.Button();
            this.treeReservations = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRemoveReservation = new System.Windows.Forms.Button();
            this.txtErrors = new System.Windows.Forms.RichTextBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.pnlResInfo = new System.Windows.Forms.Panel();
            this.lblResDetails = new System.Windows.Forms.Label();
            this.txtResDetails = new System.Windows.Forms.RichTextBox();
            this.btnResCancel = new System.Windows.Forms.Button();
            this.btnSendRes = new System.Windows.Forms.Button();
            this.lblResSeats = new System.Windows.Forms.Label();
            this.lblResName = new System.Windows.Forms.Label();
            this.lblResTit = new System.Windows.Forms.Label();
            this.txtResSeats = new System.Windows.Forms.TextBox();
            this.txtResName = new System.Windows.Forms.TextBox();
            this.txtMovieDesc = new System.Windows.Forms.RichTextBox();
            this.lblMovieDesc = new System.Windows.Forms.Label();
            this.tabQuery.SuspendLayout();
            this.tabAll.SuspendLayout();
            this.tabTitle.SuspendLayout();
            this.tabPeriod.SuspendLayout();
            this.pnlResInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateStart
            // 
            this.dateStart.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateStart.Location = new System.Drawing.Point(9, 34);
            this.dateStart.Name = "dateStart";
            this.dateStart.Size = new System.Drawing.Size(81, 20);
            this.dateStart.TabIndex = 0;
            // 
            // dateEnd
            // 
            this.dateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateEnd.Location = new System.Drawing.Point(110, 34);
            this.dateEnd.Name = "dateEnd";
            this.dateEnd.Size = new System.Drawing.Size(81, 20);
            this.dateEnd.TabIndex = 1;
            // 
            // lblDateStart
            // 
            this.lblDateStart.AutoSize = true;
            this.lblDateStart.Location = new System.Drawing.Point(6, 18);
            this.lblDateStart.Name = "lblDateStart";
            this.lblDateStart.Size = new System.Drawing.Size(69, 13);
            this.lblDateStart.TabIndex = 2;
            this.lblDateStart.Text = "Session Start";
            // 
            // lblDateEnd
            // 
            this.lblDateEnd.AutoSize = true;
            this.lblDateEnd.Location = new System.Drawing.Point(107, 18);
            this.lblDateEnd.Name = "lblDateEnd";
            this.lblDateEnd.Size = new System.Drawing.Size(66, 13);
            this.lblDateEnd.TabIndex = 3;
            this.lblDateEnd.Text = "Session End";
            // 
            // btnSendPeriod
            // 
            this.btnSendPeriod.Location = new System.Drawing.Point(208, 34);
            this.btnSendPeriod.Name = "btnSendPeriod";
            this.btnSendPeriod.Size = new System.Drawing.Size(75, 19);
            this.btnSendPeriod.TabIndex = 4;
            this.btnSendPeriod.Text = "Submit";
            this.btnSendPeriod.UseVisualStyleBackColor = true;
            this.btnSendPeriod.Click += new System.EventHandler(this.btnSendPeriod_Click);
            // 
            // tabQuery
            // 
            this.tabQuery.Controls.Add(this.tabAll);
            this.tabQuery.Controls.Add(this.tabTitle);
            this.tabQuery.Controls.Add(this.tabPeriod);
            this.tabQuery.Location = new System.Drawing.Point(170, 12);
            this.tabQuery.Name = "tabQuery";
            this.tabQuery.SelectedIndex = 0;
            this.tabQuery.Size = new System.Drawing.Size(304, 107);
            this.tabQuery.TabIndex = 5;
            // 
            // tabAll
            // 
            this.tabAll.Controls.Add(this.btnSendAll);
            this.tabAll.Location = new System.Drawing.Point(4, 22);
            this.tabAll.Name = "tabAll";
            this.tabAll.Padding = new System.Windows.Forms.Padding(3);
            this.tabAll.Size = new System.Drawing.Size(296, 81);
            this.tabAll.TabIndex = 0;
            this.tabAll.Text = "List All Movies";
            this.tabAll.UseVisualStyleBackColor = true;
            // 
            // btnSendAll
            // 
            this.btnSendAll.Location = new System.Drawing.Point(111, 32);
            this.btnSendAll.Name = "btnSendAll";
            this.btnSendAll.Size = new System.Drawing.Size(75, 19);
            this.btnSendAll.TabIndex = 0;
            this.btnSendAll.Text = "Submit";
            this.btnSendAll.UseVisualStyleBackColor = true;
            this.btnSendAll.Click += new System.EventHandler(this.btnSendAll_Click);
            // 
            // tabTitle
            // 
            this.tabTitle.Controls.Add(this.lblTitle);
            this.tabTitle.Controls.Add(this.txtTitle);
            this.tabTitle.Controls.Add(this.btnSendTitle);
            this.tabTitle.Location = new System.Drawing.Point(4, 22);
            this.tabTitle.Name = "tabTitle";
            this.tabTitle.Padding = new System.Windows.Forms.Padding(3);
            this.tabTitle.Size = new System.Drawing.Size(296, 81);
            this.tabTitle.TabIndex = 1;
            this.tabTitle.Text = "List By Title";
            this.tabTitle.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(6, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(148, 13);
            this.lblTitle.TabIndex = 7;
            this.lblTitle.Text = "Enter Keywords to match Title";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(9, 34);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(191, 20);
            this.txtTitle.TabIndex = 6;
            // 
            // btnSendTitle
            // 
            this.btnSendTitle.Location = new System.Drawing.Point(208, 34);
            this.btnSendTitle.Name = "btnSendTitle";
            this.btnSendTitle.Size = new System.Drawing.Size(75, 19);
            this.btnSendTitle.TabIndex = 5;
            this.btnSendTitle.Text = "Submit";
            this.btnSendTitle.UseVisualStyleBackColor = true;
            this.btnSendTitle.Click += new System.EventHandler(this.btnSendTitle_Click);
            // 
            // tabPeriod
            // 
            this.tabPeriod.Controls.Add(this.btnSendPeriod);
            this.tabPeriod.Controls.Add(this.dateStart);
            this.tabPeriod.Controls.Add(this.lblDateEnd);
            this.tabPeriod.Controls.Add(this.dateEnd);
            this.tabPeriod.Controls.Add(this.lblDateStart);
            this.tabPeriod.Location = new System.Drawing.Point(4, 22);
            this.tabPeriod.Name = "tabPeriod";
            this.tabPeriod.Size = new System.Drawing.Size(296, 81);
            this.tabPeriod.TabIndex = 2;
            this.tabPeriod.Text = "List By Period";
            this.tabPeriod.UseVisualStyleBackColor = true;
            // 
            // treeMovies
            // 
            this.treeMovies.Location = new System.Drawing.Point(12, 156);
            this.treeMovies.Name = "treeMovies";
            this.treeMovies.Size = new System.Drawing.Size(300, 221);
            this.treeMovies.TabIndex = 7;
            this.treeMovies.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeMovies_AfterSelect);
            // 
            // lblMovies
            // 
            this.lblMovies.AutoSize = true;
            this.lblMovies.Location = new System.Drawing.Point(9, 137);
            this.lblMovies.Name = "lblMovies";
            this.lblMovies.Size = new System.Drawing.Size(74, 13);
            this.lblMovies.TabIndex = 8;
            this.lblMovies.Text = "Movie Results";
            // 
            // btnAddReservation
            // 
            this.btnAddReservation.Location = new System.Drawing.Point(12, 384);
            this.btnAddReservation.Name = "btnAddReservation";
            this.btnAddReservation.Size = new System.Drawing.Size(300, 23);
            this.btnAddReservation.TabIndex = 9;
            this.btnAddReservation.Text = "Add Reservation";
            this.btnAddReservation.UseVisualStyleBackColor = true;
            this.btnAddReservation.Click += new System.EventHandler(this.btnAddReservation_Click);
            // 
            // treeReservations
            // 
            this.treeReservations.Location = new System.Drawing.Point(332, 156);
            this.treeReservations.Name = "treeReservations";
            this.treeReservations.Size = new System.Drawing.Size(300, 221);
            this.treeReservations.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(329, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Current Reservations";
            // 
            // btnRemoveReservation
            // 
            this.btnRemoveReservation.Location = new System.Drawing.Point(332, 384);
            this.btnRemoveReservation.Name = "btnRemoveReservation";
            this.btnRemoveReservation.Size = new System.Drawing.Size(300, 23);
            this.btnRemoveReservation.TabIndex = 12;
            this.btnRemoveReservation.Text = "Remove Reservation";
            this.btnRemoveReservation.UseVisualStyleBackColor = true;
            this.btnRemoveReservation.Click += new System.EventHandler(this.btnRemoveReservation_Click);
            // 
            // txtErrors
            // 
            this.txtErrors.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.txtErrors.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtErrors.Location = new System.Drawing.Point(332, 441);
            this.txtErrors.Name = "txtErrors";
            this.txtErrors.ReadOnly = true;
            this.txtErrors.Size = new System.Drawing.Size(300, 75);
            this.txtErrors.TabIndex = 13;
            this.txtErrors.Text = "";
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(329, 425);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(67, 13);
            this.lblLog.TabIndex = 14;
            this.lblLog.Text = "Log Window";
            // 
            // pnlResInfo
            // 
            this.pnlResInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlResInfo.Controls.Add(this.lblResDetails);
            this.pnlResInfo.Controls.Add(this.txtResDetails);
            this.pnlResInfo.Controls.Add(this.btnResCancel);
            this.pnlResInfo.Controls.Add(this.btnSendRes);
            this.pnlResInfo.Controls.Add(this.lblResSeats);
            this.pnlResInfo.Controls.Add(this.lblResName);
            this.pnlResInfo.Controls.Add(this.lblResTit);
            this.pnlResInfo.Controls.Add(this.txtResSeats);
            this.pnlResInfo.Controls.Add(this.txtResName);
            this.pnlResInfo.Location = new System.Drawing.Point(332, 156);
            this.pnlResInfo.Name = "pnlResInfo";
            this.pnlResInfo.Size = new System.Drawing.Size(300, 221);
            this.pnlResInfo.TabIndex = 16;
            this.pnlResInfo.Visible = false;
            // 
            // lblResDetails
            // 
            this.lblResDetails.AutoSize = true;
            this.lblResDetails.Location = new System.Drawing.Point(27, 45);
            this.lblResDetails.Name = "lblResDetails";
            this.lblResDetails.Size = new System.Drawing.Size(39, 13);
            this.lblResDetails.TabIndex = 8;
            this.lblResDetails.Text = "Details";
            // 
            // txtResDetails
            // 
            this.txtResDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtResDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtResDetails.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.txtResDetails.Location = new System.Drawing.Point(30, 61);
            this.txtResDetails.Name = "txtResDetails";
            this.txtResDetails.ReadOnly = true;
            this.txtResDetails.Size = new System.Drawing.Size(237, 56);
            this.txtResDetails.TabIndex = 7;
            this.txtResDetails.Text = "";
            // 
            // btnResCancel
            // 
            this.btnResCancel.Location = new System.Drawing.Point(145, 176);
            this.btnResCancel.Name = "btnResCancel";
            this.btnResCancel.Size = new System.Drawing.Size(58, 19);
            this.btnResCancel.TabIndex = 6;
            this.btnResCancel.Text = "Cancel";
            this.btnResCancel.UseVisualStyleBackColor = true;
            this.btnResCancel.Click += new System.EventHandler(this.btnResCancel_Click);
            // 
            // btnSendRes
            // 
            this.btnSendRes.Location = new System.Drawing.Point(209, 176);
            this.btnSendRes.Name = "btnSendRes";
            this.btnSendRes.Size = new System.Drawing.Size(58, 19);
            this.btnSendRes.TabIndex = 5;
            this.btnSendRes.Text = "Submit";
            this.btnSendRes.UseVisualStyleBackColor = true;
            this.btnSendRes.Click += new System.EventHandler(this.btnSendRes_Click);
            // 
            // lblResSeats
            // 
            this.lblResSeats.AutoSize = true;
            this.lblResSeats.Location = new System.Drawing.Point(30, 159);
            this.lblResSeats.Name = "lblResSeats";
            this.lblResSeats.Size = new System.Drawing.Size(34, 13);
            this.lblResSeats.TabIndex = 4;
            this.lblResSeats.Text = "Seats";
            // 
            // lblResName
            // 
            this.lblResName.AutoSize = true;
            this.lblResName.Location = new System.Drawing.Point(27, 120);
            this.lblResName.Name = "lblResName";
            this.lblResName.Size = new System.Drawing.Size(35, 13);
            this.lblResName.TabIndex = 3;
            this.lblResName.Text = "Name";
            // 
            // lblResTit
            // 
            this.lblResTit.AutoSize = true;
            this.lblResTit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResTit.Location = new System.Drawing.Point(63, 5);
            this.lblResTit.Name = "lblResTit";
            this.lblResTit.Size = new System.Drawing.Size(172, 16);
            this.lblResTit.TabIndex = 2;
            this.lblResTit.Text = "Reservation Information";
            // 
            // txtResSeats
            // 
            this.txtResSeats.Location = new System.Drawing.Point(30, 175);
            this.txtResSeats.Name = "txtResSeats";
            this.txtResSeats.Size = new System.Drawing.Size(67, 20);
            this.txtResSeats.TabIndex = 1;
            // 
            // txtResName
            // 
            this.txtResName.Location = new System.Drawing.Point(30, 136);
            this.txtResName.Name = "txtResName";
            this.txtResName.Size = new System.Drawing.Size(237, 20);
            this.txtResName.TabIndex = 0;
            // 
            // txtMovieDesc
            // 
            this.txtMovieDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.txtMovieDesc.Location = new System.Drawing.Point(12, 441);
            this.txtMovieDesc.Name = "txtMovieDesc";
            this.txtMovieDesc.Size = new System.Drawing.Size(300, 75);
            this.txtMovieDesc.TabIndex = 17;
            this.txtMovieDesc.Text = "";
            // 
            // lblMovieDesc
            // 
            this.lblMovieDesc.AutoSize = true;
            this.lblMovieDesc.Location = new System.Drawing.Point(12, 424);
            this.lblMovieDesc.Name = "lblMovieDesc";
            this.lblMovieDesc.Size = new System.Drawing.Size(92, 13);
            this.lblMovieDesc.TabIndex = 18;
            this.lblMovieDesc.Text = "Movie Description";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 528);
            this.Controls.Add(this.lblMovieDesc);
            this.Controls.Add(this.txtMovieDesc);
            this.Controls.Add(this.pnlResInfo);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.txtErrors);
            this.Controls.Add(this.btnRemoveReservation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeReservations);
            this.Controls.Add(this.btnAddReservation);
            this.Controls.Add(this.lblMovies);
            this.Controls.Add(this.treeMovies);
            this.Controls.Add(this.tabQuery);
            this.Name = "MainForm";
            this.Text = "BBC (BlockBusterClient) V.1.0";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabQuery.ResumeLayout(false);
            this.tabAll.ResumeLayout(false);
            this.tabTitle.ResumeLayout(false);
            this.tabTitle.PerformLayout();
            this.tabPeriod.ResumeLayout(false);
            this.tabPeriod.PerformLayout();
            this.pnlResInfo.ResumeLayout(false);
            this.pnlResInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateStart;
        private System.Windows.Forms.DateTimePicker dateEnd;
        private System.Windows.Forms.Label lblDateStart;
        private System.Windows.Forms.Label lblDateEnd;
        private System.Windows.Forms.Button btnSendPeriod;
        private System.Windows.Forms.TabControl tabQuery;
        private System.Windows.Forms.TabPage tabAll;
        private System.Windows.Forms.TabPage tabTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Button btnSendTitle;
        private System.Windows.Forms.TabPage tabPeriod;
        private System.Windows.Forms.Button btnSendAll;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TreeView treeMovies;
        private System.Windows.Forms.Label lblMovies;
        private System.Windows.Forms.Button btnAddReservation;
        private System.Windows.Forms.TreeView treeReservations;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRemoveReservation;
        private System.Windows.Forms.RichTextBox txtErrors;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.Panel pnlResInfo;
        private System.Windows.Forms.Label lblResName;
        private System.Windows.Forms.Label lblResTit;
        private System.Windows.Forms.TextBox txtResSeats;
        private System.Windows.Forms.TextBox txtResName;
        private System.Windows.Forms.Label lblResDetails;
        private System.Windows.Forms.RichTextBox txtResDetails;
        private System.Windows.Forms.Button btnResCancel;
        private System.Windows.Forms.Button btnSendRes;
        private System.Windows.Forms.Label lblResSeats;
        private System.Windows.Forms.RichTextBox txtMovieDesc;
        private System.Windows.Forms.Label lblMovieDesc;

    }
}
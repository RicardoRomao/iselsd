using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CinemaApplication.sd.deetc.isel.pt;
using System.Web.Services.Protocols;

namespace CinemaApplication
{
	public partial class MainForm : Form
	{
		private CinemaSvc[] cinemas;
		private WSBroker brkService;
		// private WSCinema cnmService;

		public MainForm()
		{
			InitializeComponent();
			brkService = new WSBroker();
			// cnmService = new WSCinema();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			StatusLabel.Text = "A obter cinemas...";
			brkService.GetCinemasCompleted += new GetCinemasCompletedEventHandler(brkService_GetCinemasCompleted);
			brkService.GetCinemasAsync();
		}

		private void brkService_GetCinemasCompleted(object sender, GetCinemasCompletedEventArgs e)
		{
			if (!e.Cancelled)
			{
				try
				{
					cinemas = e.Result;
					StatusLabel.Text = "";
					brkService.Dispose();
				}
				catch (SoapException ex)
				{
					MessageBox.Show("Não foi possível obter a lista de cinemas registados!", "Cinema Application", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrokerApplication.BlockBusterBroker;

namespace BrokerApplication
{
	public partial class MainForm : Form
	{
		private readonly WSBroker svc;

		public MainForm()
		{
			InitializeComponent();
            svc = new WSBroker();
            
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			AddCinema();
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			RemoveCinema();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			svc.GetCinemasCompleted += new GetCinemasCompletedEventHandler(svc_GetCinemasCompleted);
			svc.GetCinemasAsync();
		}

		private void svc_GetCinemasCompleted(object sender, GetCinemasCompletedEventArgs e)
		{
			if (!e.Cancelled && e.Error == null)
			{
				CinemaRegistry[] cinemas = e.Result;
				lstCinemas.BeginUpdate();
				foreach (CinemaRegistry cinema in cinemas)
				{
					lstCinemas.Items.Add(cinema.Name);
				}
				lstCinemas.EndUpdate();
				btnAdd.Enabled = true;
				btnRemove.Enabled = true;
			}
			else
			{
				MessageBox.Show("Não foi possível completar a operação", "Broker Application", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void RemoveCinema()
		{
			if (RemoveIsValid())
			{
				btnRemove.Enabled = false;
				svc.UnregisterCinemaCompleted += new UnregisterCinemaCompletedEventHandler(svc_UnregisterCinemaCompleted);
				svc.UnregisterCinemaAsync(lstCinemas.SelectedItem.ToString(), lstCinemas.SelectedItem.ToString());
			}
		}

		void svc_UnregisterCinemaCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (!e.Cancelled && e.Error == null)
			{
				lstCinemas.Items.Remove(e.UserState);
				btnRemove.Enabled = true;
			}
		}

		private bool RemoveIsValid()
		{
			if (lstCinemas.SelectedItems.Count != 1)
			{
				MessageBox.Show("Tem de seleccionar 1 e 1 só cinema da lista!", "BrokerApplication", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}

			return true;
		}

		private void AddCinema()
		{
			if (AddIsValid())
			{
				btnAdd.Enabled = false;
				svc.RegisterCinemaCompleted += new RegisterCinemaCompletedEventHandler(svc_RegisterCinemaCompleted);
				svc.RegisterCinemaAsync(txtNome.Text, txtUrl.Text, txtNome.Text);
			}
		}

		private void svc_RegisterCinemaCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (!e.Cancelled && e.Error == null)
			{
				lstCinemas.BeginUpdate();
				lstCinemas.Items.Add(e.UserState);
				lstCinemas.EndUpdate();
				txtNome.Text = "";
				txtUrl.Text = "";
				btnAdd.Enabled = true;
			}
		}

		private bool AddIsValid()
		{
			if (txtNome.Text.Trim().Length == 0)
			{
				MessageBox.Show("Tem de fornecer o nome do cinema!", "Broker Application", MessageBoxButtons.OK, MessageBoxIcon.Information);
				txtNome.Focus();
				return false;
			}

			if (txtUrl.Text.Trim().Length == 0)
			{
				MessageBox.Show("Tem de fornecer o url do cinema!", "Broker Application", MessageBoxButtons.OK, MessageBoxIcon.Information);
				txtUrl.Focus();
				return false;
			}

			return true;
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (svc != null)
			{
				svc.Dispose();
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClientFormsApplication.BBBroker;
using ClientFormsApplication.BBCinema;
using System.Net.Sockets;

namespace ClientFormsApplication
{
    public partial class MainForm : Form
    {
        private readonly Object _monitor = new Object();
        private ICoordinator _coord;

        public MainForm()
        {
            InitializeComponent();
            _coord = new Coordinator();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _coord.MoviesReceived += Coord_MoviesReceived;
            _coord.ErrorOccured += AddErrorMessage;
            _coord.AddReservationProcessed += Coord_AddReservationProcessed;
            _coord.RemoveReservationProcessed += Coord_RemoveReservationProcessed;
        }

        #region Coordinator callbacks
        void Coord_RemoveReservationProcessed(SessionInfo resInfo)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(
                    new Action<SessionInfo>(Coord_RemoveReservationProcessed), resInfo
                );
            }
            if (treeReservations.Nodes.ContainsKey(resInfo.MovieTitle))
            {
                if (treeReservations.Nodes[resInfo.MovieTitle].Nodes.ContainsKey(resInfo.Cinema))
                {
                    TreeNode node = treeReservations.Nodes[resInfo.MovieTitle].Nodes[resInfo.Cinema];
                    if (node.Nodes.ContainsKey(resInfo.Code.ToString()))
                    {
                        node.Nodes[resInfo.Code.ToString()].Remove();
                        if (node.Nodes.Count == 0)
                            node.Remove();
                        if (treeReservations.Nodes[resInfo.MovieTitle].Nodes.Count == 0)
                            treeReservations.Nodes[resInfo.MovieTitle].Remove();
                    }
                    else
                    {
                        AddErrorMessage("BBClient - Reference to reservation lost.");
                        return;
                    }
                }
                else
                {
                    AddErrorMessage("BBClient - Reference to cinema reservation lost.");
                    return;
                }
            }
            else
            {
                AddErrorMessage("BBClient - Reference to movie reservation lost.");
                return;
            }
        }

        void Coord_AddReservationProcessed(SessionInfo resInfo)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(
                    new Action<SessionInfo>(Coord_AddReservationProcessed), resInfo
                );
            if (resInfo.Code != Guid.Empty)
            {
                TreeNode movieNode;
                if (treeReservations.Nodes.ContainsKey(resInfo.MovieTitle))
                    movieNode = treeReservations.Nodes[resInfo.MovieTitle];
                else
                {
                    movieNode = new TreeNode(resInfo.MovieTitle);
                    movieNode.Name = resInfo.MovieTitle;
                    treeReservations.Nodes.Add(movieNode);
                }

                TreeNode cinemaNode;
                if (movieNode.Nodes.ContainsKey(resInfo.Cinema))
                    cinemaNode = movieNode.Nodes[resInfo.Cinema];
                else
                {
                    cinemaNode = new TreeNode(resInfo.Cinema);
                    cinemaNode.Name = resInfo.Cinema;
                    movieNode.Nodes.Add(cinemaNode);
                }

                TreeNode resNode = new TreeNode(resInfo.Code.ToString());
                resNode.Name = resInfo.Code.ToString();
                resNode.Tag = resInfo;
                resNode.Nodes.Add("Name:\t" + resInfo.Name);
                resNode.Nodes.Add("Seats:\t" + resInfo.Seats);
                resNode.Nodes.Add("Start At:\t" + resInfo.StartTime.TimeOfDay);

                cinemaNode.Nodes.Add(resNode);
				MessageBox.Show(String.Format("Your reservation expires in {0} day{1}.", resInfo.Expires, (resInfo.Expires>1)?"s":""), 
								"AddReservation", 
								MessageBoxButtons.OK, 
								MessageBoxIcon.Information);
            }
            else
            {
                AddErrorMessage(
                    String.Format("{0} - Number of seats not availabe for required session!",
                        resInfo.Cinema
                    )
                );
            }
        }

        void Coord_MoviesReceived(string cinemaName, List<Movie> movies)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(
                    new Action<string, List<Movie>>(Coord_MoviesReceived), cinemaName, movies
                );
            foreach (Movie m in movies)
            {
                TreeNode movieNode;
                if (treeMovies.Nodes.ContainsKey(m.Title))
                    movieNode = treeMovies.Nodes[m.Title];
                else
                {
                    movieNode = new TreeNode(m.Title);
                    movieNode.Name = m.Title;
                    movieNode.Tag = m.Desc;
                    treeMovies.Nodes.Add(movieNode);
                }

                TreeNode cinemaNode = new TreeNode(cinemaName);
                cinemaNode.Name = cinemaName;
                cinemaNode.Tag = null;
                if (m.Sessions != null)
                {
                    movieNode.Nodes.Add(cinemaNode);
                    foreach (MovieSession s in m.Sessions)
                    {
                        TreeNode sessionNode =
                            new TreeNode("Inicio às " + s.StartTime.TimeOfDay +
                                " (Sala " + s.Room.Num + ")"
                        );
                        sessionNode.Tag = new SessionInfo
                        {
                            SessionID = s.Id,
                            StartTime = s.StartTime,
                            Cinema = cinemaName,
                            MovieTitle = m.Title
                        };
                        sessionNode.Name = s.Id;
                        movieNode.Nodes[cinemaName].Nodes.Add(sessionNode);
                    }
                }
            }
        }
        #endregion
        
        #region Movie operations
        private void btnSendAll_Click(object sender, EventArgs e)
        {
            treeMovies.Nodes.Clear();
            _coord.GetMovies();
        }

        private void btnSendTitle_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text == "")
            {
                AddErrorMessage("BBClient - Keyword for title are required.");
                return;
            }
            treeMovies.Nodes.Clear();
            _coord.GetMovies(txtTitle.Text);
        }

        private void btnSendPeriod_Click(object sender, EventArgs e)
        {
            treeMovies.Nodes.Clear();
            _coord.GetMovies(dateStart.Value, dateEnd.Value);
        }
        
        private void treeMovies_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                txtMovieDesc.Text = (string)e.Node.Tag;
            }
        }
        #endregion

        #region Reservation operations
        private void btnAddReservation_Click(object sender, EventArgs e)
        {
            if (treeMovies.SelectedNode.Level > 0 &&
                    treeMovies.SelectedNode.Tag != null)
            {
                SessionInfo si = (SessionInfo)treeMovies.SelectedNode.Tag;
                treeMovies.Enabled = false;
                btnAddReservation.Enabled = false;
                btnRemoveReservation.Enabled = false;
                tabQuery.Enabled = false;
                txtResDetails.Clear();
                txtResName.Text = "";
                txtResSeats.Text = "";
                pnlResInfo.Visible = true;
                txtResDetails.AppendText("Movie:\t" + si.MovieTitle + "\n");
                txtResDetails.AppendText("Cinema:\t" + si.Cinema + "\n");
                txtResDetails.AppendText("Start At:\t" + si.StartTime);
            }

        }

        private void btnResCancel_Click(object sender, EventArgs e)
        {
            treeMovies.Enabled = true;
            btnAddReservation.Enabled = true;
            btnRemoveReservation.Enabled = true;
            tabQuery.Enabled = true;
            pnlResInfo.Visible = false;
        }

        private void btnSendRes_Click(object sender, EventArgs e)
        {
            if (txtResName.Text == "")
            {
                AddErrorMessage("BBClient - Reservation name is a required field.");
                return;
            }
            int seats;
            if (!Int32.TryParse(txtResSeats.Text, out seats))
            {
                AddErrorMessage("BBClient - Reservation seats is a required numerical field.");
                return;
            }
            pnlResInfo.Visible = false;
            btnAddReservation.Enabled = true;
            btnRemoveReservation.Enabled = true;
            treeMovies.Enabled = true;
            tabQuery.Enabled = true;

            SessionInfo si = (SessionInfo)treeMovies.SelectedNode.Tag;
            si.Name = txtResName.Text;
            si.Seats = seats;
            _coord.SendReservation(si);
        }

        private void btnRemoveReservation_Click(object sender, EventArgs e)
        {
            if (treeReservations.SelectedNode.Tag != null)
            {
                SessionInfo si = (SessionInfo)treeReservations.SelectedNode.Tag;
                _coord.RemoveReservation(si);
            }
        }
        #endregion

        private void AddErrorMessage(string message)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(
                    new Action<string>(AddErrorMessage), message
                );
            txtErrors.AppendText(message);
        }
    }
}

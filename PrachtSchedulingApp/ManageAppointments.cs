using MySql.Data.MySqlClient;
using PrachtSchedulingApp.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrachtSchedulingApp
{
    public partial class ManageAppointments : Form
    {
        public ManageAppointments()
        {
            InitializeComponent();
        }

        private void PopulateGrid()
        {
            try
            {
                // Open connection string and write query
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                MySqlConnection con = new MySqlConnection(connectionString);

                con.Open();
                string query = "SELECT * FROM appointment";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                // DataTable apptsUTC = new DataTable();
                DataTable apptsLocal = new DataTable();
                // adapter.Fill(apptsUTC);
                adapter.Fill(apptsLocal);

                // Convert UTC to Local Time
                //for (int i = 0; i < apptsUTC.Rows.Count; i++)
                //{
                //    DateTime x = (DateTime)apptsUTC.Rows[i]["start"];
                //    apptsLocal.Rows[i]["start"] = x.ToLocalTime();

                //    DateTime y = (DateTime)apptsUTC.Rows[i]["end"];
                //    apptsLocal.Rows[i]["end"] = y.ToLocalTime();
                //}

                // Set DataSource
                dgvManageAppointments.DataSource = apptsLocal;


                // Adjust column headers to preference
                dgvManageAppointments.Columns["appointmentId"].Visible = false;
                dgvManageAppointments.Columns["customerId"].Visible = false;
                dgvManageAppointments.Columns["userId"].Visible = false;
                dgvManageAppointments.Columns["title"].HeaderText = "Title";
                dgvManageAppointments.Columns["description"].HeaderText = "Description";
                dgvManageAppointments.Columns["location"].HeaderText = "Location";
                dgvManageAppointments.Columns["contact"].HeaderText = "Contact";
                dgvManageAppointments.Columns["type"].HeaderText = "Meeting Type";
                dgvManageAppointments.Columns["url"].HeaderText = "URL";
                dgvManageAppointments.Columns["start"].HeaderText = "Start";
                dgvManageAppointments.Columns["end"].HeaderText = "End";
                dgvManageAppointments.Columns["createDate"].Visible = false;
                dgvManageAppointments.Columns["createdBy"].Visible = false;
                dgvManageAppointments.Columns["lastUpdate"].Visible = false;
                dgvManageAppointments.Columns["lastUpdateBy"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void ManageAppointments_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}

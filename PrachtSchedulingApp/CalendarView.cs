using MySql.Data.MySqlClient;
using System;
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
    public partial class CalendarView : Form
    {
        public CalendarView()
        {
            InitializeComponent();
        }

        private void CalendarView_Load(object sender, EventArgs e)
        {
            PopulateGrid();
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
                dgvDisplayAppointments.DataSource = apptsLocal;


                // Adjust column headers to preference
                dgvDisplayAppointments.Columns["appointmentId"].Visible = false;
                dgvDisplayAppointments.Columns["customerId"].Visible = false;
                dgvDisplayAppointments.Columns["userId"].Visible = false;
                dgvDisplayAppointments.Columns["title"].HeaderText = "Title";
                dgvDisplayAppointments.Columns["description"].HeaderText = "Description";
                dgvDisplayAppointments.Columns["location"].HeaderText = "Location";
                dgvDisplayAppointments.Columns["contact"].HeaderText = "Contact";
                dgvDisplayAppointments.Columns["type"].HeaderText = "Meeting Type";
                dgvDisplayAppointments.Columns["url"].HeaderText = "URL";
                dgvDisplayAppointments.Columns["start"].HeaderText = "Start";
                dgvDisplayAppointments.Columns["end"].HeaderText = "End";
                dgvDisplayAppointments.Columns["createDate"].Visible = false;
                dgvDisplayAppointments.Columns["createdBy"].Visible = false;
                dgvDisplayAppointments.Columns["lastUpdate"].Visible = false;
                dgvDisplayAppointments.Columns["lastUpdateBy"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFindAppointments_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime selectedDate = dateTimePicker.Value.Date;
                DateTime startOfDay = selectedDate;
                DateTime endOfDay = selectedDate.AddDays(1);

                // Open connection string and write query
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                MySqlConnection con = new MySqlConnection(connectionString);

                con.Open();
                string query = "SELECT * FROM appointment WHERE start >= @startOfDay AND start < @endOfDay";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@startOfDay", startOfDay);
                cmd.Parameters.AddWithValue("@endOfDay", endOfDay);
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
                dgvDisplayAppointments.DataSource = apptsLocal;


                // Adjust column headers to preference
                dgvDisplayAppointments.Columns["appointmentId"].Visible = false;
                dgvDisplayAppointments.Columns["customerId"].Visible = false;
                dgvDisplayAppointments.Columns["userId"].Visible = false;
                dgvDisplayAppointments.Columns["title"].HeaderText = "Title";
                dgvDisplayAppointments.Columns["description"].HeaderText = "Description";
                dgvDisplayAppointments.Columns["location"].HeaderText = "Location";
                dgvDisplayAppointments.Columns["contact"].HeaderText = "Contact";
                dgvDisplayAppointments.Columns["type"].HeaderText = "Meeting Type";
                dgvDisplayAppointments.Columns["url"].HeaderText = "URL";
                dgvDisplayAppointments.Columns["start"].HeaderText = "Start";
                dgvDisplayAppointments.Columns["end"].HeaderText = "End";
                dgvDisplayAppointments.Columns["createDate"].Visible = false;
                dgvDisplayAppointments.Columns["createdBy"].Visible = false;
                dgvDisplayAppointments.Columns["lastUpdate"].Visible = false;
                dgvDisplayAppointments.Columns["lastUpdateBy"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}

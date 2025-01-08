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
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            PopulateGrid();
        }

        public void PopulateGrid()
        {
            try
            {
                // Open connection string and write query
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"
                    SELECT 
                        a.appointmentId,
                        c.customerName AS CustomerName,
                        u1.userName AS UserName,
                        a.title,
                        a.description,
                        a.location,
                        a.contact,
                        a.type,
                        a.url,
                        a.start,
                        a.end,
                        a.createDate,
                        u3.userName AS CreatedBy,
                        a.lastUpdate,
                        u2.userName AS LastUpdatedBy
                    FROM 
                        appointment a
                    JOIN 
                        customer c ON a.customerId = c.customerId
                    JOIN 
                        user u1 ON a.userId = u1.userId
                    JOIN 
                        user u2 ON a.lastUpdateBy = u2.userId
                    JOIN 
                        user u3 ON a.createdBy = u3.userId";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    DataTable apptsUTC = new DataTable();
                    DataTable apptsLocal = new DataTable();
                    adapter.Fill(apptsUTC);
                    apptsLocal = apptsUTC.Copy();

                    // Convert UTC to Local Time
                    foreach (DataRow row in apptsLocal.Rows)
                    {
                        row["start"] = ((DateTime)row["start"]).ToLocalTime();
                        row["end"] = ((DateTime)row["end"]).ToLocalTime();
                    }

                    // Set DataSource
                    dgvDisplayAppointments.DataSource = apptsLocal;

                    // Adjust column headers to preference
                    dgvDisplayAppointments.Columns["appointmentId"].Visible = false;
                    dgvDisplayAppointments.Columns["CustomerName"].HeaderText = "Customer";
                    dgvDisplayAppointments.Columns["UserName"].HeaderText = "User";
                    dgvDisplayAppointments.Columns["title"].HeaderText = "Title";
                    dgvDisplayAppointments.Columns["description"].HeaderText = "Description";
                    dgvDisplayAppointments.Columns["location"].HeaderText = "Location";
                    dgvDisplayAppointments.Columns["contact"].HeaderText = "Contact";
                    dgvDisplayAppointments.Columns["type"].HeaderText = "Meeting Type";
                    dgvDisplayAppointments.Columns["url"].HeaderText = "URL";
                    dgvDisplayAppointments.Columns["start"].HeaderText = "Start";
                    dgvDisplayAppointments.Columns["end"].HeaderText = "End";
                    dgvDisplayAppointments.Columns["CreatedBy"].HeaderText = "Created By";
                    dgvDisplayAppointments.Columns["createDate"].HeaderText = "Creation Date";
                    dgvDisplayAppointments.Columns["LastUpdatedBy"].HeaderText = "Last Updated By";
                    dgvDisplayAppointments.Columns["lastUpdate"].HeaderText = "Last Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"
                    SELECT 
                        a.appointmentId,
                        c.customerName AS CustomerName,
                        u1.userName AS UserName,
                        a.title,
                        a.description,
                        a.location,
                        a.contact,
                        a.type,
                        a.url,
                        a.start,
                        a.end,
                        a.createDate,
                        u3.userName AS CreatedBy,
                        a.lastUpdate,
                        u2.userName AS LastUpdatedBy
                    FROM 
                        appointment a
                    JOIN 
                        customer c ON a.customerId = c.customerId
                    JOIN 
                        user u1 ON a.userId = u1.userId
                    JOIN 
                        user u2 ON a.lastUpdateBy = u2.userId
                    JOIN 
                        user u3 ON a.createdBy = u3.userId
                    WHERE 
                        a.start >= @startOfDay AND a.start < @endOfDay";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@startOfDay", startOfDay);
                    cmd.Parameters.AddWithValue("@endOfDay", endOfDay);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    DataTable apptsUTC = new DataTable();
                    DataTable apptsLocal = new DataTable();
                    adapter.Fill(apptsUTC);
                    apptsLocal = apptsUTC.Copy();

                    // Convert UTC to Local Time
                    foreach (DataRow row in apptsLocal.Rows)
                    {
                        row["start"] = ((DateTime)row["start"]).ToLocalTime();
                        row["end"] = ((DateTime)row["end"]).ToLocalTime();
                    }

                    // Set DataSource
                    dgvDisplayAppointments.DataSource = apptsLocal;

                    // Adjust column headers to preference
                    dgvDisplayAppointments.Columns["appointmentId"].Visible = false;
                    dgvDisplayAppointments.Columns["CustomerName"].HeaderText = "Customer";
                    dgvDisplayAppointments.Columns["UserName"].HeaderText = "User";
                    dgvDisplayAppointments.Columns["title"].HeaderText = "Title";
                    dgvDisplayAppointments.Columns["description"].HeaderText = "Description";
                    dgvDisplayAppointments.Columns["location"].HeaderText = "Location";
                    dgvDisplayAppointments.Columns["contact"].HeaderText = "Contact";
                    dgvDisplayAppointments.Columns["type"].HeaderText = "Meeting Type";
                    dgvDisplayAppointments.Columns["url"].HeaderText = "URL";
                    dgvDisplayAppointments.Columns["start"].HeaderText = "Start";
                    dgvDisplayAppointments.Columns["end"].HeaderText = "End";
                    dgvDisplayAppointments.Columns["CreatedBy"].HeaderText = "Created By";
                    dgvDisplayAppointments.Columns["createDate"].HeaderText = "Creation Date";
                    dgvDisplayAppointments.Columns["LastUpdatedBy"].HeaderText = "Last Updated By";
                    dgvDisplayAppointments.Columns["lastUpdate"].HeaderText = "Last Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnReset_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}

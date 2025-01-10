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
            PopulateGrid();
        }

        private void btnFindAppointments_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime selectedDate = dateTimePicker.Value.Date;
                DateTime startOfDay = selectedDate;
                DateTime endOfDay = selectedDate.AddDays(1);

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
                    DataTable findAppointments = new DataTable();

                    // Populate DataTable
                    adapter.Fill(findAppointments);
                    // Convert to local time
                    DatabaseHelper.TimeHelper(findAppointments, "start", "end", "createDate", "lastUpdate");
                    // Populate grid
                    DatabaseHelper.PopulateAppointments(dgvAppointments, findAppointments);
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

        public void PopulateGrid()
        {
            try
            {
                // Populate DataTable
                DataTable appointments = GetAppointmentData();
                // Convert to local time
                DatabaseHelper.TimeHelper(appointments, "start", "end", "createDate", "lastUpdate");
                // Populate grid
                DatabaseHelper.PopulateAppointments(dgvAppointments, appointments);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Configuration error: {ex.Message}", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable GetAppointmentData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Database connection string is missing or invalid.");

            DataTable appts = new DataTable();
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

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(appts);
                }
            }
            return appts;
        }
    }
}

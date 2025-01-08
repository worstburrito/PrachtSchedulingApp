using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PrachtSchedulingApp.Login;

namespace PrachtSchedulingApp
{
    public partial class EditAppointment : Form
    {
        private ManageAppointments _manageAppointments;
        private int _appointmentId;

        public EditAppointment(int appointmentId, ManageAppointments manageAppointments = null)
        {
            InitializeComponent();
            _appointmentId = appointmentId;
            _manageAppointments = manageAppointments;

            dtpStart.CustomFormat = "MM/dd/yyyy hh:mm tt"; // 12-hour format
            dtpEnd.CustomFormat = "MM/dd/yyyy hh:mm tt"; // 12-hour format
        }

        // This will populate the customer combobox and load the appointment data by id
        private void EditAppointment_Load(object sender, EventArgs e)
        {
            PopulateCustomerComboBox();
            LoadAppointmentData();
        }
        
        // Submit button 
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // Gather updated values from the form
            int customerId = (int)cboCustomer.SelectedValue;
            int userId = CurrentUser.UserId;
            string title = txtTitle.Text;
            string description = txtDesc.Text;
            string location = txtLocation.Text;
            string contact = txtContact.Text;
            string type = txtType.Text;
            string url = txtURL.Text;
            DateTime start = dtpStart.Value;  // Local time from DateTimePicker
            DateTime end = dtpEnd.Value;      // Local time from DateTimePicker

            // Convert start and end to UTC time for storage
            DateTime startUTC = start.ToUniversalTime();
            DateTime endUTC = end.ToUniversalTime();

            // Check if appointment is within business hours (local time)
            TimeZoneInfo estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime startEST = TimeZoneInfo.ConvertTime(start, estZone);
            DateTime endEST = TimeZoneInfo.ConvertTime(end, estZone);

            // Check if end date is smaller than start date
            if (startUTC > endUTC)
            {
                MessageBox.Show("End date cannot be earlier than start date.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if appointment time is outside work hours
            if (startEST.DayOfWeek == DayOfWeek.Saturday || startEST.DayOfWeek == DayOfWeek.Sunday ||
                startEST.Hour < 9 || endEST.Hour > 17)
            {
                MessageBox.Show("Appointments must be scheduled between 9:00 AM and 5:00 PM, Monday–Friday, Eastern Standard Time.", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check for overlapping appointments
            if (IsAppointmentOverlapping(startUTC, endUTC, customerId))
            {
                MessageBox.Show("The selected time slot overlaps with an existing appointment. Please choose a different time.", "Overlapping Appointment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if certain fields are missing data
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(location) || string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(type))
            {
                MessageBox.Show("Title, Location, Contact and Type are required to save appointment.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Update appointment in the database (store UTC times)
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            string query = @"
            UPDATE appointment
            SET 
                customerId = @customerId,
                userId = @userId,
                title = @title,
                description = @description,
                location = @location,
                contact = @contact,
                type = @type,
                url = @url,
                start = @start,
                end = @end,
                lastUpdate = NOW(),
                lastUpdateBy = @userId
            WHERE appointmentId = @appointmentId";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@appointmentId", _appointmentId);
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@location", location);
                        cmd.Parameters.AddWithValue("@contact", contact);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@url", url);
                        cmd.Parameters.AddWithValue("@start", startUTC);  // Store as UTC
                        cmd.Parameters.AddWithValue("@end", endUTC);      // Store as UTC
                        cmd.Parameters.AddWithValue("@updatedBy", userId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            _manageAppointments.PopulateGrid();
                            MessageBox.Show("Appointment has been updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Update failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // this will load the appointment data from the passed appointmentid
        private void LoadAppointmentData()
        {
            // Retrieve the connection string from ConfigurationManager
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;

            // Establish connection to the database
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open(); // Open the connection

                    // SQL query to fetch data for the selected appointment
                    string query = "SELECT * FROM appointment WHERE appointmentId = @appointmentId";

                    // Create a MySqlCommand with the query and connection
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        // Add the parameter for appointmentId
                        cmd.Parameters.AddWithValue("@appointmentId", _appointmentId);

                        // Use a DataAdapter to execute the query and fill the data
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable appointmentData = new DataTable();
                        adapter.Fill(appointmentData);

                        // Check if any data was retrieved
                        if (appointmentData.Rows.Count > 0)
                        {
                            DataRow row = appointmentData.Rows[0];

                            // Populate the form controls with data
                            cboCustomer.SelectedValue = row["customerId"];
                            txtTitle.Text = row["title"].ToString();
                            txtDesc.Text = row["description"].ToString();
                            txtLocation.Text = row["location"].ToString();
                            txtContact.Text = row["contact"].ToString();
                            txtType.Text = row["type"].ToString();
                            txtURL.Text = row["url"].ToString();

                            // Set the DateTime pickers to the local time
                            DateTime startTime = Convert.ToDateTime(row["start"]).ToLocalTime();
                            DateTime endTime = Convert.ToDateTime(row["end"]).ToLocalTime();

                            // Assign the local times to the DateTimePicker controls
                            dtpStart.Value = startTime;
                            dtpEnd.Value = endTime;
                        }
                        else
                        {
                            MessageBox.Show($"Appointment not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Close(); // Close the form if no data is found
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // this will populate the customer combobox
        private void PopulateCustomerComboBox()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                MySqlConnection con = new MySqlConnection(connectionString);
                con.Open();
                string query = "SELECT customerId, customerName FROM customer WHERE active = 1";  // Optional: to filter only active customers
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                DataTable customerComboBox = new DataTable();
                adapter.Fill(customerComboBox);

                cboCustomer.DisplayMember = "customerName";
                cboCustomer.ValueMember = "customerId";

                cboCustomer.DataSource = customerComboBox;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // this checks if the appointment being edited overlaps with an existing customer appointment
        private bool IsAppointmentOverlapping(DateTime start, DateTime end, int customerId)
        {
            // Query the database to check for overlapping appointments
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();

            string query = @"
        SELECT COUNT(*) FROM appointment 
        WHERE customerId = @customerId 
        AND appointmentId != @appointmentId
        AND ((start BETWEEN @start AND @end) OR (end BETWEEN @start AND @end) OR (@start BETWEEN start AND end) OR (@end BETWEEN start AND end));";

            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@customerId", customerId);
            cmd.Parameters.AddWithValue("@appointmentId", _appointmentId);
            cmd.Parameters.AddWithValue("@start", start);
            cmd.Parameters.AddWithValue("@end", end);

            int overlappingAppointments = Convert.ToInt32(cmd.ExecuteScalar());

            return overlappingAppointments > 0;
        }

        // I will need to come back and edit this when customer forms are done.
        private void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"This doesn't exist yet! Pardon my dust.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

}

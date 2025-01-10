﻿using MySql.Data.MySqlClient;
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
            PopulateCustomerComboBox();
            PopulateUserComboBox();


            _appointmentId = appointmentId;
            _manageAppointments = manageAppointments;

            dtpStart.CustomFormat = "MM/dd/yyyy hh:mm tt"; // 12-hour format
            dtpEnd.CustomFormat = "MM/dd/yyyy hh:mm tt"; // 12-hour format
        }

        private void EditAppointment_Load(object sender, EventArgs e)
        {
            LoadAppointmentData();
        }

        // Submit button 
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate combo box selections
                if (cboCustomer.SelectedValue == null || cboUser.SelectedValue == null)
                {
                    MessageBox.Show("Please select valid values for customer and user.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Attempt to parse SelectedValue as integers
                if (!int.TryParse(cboCustomer.SelectedValue.ToString(), out int customerId) ||
                    !int.TryParse(cboUser.SelectedValue.ToString(), out int selectedUserId))
                {
                    MessageBox.Show("Invalid customer or user selection.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Gather other values from the form
                int currentUserId = CurrentUser.UserId;
                string title = txtTitle.Text.Trim();
                string description = txtDesc.Text.Trim();
                string location = txtLocation.Text.Trim();
                string contact = txtContact.Text.Trim();
                string type = txtType.Text.Trim();
                string url = txtURL.Text.Trim();
                DateTime start = dtpStart.Value;
                DateTime end = dtpEnd.Value;

                // Validate required fields
                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(location) ||
                    string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(type))
                {
                    MessageBox.Show("Title, Location, Contact, and Type are required to save the appointment.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Convert start and end to UTC for storage
                DateTime startUTC = start.ToUniversalTime();
                DateTime endUTC = end.ToUniversalTime();

                // Check if end date is earlier than start date
                if (startUTC >= endUTC)
                {
                    MessageBox.Show("End date cannot be earlier than or equal to the start date.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate business hours (Eastern Standard Time)
                TimeZoneInfo estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                DateTime startEST = TimeZoneInfo.ConvertTime(start, estZone);
                DateTime endEST = TimeZoneInfo.ConvertTime(end, estZone);

                if (startEST.DayOfWeek == DayOfWeek.Saturday || startEST.DayOfWeek == DayOfWeek.Sunday ||
                    startEST < startEST.Date.AddHours(9) || endEST > endEST.Date.AddHours(17))
                {
                    MessageBox.Show("Appointments must be scheduled between 9:00 AM and 5:00 PM, Monday–Friday, Eastern Standard Time.",
                        "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check for overlapping appointments
                if (IsAppointmentOverlapping(startUTC, endUTC, customerId))
                {
                    MessageBox.Show("The selected time slot overlaps with an existing appointment. Please choose a different time.",
                        "Overlapping Appointment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate connection string
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"]?.ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    MessageBox.Show("Database connection string is missing or invalid.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update appointment in the database
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
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
                            lastUpdateBy = @currentUserId
                        WHERE appointmentId = @appointmentId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@appointmentId", _appointmentId);
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        cmd.Parameters.AddWithValue("@userId", selectedUserId);
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@location", location);
                        cmd.Parameters.AddWithValue("@contact", contact);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@url", url);
                        cmd.Parameters.AddWithValue("@start", startUTC);
                        cmd.Parameters.AddWithValue("@end", endUTC);
                        cmd.Parameters.AddWithValue("@currentUserId", currentUserId);

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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // this will populate the customer combobox
        private void PopulateCustomerComboBox()
        {
            string query = "SELECT customerId, customerName FROM customer WHERE active = 1";
            DatabaseHelper.PopulateCustomerComboBox(cboCustomer, query, "customerName", "customerId");
        }
        private void PopulateUserComboBox()
        {
            string query = "SELECT userId, userName FROM user WHERE active = 1";
            DatabaseHelper.PopulateUserComboBox(cboUser, query, "userName", "userId");
        }
    }
}

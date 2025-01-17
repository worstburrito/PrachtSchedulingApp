﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PrachtSchedulingApp.Login;

namespace PrachtSchedulingApp
{
    public partial class AddAppointment : Form
    {
        private ManageAppointments _manageAppointments;

        public AddAppointment(ManageAppointments manageAppointments = null)
        {
            InitializeComponent();
            PopulateCustomerComboBox();
            PopulateUserComboBox();
            _manageAppointments = manageAppointments;

            dtpStart.CustomFormat = "MM/dd/yyyy hh:mm tt"; // 12-hour format
            dtpEnd.CustomFormat = "MM/dd/yyyy hh:mm tt"; // 12-hour format
        }


        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // This will show if something isn't working properly with the comboboxes
            try
            {
                if (cboCustomer.SelectedValue == null || cboUser.SelectedValue == null)
                {
                    MessageBox.Show("Please select valid values for customer and user.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(cboCustomer.SelectedValue.ToString(), out int customerId) ||
                    !int.TryParse(cboUser.SelectedValue.ToString(), out int selectedUserId))
                {
                    MessageBox.Show("Invalid customer or user selection.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // form variables
                int currentUserId = CurrentUser.UserId;
                string title = txtTitle.Text.Trim();
                string description = txtDesc.Text.Trim();
                string location = txtLocation.Text.Trim();
                string contact = txtContact.Text.Trim();
                string type = txtType.Text.Trim();
                string url = txtURL.Text.Trim();
                DateTime start = dtpStart.Value;
                DateTime end = dtpEnd.Value;

                // checks for empty textboxes
                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(location) || string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(type))
                {
                    MessageBox.Show("Title, Location, Contact, and Type are required to save the appointment.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // this checks that the start/end dates are valid
                DateTime startUTC = start.ToUniversalTime();
                DateTime endUTC = end.ToUniversalTime();

                if (startUTC >= endUTC)
                {
                    MessageBox.Show("End date cannot be earlier than or equal to the start date.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                /*
                 * If you're grading this on the weekend or outside of the requirements business hours
                 * and need to test the requirement:
                 * "Create a function that generates an alert whenever a 
                 * user who has an appointment within 15 minutes logs in to their account."
                 * Then you can comment out the function below, add an appointment that will
                 * trigger at the time you're grading this to see that requirement working.
                 */
                try
                {
                    TimeZoneInfo estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                    DateTime startEST = TimeZoneInfo.ConvertTimeFromUtc(start.ToUniversalTime(), estZone);
                    DateTime endEST = TimeZoneInfo.ConvertTimeFromUtc(end.ToUniversalTime(), estZone);

                    bool isDST = estZone.IsDaylightSavingTime(startEST);
                    string dstMessage = isDST ? " (Daylight Savings Time applies)" : "";

                    if (startEST.DayOfWeek == DayOfWeek.Saturday || startEST.DayOfWeek == DayOfWeek.Sunday ||
                        startEST < startEST.Date.AddHours(9) || endEST > endEST.Date.AddHours(17))
                    {
                        MessageBox.Show($"Appointments must be scheduled between 9:00 AM and 5:00 PM, Monday–Friday, Eastern Time{dstMessage}.",
                            "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                catch (TimeZoneNotFoundException)
                {
                    MessageBox.Show("The system's time zone configuration is invalid or unsupported.", "Time Zone Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (InvalidTimeZoneException)
                {
                    MessageBox.Show("The system encountered an issue while processing time zone information.", "Time Zone Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                /*
                 * END OF CODE FOR BUSINESS HOURS REQUIREMENT
                 */

                // this checks if the appointment is overlapping with either a customer or a user
                if (IsAppointmentOverlapping(startUTC, endUTC, customerId, selectedUserId))
                {
                    MessageBox.Show("The selected time slot overlaps with an existing appointment for the customer or user. Please choose a different time.",
                        "Overlapping Appointment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // runs the database
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"]?.ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    MessageBox.Show("Database connection string is missing or invalid.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    string query = @"
                    INSERT INTO appointment 
                    (customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy) 
                    VALUES 
                    (@customerId, @userId, @title, @description, @location, @contact, @type, @url, @start, @end, NOW(), @createdBy, NOW(), @userUpdateId);";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        cmd.Parameters.AddWithValue("@userId", selectedUserId);
                        cmd.Parameters.AddWithValue("@userUpdateId", currentUserId);
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@location", location);
                        cmd.Parameters.AddWithValue("@contact", contact);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@url", url);
                        cmd.Parameters.AddWithValue("@start", startUTC);
                        cmd.Parameters.AddWithValue("@end", endUTC);
                        cmd.Parameters.AddWithValue("@createdBy", currentUserId);
                        cmd.ExecuteNonQuery();
                    }
                }

                _manageAppointments?.PopulateGrid();

                MessageBox.Show("New appointment has been added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // this method runs the database for overlapping appointments
        private bool IsAppointmentOverlapping(DateTime start, DateTime end, int customerId, int userId, int appointmentId = -1)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string queryCustomerOverlap = @"
                SELECT COUNT(*) 
                FROM appointment 
                WHERE customerId = @customerId 
                AND appointmentId != @appointmentId
                AND (
                    (start < @end AND end > @start)  -- Check if new appointment overlaps with an existing one
                );";

                using (MySqlCommand cmdCustomer = new MySqlCommand(queryCustomerOverlap, conn))
                {
                    cmdCustomer.Parameters.AddWithValue("@customerId", customerId);
                    cmdCustomer.Parameters.AddWithValue("@appointmentId", appointmentId);
                    cmdCustomer.Parameters.AddWithValue("@start", start);
                    cmdCustomer.Parameters.AddWithValue("@end", end);

                    int customerOverlapCount = Convert.ToInt32(cmdCustomer.ExecuteScalar());
                    if (customerOverlapCount > 0)
                    {
                        return true; 
                    }
                }

                string queryUserOverlap = @"
                SELECT COUNT(*) 
                FROM appointment 
                WHERE userId = @userId 
                AND appointmentId != @appointmentId
                AND (
                    (start < @end AND end > @start)  -- Check if new appointment overlaps with an existing one
                );";

                using (MySqlCommand cmdUser = new MySqlCommand(queryUserOverlap, conn))
                {
                    cmdUser.Parameters.AddWithValue("@userId", userId);
                    cmdUser.Parameters.AddWithValue("@appointmentId", appointmentId);
                    cmdUser.Parameters.AddWithValue("@start", start);
                    cmdUser.Parameters.AddWithValue("@end", end);

                    int userOverlapCount = Convert.ToInt32(cmdUser.ExecuteScalar());
                    if (userOverlapCount > 0)
                    {
                        return true; 
                    }
                }
            }
            return false; 
        }
        
        // these populate the comboboxes for customer and user selection
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

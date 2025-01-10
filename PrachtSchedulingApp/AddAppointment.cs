using MySql.Data.MySqlClient;
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

            // this adds time selection to the datetimepickers
            dtpStart.CustomFormat = "MM/dd/yyyy hh:mm tt"; // 12-hour format
            dtpEnd.CustomFormat = "MM/dd/yyyy hh:mm tt"; // 12-hour format
        }


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

                // Attempt to parse SelectedValue as int
                if (!int.TryParse(cboCustomer.SelectedValue.ToString(), out int customerId) ||
                    !int.TryParse(cboUser.SelectedValue.ToString(), out int selectedUserId))
                {
                    MessageBox.Show("Invalid customer or user selection.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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
                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(location) || string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(type))
                {
                    MessageBox.Show("Title, Location, Contact, and Type are required to save the appointment.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Convert start and end times to UTC
                DateTime startUTC = start.ToUniversalTime();
                DateTime endUTC = end.ToUniversalTime();

                if (startUTC >= endUTC)
                {
                    MessageBox.Show("End date cannot be earlier than or equal to the start date.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate business hours in Eastern Standard Time
                try
                {
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
                }
                catch (TimeZoneNotFoundException)
                {
                    MessageBox.Show("The system's time zone configuration is invalid or unsupported.", "Time Zone Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Check for overlapping appointments
                if (IsAppointmentOverlapping(startUTC, endUTC, customerId))
                {
                    MessageBox.Show("The selected time slot overlaps with an existing appointment. Please choose a different time.",
                        "Overlapping Appointment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Insert into database
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

        private bool IsAppointmentOverlapping(DateTime start, DateTime end, int customerId)
        {
            // this checks to see if the customer selected already has an appt on this day/time.
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            string query = @"
            SELECT COUNT(*) 
            FROM appointment 
            WHERE customerId = @customerId 
            AND ((start <= @end AND end >= @start));";  // Overlapping condition

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        cmd.Parameters.AddWithValue("@start", start);
                        cmd.Parameters.AddWithValue("@end", end);

                        int overlappingAppointments = Convert.ToInt32(cmd.ExecuteScalar());
                        return overlappingAppointments > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while checking for overlapping appointments: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

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

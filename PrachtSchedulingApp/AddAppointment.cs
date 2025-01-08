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
            _manageAppointments = manageAppointments;

            // this adds time selection to the datetimepickers
            dtpStart.CustomFormat = "MM/dd/yyyy hh:mm tt"; // 12-hour format
            dtpEnd.CustomFormat = "MM/dd/yyyy hh:mm tt"; // 12-hour format
        }

        private void AddAppointment_Load(object sender, EventArgs e)
        {
            PopulateCustomerComboBox();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Gather values from the form
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

                // Convert start and end times to UTC for storage
                DateTime startUTC = start.ToUniversalTime();
                DateTime endUTC = end.ToUniversalTime();

                // Check if appointment is within business hours (local time)
                TimeZoneInfo estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                DateTime startEST = TimeZoneInfo.ConvertTime(start, estZone);
                DateTime endEST = TimeZoneInfo.ConvertTime(end, estZone);

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

                // Open connection string and write query
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                MySqlConnection con = new MySqlConnection(connectionString);
                con.Open();

                string query = @"
                INSERT INTO appointment 
                (customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy) 
                VALUES 
                (@customerId, @userId, @title, @description, @location, @contact, @type, @url, @start, @end, NOW(), @createdBy, NOW(), @userId);";

                MySqlCommand cmd = new MySqlCommand(query, con);

                // Add parameters to prevent SQL injection
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
                cmd.Parameters.AddWithValue("@createdBy", userId);

                // Execute the query
                cmd.ExecuteNonQuery();

                // Refresh grid if needed
                _manageAppointments?.PopulateGrid();

                MessageBox.Show("New appointment has been added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


        private void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            // Will come back and edit this when the Customer forms are done.
            MessageBox.Show($"This doesn't exist yet! Pardon my dust.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void PopulateCustomerComboBox()
        {
            // This will populate the ComboBox that allows users to select a customer from the db.
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                MySqlConnection con = new MySqlConnection(connectionString);
                con.Open();
                string query = "SELECT customerId, customerName FROM customer WHERE active = 1";
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
    }

}

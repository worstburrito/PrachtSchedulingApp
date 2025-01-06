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
        }
        
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
                            cboUser.SelectedValue = row["userId"];
                            txtTitle.Text = row["title"].ToString();
                            txtDesc.Text = row["description"].ToString();
                            txtLocation.Text = row["location"].ToString();
                            txtContact.Text = row["contact"].ToString();
                            txtType.Text = row["type"].ToString();
                            txtURL.Text = row["url"].ToString();
                            dtpStart.Value = Convert.ToDateTime(row["start"]);
                            dtpEnd.Value = Convert.ToDateTime(row["end"]);
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

        private void PopulateUserComboBox()
        {
            try
            {
                // Open connection string and write query
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                MySqlConnection con = new MySqlConnection(connectionString);

                con.Open();
                string query = "SELECT userId, userName FROM user WHERE active = 1";  // Optional: to filter only active customers
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                DataTable userComboBox = new DataTable();
                adapter.Fill(userComboBox);

                cboUser.DisplayMember = "userName";
                cboUser.ValueMember = "userId";

                cboUser.DataSource = userComboBox;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateCustomerComboBox()
        {
            try
            {
                // Open connection string and write query
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
        private void EditAppointment_Load(object sender, EventArgs e)
        {
            PopulateCustomerComboBox();
            PopulateUserComboBox();

            // Populate form fields with appointment data
            LoadAppointmentData();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // Gather updated values from the form
            int customerId = (int)cboCustomer.SelectedValue;
            int userId = (int)cboUser.SelectedValue;
            string title = txtTitle.Text;
            string description = txtDesc.Text;
            string location = txtLocation.Text;
            string contact = txtContact.Text;
            string type = txtType.Text;
            string url = txtURL.Text;
            DateTime start = dtpStart.Value;
            DateTime end = dtpEnd.Value;
            string updatedBy = cboUser.DisplayMember;

            // Retrieve the connection string from ConfigurationManager
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
                lastUpdateBy = @updatedBy
            WHERE appointmentId = @appointmentId";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open(); // Open the connection

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Add parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@appointmentId", _appointmentId);
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@location", location);
                        cmd.Parameters.AddWithValue("@contact", contact);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@url", url);
                        cmd.Parameters.AddWithValue("@start", start);
                        cmd.Parameters.AddWithValue("@end", end);
                        cmd.Parameters.AddWithValue("@updatedBy", updatedBy);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            _manageAppointments.PopulateGrid();
                            MessageBox.Show($"Appointment has been updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}

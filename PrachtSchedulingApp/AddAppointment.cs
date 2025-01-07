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

namespace PrachtSchedulingApp
{
    public partial class AddAppointment : Form
    {
        private ManageAppointments _manageAppointments;
        public AddAppointment(ManageAppointments manageAppointments = null)
        {
            InitializeComponent();
            _manageAppointments = manageAppointments;
        }

        private void AddAppointment_Load(object sender, EventArgs e)
        {
            PopulateCustomerComboBox();
            PopulateUserComboBox();
            
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

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Getting the selected values and text from the form controls
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
                string createdBy = cboUser.DisplayMember;

                // Open connection string and write query
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                MySqlConnection con = new MySqlConnection(connectionString);
                con.Open();
                string query = @"
            INSERT INTO appointment 
            (customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy) 
            VALUES 
            (@customerId, @userId, @title, @description, @location, @contact, @type, @url, @start, @end, NOW(), @createdBy, NOW(), @createdBy);";

                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                // Add parameters to avoid SQL injection
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
                cmd.Parameters.AddWithValue("@createdBy", createdBy);

                // Execute the query
                cmd.ExecuteNonQuery();

                if (_manageAppointments != null)
                {
                    _manageAppointments.PopulateGrid();
                }

                MessageBox.Show($"New appointment has been added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"This doesn't exist yet! Pardon my dust.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

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
    public partial class AddUser : Form
    {
        private ManageUsers _manageUsers;
        public AddUser(ManageUsers manageUsers = null)
        {
            InitializeComponent();
            _manageUsers = manageUsers;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
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

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Getting the selected values and text from the form controls
                string username = txtUsername.Text;
                int userId = (int)cboUser.SelectedValue;
                int active = 1;
                DateTime createDate = DateTime.Now;
                string password = "password";


                // Open connection string and write query
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                MySqlConnection con = new MySqlConnection(connectionString);
                con.Open();
                string query = @"
                INSERT INTO user
                (userName, password, active, createDate, createdBy, lastUpdate, lastUpdateBy)
                VALUES
                (@username, @password, @active, @createDate, @userId, NOW(), @userId);";

                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                // Add parameters to avoid SQL injection
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@active", active);
                cmd.Parameters.AddWithValue("@createDate", createDate);
                cmd.Parameters.AddWithValue("@userId", userId);

                // Execute the query
                cmd.ExecuteNonQuery();

                if (_manageUsers != null)
                {
                    _manageUsers.PopulateGrid();
                }

                MessageBox.Show($"New user has been added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

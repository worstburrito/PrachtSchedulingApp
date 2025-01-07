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
    public partial class ManageUsers : Form
    {
        public ManageUsers()
        {
            InitializeComponent();
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        public void PopulateGrid()
        {
            try
            {
                // Open connection string and write query
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                MySqlConnection con = new MySqlConnection(connectionString);

                // Open the connection and make a SQL query call
                con.Open();
                string query = "SELECT * FROM user";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable userLocal = new DataTable();
                adapter.Fill(userLocal);

                // Set the data grid source
                dgvManageUsers.DataSource = userLocal;

                // Adjust column headers to preference
                dgvManageUsers.Columns["userId"].Visible = false;
                dgvManageUsers.Columns["password"].Visible = false;
                dgvManageUsers.Columns["userName"].HeaderText = "Username";
                dgvManageUsers.Columns["active"].HeaderText = "Active Status";
                dgvManageUsers.Columns["createDate"].HeaderText = "Date Created";
                dgvManageUsers.Columns["createdBy"].HeaderText = "Created By";
                dgvManageUsers.Columns["lastUpdate"].HeaderText = "Last Update Made";
                dgvManageUsers.Columns["lastUpdateBy"].HeaderText = "Last Updated By";


            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddUser"))
            {
                var addUser = new AddUser(this);
                addUser.MdiParent = this.MdiParent;
                addUser.Show();
            }
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvManageUsers.SelectedRows.Count > 0)
                {
                    // Get the ID of the user to update
                    int userId = Convert.ToInt32(dgvManageUsers.SelectedRows[0].Cells["userId"].Value);

                    // Get the connection string running
                    string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                    using (MySqlConnection con = new MySqlConnection(connectionString))
                    {
                        con.Open();
                        string query = "UPDATE user SET active = 1 WHERE userId = @userId";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@userId", userId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Refresh the grid to show the updated data
                    PopulateGrid();
                    MessageBox.Show("User has been activated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeactive_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvManageUsers.SelectedRows.Count > 0)
                {
                    // Get the ID of the user to update
                    int userId = Convert.ToInt32(dgvManageUsers.SelectedRows[0].Cells["userId"].Value);

                    // Get the connection string running
                    string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                    using (MySqlConnection con = new MySqlConnection(connectionString))
                    {
                        con.Open();
                        string query = "UPDATE user SET active = 0 WHERE userId = @userId";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@userId", userId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Refresh the grid to show the updated data
                    PopulateGrid();
                    MessageBox.Show("User has been deactivated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

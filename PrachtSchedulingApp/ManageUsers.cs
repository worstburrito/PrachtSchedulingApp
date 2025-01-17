﻿using MySql.Data.MySqlClient;
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
using static PrachtSchedulingApp.Login;

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

                // Use 'using' for automatic disposal of the connection and command objects
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    // Open the connection and make a SQL query call
                    con.Open();
                    string query = @"
                    SELECT
                        u.userId,
                        u.userName,
                        u.password,
                        u.active,
                        u.createDate,
                        uc.userName AS createdBy,
                        u.lastUpdate,
                        ul.userName AS lastUpdateBy
                    FROM user u
                    LEFT JOIN user uc ON u.createdBy = uc.userId
                    LEFT JOIN user ul ON u.lastUpdateBy = ul.userId";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable users = new DataTable();
                        adapter.Fill(users);

                        // Convert date/times
                        foreach (DataRow row in users.Rows)
                        {
                            row["createDate"] = ((DateTime)row["createDate"]).ToLocalTime();
                            row["lastUpdate"] = ((DateTime)row["lastUpdate"]).ToLocalTime();
                        }

                        // Set the data grid source
                        dgvManageUsers.DataSource = users;

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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        string query = "UPDATE user SET active = 1, lastUpdate = NOW(), lastUpdateBy = @lastUpdateBy WHERE userId = @userId";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@userId", userId);
                            cmd.Parameters.AddWithValue("@lastUpdateBy", CurrentUser.UserId);
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
                        string query = "UPDATE user SET active = 1, lastUpdate = NOW(), lastUpdateBy = @lastUpdateBy WHERE userId = @userId";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@userId", userId);
                            cmd.Parameters.AddWithValue("@lastUpdateBy", CurrentUser.UserId);
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

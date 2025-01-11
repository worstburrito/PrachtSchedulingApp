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

namespace PrachtSchedulingApp
{
    public partial class ManageCustomerRecords : Form
    {
        public ManageCustomerRecords()
        {
            InitializeComponent();
            PopulateGrid();
        }

        public void PopulateGrid()
        {
            try
            {
                DataTable customers = GetCustomerData();

                // Convert date/times
                foreach (DataRow row in customers.Rows)
                {
                    row["createDate"] = ((DateTime)row["createDate"]).ToLocalTime();
                    row["lastUpdate"] = ((DateTime)row["lastUpdate"]).ToLocalTime();
                }

                DatabaseHelper.PopulateCustomers(dgvManageCustomerRecords, customers);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable GetCustomerData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Database connection string is missing or invalid.");

            DataTable customers = new DataTable();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = @"
                    SELECT
                        c.customerId,
                        c.customerName,
                        a.address AS customerAddress,
                        a.address2 AS customerAddress2,
                        a.postalCode AS postalCode,
                        a.phone AS phone,
                        ci.city AS cityName,
                        co.country AS countryName,
                        CAST(c.active AS CHAR) as active,
                        c.createDate,
                        cu.userName AS createdBy,
                        lu.userName AS lastUpdateBy,
                        c.lastUpdate
                    FROM customer c
                    JOIN address a ON c.addressId = a.addressId
                    JOIN city ci ON a.cityId = ci.cityId
                    JOIN country co ON ci.countryId = co.countryId
                    JOIN `user` cu ON c.createdBy = cu.userId
                    JOIN `user` lu ON c.lastUpdateBy = lu.userId";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(customers);
                }
            }
            return customers;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddCustomer"))
            {
                var addCustomer = new AddCustomer(this);
                addCustomer.MdiParent = this.MdiParent;
                addCustomer.Show();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvManageCustomerRecords.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to remove.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["localdb"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("Database connection string is missing or invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Retrieve customerId from the selected row
            int customerId;
            DataGridViewRow selectedRow = dgvManageCustomerRecords.SelectedRows[0];
            if (!int.TryParse(selectedRow.Cells["customerId"].Value?.ToString(), out customerId))
            {
                MessageBox.Show("Unable to retrieve Customer ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Confirm deletion
            DialogResult confirmation = MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmation != DialogResult.Yes)
            {
                return; // User chose not to delete
            }

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlTransaction transaction = con.BeginTransaction();
                try
                {
                    // Check if the customer is linked to appointments
                    string checkAppointmentsQuery = "SELECT COUNT(*) FROM appointment WHERE customerId = @customerId;";
                    using (MySqlCommand cmd = new MySqlCommand(checkAppointmentsQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        int appointmentCount = Convert.ToInt32(cmd.ExecuteScalar());
                        if (appointmentCount > 0)
                        {
                            MessageBox.Show("This customer has associated appointments and cannot be removed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Get the addressId of the customer
                    int addressId;
                    string getAddressIdQuery = "SELECT addressId FROM customer WHERE customerId = @customerId;";
                    using (MySqlCommand cmd = new MySqlCommand(getAddressIdQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        object result = cmd.ExecuteScalar();
                        if (result == null)
                        {
                            MessageBox.Show("Customer not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        addressId = Convert.ToInt32(result);
                    }

                    // Delete the customer
                    string deleteCustomerQuery = "DELETE FROM customer WHERE customerId = @customerId;";
                    using (MySqlCommand cmd = new MySqlCommand(deleteCustomerQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        cmd.ExecuteNonQuery();
                    }

                    // Delete the address
                    string deleteAddressQuery = "DELETE FROM address WHERE addressId = @addressId;";
                    using (MySqlCommand cmd = new MySqlCommand(deleteAddressQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@addressId", addressId);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    PopulateGrid();
                    MessageBox.Show("Customer removed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Error while removing customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

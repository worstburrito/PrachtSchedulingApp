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
using static PrachtSchedulingApp.Login;

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
                LEFT JOIN address a ON c.addressId = a.addressId
                LEFT JOIN city ci ON a.cityId = ci.cityId
                LEFT JOIN country co ON ci.countryId = co.countryId
                LEFT JOIN `user` cu ON c.createdBy = cu.userId
                LEFT JOIN `user` lu ON c.lastUpdateBy = lu.userId;";

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
            if (dgvManageCustomerRecords.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int customerId = Convert.ToInt32(dgvManageCustomerRecords.SelectedRows[0].Cells["customerId"].Value);
            EditCustomer editCustomerForm = new EditCustomer(customerId, this);
            editCustomerForm.MdiParent = this.MdiParent;
            editCustomerForm.Show();
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

            int customerId;
            DataGridViewRow selectedRow = dgvManageCustomerRecords.SelectedRows[0];
            if (!int.TryParse(selectedRow.Cells["customerId"].Value?.ToString(), out customerId))
            {
                MessageBox.Show("Unable to retrieve Customer ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult confirmation = MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmation != DialogResult.Yes)
            {
                return;
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

        private void btnActivate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvManageCustomerRecords.SelectedRows.Count > 0)
                {
                    int customerId = Convert.ToInt32(dgvManageCustomerRecords.SelectedRows[0].Cells["customerId"].Value);

                    string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                    using (MySqlConnection con = new MySqlConnection(connectionString))
                    {
                        con.Open();
                        string query = "UPDATE customer SET active = 1, lastUpdate = NOW(), lastUpdateBy = @lastUpdateBy WHERE customerId = @customerId";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@customerId", customerId);
                            cmd.Parameters.AddWithValue("@lastUpdateBy", CurrentUser.UserId); 
                            cmd.ExecuteNonQuery();
                        }
                    }

                    PopulateGrid();
                    MessageBox.Show("Customer has been activated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvManageCustomerRecords.SelectedRows.Count > 0)
                {
                    int customerId = Convert.ToInt32(dgvManageCustomerRecords.SelectedRows[0].Cells["customerId"].Value);

                    string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                    using (MySqlConnection con = new MySqlConnection(connectionString))
                    {
                        con.Open();
                        string query = "UPDATE customer SET active = 0, lastUpdate = NOW(), lastUpdateBy = @lastUpdateBy WHERE customerId = @customerId";
                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@customerId", customerId);
                            cmd.Parameters.AddWithValue("@lastUpdateBy", CurrentUser.UserId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    PopulateGrid();
                    MessageBox.Show("Customer has been deactivated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

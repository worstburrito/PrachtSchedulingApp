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
    public partial class AddCustomer : Form
    {
        private ManageCustomerRecords _manageCustomerRecords;
        public AddCustomer(ManageCustomerRecords manageCustomerRecords = null)
        {
            InitializeComponent();
            _manageCustomerRecords = manageCustomerRecords;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("Database connection string is missing or invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string customerName = txtCustomerName.Text.Trim();
            string address = txtAddress.Text.Trim();
            string address2 = txtAddress2.Text.Trim();
            string postalCode = txtPostalCode.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string city = txtCity.Text.Trim();
            string country = txtCountry.Text.Trim();
            int userId = CurrentUser.UserId;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlTransaction transaction = con.BeginTransaction();
                try
                {
                    int countryId;
                    int cityId;
                    int addressId;

                    // Insert into Country table
                    string insertCountryQuery = @"
                INSERT INTO country (country, createDate, createdBy, lastUpdate, lastUpdateBy)
                VALUES (@country, NOW(), @userId, NOW(), @userId);
                SELECT LAST_INSERT_ID();";
                    using (MySqlCommand cmd = new MySqlCommand(insertCountryQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@country", country);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        countryId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Insert into City table
                    string insertCityQuery = @"
                INSERT INTO city (city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy)
                VALUES (@city, @countryId, NOW(), @userId, NOW(), @userId);
                SELECT LAST_INSERT_ID();";
                    using (MySqlCommand cmd = new MySqlCommand(insertCityQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@city", city);
                        cmd.Parameters.AddWithValue("@countryId", countryId);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cityId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Insert into Address table
                    string insertAddressQuery = @"
                INSERT INTO address (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy)
                VALUES (@address, @address2, @cityId, @postalCode, @phone, NOW(), @userId, NOW(), @userId);
                SELECT LAST_INSERT_ID();";
                    using (MySqlCommand cmd = new MySqlCommand(insertAddressQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@address2", address2);
                        cmd.Parameters.AddWithValue("@cityId", cityId);
                        cmd.Parameters.AddWithValue("@postalCode", postalCode);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        addressId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Insert into Customer table
                    string insertCustomerQuery = @"
                INSERT INTO customer (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy)
                VALUES (@customerName, @addressId, 1, NOW(), @userId, NOW(), @userId);";
                    using (MySqlCommand cmd = new MySqlCommand(insertCustomerQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@customerName", customerName);
                        cmd.Parameters.AddWithValue("@addressId", addressId);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    _manageCustomerRecords?.PopulateGrid();
                    MessageBox.Show("Customer added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Error while adding customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

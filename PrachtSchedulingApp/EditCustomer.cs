using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PrachtSchedulingApp.Login;

namespace PrachtSchedulingApp
{
    public partial class EditCustomer : Form
    {
        private int _customerId;
        private ManageCustomerRecords _manageCustomerRecords;
        public EditCustomer(int customerId, ManageCustomerRecords manageCustomerRecords = null)
        {
            InitializeComponent();
            _customerId = customerId;
            _manageCustomerRecords = manageCustomerRecords;
            LoadCustomerDetails();
        }

        private void LoadCustomerDetails()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("Database connection string is missing or invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = @"
                SELECT c.customerName, a.address, a.address2, a.postalCode, a.phone, ci.city, co.country
                FROM customer c
                JOIN address a ON c.addressId = a.addressId
                JOIN city ci ON a.cityId = ci.cityId
                JOIN country co ON ci.countryId = co.countryId
                WHERE c.customerId = @customerId;";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@customerId", _customerId);
                    con.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtCustomerName.Text = reader["customerName"].ToString();
                            txtAddress.Text = reader["address"].ToString();
                            txtAddress2.Text = reader["address2"].ToString();
                            txtPostalCode.Text = reader["postalCode"].ToString();
                            txtPhone.Text = reader["phone"].ToString();
                            txtCity.Text = reader["city"].ToString();
                            txtCountry.Text = reader["country"].ToString();
                        }
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
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

            // Validate required fields
            if (string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(postalCode) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
            {
                MessageBox.Show("You are missing fields required to save this customer.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string phonePattern = @"^\d{3}-\d{3}-\d{4}$";
            if (!Regex.IsMatch(phone, phonePattern))
            {
                MessageBox.Show("Phone number must be in the format 000-000-0000.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlTransaction transaction = con.BeginTransaction();
                try
                {
                    int countryId, cityId, addressId;

                    // Get countryId or insert new country
                    string checkCountryQuery = "SELECT countryId FROM country WHERE country = @country;";
                    using (MySqlCommand cmd = new MySqlCommand(checkCountryQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@country", country);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            countryId = Convert.ToInt32(result);
                        }
                        else
                        {
                            string insertCountryQuery = @"
                        INSERT INTO country (country, createDate, createdBy, lastUpdate, lastUpdateBy)
                        VALUES (@country, NOW(), @userId, NOW(), @userId);
                        SELECT LAST_INSERT_ID();";
                            using (MySqlCommand insertCmd = new MySqlCommand(insertCountryQuery, con, transaction))
                            {
                                insertCmd.Parameters.AddWithValue("@country", country);
                                insertCmd.Parameters.AddWithValue("@userId", userId);
                                countryId = Convert.ToInt32(insertCmd.ExecuteScalar());
                            }
                        }
                    }

                    // Get cityId or insert new city
                    string checkCityQuery = "SELECT cityId FROM city WHERE city = @city AND countryId = @countryId;";
                    using (MySqlCommand cmd = new MySqlCommand(checkCityQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@city", city);
                        cmd.Parameters.AddWithValue("@countryId", countryId);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            cityId = Convert.ToInt32(result);
                        }
                        else
                        {
                            string insertCityQuery = @"
                        INSERT INTO city (city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy)
                        VALUES (@city, @countryId, NOW(), @userId, NOW(), @userId);
                        SELECT LAST_INSERT_ID();";
                            using (MySqlCommand insertCmd = new MySqlCommand(insertCityQuery, con, transaction))
                            {
                                insertCmd.Parameters.AddWithValue("@city", city);
                                insertCmd.Parameters.AddWithValue("@countryId", countryId);
                                insertCmd.Parameters.AddWithValue("@userId", userId);
                                cityId = Convert.ToInt32(insertCmd.ExecuteScalar());
                            }
                        }
                    }

                    // Update or insert address
                    string getAddressIdQuery = "SELECT addressId FROM customer WHERE customerId = @customerId;";
                    using (MySqlCommand cmd = new MySqlCommand(getAddressIdQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@customerId", _customerId);
                        addressId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    string updateAddressQuery = @"
                UPDATE address
                SET address = @address, address2 = @address2, cityId = @cityId, postalCode = @postalCode, phone = @phone, 
                    lastUpdate = NOW(), lastUpdateBy = @userId
                WHERE addressId = @addressId;";
                    using (MySqlCommand cmd = new MySqlCommand(updateAddressQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@address2", address2);
                        cmd.Parameters.AddWithValue("@cityId", cityId);
                        cmd.Parameters.AddWithValue("@postalCode", postalCode);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@addressId", addressId);
                        cmd.ExecuteNonQuery();
                    }

                    // Update customer
                    string updateCustomerQuery = @"
                UPDATE customer
                SET customerName = @customerName, lastUpdate = NOW(), lastUpdateBy = @userId
                WHERE customerId = @customerId;";
                    using (MySqlCommand cmd = new MySqlCommand(updateCustomerQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@customerName", customerName);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@customerId", _customerId);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    _manageCustomerRecords?.PopulateGrid();
                    MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Error while updating customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}

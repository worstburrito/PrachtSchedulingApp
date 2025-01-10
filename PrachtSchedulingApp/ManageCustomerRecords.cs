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
                        c.createdBy,
                        c.lastUpdate,
                        c.lastUpdateBy
                    FROM customer c
                    JOIN address a ON c.addressId = a.addressId
                    JOIN city ci ON a.cityId = ci.cityId
                    JOIN country co ON ci.countryId = co.countryId";

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

        }
    }
}

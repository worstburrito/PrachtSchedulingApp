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
        }

        private void ManageCustomerRecords_Load(object sender, EventArgs e)
        {
            try
            {
                // Open connection string and write query
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                MySqlConnection con = new MySqlConnection(connectionString);

                con.Open();
                string query = "SELECT * FROM customer";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                DataTable custUTC = new DataTable();
                DataTable custLocal = new DataTable();
                adapter.Fill(custUTC);
                adapter.Fill(custLocal);

                // Convert UTC to Local Time
                for (int i = 0; i < custUTC.Rows.Count; i++)
                {
                    DateTime x = (DateTime)custUTC.Rows[i]["createDate"];
                    custLocal.Rows[i]["createDate"] = x.ToLocalTime();

                    DateTime y = (DateTime)custUTC.Rows[i]["lastUpdate"];
                    custLocal.Rows[i]["lastUpdate"] = y.ToLocalTime();
                }

                // Set Data Grid View - Data Source
                dgvManageCustomerRecords.DataSource = custLocal;

                // Adjust column headers to preference
                dgvManageCustomerRecords.Columns["customerId"].Visible = false;
                dgvManageCustomerRecords.Columns["addressId"].Visible = false;
                dgvManageCustomerRecords.Columns["customerName"].HeaderText = "Name";
                dgvManageCustomerRecords.Columns["active"].HeaderText = "Active Status";
                dgvManageCustomerRecords.Columns["createDate"].HeaderText = "Date Created";
                dgvManageCustomerRecords.Columns["createdBy"].HeaderText = "Created By";
                dgvManageCustomerRecords.Columns["lastUpdate"].HeaderText = "Last Updated";
                dgvManageCustomerRecords.Columns["lastUpdateBy"].HeaderText = "Last Updated By";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}

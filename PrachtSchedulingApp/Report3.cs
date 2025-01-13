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
    public partial class Report3 : Form
    {
        public Report3()
        {
            InitializeComponent();
            PopulateCustomerComboBox();
        }

        private void PopulateCustomerComboBox()
        {
            string query = "SELECT customerId, customerName FROM customer WHERE active = 1";
            DatabaseHelper.PopulateCustomerComboBox(cboCustomer, query, "customerName", "customerId");
        }

        private void btnRunReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboCustomer.SelectedValue == null || cboCustomer.Text == null)
                {
                    MessageBox.Show("Please select a valid customer.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int selectedCustomerId = Convert.ToInt32(cboCustomer.SelectedValue);
                string selectedCustomerName = cboCustomer.Text;

                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"
                SELECT 
                    a.customerId AS CustomerId,
                    c.customerName AS CustomerName,
                    a.appointmentId AS AppointmentId
                FROM 
                    appointment a
                INNER JOIN 
                    customer c ON a.customerId = c.customerId";

                    MySqlCommand cmd = new MySqlCommand(query, con);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable appointmentData = new DataTable();
                    adapter.Fill(appointmentData);

                    var appointmentCounts = appointmentData.AsEnumerable()
                        .GroupBy(row => new
                        {
                            CustomerId = row.Field<int>("CustomerId"),
                            CustomerName = row.Field<string>("CustomerName")
                        })
                        .Select(group => new
                        {
                            CustomerId = group.Key.CustomerId,
                            CustomerName = group.Key.CustomerName,
                            AppointmentCount = group.Count()
                        })
                        .ToList();

                    var selectedCustomer = appointmentCounts.FirstOrDefault(c => c.CustomerId == selectedCustomerId);

                    StringBuilder report = new StringBuilder();
                    if (selectedCustomer != null)
                    {
                        report.AppendLine($"Appointments for Customer: {selectedCustomer.CustomerName} (ID: {selectedCustomer.CustomerId})");
                        report.AppendLine(new string('-', 50));
                        report.AppendLine($"Total Appointments: {selectedCustomer.AppointmentCount}");
                    }
                    else
                    {
                        report.AppendLine($"No appointments found for Customer: {selectedCustomerName} (ID: {selectedCustomerId})");
                    }

                    rtbReport.Text = report.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

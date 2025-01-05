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
    public partial class MainWindow : Form
    {
        private Login _login;
        public MainWindow(Login login)
        {
            InitializeComponent();
            _login = login;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            // Read the connection string from app.config
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            try
            {
                // Create a connection object
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();
                    MessageBox.Show($"Connection to the database was successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Example query
                    string query = "SELECT * FROM appointment;";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MessageBox.Show($"Row: {reader[0]}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            _login.Close();
        }

        private void viewCustomerRecords_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("ManageCustomerRecords"))
            {
                var manageCustomerRecords = new ManageCustomerRecords();
                manageCustomerRecords.MdiParent = this;
                manageCustomerRecords.Show();
            }
        }

        private void viewAllAppointments_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("ManageAppointments"))
            {
                var manageAppointments = new ManageAppointments();
                manageAppointments.MdiParent = this;
                manageAppointments.Show();
            }
        }

        private void viewSchedule_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("CalendarView"))
            {
                var calendarView = new CalendarView();
                calendarView.MdiParent = this;
                calendarView.Show();
            }
        }
    }
}

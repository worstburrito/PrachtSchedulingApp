using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using static PrachtSchedulingApp.Login;
using MessageBox = System.Windows.Forms.MessageBox;

namespace PrachtSchedulingApp
{
    public partial class MainWindow : Form
    {
        private Login _login;
        private int currentUserId;
        private string currentUserName;
        public MainWindow(Login login)
        {
            InitializeComponent();
            _login = login;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            currentUserId = CurrentUser.UserId;
            currentUserName = CurrentUser.Username;
            CheckUpcomingAppointments(currentUserId);
            lblUserStatus.Text = $"Currently logged in as: {currentUserName}";
        }

        public void CheckUpcomingAppointments(int userId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;

            // Get the current UTC time to compare with the appointment start time
            DateTime currentTime = DateTime.UtcNow; // UTC time to compare

            // Query to find appointments within 15 minutes of the current time
            string query = @"SELECT appointmentId, title, start 
                     FROM appointment 
                     WHERE userId = @userId 
                     AND start BETWEEN @currentTime AND @currentTimePlus15Minutes";

            // Adjust the current time to calculate the time range (current time + 15 minutes)
            DateTime currentTimePlus15Minutes = currentTime.AddMinutes(15);

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@currentTime", currentTime);
                        cmd.Parameters.AddWithValue("@currentTimePlus15Minutes", currentTimePlus15Minutes);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    // Read appointment data
                                    string appointmentTitle = reader.GetString("title");
                                    DateTime appointmentStart = reader.GetDateTime("start");

                                    // Convert the appointment start time from UTC to local time
                                    appointmentStart = appointmentStart.ToLocalTime();

                                    // Check if the appointment is within 15 minutes of the current time
                                    if (appointmentStart <= currentTimePlus15Minutes)
                                    {
                                        MessageBox.Show($"You have an upcoming appointment: {appointmentTitle} at {appointmentStart}.", "Upcoming Appointment", MessageBoxButtons.OK);
                                        
                                        if (!Utils.FormIsOpen("CalendarView"))
                                        {
                                            var calendarView = new CalendarView();
                                            calendarView.MdiParent = this;
                                            calendarView.Show();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // If no appointments were found - deciding whether or not to show this.
                                // MessageBox.Show("No upcoming appointments within the next 15 minutes.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            _login.Close();
        }

        private void viewCustomerRecords_Click(object sender, EventArgs e)
        {
            
        }

        private void viewAllAppointments_Click(object sender, EventArgs e)
        {
            
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

        private void addAppointment_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddAppointment"))
            {
                var addAppointment = new AddAppointment();
                addAppointment.MdiParent = this;
                addAppointment.Show();
            }
        }

        private void addCustomerRecords_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddCustomer"))
            {
                var addCustomer = new AddCustomer();
                addCustomer.MdiParent = this;
                addCustomer.Show();
            }

        }

        private void Report1_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddAppointment"))
            {
                var report1 = new Report1();
                report1.MdiParent = this;
                report1.Show();
            }
        }

        private void Report2_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddAppointment"))
            {
                var report2 = new Report2();
                report2.MdiParent = this;
                report2.Show();
            }
        }

        private void Report3_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddAppointment"))
            {
                var loginHistoryReport = new LoginHistoryReport();
                loginHistoryReport.MdiParent = this;
                loginHistoryReport.Show();
            }
        }

        private void Report4_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddAppointment"))
            {
                var loginHistoryReport = new LoginHistoryReport();
                loginHistoryReport.MdiParent = this;
                loginHistoryReport.Show();
            }
        }

        private void LoginHistory_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddAppointment"))
            {
                var loginHistoryReport = new LoginHistoryReport();
                loginHistoryReport.MdiParent = this;
                loginHistoryReport.Show();
            }
        }
        
               private void manageUsers_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("ManageUsers"))
            {
                var manageUsers = new ManageUsers();
                manageUsers.MdiParent = this;
                manageUsers.Show();
            }
        }

        private void manageCustomerRecords_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("ManageCustomerRecords"))
            {
                var manageCustomerRecords = new ManageCustomerRecords();
                manageCustomerRecords.MdiParent = this;
                manageCustomerRecords.Show();
            }
        }

        private void manageAppointments_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("ManageAppointments"))
            {
                var manageAppointments = new ManageAppointments();
                manageAppointments.MdiParent = this;
                manageAppointments.Show();
            }
        }
    }
}

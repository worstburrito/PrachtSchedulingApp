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
            string loggedInUser = CurrentUser.Username;
            lblUserStatus.Text = $"Currently logged in as: {loggedInUser}";
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
    }
}

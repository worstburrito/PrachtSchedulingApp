using System;
using System.Collections.Generic;
using System.ComponentModel;
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

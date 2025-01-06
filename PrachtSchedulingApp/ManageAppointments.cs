using MySql.Data.MySqlClient;
using PrachtSchedulingApp.Database;
using System;
using System.Collections;
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
    public partial class ManageAppointments : Form
    {
        public ManageAppointments()
        {
            InitializeComponent();
        }

        private void ManageAppointments_Load(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost;Port=3306;Database=client_schedule;Username=sqlUser;Password=Passw0rd!;";
            string query = "SELECT * FROM appointment";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dgvManageAppointments.DataSource = table;

                        dgvManageAppointments.Columns["appointmentId"].Visible = false;
                        dgvManageAppointments.Columns["customerId"].Visible = false;
                        dgvManageAppointments.Columns["userId"].Visible = false;
                        dgvManageAppointments.Columns["title"].HeaderText = "Title";
                        dgvManageAppointments.Columns["description"].HeaderText = "Description";
                        dgvManageAppointments.Columns["location"].HeaderText = "Location";
                        dgvManageAppointments.Columns["contact"].HeaderText = "Contact";
                        dgvManageAppointments.Columns["type"].HeaderText = "Meeting Type";
                        dgvManageAppointments.Columns["url"].HeaderText = "URL";
                        dgvManageAppointments.Columns["start"].HeaderText = "Start";
                        dgvManageAppointments.Columns["end"].HeaderText = "End";
                        dgvManageAppointments.Columns["createDate"].Visible = false;
                        dgvManageAppointments.Columns["createdBy"].Visible = false;
                        dgvManageAppointments.Columns["lastUpdate"].Visible = false;
                        dgvManageAppointments.Columns["lastUpdateBy"].Visible = false;

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}

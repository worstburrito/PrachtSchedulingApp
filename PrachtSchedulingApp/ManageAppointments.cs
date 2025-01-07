using MySql.Data.MySqlClient;
using PrachtSchedulingApp.Database;
using System;
using System.Collections;
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
    public partial class ManageAppointments : Form
    {
        public ManageAppointments()
        {
            InitializeComponent();
        }

        public void PopulateGrid()
        {
            try
            {
                // Open connection string and write query
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                MySqlConnection con = new MySqlConnection(connectionString);

                con.Open();
                string query = "SELECT * FROM appointment";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                DataTable apptsUTC = new DataTable();
                DataTable apptsLocal = new DataTable();
                adapter.Fill(apptsUTC);
                adapter.Fill(apptsLocal);

                // Convert UTC to Local Time
                for (int i = 0; i < apptsUTC.Rows.Count; i++)
                {
                    DateTime x = (DateTime)apptsUTC.Rows[i]["start"];
                    apptsLocal.Rows[i]["start"] = x.ToLocalTime();

                    DateTime y = (DateTime)apptsUTC.Rows[i]["end"];
                    apptsLocal.Rows[i]["end"] = y.ToLocalTime();
                }

                // Set DataSource
                dgvManageAppointments.DataSource = apptsLocal;


                // Adjust column headers to preference
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
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ManageAppointments_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("ManageCustomerRecords"))
            {
                var addAppointment = new AddAppointment(this);
                addAppointment.MdiParent = this.MdiParent;
                addAppointment.Show();
            }
        }

        private void btnUpdateRecord_Click(object sender, EventArgs e)
        {
            if (dgvManageAppointments.SelectedRows.Count > 0)
            {
                int appointmentId = Convert.ToInt32(dgvManageAppointments.SelectedRows[0].Cells["appointmentId"].Value);

                EditAppointment editAppointment = new EditAppointment(appointmentId, this);
                editAppointment.MdiParent = this.MdiParent;
                editAppointment.Show();

            }
            else
            {
                MessageBox.Show($"Please select a row to proceed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            // Ensure a row is selected
            if (dgvManageAppointments.SelectedRows.Count > 0)
            {
                // Get the appointmentId of the selected row
                int appointmentId = Convert.ToInt32(dgvManageAppointments.SelectedRows[0].Cells["appointmentId"].Value);

                // Confirm deletion
                var confirmResult = MessageBox.Show("Are you sure you want to delete this appointment?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    // Retrieve the connection string from ConfigurationManager
                    string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                    string query = "DELETE FROM appointment WHERE appointmentId = @appointmentId";

                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            conn.Open(); // Open the connection

                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                // Add the appointmentId parameter
                                cmd.Parameters.AddWithValue("@appointmentId", appointmentId);

                                // Execute the delete query
                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Refresh the grid to reflect changes
                                    PopulateGrid();
                                    MessageBox.Show("Appointment has been deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Deletion failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle errors and display a message
                            MessageBox.Show($"An error occurred: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                // Inform the user to select a row if none is selected
                MessageBox.Show("Please select an appointment to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

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

        // Manage Appointments Form on Load
        private void ManageAppointments_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        // Add Appointment Button - opens Add Appointment Form
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("ManageCustomerRecords"))
            {
                var addAppointment = new AddAppointment(this);
                addAppointment.MdiParent = this.MdiParent;
                addAppointment.Show();
            }
        }

        // Update Appointment Button - opens Edit Appointment Form
        private void btnUpdateRecord_Click(object sender, EventArgs e)
        {
            // Ensure a row is selected
            if (dgvManageAppointments.SelectedRows.Count > 0)
            {
                // Get the appointmentId of the selected row
                int appointmentId = Convert.ToInt32(dgvManageAppointments.SelectedRows[0].Cells["appointmentId"].Value);

                // Open Add Appointment window and pass over appointmentId
                EditAppointment editAppointment = new EditAppointment(appointmentId, this);
                editAppointment.MdiParent = this.MdiParent;
                editAppointment.Show();

            }
            else
            {
                // Inform the user to select a row if none is selected
                MessageBox.Show($"Please select a row to proceed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Delete Appointment Button - acts on Manage Appointments window
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

        // Populate Grid with Appointments Database and join ids with corresponding databases
        public void PopulateGrid()
        {
            try
            {
                // Open connection string and write query
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"
                    SELECT 
                        a.appointmentId,
                        c.customerName AS CustomerName,
                        u1.userName AS UserName,
                        a.title,
                        a.description,
                        a.location,
                        a.contact,
                        a.type,
                        a.url,
                        a.start,
                        a.end,
                        a.createDate,
                        u3.userName AS CreatedBy,
                        a.lastUpdate,
                        u2.userName AS LastUpdatedBy
                    FROM 
                        appointment a
                    JOIN 
                        customer c ON a.customerId = c.customerId
                    JOIN 
                        user u1 ON a.userId = u1.userId
                    JOIN 
                        user u2 ON a.lastUpdateBy = u2.userId
                    JOIN 
                        user u3 ON a.createdBy = u3.userId";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    DataTable apptsUTC = new DataTable();
                    DataTable apptsLocal = new DataTable();
                    adapter.Fill(apptsUTC);
                    apptsLocal = apptsUTC.Copy();

                    // Convert UTC to Local Time
                    foreach (DataRow row in apptsLocal.Rows)
                    {
                        row["start"] = ((DateTime)row["start"]).ToLocalTime();
                        row["end"] = ((DateTime)row["end"]).ToLocalTime();
                    }

                    // Set DataSource
                    dgvManageAppointments.DataSource = apptsLocal;

                    // Adjust column headers to preference
                    dgvManageAppointments.Columns["appointmentId"].Visible = false;
                    dgvManageAppointments.Columns["CustomerName"].HeaderText = "Customer";
                    dgvManageAppointments.Columns["UserName"].HeaderText = "User";
                    dgvManageAppointments.Columns["title"].HeaderText = "Title";
                    dgvManageAppointments.Columns["description"].HeaderText = "Description";
                    dgvManageAppointments.Columns["location"].HeaderText = "Location";
                    dgvManageAppointments.Columns["contact"].HeaderText = "Contact";
                    dgvManageAppointments.Columns["type"].HeaderText = "Meeting Type";
                    dgvManageAppointments.Columns["url"].HeaderText = "URL";
                    dgvManageAppointments.Columns["start"].HeaderText = "Start";
                    dgvManageAppointments.Columns["end"].HeaderText = "End";
                    dgvManageAppointments.Columns["CreatedBy"].HeaderText = "Created By";
                    dgvManageAppointments.Columns["createDate"].HeaderText = "Creation Date";
                    dgvManageAppointments.Columns["LastUpdatedBy"].HeaderText = "Last Updated By";
                    dgvManageAppointments.Columns["lastUpdate"].HeaderText = "Last Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

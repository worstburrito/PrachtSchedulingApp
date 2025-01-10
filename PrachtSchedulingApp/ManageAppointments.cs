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

        private void btnUpdateRecord_Click(object sender, EventArgs e)
        {
            // Ensure a row is selected
            if (dgvAppointments.SelectedRows.Count > 0)
            {
                // Get the appointmentId of the selected row
                int appointmentId = Convert.ToInt32(dgvAppointments.SelectedRows[0].Cells["appointmentId"].Value);

                // Open Add Appointment window and pass over appointmentId
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
            if (dgvAppointments.SelectedRows.Count > 0)
            {
                // Get the appointmentId of the selected row
                int appointmentId = Convert.ToInt32(dgvAppointments.SelectedRows[0].Cells["appointmentId"].Value);

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

        public void PopulateGrid()
        {
            
            try
            {

                DataTable appointments = GetAppointmentData();
                DatabaseHelper.TimeHelper(appointments, "start", "end", "createDate", "lastUpdate");
                //ConvertToLocalTime(appointments);
                DatabaseHelper.PopulateAppointments(dgvAppointments, appointments);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Configuration error: {ex.Message}", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable GetAppointmentData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Database connection string is missing or invalid.");

            DataTable appts = new DataTable();
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

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(appts);
                }
            }
            return appts;
        }
    }
}

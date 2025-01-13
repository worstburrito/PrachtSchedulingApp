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
    public partial class Report2 : Form
    {
        public Report2()
        {
            InitializeComponent();
            PopulateUserComboBox();
        }

        private void btnFindAppointments_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboUser.SelectedValue == null || cboUser.Text == null)
                {
                    MessageBox.Show("Please select a valid user.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int selectedUserId = Convert.ToInt32(cboUser.SelectedValue);
                string selectedUsername = cboUser.Text;

                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"
                SELECT 
                    a.title AS Title, 
                    a.description AS Description, 
                    a.location AS Location, 
                    a.contact AS Contact, 
                    a.type AS Type, 
                    a.url AS URL, 
                    a.start AS StartDate, 
                    a.end AS EndDate
                FROM 
                    appointment a
                WHERE 
                    a.userId = @UserId";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UserId", selectedUserId);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable appointmentData = new DataTable();
                    adapter.Fill(appointmentData);

                    var sortedAppointments = appointmentData.AsEnumerable()
                        .OrderBy(row => row.Field<DateTime>("StartDate"))
                        .Select(row => new
                        {
                            Title = row.Field<string>("Title"),
                            Description = row.Field<string>("Description"),
                            Location = row.Field<string>("Location"),
                            Contact = row.Field<string>("Contact"),
                            Type = row.Field<string>("Type"),
                            URL = row.Field<string>("URL"),
                            StartDate = TimeZoneInfo.ConvertTimeFromUtc(row.Field<DateTime>("StartDate"), TimeZoneInfo.Local),
                            EndDate = TimeZoneInfo.ConvertTimeFromUtc(row.Field<DateTime>("EndDate"), TimeZoneInfo.Local)
                        })
                        .ToList();

                    StringBuilder report = new StringBuilder();
                    report.AppendLine($"Appointment Schedule for Username: {selectedUsername}");
                    report.AppendLine(new string('-', 50));

                    foreach (var appointment in sortedAppointments)
                    {
                        report.AppendLine($"Title: {appointment.Title}");
                        report.AppendLine($"Description: {appointment.Description}");
                        report.AppendLine($"Location: {appointment.Location}");
                        report.AppendLine($"Contact: {appointment.Contact}");
                        report.AppendLine($"Type: {appointment.Type}");
                        report.AppendLine($"URL: {appointment.URL}");
                        report.AppendLine($"Start: {appointment.StartDate}");
                        report.AppendLine($"End: {appointment.EndDate}");
                        report.AppendLine(new string('-', 50));
                    }

                    rtbReport.Text = report.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateUserComboBox()
        {
            string query = "SELECT userId, userName FROM user WHERE active = 1";
            DatabaseHelper.PopulateUserComboBox(cboUser, query, "userName", "userId");
        }
    }
}

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
    public partial class Report1 : Form
    {
        public Report1()
        {
            InitializeComponent();
        }

        private void btnRunReport_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"
                    SELECT 
                        a.start AS StartDate, 
                        a.type AS AppointmentType
                    FROM 
                        appointment a";

                    MySqlCommand cmd = new MySqlCommand(query, con);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable appointmentsData = new DataTable();
                    adapter.Fill(appointmentsData);


                    var appointmentReport = appointmentsData.AsEnumerable()
                        .GroupBy(row => new
                        {
                            Month = ((DateTime)row["StartDate"]).ToString("yyyy-MM"),
                            Type = row["AppointmentType"].ToString()
                        })
                        .Select(group => new
                        {
                            Month = group.Key.Month,
                            Type = group.Key.Type,
                            Count = group.Count()
                        })
                        .OrderBy(item => item.Month)
                        .ThenBy(item => item.Type)
                        .ToList();

                    StringBuilder output = new StringBuilder();
                    output.AppendLine("Appointment Types By Month:");
                    output.AppendLine("---------------------------");

                    foreach (var item in appointmentReport)
                    {
                        output.AppendLine($"Month: {item.Month}, Type: {item.Type}, Count: {item.Count}");
                    }

                    rtbReport.Text = output.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

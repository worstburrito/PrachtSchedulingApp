using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrachtSchedulingApp
{
    public partial class LoginHistoryReport : Form
    {
        public LoginHistoryReport()
        {
            InitializeComponent();
        }

        private void btnRunReport_Click(object sender, EventArgs e)
        {
            // Get the root project directory
            string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.FullName;

            // Ensure projectRoot is not null
            if (string.IsNullOrEmpty(projectRoot))
            {
                MessageBox.Show("Unable to determine the project root directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Construct the correct log file path in the project root directory
            string logPath = Path.Combine(projectRoot, "Login_History.txt");

            try
            {
                if (File.Exists(logPath))
                {
                    rtbReport.Clear();
                    rtbReport.Lines = File.ReadAllLines(logPath)
                                          .Reverse()  
                                          .Take(25)   
                                          .ToArray();
                }
                else
                {
                    MessageBox.Show("Log file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

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
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Directory.GetParent(baseDirectory)?.Parent?.Parent?.FullName;

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
                // Read all lines from the log file
                if (File.Exists(logPath))
                {
                    var allLines = File.ReadAllLines(logPath);

                    // Get the last 10 lines
                    var last10Lines = allLines.Reverse().Take(10).Reverse();

                    // Display the lines in the RichTextBox
                    rtbReport.Clear();
                    rtbReport.Lines = last10Lines.ToArray();
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

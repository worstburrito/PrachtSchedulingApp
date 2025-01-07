using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrachtSchedulingApp
{
    public partial class Login : Form
    {
        
        public Login()
        {
            InitializeComponent();
            

        }

        private bool ValidateLogin(string username, string password)
        {
            try
            {
                // Open connection using connection string
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    // Query to validate username, password, and active status
                    string query = @"SELECT COUNT(*) 
                             FROM user 
                             WHERE userName = @username 
                             AND password = @password 
                             AND active = 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        // Add parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        // Execute the query and check if a matching user exists
                        int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                        return userCount > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Get user location
            string location = GetUserLocation();

            if (ValidateLogin(username, password))
            {
                // Log success
                LogLoginHistory(username, true);

                // Translate success message
                string successMessage = TranslateMessage("Login successful!", location);
                MessageBox.Show(successMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Proceed to the next step in your application
                var mainWindow = new MainWindow(this);
                mainWindow.Show();
                this.Hide();
            }
            else
            {
                // Log failure
                LogLoginHistory(username, false);

                // Translate error message
                string errorMessage = TranslateMessage("The username and password do not match.", location);
                MessageBox.Show(errorMessage, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string GetUserLocation()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetStringAsync("http://ip-api.com/json/").Result;
                    dynamic locationData = JsonConvert.DeserializeObject(response);
                    string country = locationData.country;
                    return country;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to fetch location: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return "Unknown";
            }
        }

        private string TranslateMessage(string message, string location)
        {
            if (location.Equals("Spain") || location.Equals("Mexico") || location.Equals("Argentina") || location.Equals("Colombia"))
            {
                // Spanish-speaking countries
                switch (message)
                {
                    case "Login successful!":
                        return "¡Inicio de sesión exitoso!";
                    case "The username and password do not match.":
                        return "El nombre de usuario y la contraseña no coinciden.";
                    case "Invalid login. Please check your username, password, or ensure your account is active.":
                        return "Inicio de sesión no válido. Por favor, revise su nombre de usuario, contraseña o asegúrese de que su cuenta esté activa.";
                    default:
                        return message;
                }
            }
            return message; // Default to English
        }

        private void LogLoginHistory(string username, bool isSuccess)
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

            // Construct the log file path in the project root directory
            string logPath = Path.Combine(projectRoot, "Login_History.txt");

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string status = isSuccess ? "Success" : "Failure";
            string logEntry = $"{timestamp} - Username: {username} - Status: {status}{Environment.NewLine}";

            try
            {
                // Create the file if it doesn't exist
                if (!File.Exists(logPath))
                {
                    File.Create(logPath).Close(); // Close after creation
                }

                // Append the log entry
                File.AppendAllText(logPath, logEntry);

                // Debug: Confirm write success
                MessageBox.Show($"Log entry written successfully to: {logPath}", "Debug Info");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to write to log file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
        }
    }
}

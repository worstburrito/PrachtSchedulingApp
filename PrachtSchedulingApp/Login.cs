using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrachtSchedulingApp
{
    public partial class Login : Form
    {
        /*
         * This program looks at the regional format for Spanish (Mexico)
         * and translates the login form from English to Spanish.
         */

        public Login()
        {
            InitializeComponent();
        }

        // These are all the functions that load when the form loads
        private void Login_Load(object sender, EventArgs e)
        {
            string region = GetRegionFromLocaleCode();


            if (region == "Mexico")
            {
                lblLocation.Text = $"Ubicación detectada: {region}";
                lblUsername.Text = $"Nombre de usuario";
                lblPassword.Text = $"Contraseña";
                btnLogin.Text = $"Acceso";
                btnCreateAccount.Text = $"Crear una cuenta";
                this.Text = $"Acceso";
            }
            else
            {
                lblLocation.Text = $"Detected Location: {region}";
                lblUsername.Text = $"Username";
                lblPassword.Text = $"Password";
                btnLogin.Text = $"Login";
                btnCreateAccount.Text = $"Create Account";
                this.Text = $"Login";
            }
        }

        // This uses CultureInfo and RegionInfo to get users regional format
        private string GetRegionFromLocaleCode()
        {
            string region = string.Empty;
            try
            {
                // Get the current user's locale code (culture)
                CultureInfo currentCulture = CultureInfo.CurrentCulture;

                // Combine language and country (e.g., "en-US", "es-MX")
                string localeCode = currentCulture.Name;

                // Get the region name based on the locale code (e.g., "United States", "Mexico")
                RegionInfo regionInfo = new RegionInfo(currentCulture.Name);
                region = regionInfo.EnglishName;  // Country/Region name
            }
            catch (Exception ex)
            {
                region = "Error retrieving region: " + ex.Message;
            }

            return region;
        }

        // This checks the database to match the user/pw combo 
        private (bool isValid, int userId) ValidateLogin(string username, string password)
        {
            try
            {
                // Open connection using connection string
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    // Query to validate username, password, and active status, and also retrieve UserId
                    string query = @"SELECT userId 
                             FROM user 
                             WHERE userName = @username 
                             AND password = @password 
                             AND active = 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = reader.GetInt32(0);
                                return (true, userId);
                            }
                            else
                            {
                                return (false, 0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (false, 0);
            }
        }

        // This is the Login click button
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string region = GetRegionFromLocaleCode();

            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                // Validate login and get userId
                var (isValid, userId) = ValidateLogin(username, password);

                if (isValid)
                {
                    LogLoginHistory(username, true);
                    CurrentUser.Username = username;
                    CurrentUser.UserId = userId;

                    // Display success message
                    if (region == "Mexico")
                    {
                        MessageBox.Show($"Inicio de sesión exitoso!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    

                    // Proceed to the next window
                    var mainWindow = new MainWindow(this);
                    mainWindow.Show();
                    this.Hide();
                }
                else
                {
                    LogLoginHistory(username, false);

                    // Display error message
                    if (region == "Mexico")
                    {
                        MessageBox.Show("No se puede iniciar sesión. Verifique las credenciales de inicio de sesión e inténtelo nuevamente.", "Error de inicio de sesión", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    else
                    {
                        MessageBox.Show("Unable to login. Please check login credentials and try again.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                if (region == "Mexico")
                {
                    MessageBox.Show($"Se produjo un error. Por favor inténtalo de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // This updates the Login History Log
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
            string logEntry = $"{timestamp} - Username: {username} - Status: {status}";

            try
            {
                using (StreamWriter writer = new StreamWriter(logPath, true))
                {
                    writer.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to write to log file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // This grabs the username and userid of the user logging in
        public static class CurrentUser
        {
            public static string Username { get; set; }
            public static int UserId { get; set; }
        }

        // This opens the Create Account form to create an account => mostly for debugging/access the user db
        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddUser"))
            {
                var addUser = new AddUser();
                addUser.MdiParent = this.MdiParent;
                addUser.Show();
            }
        }
    }
}

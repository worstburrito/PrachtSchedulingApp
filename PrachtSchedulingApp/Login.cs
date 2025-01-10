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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrachtSchedulingApp
{
    public partial class Login : Form
    {
        /* Note to evaluator: This Login form defaults to English. 
         * If your region and language are set to Mexico and Mexico (Spanish) 
         * the success/failure messages will translate */

        public Login()
        {
            InitializeComponent();
        }

        private string GetLanguageBasedOnRegion()
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;

            if (culture.Name == "es-MX")
                return "Spanish";
            return "English"; // Default to English
        }

        private void Login_Load(object sender, EventArgs e)
        {
            string language = GetLanguageBasedOnRegion();

            if (language == "Spanish")
            {
                this.Text = "Iniciar sesión";
                lblUsername.Text = "Nombre de usuario";
                lblPassword.Text = "Contraseña";
                btnLogin.Text = "Iniciar sesión";
                btnCreateAccount.Text = "Crear cuenta";
            }
            else
            {
                this.Text = "Login";
                lblUsername.Text = "Username";
                lblPassword.Text = "Password";
                btnLogin.Text = "Login";
                btnCreateAccount.Text = "Create Account";
            }
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                string language = GetLanguageBasedOnRegion();

                // Validate login and get userId
                var (isValid, userId) = ValidateLogin(username, password);

                if (isValid)
                {
                    LogLoginHistory(username, true);
                    CurrentUser.Username = username;
                    CurrentUser.UserId = userId;

                    // Display success message
                    MessageBox.Show(language == "Spanish" ? Messages["LoginSuccess"].Spanish : Messages["LoginSuccess"].English,
                                    language == "Spanish" ? "Éxito" : "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Proceed to the next window
                    var mainWindow = new MainWindow(this);
                    mainWindow.Show();
                    this.Hide();
                }
                else
                {
                    LogLoginHistory(username, false);

                    // Display error message
                    MessageBox.Show(language == "Spanish" ? Messages["LoginFailed"].Spanish : Messages["LoginFailed"].English,
                                    language == "Spanish" ? "Error de inicio de sesión" : "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // This stores the messages for the translation
        private Dictionary<string, (string English, string Spanish)> Messages = new Dictionary<string, (string English, string Spanish)>()
        {
            { "LoginSuccess", ("Login successful!", "¡Inicio de sesión exitoso!") },
            { "LoginFailed", ("The username and password do not match.", "El nombre de usuario y la contraseña no coinciden.") },
            { "ErrorOccurred", ("An error occurred: {0}", "Ocurrió un error: {0}") }
        };

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

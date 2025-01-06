using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrachtSchedulingApp.Database
{
    public class DBConnection
    {
        public static MySqlConnection conn { get; set; }

        public static void startConnection()
        {
            // get connection string
            string constr = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;

            // make connection
            MySqlConnection conn = null;


            try
            {
                conn = new MySqlConnection(constr);

                // open the connection
                conn.Open();

                // MessageBox.Show($"Connection was opened!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (MySqlException ex)
            {

                MessageBox.Show($"An error has occurred: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void closeConnection()
        {
            try
            {
                if (conn != null)
                {
                    conn.Close();
                }

                conn = null;
            }
            catch (MySqlException ex)
            {

                MessageBox.Show($"An error has occurred: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

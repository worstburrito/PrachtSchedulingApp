using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrachtSchedulingApp.Database
{
    public class DBConnection
    {
        public static MySqlConnection conn { get; set; }

        // Start the connection and return the MySqlConnection object
        public static MySqlConnection StartConnection()
        {
            if (conn != null && conn.State == ConnectionState.Open)
                return conn;

            string constr = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;

            try
            {
                conn = new MySqlConnection(constr);
                conn.Open();
                return conn;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"An error has occurred: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Close the connection
        public static void CloseConnection()
        {
            try
            {
                if (conn != null && conn.State == ConnectionState.Open)
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
        
        // Execute a query and return a DataTable
        public static DataTable ExecuteQuery(string query)
        {
            try
            {
                using (MySqlConnection con = StartConnection())
                {
                    if (con == null) return null;

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    return dataTable;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"An error has occurred: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}

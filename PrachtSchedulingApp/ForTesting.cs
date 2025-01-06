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
    public partial class ForTesting : Form
    {
        public ForTesting()
        {
            InitializeComponent();

            // Connect to database
            string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            MySqlConnection con = new MySqlConnection(connectionString);

            // Open connection and set query
            con.Open();
            string query = "SELECT * FROM fortesting";
            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

            // Create tables
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            adapter.Fill(dt1);
            adapter.Fill(dt2);

            // Assign and manipulate tables
            dgv1.DataSource = dt1;
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                DateTime y = (DateTime)dt1.Rows[i]["testDateTime"];
                dt2.Rows[i]["testDateTime"] = y.ToLocalTime();
            }

            dgv2.DataSource = dt2;

            // Create report

            // Set vars
            var numAnimal = 0;
            var numVegetable = 0;
            var numMineral = 0;

            foreach (DataRow row in dt1.Rows)
            {
                if (row["testType"].ToString() == "Animal")
                {
                    numAnimal++;
                }

                if (row["testType"].ToString() == "Vegetable")
                {
                    numVegetable++;
                }

                if (row["testType"].ToString() == "Mineral")
                {
                    numMineral++;
                }
            }

            txt.Text = txt.Text + 
                $"Animal {numAnimal}\n\r" +
                $"Vegetable {numVegetable}\n\r" +
                $"Mineral {numMineral}";
            txt.Select(0, 0);

        }
    }
}

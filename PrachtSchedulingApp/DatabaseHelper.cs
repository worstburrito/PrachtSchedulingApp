﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrachtSchedulingApp
{
    internal class DatabaseHelper
    {
        public static void PopulateAppointments(DataGridView gridView, DataTable dataTable)
        {
            gridView.DataSource = dataTable;

            // Column visibility and header adjustments
            gridView.Columns["appointmentId"].Visible = false;
            gridView.Columns["CustomerName"].HeaderText = "Customer";
            gridView.Columns["UserName"].HeaderText = "User";
            gridView.Columns["title"].HeaderText = "Title";
            gridView.Columns["description"].HeaderText = "Description";
            gridView.Columns["location"].HeaderText = "Location";
            gridView.Columns["contact"].HeaderText = "Contact";
            gridView.Columns["type"].HeaderText = "Meeting Type";
            gridView.Columns["url"].HeaderText = "URL";
            gridView.Columns["start"].HeaderText = "Start";
            gridView.Columns["end"].HeaderText = "End";
            gridView.Columns["CreatedBy"].HeaderText = "Created By";
            gridView.Columns["createDate"].HeaderText = "Creation Date";
            gridView.Columns["LastUpdatedBy"].HeaderText = "Last Updated By";
            gridView.Columns["lastUpdate"].HeaderText = "Last Update";
        }

        public static void PopulateCustomers(DataGridView gridView, DataTable dataTable)
        {
            gridView.DataSource = dataTable;

            // Adjust column headers and visibility
            gridView.Columns["customerId"].Visible = false;
            gridView.Columns["customerName"].HeaderText = "Customer Name";
            gridView.Columns["customerAddress"].HeaderText = "Address Line 1";
            gridView.Columns["customerAddress2"].HeaderText = "Address Line 2";
            gridView.Columns["postalCode"].HeaderText = "Postal Code";
            gridView.Columns["phone"].HeaderText = "Phone Number";
            gridView.Columns["cityName"].HeaderText = "City";
            gridView.Columns["countryName"].HeaderText = "Country";
            gridView.Columns["active"].HeaderText = "Active Status";
            gridView.Columns["createDate"].HeaderText = "Date Created";
            gridView.Columns["createdBy"].HeaderText = "Created By";
            gridView.Columns["lastUpdate"].HeaderText = "Last Update";
            gridView.Columns["lastUpdateBy"].HeaderText = "Last Updated By";
        }
        public static void TimeHelper(DataTable dataTable, string startColumn, string endColumn, string createdColumn, string updatedColumn)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                row[startColumn] = ((DateTime)row[startColumn]).ToLocalTime();
                row[endColumn] = ((DateTime)row[endColumn]).ToLocalTime();
                row[createdColumn] = ((DateTime)row[createdColumn]).ToLocalTime();
                row[updatedColumn] = ((DateTime)row[updatedColumn]).ToLocalTime();
            }
        }

        public static void PopulateUserComboBox(ComboBox comboBox, string query, string displayMember, string valueMember)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    DataTable comboBoxData = new DataTable();
                    adapter.Fill(comboBoxData);

                    comboBox.DisplayMember = displayMember;
                    comboBox.ValueMember = valueMember;
                    comboBox.DataSource = comboBoxData;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void PopulateCustomerComboBox(ComboBox comboBox, string query, string displayMember, string valueMember)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    DataTable comboBoxData = new DataTable();
                    adapter.Fill(comboBoxData);

                    comboBox.DisplayMember = displayMember;
                    comboBox.ValueMember = valueMember;
                    comboBox.DataSource = comboBoxData;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}

﻿namespace PrachtSchedulingApp
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.manageCustomerRecords = new System.Windows.Forms.ToolStripMenuItem();
            this.manageAppointments = new System.Windows.Forms.ToolStripMenuItem();
            this.viewSchedule = new System.Windows.Forms.ToolStripMenuItem();
            this.runReport = new System.Windows.Forms.ToolStripMenuItem();
            this.Report1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Report2 = new System.Windows.Forms.ToolStripMenuItem();
            this.Report3 = new System.Windows.Forms.ToolStripMenuItem();
            this.manageUsers = new System.Windows.Forms.ToolStripMenuItem();
            this.userStatus = new System.Windows.Forms.StatusStrip();
            this.lblUserStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.numberOfAppointmentsByCustomerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            this.userStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageCustomerRecords,
            this.manageAppointments,
            this.viewSchedule,
            this.runReport,
            this.manageUsers});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.mainMenu.Size = new System.Drawing.Size(730, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // manageCustomerRecords
            // 
            this.manageCustomerRecords.Name = "manageCustomerRecords";
            this.manageCustomerRecords.Size = new System.Drawing.Size(162, 22);
            this.manageCustomerRecords.Text = "Manage Customer Records";
            this.manageCustomerRecords.Click += new System.EventHandler(this.manageCustomerRecords_Click);
            // 
            // manageAppointments
            // 
            this.manageAppointments.Name = "manageAppointments";
            this.manageAppointments.Size = new System.Drawing.Size(141, 22);
            this.manageAppointments.Text = "Manage Appointments";
            this.manageAppointments.Click += new System.EventHandler(this.manageAppointments_Click);
            // 
            // viewSchedule
            // 
            this.viewSchedule.Name = "viewSchedule";
            this.viewSchedule.Size = new System.Drawing.Size(95, 22);
            this.viewSchedule.Text = "View Schedule";
            this.viewSchedule.Click += new System.EventHandler(this.viewSchedule_Click);
            // 
            // runReport
            // 
            this.runReport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Report1,
            this.Report2,
            this.numberOfAppointmentsByCustomerToolStripMenuItem,
            this.Report3});
            this.runReport.Name = "runReport";
            this.runReport.Size = new System.Drawing.Size(78, 22);
            this.runReport.Text = "Run Report";
            // 
            // Report1
            // 
            this.Report1.Name = "Report1";
            this.Report1.Size = new System.Drawing.Size(282, 22);
            this.Report1.Text = "Appointment Types by Month";
            this.Report1.Click += new System.EventHandler(this.Report1_Click);
            // 
            // Report2
            // 
            this.Report2.Name = "Report2";
            this.Report2.Size = new System.Drawing.Size(282, 22);
            this.Report2.Text = "User Schedules";
            this.Report2.Click += new System.EventHandler(this.Report2_Click);
            // 
            // Report3
            // 
            this.Report3.Name = "Report3";
            this.Report3.Size = new System.Drawing.Size(282, 22);
            this.Report3.Text = "Login History Report";
            this.Report3.Click += new System.EventHandler(this.Report3_Click);
            // 
            // manageUsers
            // 
            this.manageUsers.Name = "manageUsers";
            this.manageUsers.Size = new System.Drawing.Size(93, 22);
            this.manageUsers.Text = "Manage Users";
            this.manageUsers.Click += new System.EventHandler(this.manageUsers_Click);
            // 
            // userStatus
            // 
            this.userStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblUserStatus});
            this.userStatus.Location = new System.Drawing.Point(0, 457);
            this.userStatus.Name = "userStatus";
            this.userStatus.Padding = new System.Windows.Forms.Padding(1, 0, 9, 0);
            this.userStatus.Size = new System.Drawing.Size(730, 22);
            this.userStatus.TabIndex = 3;
            this.userStatus.Text = "statusStrip1";
            // 
            // lblUserStatus
            // 
            this.lblUserStatus.Name = "lblUserStatus";
            this.lblUserStatus.Size = new System.Drawing.Size(93, 17);
            this.lblUserStatus.Text = "Placeholder Text";
            // 
            // numberOfAppointmentsByCustomerToolStripMenuItem
            // 
            this.numberOfAppointmentsByCustomerToolStripMenuItem.Name = "numberOfAppointmentsByCustomerToolStripMenuItem";
            this.numberOfAppointmentsByCustomerToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.numberOfAppointmentsByCustomerToolStripMenuItem.Text = "Number of Appointments by Customer";
            this.numberOfAppointmentsByCustomerToolStripMenuItem.Click += new System.EventHandler(this.numberOfAppointmentsByCustomerToolStripMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(730, 479);
            this.Controls.Add(this.userStatus);
            this.Controls.Add(this.mainMenu);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pracht Scheduling Application";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.userStatus.ResumeLayout(false);
            this.userStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem manageCustomerRecords;
        private System.Windows.Forms.ToolStripMenuItem manageAppointments;
        private System.Windows.Forms.ToolStripMenuItem viewSchedule;
        private System.Windows.Forms.ToolStripMenuItem runReport;
        private System.Windows.Forms.ToolStripMenuItem Report1;
        private System.Windows.Forms.ToolStripMenuItem Report2;
        private System.Windows.Forms.ToolStripMenuItem Report3;
        private System.Windows.Forms.ToolStripMenuItem manageUsers;
        private System.Windows.Forms.StatusStrip userStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblUserStatus;
        private System.Windows.Forms.ToolStripMenuItem numberOfAppointmentsByCustomerToolStripMenuItem;
    }
}


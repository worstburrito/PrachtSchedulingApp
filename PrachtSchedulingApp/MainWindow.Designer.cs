namespace PrachtSchedulingApp
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
            this.viewCustomerRecords = new System.Windows.Forms.ToolStripMenuItem();
            this.addCustomerRecords = new System.Windows.Forms.ToolStripMenuItem();
            this.manageAppointments = new System.Windows.Forms.ToolStripMenuItem();
            this.viewAllAppointments = new System.Windows.Forms.ToolStripMenuItem();
            this.addAppointment = new System.Windows.Forms.ToolStripMenuItem();
            this.viewSchedule = new System.Windows.Forms.ToolStripMenuItem();
            this.runReport = new System.Windows.Forms.ToolStripMenuItem();
            this.Report1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Report2 = new System.Windows.Forms.ToolStripMenuItem();
            this.Report3 = new System.Windows.Forms.ToolStripMenuItem();
            this.Report4 = new System.Windows.Forms.ToolStripMenuItem();
            this.mainStatus = new System.Windows.Forms.StatusStrip();
            this.lblMainStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainMenu.SuspendLayout();
            this.mainStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageCustomerRecords,
            this.manageAppointments,
            this.viewSchedule,
            this.runReport});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(1095, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // manageCustomerRecords
            // 
            this.manageCustomerRecords.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewCustomerRecords,
            this.addCustomerRecords});
            this.manageCustomerRecords.Name = "manageCustomerRecords";
            this.manageCustomerRecords.Size = new System.Drawing.Size(162, 20);
            this.manageCustomerRecords.Text = "Manage Customer Records";
            // 
            // viewCustomerRecords
            // 
            this.viewCustomerRecords.Name = "viewCustomerRecords";
            this.viewCustomerRecords.Size = new System.Drawing.Size(199, 22);
            this.viewCustomerRecords.Text = "View Customer Records";
            this.viewCustomerRecords.Click += new System.EventHandler(this.viewCustomerRecords_Click);
            // 
            // addCustomerRecords
            // 
            this.addCustomerRecords.Name = "addCustomerRecords";
            this.addCustomerRecords.Size = new System.Drawing.Size(199, 22);
            this.addCustomerRecords.Text = "Add Customer Record";
            this.addCustomerRecords.Click += new System.EventHandler(this.addCustomerRecords_Click);
            // 
            // manageAppointments
            // 
            this.manageAppointments.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewAllAppointments,
            this.addAppointment});
            this.manageAppointments.Name = "manageAppointments";
            this.manageAppointments.Size = new System.Drawing.Size(141, 20);
            this.manageAppointments.Text = "Manage Appointments";
            // 
            // viewAllAppointments
            // 
            this.viewAllAppointments.Name = "viewAllAppointments";
            this.viewAllAppointments.Size = new System.Drawing.Size(195, 22);
            this.viewAllAppointments.Text = "View All Appointments";
            this.viewAllAppointments.Click += new System.EventHandler(this.viewAllAppointments_Click);
            // 
            // addAppointment
            // 
            this.addAppointment.Name = "addAppointment";
            this.addAppointment.Size = new System.Drawing.Size(195, 22);
            this.addAppointment.Text = "Add Appointment";
            this.addAppointment.Click += new System.EventHandler(this.addAppointment_Click);
            // 
            // viewSchedule
            // 
            this.viewSchedule.Name = "viewSchedule";
            this.viewSchedule.Size = new System.Drawing.Size(95, 20);
            this.viewSchedule.Text = "View Schedule";
            this.viewSchedule.Click += new System.EventHandler(this.viewSchedule_Click);
            // 
            // runReport
            // 
            this.runReport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Report1,
            this.Report2,
            this.Report3,
            this.Report4});
            this.runReport.Name = "runReport";
            this.runReport.Size = new System.Drawing.Size(78, 20);
            this.runReport.Text = "Run Report";
            // 
            // Report1
            // 
            this.Report1.Name = "Report1";
            this.Report1.Size = new System.Drawing.Size(232, 22);
            this.Report1.Text = "Appointment Types by Month";
            this.Report1.Click += new System.EventHandler(this.Report1_Click);
            // 
            // Report2
            // 
            this.Report2.Name = "Report2";
            this.Report2.Size = new System.Drawing.Size(232, 22);
            this.Report2.Text = "User Schedules";
            this.Report2.Click += new System.EventHandler(this.Report2_Click);
            // 
            // Report3
            // 
            this.Report3.Name = "Report3";
            this.Report3.Size = new System.Drawing.Size(232, 22);
            this.Report3.Text = "Report 3";
            this.Report3.Click += new System.EventHandler(this.Report3_Click);
            // 
            // Report4
            // 
            this.Report4.Name = "Report4";
            this.Report4.Size = new System.Drawing.Size(232, 22);
            this.Report4.Text = "Login History";
            this.Report4.Click += new System.EventHandler(this.Report4_Click);
            // 
            // mainStatus
            // 
            this.mainStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblMainStatus});
            this.mainStatus.Location = new System.Drawing.Point(0, 715);
            this.mainStatus.Name = "mainStatus";
            this.mainStatus.Size = new System.Drawing.Size(1095, 22);
            this.mainStatus.TabIndex = 2;
            this.mainStatus.Text = "statusStrip1";
            // 
            // lblMainStatus
            // 
            this.lblMainStatus.Name = "lblMainStatus";
            this.lblMainStatus.Size = new System.Drawing.Size(93, 17);
            this.lblMainStatus.Text = "Placeholder Text";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1095, 737);
            this.Controls.Add(this.mainStatus);
            this.Controls.Add(this.mainMenu);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenu;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pracht Scheduling Application";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.mainStatus.ResumeLayout(false);
            this.mainStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem manageCustomerRecords;
        private System.Windows.Forms.ToolStripMenuItem viewCustomerRecords;
        private System.Windows.Forms.ToolStripMenuItem addCustomerRecords;
        private System.Windows.Forms.ToolStripMenuItem manageAppointments;
        private System.Windows.Forms.ToolStripMenuItem viewAllAppointments;
        private System.Windows.Forms.ToolStripMenuItem addAppointment;
        private System.Windows.Forms.ToolStripMenuItem viewSchedule;
        private System.Windows.Forms.ToolStripMenuItem runReport;
        private System.Windows.Forms.ToolStripMenuItem Report1;
        private System.Windows.Forms.ToolStripMenuItem Report2;
        private System.Windows.Forms.ToolStripMenuItem Report3;
        private System.Windows.Forms.ToolStripMenuItem Report4;
        private System.Windows.Forms.StatusStrip mainStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblMainStatus;
    }
}


namespace PrachtSchedulingApp
{
    partial class ManageAppointments
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
            this.btnDeleteRecord = new System.Windows.Forms.Button();
            this.btnUpdateRecord = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dgvManageAppointments = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvManageAppointments)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDeleteRecord
            // 
            this.btnDeleteRecord.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnDeleteRecord.Location = new System.Drawing.Point(0, 300);
            this.btnDeleteRecord.Name = "btnDeleteRecord";
            this.btnDeleteRecord.Size = new System.Drawing.Size(734, 37);
            this.btnDeleteRecord.TabIndex = 7;
            this.btnDeleteRecord.Text = "Delete Appointment";
            this.btnDeleteRecord.UseVisualStyleBackColor = true;
            this.btnDeleteRecord.Click += new System.EventHandler(this.btnDeleteRecord_Click);
            // 
            // btnUpdateRecord
            // 
            this.btnUpdateRecord.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnUpdateRecord.Location = new System.Drawing.Point(0, 337);
            this.btnUpdateRecord.Name = "btnUpdateRecord";
            this.btnUpdateRecord.Size = new System.Drawing.Size(734, 37);
            this.btnUpdateRecord.TabIndex = 6;
            this.btnUpdateRecord.Text = "Update Appointment";
            this.btnUpdateRecord.UseVisualStyleBackColor = true;
            this.btnUpdateRecord.Click += new System.EventHandler(this.btnUpdateRecord_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAdd.Location = new System.Drawing.Point(0, 374);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(734, 37);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Add New Appointment";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgvManageAppointments
            // 
            this.dgvManageAppointments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvManageAppointments.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvManageAppointments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvManageAppointments.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvManageAppointments.Location = new System.Drawing.Point(0, 0);
            this.dgvManageAppointments.Name = "dgvManageAppointments";
            this.dgvManageAppointments.Size = new System.Drawing.Size(734, 300);
            this.dgvManageAppointments.TabIndex = 4;
            // 
            // ManageAppointments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(734, 411);
            this.Controls.Add(this.btnDeleteRecord);
            this.Controls.Add(this.btnUpdateRecord);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvManageAppointments);
            this.HelpButton = true;
            this.Name = "ManageAppointments";
            this.Text = "Manage Appointments";
            this.Load += new System.EventHandler(this.ManageAppointments_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvManageAppointments)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDeleteRecord;
        private System.Windows.Forms.Button btnUpdateRecord;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dgvManageAppointments;
    }
}
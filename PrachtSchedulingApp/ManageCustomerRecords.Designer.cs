﻿namespace PrachtSchedulingApp
{
    partial class ManageCustomerRecords
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
            this.dgvManageCustomerRecords = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdateRecord = new System.Windows.Forms.Button();
            this.btnDeleteRecord = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvManageCustomerRecords)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvManageCustomerRecords
            // 
            this.dgvManageCustomerRecords.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvManageCustomerRecords.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvManageCustomerRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvManageCustomerRecords.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvManageCustomerRecords.Location = new System.Drawing.Point(0, 111);
            this.dgvManageCustomerRecords.Name = "dgvManageCustomerRecords";
            this.dgvManageCustomerRecords.Size = new System.Drawing.Size(734, 300);
            this.dgvManageCustomerRecords.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAdd.Location = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(734, 37);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add New Record";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdateRecord
            // 
            this.btnUpdateRecord.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUpdateRecord.Location = new System.Drawing.Point(0, 37);
            this.btnUpdateRecord.Name = "btnUpdateRecord";
            this.btnUpdateRecord.Size = new System.Drawing.Size(734, 37);
            this.btnUpdateRecord.TabIndex = 2;
            this.btnUpdateRecord.Text = "Update Record";
            this.btnUpdateRecord.UseVisualStyleBackColor = true;
            this.btnUpdateRecord.Click += new System.EventHandler(this.btnUpdateRecord_Click);
            // 
            // btnDeleteRecord
            // 
            this.btnDeleteRecord.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDeleteRecord.Location = new System.Drawing.Point(0, 74);
            this.btnDeleteRecord.Name = "btnDeleteRecord";
            this.btnDeleteRecord.Size = new System.Drawing.Size(734, 37);
            this.btnDeleteRecord.TabIndex = 3;
            this.btnDeleteRecord.Text = "Delete Record";
            this.btnDeleteRecord.UseVisualStyleBackColor = true;
            this.btnDeleteRecord.Click += new System.EventHandler(this.btnDeleteRecord_Click);
            // 
            // ManageCustomerRecords
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 411);
            this.Controls.Add(this.btnDeleteRecord);
            this.Controls.Add(this.dgvManageCustomerRecords);
            this.Controls.Add(this.btnUpdateRecord);
            this.Controls.Add(this.btnAdd);
            this.Name = "ManageCustomerRecords";
            this.Text = "Manage Customer Records";
            this.Load += new System.EventHandler(this.ManageCustomerRecords_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvManageCustomerRecords)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvManageCustomerRecords;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdateRecord;
        private System.Windows.Forms.Button btnDeleteRecord;
    }
}
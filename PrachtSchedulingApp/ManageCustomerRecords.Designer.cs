namespace PrachtSchedulingApp
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
            this.dgvManageCustomerRecords.Location = new System.Drawing.Point(12, 12);
            this.dgvManageCustomerRecords.Name = "dgvManageCustomerRecords";
            this.dgvManageCustomerRecords.Size = new System.Drawing.Size(776, 366);
            this.dgvManageCustomerRecords.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(156, 384);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(159, 37);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add New Record";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnUpdateRecord
            // 
            this.btnUpdateRecord.Location = new System.Drawing.Point(321, 384);
            this.btnUpdateRecord.Name = "btnUpdateRecord";
            this.btnUpdateRecord.Size = new System.Drawing.Size(159, 37);
            this.btnUpdateRecord.TabIndex = 2;
            this.btnUpdateRecord.Text = "Update Record";
            this.btnUpdateRecord.UseVisualStyleBackColor = true;
            // 
            // btnDeleteRecord
            // 
            this.btnDeleteRecord.Location = new System.Drawing.Point(486, 384);
            this.btnDeleteRecord.Name = "btnDeleteRecord";
            this.btnDeleteRecord.Size = new System.Drawing.Size(159, 37);
            this.btnDeleteRecord.TabIndex = 3;
            this.btnDeleteRecord.Text = "Delete Record";
            this.btnDeleteRecord.UseVisualStyleBackColor = true;
            // 
            // ManageCustomerRecords
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnDeleteRecord);
            this.Controls.Add(this.btnUpdateRecord);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvManageCustomerRecords);
            this.Name = "ManageCustomerRecords";
            this.Text = "Manage Customer Records";
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
namespace PrachtSchedulingApp
{
    partial class Report2
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
            this.dgvAppointments = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblSelectUser = new System.Windows.Forms.Label();
            this.btnFindAppointments = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.cboUser = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvAppointments
            // 
            this.dgvAppointments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAppointments.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvAppointments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAppointments.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvAppointments.Location = new System.Drawing.Point(0, 105);
            this.dgvAppointments.Name = "dgvAppointments";
            this.dgvAppointments.Size = new System.Drawing.Size(1155, 383);
            this.dgvAppointments.TabIndex = 7;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblSelectUser, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnFindAppointments, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnReset, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cboUser, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1155, 74);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // lblSelectUser
            // 
            this.lblSelectUser.AutoSize = true;
            this.lblSelectUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblSelectUser.Location = new System.Drawing.Point(3, 0);
            this.lblSelectUser.Name = "lblSelectUser";
            this.lblSelectUser.Size = new System.Drawing.Size(65, 13);
            this.lblSelectUser.TabIndex = 3;
            this.lblSelectUser.Text = "Select User:";
            // 
            // btnFindAppointments
            // 
            this.btnFindAppointments.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnFindAppointments.Location = new System.Drawing.Point(580, 3);
            this.btnFindAppointments.Name = "btnFindAppointments";
            this.btnFindAppointments.Size = new System.Drawing.Size(190, 31);
            this.btnFindAppointments.TabIndex = 1;
            this.btnFindAppointments.Text = "Find Appointments";
            this.btnFindAppointments.UseVisualStyleBackColor = true;
            this.btnFindAppointments.Click += new System.EventHandler(this.btnFindAppointments_Click);
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnReset.Location = new System.Drawing.Point(580, 40);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(190, 31);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // cboUser
            // 
            this.cboUser.FormattingEnabled = true;
            this.cboUser.Location = new System.Drawing.Point(3, 40);
            this.cboUser.Name = "cboUser";
            this.cboUser.Size = new System.Drawing.Size(323, 21);
            this.cboUser.TabIndex = 5;
            // 
            // Report2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 488);
            this.Controls.Add(this.dgvAppointments);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Report2";
            this.Text = "User Schedule - Report";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAppointments;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblSelectUser;
        private System.Windows.Forms.Button btnFindAppointments;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ComboBox cboUser;
    }
}
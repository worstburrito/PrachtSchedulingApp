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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblSelectUser = new System.Windows.Forms.Label();
            this.cboUser = new System.Windows.Forms.ComboBox();
            this.btnRunReport = new System.Windows.Forms.Button();
            this.rtbReport = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblSelectUser, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cboUser, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnRunReport, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32.43243F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 67.56757F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(809, 73);
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
            // cboUser
            // 
            this.cboUser.FormattingEnabled = true;
            this.cboUser.Location = new System.Drawing.Point(3, 26);
            this.cboUser.Name = "cboUser";
            this.cboUser.Size = new System.Drawing.Size(323, 21);
            this.cboUser.TabIndex = 5;
            // 
            // btnRunReport
            // 
            this.btnRunReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnRunReport.Location = new System.Drawing.Point(407, 26);
            this.btnRunReport.Name = "btnRunReport";
            this.btnRunReport.Size = new System.Drawing.Size(190, 31);
            this.btnRunReport.TabIndex = 1;
            this.btnRunReport.Text = "Run Report";
            this.btnRunReport.UseVisualStyleBackColor = true;
            this.btnRunReport.Click += new System.EventHandler(this.btnFindAppointments_Click);
            // 
            // rtbReport
            // 
            this.rtbReport.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtbReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.rtbReport.Location = new System.Drawing.Point(10, 99);
            this.rtbReport.Name = "rtbReport";
            this.rtbReport.Size = new System.Drawing.Size(809, 455);
            this.rtbReport.TabIndex = 7;
            this.rtbReport.Text = "";
            // 
            // Report2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 564);
            this.Controls.Add(this.rtbReport);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Report2";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "User Schedule - Report";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblSelectUser;
        private System.Windows.Forms.Button btnRunReport;
        private System.Windows.Forms.ComboBox cboUser;
        private System.Windows.Forms.RichTextBox rtbReport;
    }
}
namespace PrachtSchedulingApp
{
    partial class CalendarView
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
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.btnFindAppointments = new System.Windows.Forms.Button();
            this.dgvDisplayAppointments = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplayAppointments)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.03554F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.96446F));
            this.tableLayoutPanel1.Controls.Add(this.dateTimePicker, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnFindAppointments, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(105, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(591, 37);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dateTimePicker.Location = new System.Drawing.Point(3, 3);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(360, 26);
            this.dateTimePicker.TabIndex = 0;
            // 
            // btnFindAppointments
            // 
            this.btnFindAppointments.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnFindAppointments.Location = new System.Drawing.Point(369, 3);
            this.btnFindAppointments.Name = "btnFindAppointments";
            this.btnFindAppointments.Size = new System.Drawing.Size(219, 31);
            this.btnFindAppointments.TabIndex = 1;
            this.btnFindAppointments.Text = "Find Appointments";
            this.btnFindAppointments.UseVisualStyleBackColor = true;
            // 
            // dgvDisplayAppointments
            // 
            this.dgvDisplayAppointments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDisplayAppointments.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvDisplayAppointments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDisplayAppointments.Location = new System.Drawing.Point(12, 55);
            this.dgvDisplayAppointments.Name = "dgvDisplayAppointments";
            this.dgvDisplayAppointments.Size = new System.Drawing.Size(776, 383);
            this.dgvDisplayAppointments.TabIndex = 5;
            // 
            // CalendarView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvDisplayAppointments);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CalendarView";
            this.Text = "Calendar View";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplayAppointments)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Button btnFindAppointments;
        private System.Windows.Forms.DataGridView dgvDisplayAppointments;
    }
}
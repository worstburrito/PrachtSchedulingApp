namespace PrachtSchedulingApp
{
    partial class LoginHistoryReport
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
            this.btnRunReport = new System.Windows.Forms.Button();
            this.rtbReport = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnRunReport
            // 
            this.btnRunReport.Location = new System.Drawing.Point(13, 13);
            this.btnRunReport.Name = "btnRunReport";
            this.btnRunReport.Size = new System.Drawing.Size(112, 37);
            this.btnRunReport.TabIndex = 0;
            this.btnRunReport.Text = "Run Report";
            this.btnRunReport.UseVisualStyleBackColor = true;
            this.btnRunReport.Click += new System.EventHandler(this.btnRunReport_Click);
            // 
            // rtbReport
            // 
            this.rtbReport.Location = new System.Drawing.Point(13, 56);
            this.rtbReport.Name = "rtbReport";
            this.rtbReport.Size = new System.Drawing.Size(775, 300);
            this.rtbReport.TabIndex = 1;
            this.rtbReport.Text = "";
            // 
            // LoginHistoryReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rtbReport);
            this.Controls.Add(this.btnRunReport);
            this.Name = "LoginHistoryReport";
            this.Text = "Login History Report";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRunReport;
        private System.Windows.Forms.RichTextBox rtbReport;
    }
}
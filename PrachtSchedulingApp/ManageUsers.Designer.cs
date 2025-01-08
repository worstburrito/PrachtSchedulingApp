namespace PrachtSchedulingApp
{
    partial class ManageUsers
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.dgvManageUsers = new System.Windows.Forms.DataGridView();
            this.btnActivate = new System.Windows.Forms.Button();
            this.btnDeactive = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvManageUsers)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAdd.Location = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(734, 37);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Add New User";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgvManageUsers
            // 
            this.dgvManageUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvManageUsers.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvManageUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvManageUsers.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvManageUsers.Location = new System.Drawing.Point(0, 111);
            this.dgvManageUsers.Name = "dgvManageUsers";
            this.dgvManageUsers.Size = new System.Drawing.Size(734, 300);
            this.dgvManageUsers.TabIndex = 4;
            // 
            // btnActivate
            // 
            this.btnActivate.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnActivate.Location = new System.Drawing.Point(0, 37);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(734, 37);
            this.btnActivate.TabIndex = 8;
            this.btnActivate.Text = "Activate User";
            this.btnActivate.UseVisualStyleBackColor = true;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // btnDeactive
            // 
            this.btnDeactive.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDeactive.Location = new System.Drawing.Point(0, 74);
            this.btnDeactive.Name = "btnDeactive";
            this.btnDeactive.Size = new System.Drawing.Size(734, 37);
            this.btnDeactive.TabIndex = 9;
            this.btnDeactive.Text = "Deactivate User";
            this.btnDeactive.UseVisualStyleBackColor = true;
            this.btnDeactive.Click += new System.EventHandler(this.btnDeactive_Click);
            // 
            // ManageUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 411);
            this.Controls.Add(this.dgvManageUsers);
            this.Controls.Add(this.btnDeactive);
            this.Controls.Add(this.btnActivate);
            this.Controls.Add(this.btnAdd);
            this.Name = "ManageUsers";
            this.Text = "Manage Users";
            this.Load += new System.EventHandler(this.ManageUsers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvManageUsers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dgvManageUsers;
        private System.Windows.Forms.Button btnActivate;
        private System.Windows.Forms.Button btnDeactive;
    }
}
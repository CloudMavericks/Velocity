namespace Velocity.Frontend
{
    partial class CustomerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerForm));
            dataGridView1 = new DataGridView();
            ColumnSno = new DataGridViewTextBoxColumn();
            ColumnName = new DataGridViewTextBoxColumn();
            ColumnDescription = new DataGridViewTextBoxColumn();
            ColumnIsCustomer = new DataGridViewCheckBoxColumn();
            ColumnIsVendor = new DataGridViewCheckBoxColumn();
            ColumnEdit = new DataGridViewButtonColumn();
            ColumnDelete = new DataGridViewButtonColumn();
            BtnNew = new ToolStripButton();
            BtnRefresh = new ToolStripButton();
            BtnExit = new ToolStripButton();
            toolStrip1 = new ToolStrip();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { ColumnSno, ColumnName, ColumnDescription, ColumnIsCustomer, ColumnIsVendor, ColumnEdit, ColumnDelete });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 27);
            dataGridView1.Margin = new Padding(3, 2, 3, 2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(700, 311);
            dataGridView1.TabIndex = 3;
            // 
            // Sno
            // 
            ColumnSno.HeaderText = "#";
            ColumnSno.MinimumWidth = 6;
            ColumnSno.Name = "Sno";
            ColumnSno.ReadOnly = true;
            ColumnSno.Width = 125;
            // 
            // Name
            // 
            ColumnName.HeaderText = "Name";
            ColumnName.MinimumWidth = 6;
            ColumnName.Name = "Name";
            ColumnName.ReadOnly = true;
            ColumnName.Width = 125;
            // 
            // Description
            // 
            ColumnDescription.HeaderText = "Description";
            ColumnDescription.MinimumWidth = 6;
            ColumnDescription.Name = "Description";
            ColumnDescription.Width = 125;
            // 
            // IsCustomer
            // 
            ColumnIsCustomer.HeaderText = "Customer";
            ColumnIsCustomer.MinimumWidth = 6;
            ColumnIsCustomer.Name = "IsCustomer";
            ColumnIsCustomer.ReadOnly = true;
            ColumnIsCustomer.Width = 125;
            // 
            // IsVendor
            // 
            ColumnIsVendor.HeaderText = "Vendor";
            ColumnIsVendor.MinimumWidth = 6;
            ColumnIsVendor.Name = "IsVendor";
            ColumnIsVendor.ReadOnly = true;
            ColumnIsVendor.Width = 125;
            // 
            // Edit
            // 
            ColumnEdit.HeaderText = "Edit";
            ColumnEdit.MinimumWidth = 6;
            ColumnEdit.Name = "Edit";
            ColumnEdit.ReadOnly = true;
            ColumnEdit.Width = 125;
            // 
            // Delete
            // 
            ColumnDelete.HeaderText = "Delete";
            ColumnDelete.MinimumWidth = 6;
            ColumnDelete.Name = "Delete";
            ColumnDelete.ReadOnly = true;
            ColumnDelete.Width = 125;
            // 
            // BtnNew
            // 
            BtnNew.AccessibleName = "";
            BtnNew.Image = (Image)resources.GetObject("BtnNew.Image");
            BtnNew.ImageTransparentColor = Color.Magenta;
            BtnNew.Name = "BtnNew";
            BtnNew.Size = new Size(55, 24);
            BtnNew.Text = "New";
            BtnNew.ToolTipText = "toolStripButton1";
            // 
            // BtnRefresh
            // 
            BtnRefresh.Image = (Image)resources.GetObject("BtnRefresh.Image");
            BtnRefresh.ImageTransparentColor = Color.Magenta;
            BtnRefresh.Name = "BtnRefresh";
            BtnRefresh.Size = new Size(70, 24);
            BtnRefresh.Text = "Refresh";
            // 
            // BtnExit
            // 
            BtnExit.Image = (Image)resources.GetObject("BtnExit.Image");
            BtnExit.ImageTransparentColor = Color.Magenta;
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(50, 24);
            BtnExit.Text = "Exit";
            BtnExit.Click += Exit_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Items.AddRange(new ToolStripItem[] { BtnNew, BtnRefresh, BtnExit });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(700, 27);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // CustomerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 338);
            Controls.Add(dataGridView1);
            Controls.Add(toolStrip1);
            Margin = new Padding(3, 2, 3, 2);
            Text = "CustomerForm";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private ToolStripButton BtnNew;
        private ToolStripButton BtnRefresh;
        private ToolStripButton BtnExit;
        private ToolStrip toolStrip1;
        private DataGridViewTextBoxColumn ColumnSno;
        private DataGridViewTextBoxColumn ColumnName;
        private DataGridViewTextBoxColumn ColumnDescription;
        private DataGridViewCheckBoxColumn ColumnIsCustomer;
        private DataGridViewCheckBoxColumn ColumnIsVendor;
        private DataGridViewButtonColumn ColumnEdit;
        private DataGridViewButtonColumn ColumnDelete;
    }
}
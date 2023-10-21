namespace Velocity.Frontend
{
    partial class MainForm
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
            menuStrip1 = new MenuStrip();
            homeToolStripMenuItem = new ToolStripMenuItem();
            customerVendorMasterToolStripMenuItem = new ToolStripMenuItem();
            purchaseOrderToolStripMenuItem = new ToolStripMenuItem();
            invoicesToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { homeToolStripMenuItem, aboutToolStripMenuItem, exitToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.ItemClicked += menuStrip1_ItemClicked_1;
            // 
            // homeToolStripMenuItem
            // 
            homeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { customerVendorMasterToolStripMenuItem, purchaseOrderToolStripMenuItem, invoicesToolStripMenuItem });
            homeToolStripMenuItem.Name = "homeToolStripMenuItem";
            homeToolStripMenuItem.Size = new Size(68, 24);
            homeToolStripMenuItem.Text = "Master";
            homeToolStripMenuItem.Click += homeToolStripMenuItem_Click;
            // 
            // customerVendorMasterToolStripMenuItem
            // 
            customerVendorMasterToolStripMenuItem.Name = "customerVendorMasterToolStripMenuItem";
            customerVendorMasterToolStripMenuItem.Size = new Size(255, 26);
            customerVendorMasterToolStripMenuItem.Text = "Customer Vendor Master";
            customerVendorMasterToolStripMenuItem.Click += customerVendorMasterToolStripMenuItem_Click;
            // 
            // purchaseOrderToolStripMenuItem
            // 
            purchaseOrderToolStripMenuItem.Name = "purchaseOrderToolStripMenuItem";
            purchaseOrderToolStripMenuItem.Size = new Size(255, 26);
            purchaseOrderToolStripMenuItem.Text = "Purchase Order";
            // 
            // invoicesToolStripMenuItem
            // 
            invoicesToolStripMenuItem.Name = "invoicesToolStripMenuItem";
            invoicesToolStripMenuItem.Size = new Size(255, 26);
            invoicesToolStripMenuItem.Text = "Invoices";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(64, 24);
            aboutToolStripMenuItem.Text = "About";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(47, 24);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(menuStrip1);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "MainForm";
            FormClosing += MainForm_FormClosing;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem homeToolStripMenuItem;
        private ToolStripMenuItem customerVendorMasterToolStripMenuItem;
        private ToolStripMenuItem purchaseOrderToolStripMenuItem;
        private ToolStripMenuItem invoicesToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
    }
}
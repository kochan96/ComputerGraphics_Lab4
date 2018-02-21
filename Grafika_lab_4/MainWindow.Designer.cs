namespace Grafika_lab_4
{
    partial class MainWindow
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
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shadingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shadingToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ShadingCombobBox = new System.Windows.Forms.ToolStripComboBox();
            this.lightningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LightningComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.glControl = new OpenTK.GLControl();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.shadingToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.MainMenu.Size = new System.Drawing.Size(673, 24);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "menuStrip1";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // shadingToolStripMenuItem
            // 
            this.shadingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shadingToolStripMenuItem1,
            this.ShadingCombobBox,
            this.lightningToolStripMenuItem,
            this.LightningComboBox});
            this.shadingToolStripMenuItem.Name = "shadingToolStripMenuItem";
            this.shadingToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.shadingToolStripMenuItem.Text = "Shading";
            // 
            // shadingToolStripMenuItem1
            // 
            this.shadingToolStripMenuItem1.Name = "shadingToolStripMenuItem1";
            this.shadingToolStripMenuItem1.Size = new System.Drawing.Size(181, 22);
            this.shadingToolStripMenuItem1.Text = "Shading";
            // 
            // ShadingCombobBox
            // 
            this.ShadingCombobBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ShadingCombobBox.Items.AddRange(new object[] {
            "Phong",
            "Gouroud"});
            this.ShadingCombobBox.Name = "ShadingCombobBox";
            this.ShadingCombobBox.Size = new System.Drawing.Size(121, 23);
            this.ShadingCombobBox.SelectedIndexChanged += new System.EventHandler(this.CombobBox_SelectedIndexChanged);
            // 
            // lightningToolStripMenuItem
            // 
            this.lightningToolStripMenuItem.Name = "lightningToolStripMenuItem";
            this.lightningToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.lightningToolStripMenuItem.Text = "Lightning";
            // 
            // LightningComboBox
            // 
            this.LightningComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LightningComboBox.Items.AddRange(new object[] {
            "Phong",
            "Blinn-Phong"});
            this.LightningComboBox.Name = "LightningComboBox";
            this.LightningComboBox.Size = new System.Drawing.Size(121, 23);
            this.LightningComboBox.SelectedIndexChanged += new System.EventHandler(this.CombobBox_SelectedIndexChanged);
            // 
            // glControl
            // 
            this.glControl.AutoSize = true;
            this.glControl.BackColor = System.Drawing.Color.Black;
            this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl.Location = new System.Drawing.Point(0, 24);
            this.glControl.Margin = new System.Windows.Forms.Padding(4);
            this.glControl.MinimumSize = new System.Drawing.Size(100, 100);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(673, 432);
            this.glControl.TabIndex = 1;
            this.glControl.VSync = false;
            this.glControl.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl_Paint);
            this.glControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glControl_KeyDown);
            this.glControl.Resize += new System.EventHandler(this.glControl_Resize);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 456);
            this.Controls.Add(this.glControl);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(689, 495);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shadingToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox ShadingCombobBox;
        private System.Windows.Forms.ToolStripComboBox LightningComboBox;
        private System.Windows.Forms.ToolStripMenuItem shadingToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem lightningToolStripMenuItem;
        private OpenTK.GLControl glControl;
    }
}
namespace GCore.AppSystem.WinForms.ToolWindows
{
    partial class PropertiesWindow
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
            this.components = new System.ComponentModel.Container();
            this.xPropertyGrid = new GCore.WinForms.Controls.RuntimeProperty.XPropertyGrid();
            this.SuspendLayout();
            // 
            // xPropertyGrid
            // 
            this.xPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.xPropertyGrid.Name = "xPropertyGrid";
            this.xPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.xPropertyGrid.Size = new System.Drawing.Size(431, 457);
            this.xPropertyGrid.TabIndex = 0;
            // 
            // PropertiesWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 457);
            this.Controls.Add(this.xPropertyGrid);
            this.Name = "PropertiesWindow";
            this.Text = "PropertiesWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private GCore.WinForms.Controls.RuntimeProperty.XPropertyGrid xPropertyGrid;
    }
}
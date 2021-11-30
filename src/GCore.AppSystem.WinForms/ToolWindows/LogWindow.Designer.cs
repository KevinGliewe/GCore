namespace GCore.AppSystem.WinForms.ToolWindows
{
    partial class LogWindow
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
            this.logList1 = new GCore.WinForms.Controls.LogList();
            this.SuspendLayout();
            // 
            // logList1
            // 
            this.logList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logList1.Location = new System.Drawing.Point(0, 0);
            this.logList1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.logList1.Name = "logList1";
            this.logList1.Size = new System.Drawing.Size(800, 450);
            this.logList1.TabIndex = 0;
            // 
            // LogWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.logList1);
            this.Name = "LogWindow";
            this.Text = "LogWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private GCore.WinForms.Controls.LogList logList1;
    }
}
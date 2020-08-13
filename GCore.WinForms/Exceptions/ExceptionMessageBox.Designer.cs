namespace GCore.WinForms.Exceptions {
    partial class ExceptionMessageBox {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.xPropertyGrid1 = new GCore.WinForms.Controls.RuntimeProperty.XPropertyGrid();
            this.SuspendLayout();
            // 
            // xPropertyGrid1
            // 
            this.xPropertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xPropertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.xPropertyGrid1.Name = "xPropertyGrid1";
            this.xPropertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.xPropertyGrid1.Size = new System.Drawing.Size(371, 450);
            this.xPropertyGrid1.TabIndex = 0;
            // 
            // ExceptionMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 450);
            this.Controls.Add(this.xPropertyGrid1);
            this.Name = "ExceptionMessageBox";
            this.Text = "ExceptionMessageBox";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.RuntimeProperty.XPropertyGrid xPropertyGrid1;
    }
}
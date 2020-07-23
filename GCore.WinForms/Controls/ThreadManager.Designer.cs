namespace GCore.WinForms.Controls {
    partial class ThreadManager {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            GCore.WinForms.Controls.GracialList.GLColumn glColumn1 = new GCore.WinForms.Controls.GracialList.GLColumn();
            GCore.WinForms.Controls.GracialList.GLColumn glColumn2 = new GCore.WinForms.Controls.GracialList.GLColumn();
            GCore.WinForms.Controls.GracialList.GLColumn glColumn3 = new GCore.WinForms.Controls.GracialList.GLColumn();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.pbCPU = new System.Windows.Forms.ProgressBar();
            this.glGTreadList = new GCore.WinForms.Controls.GracialList.GlacialList();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPRAM = new System.Windows.Forms.Label();
            this.lblSRAM = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timerUpdate
            // 
            this.timerUpdate.Enabled = true;
            this.timerUpdate.Interval = 500;
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // pbCPU
            // 
            this.pbCPU.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbCPU.Location = new System.Drawing.Point(82, 388);
            this.pbCPU.Name = "pbCPU";
            this.pbCPU.Size = new System.Drawing.Size(219, 23);
            this.pbCPU.TabIndex = 1;
            // 
            // glGTreadList
            // 
            this.glGTreadList.AllowColumnResize = true;
            this.glGTreadList.AllowMultiselect = false;
            this.glGTreadList.AlternateBackground = System.Drawing.Color.DarkGreen;
            this.glGTreadList.AlternatingColors = false;
            this.glGTreadList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glGTreadList.AutoHeight = true;
            this.glGTreadList.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.glGTreadList.BackgroundStretchToFit = true;
            glColumn1.ActivatedEmbeddedType = GCore.WinForms.Controls.GracialList.GLActivatedEmbeddedTypes.None;
            glColumn1.CheckBoxes = false;
            glColumn1.ImageIndex = -1;
            glColumn1.Name = "clName";
            glColumn1.NumericSort = false;
            glColumn1.Text = "Name";
            glColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            glColumn1.Width = 100;
            glColumn2.ActivatedEmbeddedType = GCore.WinForms.Controls.GracialList.GLActivatedEmbeddedTypes.None;
            glColumn2.CheckBoxes = false;
            glColumn2.ImageIndex = -1;
            glColumn2.Name = "clUsage";
            glColumn2.NumericSort = false;
            glColumn2.Text = "Usage";
            glColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            glColumn2.Width = 100;
            glColumn3.ActivatedEmbeddedType = GCore.WinForms.Controls.GracialList.GLActivatedEmbeddedTypes.None;
            glColumn3.CheckBoxes = false;
            glColumn3.ImageIndex = -1;
            glColumn3.Name = "clPecent";
            glColumn3.NumericSort = false;
            glColumn3.Text = "[%]";
            glColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            glColumn3.Width = 100;
            this.glGTreadList.Columns.AddRange(new GCore.WinForms.Controls.GracialList.GLColumn[] {
            glColumn1,
            glColumn2,
            glColumn3});
            this.glGTreadList.ControlStyle = GCore.WinForms.Controls.GracialList.GLControlStyles.Normal;
            this.glGTreadList.FullRowSelect = true;
            this.glGTreadList.GridColor = System.Drawing.Color.LightGray;
            this.glGTreadList.GridLines = GCore.WinForms.Controls.GracialList.GLGridLines.gridBoth;
            this.glGTreadList.GridLineStyle = GCore.WinForms.Controls.GracialList.GLGridLineStyles.gridSolid;
            this.glGTreadList.GridTypes = GCore.WinForms.Controls.GracialList.GLGridTypes.gridOnExists;
            this.glGTreadList.HeaderHeight = 22;
            this.glGTreadList.HeaderVisible = true;
            this.glGTreadList.HeaderWordWrap = false;
            this.glGTreadList.HotColumnTracking = false;
            this.glGTreadList.HotItemTracking = false;
            this.glGTreadList.HotTrackingColor = System.Drawing.Color.LightGray;
            this.glGTreadList.HoverEvents = false;
            this.glGTreadList.HoverTime = 1;
            this.glGTreadList.ImageList = null;
            this.glGTreadList.ItemHeight = 17;
            this.glGTreadList.ItemWordWrap = false;
            this.glGTreadList.Location = new System.Drawing.Point(0, 0);
            this.glGTreadList.Name = "glGTreadList";
            this.glGTreadList.Selectable = true;
            this.glGTreadList.SelectedTextColor = System.Drawing.Color.White;
            this.glGTreadList.SelectionColor = System.Drawing.Color.DarkBlue;
            this.glGTreadList.ShowBorder = true;
            this.glGTreadList.ShowFocusRect = false;
            this.glGTreadList.Size = new System.Drawing.Size(304, 382);
            this.glGTreadList.SortType = GCore.WinForms.Controls.GracialList.SortTypes.InsertionSort;
            this.glGTreadList.SuperFlatHeaderColor = System.Drawing.Color.White;
            this.glGTreadList.TabIndex = 0;
            this.glGTreadList.Text = "glacialList1";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 394);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Process CPU:";
            // 
            // lblPRAM
            // 
            this.lblPRAM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPRAM.AutoSize = true;
            this.lblPRAM.Location = new System.Drawing.Point(6, 421);
            this.lblPRAM.Name = "lblPRAM";
            this.lblPRAM.Size = new System.Drawing.Size(35, 13);
            this.lblPRAM.TabIndex = 3;
            this.lblPRAM.Text = "label2";
            // 
            // lblSRAM
            // 
            this.lblSRAM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSRAM.AutoSize = true;
            this.lblSRAM.Location = new System.Drawing.Point(110, 421);
            this.lblSRAM.Name = "lblSRAM";
            this.lblSRAM.Size = new System.Drawing.Size(35, 13);
            this.lblSRAM.TabIndex = 4;
            this.lblSRAM.Text = "label2";
            // 
            // ThreadManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblSRAM);
            this.Controls.Add(this.lblPRAM);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbCPU);
            this.Controls.Add(this.glGTreadList);
            this.Name = "ThreadManager";
            this.Size = new System.Drawing.Size(304, 468);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GracialList.GlacialList glGTreadList;
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.ProgressBar pbCPU;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPRAM;
        private System.Windows.Forms.Label lblSRAM;
    }
}

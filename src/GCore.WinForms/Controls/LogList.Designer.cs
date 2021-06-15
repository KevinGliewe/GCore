namespace GCore.WinForms.Controls {
    partial class LogList {
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
            GCore.WinForms.Controls.GracialList.GLColumn glColumn1 = new GCore.WinForms.Controls.GracialList.GLColumn();
            GCore.WinForms.Controls.GracialList.GLColumn glColumn2 = new GCore.WinForms.Controls.GracialList.GLColumn();
            GCore.WinForms.Controls.GracialList.GLColumn glColumn3 = new GCore.WinForms.Controls.GracialList.GLColumn();
            GCore.WinForms.Controls.GracialList.GLColumn glColumn4 = new GCore.WinForms.Controls.GracialList.GLColumn();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.cbFatal = new System.Windows.Forms.CheckBox();
            this.cbError = new System.Windows.Forms.CheckBox();
            this.cbException = new System.Windows.Forms.CheckBox();
            this.cbWarn = new System.Windows.Forms.CheckBox();
            this.cbDebug = new System.Windows.Forms.CheckBox();
            this.cbInfo = new System.Windows.Forms.CheckBox();
            this.cbSuccess = new System.Windows.Forms.CheckBox();
            this.cbParams = new System.Windows.Forms.ComboBox();
            this.cbAutoscroll = new System.Windows.Forms.CheckBox();
            this.glLogList = new GCore.WinForms.Controls.GracialList.GlacialList();
            this.rPropertyGrid = new GCore.WinForms.Controls.RuntimeProperty.XPropertyGrid();
            this.btnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.btnClear);
            this.splitContainer.Panel1.Controls.Add(this.cbAutoscroll);
            this.splitContainer.Panel1.Controls.Add(this.cbFatal);
            this.splitContainer.Panel1.Controls.Add(this.cbError);
            this.splitContainer.Panel1.Controls.Add(this.cbException);
            this.splitContainer.Panel1.Controls.Add(this.cbWarn);
            this.splitContainer.Panel1.Controls.Add(this.cbDebug);
            this.splitContainer.Panel1.Controls.Add(this.cbInfo);
            this.splitContainer.Panel1.Controls.Add(this.cbSuccess);
            this.splitContainer.Panel1.Controls.Add(this.glLogList);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.cbParams);
            this.splitContainer.Panel2.Controls.Add(this.rPropertyGrid);
            this.splitContainer.Size = new System.Drawing.Size(1011, 406);
            this.splitContainer.SplitterDistance = 657;
            this.splitContainer.TabIndex = 0;
            // 
            // cbFatal
            // 
            this.cbFatal.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbFatal.AutoSize = true;
            this.cbFatal.Checked = true;
            this.cbFatal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFatal.Location = new System.Drawing.Point(327, 3);
            this.cbFatal.Name = "cbFatal";
            this.cbFatal.Size = new System.Drawing.Size(40, 23);
            this.cbFatal.TabIndex = 7;
            this.cbFatal.Text = "Fatal";
            this.cbFatal.UseVisualStyleBackColor = true;
            this.cbFatal.CheckedChanged += new System.EventHandler(this.cbFatal_CheckedChanged);
            // 
            // cbError
            // 
            this.cbError.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbError.AutoSize = true;
            this.cbError.Checked = true;
            this.cbError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbError.Location = new System.Drawing.Point(282, 3);
            this.cbError.Name = "cbError";
            this.cbError.Size = new System.Drawing.Size(39, 23);
            this.cbError.TabIndex = 6;
            this.cbError.Text = "Error";
            this.cbError.UseVisualStyleBackColor = true;
            this.cbError.CheckedChanged += new System.EventHandler(this.cbError_CheckedChanged);
            // 
            // cbException
            // 
            this.cbException.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbException.AutoSize = true;
            this.cbException.Checked = true;
            this.cbException.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbException.Location = new System.Drawing.Point(212, 3);
            this.cbException.Name = "cbException";
            this.cbException.Size = new System.Drawing.Size(64, 23);
            this.cbException.TabIndex = 5;
            this.cbException.Text = "Exception";
            this.cbException.UseVisualStyleBackColor = true;
            this.cbException.CheckedChanged += new System.EventHandler(this.cbException_CheckedChanged);
            // 
            // cbWarn
            // 
            this.cbWarn.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbWarn.AutoSize = true;
            this.cbWarn.Checked = true;
            this.cbWarn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbWarn.Location = new System.Drawing.Point(163, 3);
            this.cbWarn.Name = "cbWarn";
            this.cbWarn.Size = new System.Drawing.Size(43, 23);
            this.cbWarn.TabIndex = 4;
            this.cbWarn.Text = "Warn";
            this.cbWarn.UseVisualStyleBackColor = true;
            this.cbWarn.CheckedChanged += new System.EventHandler(this.cbWarn_CheckedChanged);
            // 
            // cbDebug
            // 
            this.cbDebug.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbDebug.AutoSize = true;
            this.cbDebug.Checked = true;
            this.cbDebug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDebug.Location = new System.Drawing.Point(108, 3);
            this.cbDebug.Name = "cbDebug";
            this.cbDebug.Size = new System.Drawing.Size(49, 23);
            this.cbDebug.TabIndex = 3;
            this.cbDebug.Text = "Debug";
            this.cbDebug.UseVisualStyleBackColor = true;
            this.cbDebug.CheckedChanged += new System.EventHandler(this.cbDebug_CheckedChanged);
            // 
            // cbInfo
            // 
            this.cbInfo.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbInfo.AutoSize = true;
            this.cbInfo.Checked = true;
            this.cbInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbInfo.Location = new System.Drawing.Point(67, 3);
            this.cbInfo.Name = "cbInfo";
            this.cbInfo.Size = new System.Drawing.Size(35, 23);
            this.cbInfo.TabIndex = 2;
            this.cbInfo.Text = "Info";
            this.cbInfo.UseVisualStyleBackColor = true;
            this.cbInfo.CheckedChanged += new System.EventHandler(this.cbInfo_CheckedChanged);
            // 
            // cbSuccess
            // 
            this.cbSuccess.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbSuccess.AutoSize = true;
            this.cbSuccess.Checked = true;
            this.cbSuccess.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSuccess.Location = new System.Drawing.Point(3, 3);
            this.cbSuccess.Name = "cbSuccess";
            this.cbSuccess.Size = new System.Drawing.Size(58, 23);
            this.cbSuccess.TabIndex = 1;
            this.cbSuccess.Text = "Success";
            this.cbSuccess.UseVisualStyleBackColor = true;
            this.cbSuccess.CheckedChanged += new System.EventHandler(this.cbSuccess_CheckedChanged);
            // 
            // cbParams
            // 
            this.cbParams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbParams.FormattingEnabled = true;
            this.cbParams.Location = new System.Drawing.Point(4, 378);
            this.cbParams.Name = "cbParams";
            this.cbParams.Size = new System.Drawing.Size(343, 21);
            this.cbParams.TabIndex = 1;
            this.cbParams.SelectedIndexChanged += new System.EventHandler(this.cbParams_SelectedIndexChanged);
            // 
            // cbAutoscroll
            // 
            this.cbAutoscroll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAutoscroll.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbAutoscroll.AutoSize = true;
            this.cbAutoscroll.Checked = true;
            this.cbAutoscroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoscroll.Location = new System.Drawing.Point(594, 3);
            this.cbAutoscroll.Name = "cbAutoscroll";
            this.cbAutoscroll.Size = new System.Drawing.Size(63, 23);
            this.cbAutoscroll.TabIndex = 8;
            this.cbAutoscroll.Text = "Autoscroll";
            this.cbAutoscroll.UseVisualStyleBackColor = true;
            // 
            // glLogList
            // 
            this.glLogList.AllowColumnResize = true;
            this.glLogList.AllowMultiselect = false;
            this.glLogList.AlternateBackground = System.Drawing.Color.DarkGreen;
            this.glLogList.AlternatingColors = false;
            this.glLogList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glLogList.AutoHeight = true;
            this.glLogList.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.glLogList.BackgroundStretchToFit = true;
            glColumn1.ActivatedEmbeddedType = GCore.WinForms.Controls.GracialList.GLActivatedEmbeddedTypes.None;
            glColumn1.CheckBoxes = false;
            glColumn1.ImageIndex = -1;
            glColumn1.Name = "clTime";
            glColumn1.NumericSort = false;
            glColumn1.Text = "Time";
            glColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            glColumn1.Width = 140;
            glColumn2.ActivatedEmbeddedType = GCore.WinForms.Controls.GracialList.GLActivatedEmbeddedTypes.None;
            glColumn2.CheckBoxes = false;
            glColumn2.ImageIndex = -1;
            glColumn2.Name = "clType";
            glColumn2.NumericSort = false;
            glColumn2.Text = "Type";
            glColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            glColumn2.Width = 60;
            glColumn3.ActivatedEmbeddedType = GCore.WinForms.Controls.GracialList.GLActivatedEmbeddedTypes.None;
            glColumn3.CheckBoxes = false;
            glColumn3.ImageIndex = -1;
            glColumn3.Name = "clMessage";
            glColumn3.NumericSort = false;
            glColumn3.Text = "Message";
            glColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            glColumn3.Width = 600;
            glColumn4.ActivatedEmbeddedType = GCore.WinForms.Controls.GracialList.GLActivatedEmbeddedTypes.None;
            glColumn4.CheckBoxes = false;
            glColumn4.ImageIndex = -1;
            glColumn4.Name = "clParams";
            glColumn4.NumericSort = false;
            glColumn4.Text = "Params";
            glColumn4.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            glColumn4.Width = 200;
            this.glLogList.Columns.AddRange(new GCore.WinForms.Controls.GracialList.GLColumn[] {
            glColumn1,
            glColumn2,
            glColumn3,
            glColumn4});
            this.glLogList.ControlStyle = GCore.WinForms.Controls.GracialList.GLControlStyles.Normal;
            this.glLogList.FullRowSelect = true;
            this.glLogList.GridColor = System.Drawing.Color.LightGray;
            this.glLogList.GridLines = GCore.WinForms.Controls.GracialList.GLGridLines.gridBoth;
            this.glLogList.GridLineStyle = GCore.WinForms.Controls.GracialList.GLGridLineStyles.gridSolid;
            this.glLogList.GridTypes = GCore.WinForms.Controls.GracialList.GLGridTypes.gridOnExists;
            this.glLogList.HeaderHeight = 22;
            this.glLogList.HeaderVisible = true;
            this.glLogList.HeaderWordWrap = false;
            this.glLogList.HotColumnTracking = false;
            this.glLogList.HotItemTracking = false;
            this.glLogList.HotTrackingColor = System.Drawing.Color.LightGray;
            this.glLogList.HoverEvents = false;
            this.glLogList.HoverTime = 1;
            this.glLogList.ImageList = null;
            this.glLogList.ItemHeight = 17;
            this.glLogList.ItemWordWrap = false;
            this.glLogList.Location = new System.Drawing.Point(0, 32);
            this.glLogList.Name = "glLogList";
            this.glLogList.Selectable = true;
            this.glLogList.SelectedTextColor = System.Drawing.Color.White;
            this.glLogList.SelectionColor = System.Drawing.Color.DarkBlue;
            this.glLogList.ShowBorder = true;
            this.glLogList.ShowFocusRect = false;
            this.glLogList.Size = new System.Drawing.Size(657, 374);
            this.glLogList.SortType = GCore.WinForms.Controls.GracialList.SortTypes.None;
            this.glLogList.SuperFlatHeaderColor = System.Drawing.Color.White;
            this.glLogList.TabIndex = 0;
            this.glLogList.Text = "LogList";
            this.glLogList.SelectedIndexChanged += new GCore.WinForms.Controls.GracialList.GlacialList.ClickedEventHandler(this.glLogList_SelectedIndexChanged);
            // 
            // rPropertyGrid
            // 
            this.rPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.rPropertyGrid.Name = "rPropertyGrid";
            this.rPropertyGrid.Size = new System.Drawing.Size(350, 371);
            this.rPropertyGrid.TabIndex = 0;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(513, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // LogList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Name = "LogList";
            this.Size = new System.Drawing.Size(1011, 406);
            this.Load += new System.EventHandler(this.LogList_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private GracialList.GlacialList glLogList;
        private RuntimeProperty.XPropertyGrid rPropertyGrid;
        private System.Windows.Forms.CheckBox cbFatal;
        private System.Windows.Forms.CheckBox cbError;
        private System.Windows.Forms.CheckBox cbException;
        private System.Windows.Forms.CheckBox cbWarn;
        private System.Windows.Forms.CheckBox cbDebug;
        private System.Windows.Forms.CheckBox cbInfo;
        private System.Windows.Forms.CheckBox cbSuccess;
        private System.Windows.Forms.ComboBox cbParams;
        private System.Windows.Forms.CheckBox cbAutoscroll;
        private System.Windows.Forms.Button btnClear;
    }
}

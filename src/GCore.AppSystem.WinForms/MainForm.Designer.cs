using System.Windows.Forms;

namespace GCore.AppSystem.WinForms
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.menuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.mainMenu.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockPanel
            // 
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.Location = new System.Drawing.Point(0, 0);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(1285, 700);
            this.dockPanel.TabIndex = 0;
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFile,
            this.menuItemView,
            this.menuItemHelp});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(1285, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "mainMenu";
            // 
            // menuItemFile
            // 
            this.menuItemFile.Name = "menuItemFile";
            this.menuItemFile.Size = new System.Drawing.Size(37, 20);
            this.menuItemFile.Text = "&File";
            // 
            // menuItemView
            // 
            this.menuItemView.Name = "menuItemView";
            this.menuItemView.Size = new System.Drawing.Size(44, 20);
            this.menuItemView.Text = "&View";
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAbout});
            this.menuItemHelp.Name = "menuItemHelp";
            this.menuItemHelp.Size = new System.Drawing.Size(44, 20);
            this.menuItemHelp.Text = "&Help";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Name = "menuItemAbout";
            this.menuItemAbout.Size = new System.Drawing.Size(288, 22);
            this.menuItemAbout.Text = "&About GCore.AppSystem.WinForms";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 751);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1285, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip";
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Add.128.png");
            this.imageList.Images.SetKeyName(1, "BlankFile.128.png");
            this.imageList.Images.SetKeyName(2, "ConnectPlugged.128.png");
            this.imageList.Images.SetKeyName(3, "ConnectUnplugged.128.png");
            this.imageList.Images.SetKeyName(4, "Database.128.png");
            this.imageList.Images.SetKeyName(5, "Down.128.png");
            this.imageList.Images.SetKeyName(6, "Edit.128.png");
            this.imageList.Images.SetKeyName(7, "Example.128.png");
            this.imageList.Images.SetKeyName(8, "Folder.128.png");
            this.imageList.Images.SetKeyName(9, "FolderOpen.128.png");
            this.imageList.Images.SetKeyName(10, "Form.128.png");
            this.imageList.Images.SetKeyName(11, "GlobalVariable.128.png");
            this.imageList.Images.SetKeyName(12, "Image.128.png");
            this.imageList.Images.SetKeyName(13, "Interface.128.png");
            this.imageList.Images.SetKeyName(14, "NewFile.128.png");
            this.imageList.Images.SetKeyName(15, "Open.128.png");
            this.imageList.Images.SetKeyName(16, "Output.128.png");
            this.imageList.Images.SetKeyName(17, "Property.128.png");
            this.imageList.Images.SetKeyName(18, "Reference.128.png");
            this.imageList.Images.SetKeyName(19, "Remove.128.png");
            this.imageList.Images.SetKeyName(20, "RunUpdate.128.png");
            this.imageList.Images.SetKeyName(21, "Settings.128.png");
            this.imageList.Images.SetKeyName(22, "SourceFile.128.png");
            this.imageList.Images.SetKeyName(23, "Table.128.png");
            this.imageList.Images.SetKeyName(24, "Task.128.png");
            this.imageList.Images.SetKeyName(25, "Toolbox.128.png");
            this.imageList.Images.SetKeyName(26, "Up.128.png");
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.dockPanel);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(1285, 700);
            this.toolStripContainer.Location = new System.Drawing.Point(0, 27);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(1285, 725);
            this.toolStripContainer.TabIndex = 3;
            this.toolStripContainer.Text = "toolStripContainer";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1285, 773);
            this.Controls.Add(this.toolStripContainer);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "GCore.AppSystem.WinForms";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private MenuStrip mainMenu;
        private ToolStripMenuItem menuItemFile;
        private ToolStripMenuItem menuItemView;
        private ToolStripMenuItem menuItemHelp;
        private ToolStripMenuItem menuItemAbout;
        protected StatusStrip statusStrip;
        private ImageList imageList;
        private ToolStripContainer toolStripContainer;
    }
}
/* ****************************************************************************
 *  RuntimeObjectEditor
 * 
 * Copyright (c) 2005 Corneliu I. Tusnea
 * 
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the author be held liable for any damages arising from 
 * the use of this software.
 * Permission to use, copy, modify, distribute and sell this software for any 
 * purpose is hereby granted without fee, provided that the above copyright 
 * notice appear in all copies and that both that copyright notice and this 
 * permission notice appear in supporting documentation.
 * 
 * Corneliu I. Tusnea (corneliutusnea@yahoo.com.au)
 * www.acorns.com.au
 * ****************************************************************************/

using System;
using System.Drawing;
using System.Windows.Forms;
using GCore.WinForms.Controls.RuntimeProperty.Utils;

namespace GCore.WinForms.Controls.RuntimeProperty
{
	/// <summary>
	/// RuntimeEditor - The editor with the properties window and the window finder
	/// </summary>
	public class RuntimeEditor : Form
	{
		private TextBox txtType;
		private TextBox txtToString;
		private WindowFinder windowFinder;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Button btnCollapseAll;
		private System.Windows.Forms.Button btnRefresh;
		private XPropertyGrid propertyGrid;

		public RuntimeEditor()
		{
			InitializeComponent();
			this.Text = "RuntimeObjectEditor " + this.GetType().Assembly.GetName().Version.ToString(3);
		}

        public bool EnableWindowFinder {
            get { return this.windowFinder.Enabled; }
            set { this.windowFinder.Enabled = value; }
        }

		private System.ComponentModel.IContainer components;

		#region Windows Form Designer generated code


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#endregion

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RuntimeEditor));
            this.txtType = new System.Windows.Forms.TextBox();
            this.txtToString = new System.Windows.Forms.TextBox();
            this.btnCollapseAll = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnRefresh = new System.Windows.Forms.Button();
            this.windowFinder = new GCore.WinForms.Controls.RuntimeProperty.WindowFinder();
            this.propertyGrid = new GCore.WinForms.Controls.RuntimeProperty.XPropertyGrid();
            this.SuspendLayout();
            // 
            // txtType
            // 
            this.txtType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtType.Location = new System.Drawing.Point(32, 0);
            this.txtType.Name = "txtType";
            this.txtType.ReadOnly = true;
            this.txtType.Size = new System.Drawing.Size(292, 20);
            this.txtType.TabIndex = 1;
            this.txtType.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.txtType, "Object Type");
            // 
            // txtToString
            // 
            this.txtToString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtToString.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtToString.Location = new System.Drawing.Point(32, 20);
            this.txtToString.Name = "txtToString";
            this.txtToString.ReadOnly = true;
            this.txtToString.Size = new System.Drawing.Size(252, 20);
            this.txtToString.TabIndex = 2;
            this.txtToString.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.txtToString, "Object.ToString()");
            // 
            // btnCollapseAll
            // 
            this.btnCollapseAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollapseAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCollapseAll.Location = new System.Drawing.Point(288, 22);
            this.btnCollapseAll.Name = "btnCollapseAll";
            this.btnCollapseAll.Size = new System.Drawing.Size(16, 16);
            this.btnCollapseAll.TabIndex = 3;
            this.btnCollapseAll.Text = "-";
            this.toolTip.SetToolTip(this.btnCollapseAll, "Collapse All");
            this.btnCollapseAll.Click += new System.EventHandler(this.btnCollapseAll_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(306, 22);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(16, 16);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "*";
            this.toolTip.SetToolTip(this.btnRefresh, "Refresh");
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // windowFinder
            // 
            this.windowFinder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("windowFinder.BackgroundImage")));
            this.windowFinder.Location = new System.Drawing.Point(0, 4);
            this.windowFinder.Name = "windowFinder";
// TODO: Beim Generieren des Codes für "" ist die Ausnahme "Ungültiger primitiver Typ: System.IntPtr. Verwenden Sie CodeObjectCreateExpression." aufgetreten.
            this.windowFinder.Size = new System.Drawing.Size(32, 32);
            this.windowFinder.TabIndex = 0;
            this.toolTip.SetToolTip(this.windowFinder, "WindowFinder - click & drag to select any .Net window from any process.");
            this.windowFinder.ActiveWindowChanged += new System.EventHandler(this.windowFinder_ActiveWindowChanged);
            this.windowFinder.ActiveWindowSelected += new System.EventHandler(this.windowFinder_ActiveWindowSelected);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.CommandsActiveLinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.propertyGrid.CommandsDisabledLinkColor = System.Drawing.SystemColors.ControlDark;
            this.propertyGrid.CommandsLinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid.Location = new System.Drawing.Point(1, 44);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGrid.Size = new System.Drawing.Size(322, 309);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.SelectRequest += new GCore.WinForms.Controls.RuntimeProperty.XPropertyGrid.SelectedObjectRequestHandler(this.propertyGrid_SelectRequest);
            // 
            // RuntimeEditor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(324, 354);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnCollapseAll);
            this.Controls.Add(this.txtToString);
            this.Controls.Add(this.txtType);
            this.Controls.Add(this.windowFinder);
            this.Controls.Add(this.propertyGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RuntimeEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Runtime Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void propertyGrid_SelectRequest(object newObject)
		{
			ChangeSelectedObject(newObject);
		}

		private void windowFinder_ActiveWindowChanged(object sender, EventArgs e)
		{
			object selectedObject = windowFinder.SelectedObject;

			ChangeSelectedObject(selectedObject);
		}

		private void windowFinder_ActiveWindowSelected(object sender, EventArgs e)
		{
			object selectedObject = windowFinder.SelectedObject;

			if (!ChangeSelectedObject(selectedObject))
			{ // could not set window - might be another process
				if (windowFinder.SelectedHandle != IntPtr.Zero)
				{
					// check if different process
					if (NativeUtils.IsTargetInDifferentProcess(windowFinder.SelectedHandle) /*&& this.windowFinder.IsManagedByClassName*/)
					{ // inject
						//RuntimeEditorHook.Hook(windowFinder.SelectedHandle, this.Handle);
					}
				}
			}
		}


		private bool ChangeSelectedObject(object selectedObject)
		{
			propertyGrid.SelectedObject = selectedObject;
			if (selectedObject != null)
			{
				Control ctl = selectedObject as Control;
				if (ctl != null)
					this.Text = "Runtime Editor:" + ctl.Name;

				txtType.Text = selectedObject.GetType().FullName;

				try
				{
					txtToString.Text = selectedObject.ToString();
				}
				catch (Exception ex)
				{
					txtToString.Text = "<ex:>" + ex.Message;
				}
				ShowTail(txtType);
				ShowTail(txtToString);
				return true;
			}
			else
			{
				txtToString.Text = "";
				if (NativeUtils.IsTargetInDifferentProcess(windowFinder.SelectedHandle))
				{
					if (this.windowFinder.IsManagedByClassName)
					{
						txtType.Text = "<target in different process. release selection to hook>";
					}
					else
					{
						txtType.Text = "<target not in a managed process>";
					}
				}
				else
				{
					if (windowFinder.Window.IsValid)
					{
						txtType.Text = windowFinder.Window.Text;
						txtToString.Text = "ClassName:" + windowFinder.Window.ClassName;
					}
					else
					{
						txtType.Text = "<no selection or unknown window>";
					}
				}
				return false;
			}
		}

		private void ShowTail(TextBox txtBox)
		{
			txtBox.SelectionStart = txtBox.Text.Length;
			txtBox.SelectionLength = 0;
		}

		#region OnLoad

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			int top = SystemInformation.WorkingArea.Bottom - this.Height;
			this.Location = new Point(0, top);
		}

		#endregion

		private void btnCollapseAll_Click(object sender, System.EventArgs e)
		{
			propertyGrid.Visible = false;
			propertyGrid.CollapseAllGridItems();
			propertyGrid.Visible = true;
		}

		private void btnRefresh_Click(object sender, System.EventArgs e)
		{
			object oldSelectedObject = this.SelectedObject;
			propertyGrid.Refresh();
			//this.SelectedObject = null;
		}

		#region Properties

		public object SelectedObject
		{
			get { return propertyGrid.SelectedObject; }
			set
			{
				ChangeSelectedObject(value);
			}
		}

		internal IntPtr SelectedWindowHandle
		{
			get { return this.windowFinder.SelectedHandle; }
			set { this.windowFinder.SelectedHandle = value; }
		}

		#endregion
	}
}
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
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using GCore.WinForms.Controls.RuntimeProperty.Tabs.Events;
using GCore.WinForms.Controls.RuntimeProperty.Tabs.Fields;
using GCore.WinForms.Controls.RuntimeProperty.Tabs.Methods;
using GCore.WinForms.Controls.RuntimeProperty.Tabs.ProcessInfo;
using GCore.WinForms.Controls.RuntimeProperty.Tabs.Properties;
using GCore.WinForms.Controls.RuntimeProperty.Utils;

namespace GCore.WinForms.Controls.RuntimeProperty
{
    /// <summary>
    /// Summary description for XPropertyGrid.
    /// </summary>
    public class XPropertyGrid : PropertyGrid
    {
        public delegate void SelectedObjectRequestHandler(object newObject);

        public event SelectedObjectRequestHandler SelectRequest;

        public XPropertyGrid()
        {
            InitializeComponent();
        }

#if NET2
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem selectThisItem;
        private ToolStripMenuItem goBackOneItem;
        private ToolStripMenuItem goForwardOneItem;
#else
        private ContextMenu contextMenu;
        private MenuItem selectThisItem;
        private MenuItem goBackOneItem;
        private MenuItem goForwardOneItem;
#endif
        private ArrayList historyObjects = new ArrayList();

        private int activeObject = -1;

        #region Component Designer generated code

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();
            this.PropertyTabChanged += new PropertyTabChangedEventHandler(XPropertyGrid_PropertyTabChanged);
            this.ResumeLayout(false);
            this.OnCreateControl();
        }

        #endregion

        #region Context Menu

#if NET2
        private void InitContextMenu()
        {
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectThisItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goBackOneItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goForwardOneItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();

            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
                {
                    this.selectThisItem,
                    this.goBackOneItem,
                    this.goForwardOneItem
                });
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new Size(126, 70);
            // 
            // selectThisItem
            // 
            this.selectThisItem.Name = "selectThisItem";
            this.selectThisItem.Size = new Size(125, 22);
            this.selectThisItem.Text = "Select";
            this.selectThisItem.Click += new EventHandler(selectThisItem_Click);
            // 
            // goBackOneItem
            // 
            this.goBackOneItem.Name = "goBackOneItem";
            this.goBackOneItem.Size = new Size(125, 22);
            this.goBackOneItem.Text = "Back";
            this.goBackOneItem.Click += new EventHandler(this.goBackOneItem_Click);
            // 
            // goForwardOneItem
            // 
            this.goForwardOneItem.Name = "goForwardOneItem";
            this.goForwardOneItem.Size = new Size(125, 22);
            this.goForwardOneItem.Text = "Forward";
            this.goForwardOneItem.Click += new EventHandler(this.goForwardOneItem_Click);
            this.contextMenu.ResumeLayout(false);
        }

#else
        private void InitContextMenu()
        {
            contextMenu = new ContextMenu();
            goBackOneItem = new MenuItem("Back");
            goForwardOneItem = new MenuItem("Forward");
            selectThisItem = new MenuItem("Select");

            selectThisItem.Click += new EventHandler(selectThisItem_Click);
            goBackOneItem.Click += new EventHandler(goBackOneItem_Click);
            goForwardOneItem.Click += new EventHandler(goForwardOneItem_Click);

            contextMenu.MenuItems.AddRange(new MenuItem[] {selectThisItem, goBackOneItem, goForwardOneItem});
        }
#endif

        #endregion

        #region Properties

        public override bool CanShowCommands
        {
            get { return true; }
        }

        public override bool CommandsVisibleIfAvailable
        {
            get { return true; }
            set { base.CommandsVisibleIfAvailable = value; }
        }

        #endregion

        protected override void OnCreateControl()
        {
            DrawFlatToolbar = true;
            HelpVisible = true;

            PropertySort = PropertySort.Alphabetical;

            InitContextMenu();

#if NET2
            this.ContextMenuStrip = this.contextMenu;
#else
            this.ContextMenu = this.contextMenu;
#endif
            goBackOneItem.Enabled = false;
            goForwardOneItem.Enabled = false;

            base.OnCreateControl();


            // Add New Tabs here
            base.PropertyTabs.AddTabType(typeof (AllPropertiesTab));
            base.PropertyTabs.AddTabType(typeof (AllFieldsTab));
            base.PropertyTabs.AddTabType(typeof (InstanceEventsTab));
            base.PropertyTabs.AddTabType(typeof (MethodsTab));
            base.PropertyTabs.AddTabType(typeof (ProcessInfoTab));

            historyObjects.Clear();
        }

        private void XPropertyGrid_PropertyTabChanged(object s, PropertyTabChangedEventArgs e)
        {
        }

        #region Navigation

        protected override void OnSelectedObjectsChanged(EventArgs e)
        { // put in history
            if (SelectedObject != null)
            {
                if (!historyObjects.Contains(SelectedObject))
                {
                    if (activeObject < historyObjects.Count - 1)
                    {
                        historyObjects.RemoveRange(activeObject + 1, historyObjects.Count - activeObject - 1);
                    }
                    activeObject = historyObjects.Add(SelectedObject);
                    goBackOneItem.Enabled = true;
                    goForwardOneItem.Enabled = false;

                    if (historyObjects.Count > 10)
                    {
                        historyObjects.RemoveRange(0, historyObjects.Count - 10);
                    }
                }
                else
                {
                    activeObject = historyObjects.IndexOf(SelectedObject);
                }
            }
            base.OnSelectedObjectsChanged(e);
        }

        private void selectThisItem_Click(object sender, EventArgs e)
        {
            GridItem selectedGridItem = this.SelectedGridItem;

            if (selectedGridItem != null)
            {
                /*object value = selectedGridItem.Value;
                IRealValueHolder valueHolder = value as IRealValueHolder;
                if (valueHolder != null)
                {
                    value = valueHolder.RealValue;
                }
                InvokeSelectRequest(value);*/
                RuntimeEditor re = new RuntimeEditor();
                re.SelectedObject = selectedGridItem.Value;
                re.EnableWindowFinder = false;
                re.ShowDialog();
            }
        }

        private void goBackOneItem_Click(object sender, EventArgs e)
        {
            if (activeObject > 0)
            {
                activeObject--;
                goForwardOneItem.Enabled = true;
            }
            else
            {
                goBackOneItem.Enabled = false;
            }
            InvokeSelectRequest();
        }

        private void goForwardOneItem_Click(object sender, EventArgs e)
        {
            if (activeObject < historyObjects.Count)
            {
                activeObject ++;
                goBackOneItem.Enabled = true;
            }
            else
            {
                goForwardOneItem.Enabled = false;
            }
            InvokeSelectRequest();
        }

        private object GetActiveObject()
        {
            if (activeObject >= 0 && activeObject < historyObjects.Count)
            {
                return historyObjects[activeObject];
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Invoke

        private void InvokeSelectRequest()
        {
            if (SelectRequest != null)
            {
                SelectRequest(GetActiveObject());
            }
        }

        private void InvokeSelectRequest(object newOBject)
        {
            if (SelectRequest != null)
            {
                SelectRequest(newOBject);
            }
        }

        #endregion
    }
}
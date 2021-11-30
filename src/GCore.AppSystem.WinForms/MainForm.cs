using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Autofac;
using GCore.Logging;
using GCore.AppSystem.WinForms.Config;
using GCore.AppSystem.WinForms.MainFormPlugins;
using GCore.AppSystem.WinForms.ToolWindows;
using GCore.AppSystem.WinForms.ToolWindows.Administration;
using WeifenLuo.WinFormsUI.Docking;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GCore.AppSystem.WinForms
{
    public partial class MainForm : Form, IMainForm
    {
        private IToolWindowsAdmin _toolWindowsAdmin;
        private IContainer _services;
        private MainFormOptions _options;

        public MainForm(IToolWindowsAdmin toolWindowsAdmin, IContainer services, MainFormOptions options)
        {
            InitializeComponent();
            _toolWindowsAdmin = toolWindowsAdmin;
            _services = services;
            _options = options;

            dockPanel.Theme = new VS2015BlueTheme();
        }

        protected virtual void OpenInitialWindows()
        {
            foreach (var openWindow in _options.OpenWindows)
            {
                OpenToolWindow(openWindow);
            }
        }

        #region GUI Events
        private void menuItemViewToolWindow_Click(object? sender, EventArgs e)
        {
            var windowName = (sender as ToolStripLabel)?.Text ?? throw new Exception();
            OpenToolWindow(windowName);
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine("FileVersion : " + AssemblyVersionConstants.FileVersion);
            sb.AppendLine("InformationalVersion : " + AssemblyVersionConstants.InformationalVersion);
            //sb.AppendLine("Build Timestamp : " + AppSystemManager.GetBuildDate(typeof(MainForm).Assembly));
            MessageBox.Show(sb.ToString(), "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            foreach (var entry in _toolWindowsAdmin)
                NotifyToolWindowAdded(entry.Key);

            OpenInitialWindows();

            foreach (var mainFormPlugin in IMainFormPlugin.GetPlugins(_services))
            {
                mainFormPlugin.Loading(this);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var mainFormPlugin in IMainFormPlugin.GetPlugins(_services))
            {
                mainFormPlugin.Closing(this);
            }
        }
        #endregion

        #region IMainForm
        public virtual DockPanel DockPanel => dockPanel;

        public virtual MenuStrip MainMenu => mainMenu;

        public virtual ImageList ImageList => imageList;

        public virtual ToolStripContainer ToolStripContainer => toolStripContainer;

        public virtual StatusStrip StatusStrip => statusStrip;

        public virtual void NotifyToolWindowAdded(string name)
        {
            Debug.Assert(_toolWindowsAdmin.Keys.Contains(name));
            
            var tsl = new ToolStripLabel()
            {
                Text = name,
                Tag = name,
                AutoSize = true
            };
            tsl.Width = TextRenderer.MeasureText(name, tsl.Font).Width;
            tsl.Click += menuItemViewToolWindow_Click;

            menuItemView.DropDownItems.Add(tsl);

            Log.Debug($"NotifyToolWindowAdded '{name}'");
        }

        public virtual IToolWindowBase OpenToolWindow(string name)
        {
            var window = _toolWindowsAdmin.GetToolWindow(name);
            window.ToolWindowShow(this);
            return window;
        }

        public void SpawnPropertiesWindow(object obj)
        {
            var window = _services.Resolve<IPropertiesWindow>() as IPropertiesWindow;
            window.SelectObject(obj);
            window.ToolWindowShow(this);
        }

        public T OpenToolWindow<T>() where T : class, IToolWindowBase
        {
            IToolWindowBase window = _services.Resolve<T>() as T ?? throw new Exception("Error while opening tool window " + typeof(T));
            window.ToolWindowShow(this);
            return (T)window;
        }

        public IToolWindowBase OpenToolWindow(Type type)
        {
            var window = _services.Resolve(type) as IToolWindowBase ?? throw new Exception("Error while opening tool window " + type); ;
            window.ToolWindowShow(this);
            return window;
        }

        public void Invoke(Action action) => this.dockPanel.Invoke(action);

        #endregion
    }
}
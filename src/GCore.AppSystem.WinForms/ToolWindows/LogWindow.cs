using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCore.AppSystem.WinForms.ToolWindows.Administration;
using WeifenLuo.WinFormsUI.Docking;

namespace GCore.AppSystem.WinForms.ToolWindows
{
    public partial class LogWindow : ToolWindowBase, ILogWindow
    {
        public LogWindow()
        {
            InitializeComponent();
        }

        public override void ToolWindowShow(IMainForm form)
        {
            base.Show(form.DockPanel, DockState.DockBottom);
        }
    }
}

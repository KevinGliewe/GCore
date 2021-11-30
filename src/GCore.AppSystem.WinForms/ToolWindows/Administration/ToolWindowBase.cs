using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCore.AppSystem.Handler;
using WeifenLuo.WinFormsUI.Docking;

namespace GCore.AppSystem.WinForms.ToolWindows.Administration
{
    public partial class ToolWindowBase : DockContent, IToolWindowBase
    {
        public Lifetime Lifetime { get; private set; }

        public ToolWindowBase()
        {
            AutoScaleMode = AutoScaleMode.Dpi;
            Lifetime = LifetimeAttribute.GetLifetime(GetType());

            FormClosing += (sender, args) =>
            {
                if (Lifetime == Lifetime.Singleton)
                {
                    args.Cancel = true;
                    base.Hide();
                }
            };
        }

        public virtual void ToolWindowShow(IMainForm form)
        {
            base.Show(form.DockPanel);
        }

    }
}

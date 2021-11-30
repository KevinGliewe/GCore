using GCore.AppSystem.Handler;
using GCore.AppSystem.WinForms.ToolWindows.Administration;
using WeifenLuo.WinFormsUI.Docking;

namespace GCore.AppSystem.WinForms.ToolWindows
{
    [Lifetime(Lifetime.Transient)]
    public partial class PropertiesWindow : ToolWindowBase, IPropertiesWindow
    {
        public PropertiesWindow()
        {
            InitializeComponent();
        }

        public void SelectObject(object obj)
        {
            this.Text = obj.ToString();
            xPropertyGrid.SelectedObject = obj;
        }

        public override void ToolWindowShow(IMainForm form)
        {
            base.Show(form.DockPanel, DockState.DockLeft);
        }
    }
}

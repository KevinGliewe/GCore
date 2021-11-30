using WeifenLuo.WinFormsUI.Docking;

namespace GCore.AppSystem.WinForms.ToolWindows.Administration;

public interface IToolWindowBase : IDockContent
{
    void ToolWindowShow(IMainForm dockPanel);
}
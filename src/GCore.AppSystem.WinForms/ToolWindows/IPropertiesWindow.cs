using GCore.AppSystem.Handler;
using GCore.AppSystem.WinForms.ToolWindows.Administration;

namespace GCore.AppSystem.WinForms.ToolWindows;

[ToolWindow("Properties Window")]
[Handler("ToolWindows:PropertiesWindow", nameof(PropertiesWindow))]
[Lifetime(Lifetime.Transient)]
public interface IPropertiesWindow : IToolWindowBase
{
    void SelectObject(object obj);
}
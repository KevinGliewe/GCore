using GCore.AppSystem.Handler;
using GCore.AppSystem.WinForms.ToolWindows.Administration;

namespace GCore.AppSystem.WinForms.ToolWindows;

[ToolWindow("Log Window")]
[Handler("ToolWindows:LogWindow", nameof(LogWindow))]
[Lifetime(Lifetime.Singleton)]
public interface ILogWindow: IToolWindowBase
{
    
}
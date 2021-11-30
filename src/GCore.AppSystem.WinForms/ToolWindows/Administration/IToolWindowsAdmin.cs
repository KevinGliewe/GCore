using System;
using System.Collections.Generic;
using Autofac.Core;
using GCore.AppSystem.Handler;
using GCore.Data;

namespace GCore.AppSystem.WinForms.ToolWindows.Administration;

[Handler("Handlers:ToolWindowsAdmin", nameof(ToolWindowsAdmin))]
[Lifetime(Lifetime.Singleton)]
public interface IToolWindowsAdmin : IReadOnlyDictionary<String, Type>
{
    ToolWindowBase GetToolWindow(string name);
    ToolWindowBase GetToolWindow(string name, IEnumerable<Parameter> parameters);
    ToolWindowBase GetToolWindow(string name, params Parameter[] parameters);
}
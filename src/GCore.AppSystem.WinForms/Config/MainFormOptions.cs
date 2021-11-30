using GCore.AppSystem.Config;

namespace GCore.AppSystem.WinForms.Config;

[ConfigOption("MainForm")]

public sealed record MainFormOptions
{
    public string[] OpenWindows { get; set; } = new string[] { };
}
using System.Collections.Generic;
using System.Windows.Forms;

namespace GCore.AppSystem.WinForms.MainFormPlugins;

public class StatusStripBase : IMainFormPlugin
{
    public IMainForm MainForm { get; private set; }
    public IList<ToolStripItem> StripItems { get; private set; } = new List<ToolStripItem>();

    public ToolStripStatusLabel AddLabel(string text = "")
    {
        var label = new ToolStripStatusLabel(text);
        StripItems.Add(label);
        return label;
    }

    public StatusStripBase(IMainForm mainForm)
    {
        MainForm = mainForm;
    }

    public virtual void Closing(IMainForm form)
    {
        foreach (var item in StripItems)
        {
            MainForm.StatusStrip.Items.Remove(item);
        }
    }

    public virtual void Loading(IMainForm form)
    {
        foreach (var item in StripItems)
        {
            MainForm.StatusStrip.Items.Add(item);
        }
    }
}
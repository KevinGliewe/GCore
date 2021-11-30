using System;
using System.Windows.Forms;

namespace GCore.AppSystem.WinForms.MainFormPlugins;

public abstract class ToolStripBase : ToolStrip, IMainFormPlugin
{
    public IMainForm MainForm { get; private set; }

    public ToolStripBase(IMainForm mainForm)
    {
        MainForm = mainForm;
    }

    public ToolStripButton AddButton(ImageListId imid, string toolTip, Action<ToolStripButton> action)
    {
        var button = new ToolStripButton()
        {
            ToolTipText = toolTip,
            Image = MainForm.ImageList.Images[(int)imid]
        };
        button.Click += (sender, args) => action(sender as ToolStripButton ?? throw new Exception());
        this.Items.Add(button);
        return button;
    }

    public ToolStripButton AddButton(ImageListId imid, Action<ToolStripButton> action) => AddButton(imid, "", action);

    public void AddSeparator() => this.Items.Add(new ToolStripSeparator());



    #region IMainFormPlugin
    public void Closing(IMainForm form)
    {
        form.ToolStripContainer.TopToolStripPanel.Controls.Remove(this);
    }

    public void Loading(IMainForm form)
    {
        form.ToolStripContainer.TopToolStripPanel.Controls.Add(this);

    }
    #endregion
}
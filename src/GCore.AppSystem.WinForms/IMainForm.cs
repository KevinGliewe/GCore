using System;
using System.Windows.Forms;
using GCore.AppSystem.WinForms.ToolWindows.Administration;
using WeifenLuo.WinFormsUI.Docking;

namespace GCore.AppSystem.WinForms;

public interface IMainForm
{
    MenuStrip MainMenu { get; }
    ToolStripContainer ToolStripContainer { get; }
    StatusStrip StatusStrip { get; }
    DockPanel DockPanel { get; }
    ImageList ImageList { get; }

    void Invoke(Action action);

    void NotifyToolWindowAdded(string name);

    IToolWindowBase OpenToolWindow(string name);
    T OpenToolWindow<T>() where T : class, IToolWindowBase;
    IToolWindowBase OpenToolWindow(Type type);

    void SpawnPropertiesWindow(object obj);
}

public enum ImageListId
{
    Add = 0,
    BlankFile = 1,
    ConnectPlugged = 2,
    ConnectUnplugged = 3,
    Database = 4,
    Down = 5,
    Edit = 6,
    Example = 7,
    Folder = 8,
    FolderOpen = 9,
    Form = 10,
    GlobalVariable = 11,
    Image = 12,
    Interface = 13,
    NewFile = 14,
    Open = 15,
    Output = 16,
    Property = 17,
    Reference = 18,
    Remove = 19,
    RunUpdate = 20,
    Settings = 21,
    SourceFile = 22,
    Table = 23,
    Task = 24,
    Toolbox = 25,
    Up = 26,
}
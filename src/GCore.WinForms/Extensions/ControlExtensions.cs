using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GCore.Sys.Windows;

namespace GCore.WinForms.Extensions;

public static class ControlExtensions
{
    public static IDisposable RetainScrollPos(this Control self, int bar = 1)
    {
        return new ScrollPosRetainer(self.Handle, bar);
    }

    public class ScrollPosRetainer : IDisposable
    {


        private int _pos;
        private IntPtr _handle;
        private int _bar;

        public ScrollPosRetainer(IntPtr handle, int bar)
        {
            _handle = handle;
            _bar = bar;
            _pos = Win32API.GetScrollPos(handle, bar);
        }

        public void Dispose()
        {
            var delta = _pos - Win32API.GetScrollPos(_handle, _bar);

            Win32API.SetScrollPos(_handle, _bar, _pos, true);
            Win32API.SendMessage(_handle, Win32API.EM_LINESCROLL, 0, (uint)delta);
        }
    }
}
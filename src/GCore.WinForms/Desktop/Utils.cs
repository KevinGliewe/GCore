using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using GCore.Sys.Windows;
using System.Runtime.InteropServices;
using GCore.Sys;

namespace GCore.WinForms.Desktop {
    public static class Utils {
        public static Bitmap CaptureScreen() {
            Bitmap b = new Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            Graphics g = Graphics.FromImage(b);
            g.CopyFromScreen(0, 0, 0, 0, b.Size);
            g.Dispose();
            return b;
        }

        #region Win32
        //This structure shall be used to keep the size of the screen.
        public struct SIZE {
            public int cx;
            public int cy;
        }

        static Bitmap Win32CaptureDesktop() {
            SIZE size;
            IntPtr hBitmap;
            IntPtr hDC = Win32API.GetDC(Win32API.GetDesktopWindow());
            IntPtr hMemDC = GDIAPI.CreateCompatibleDC(hDC);

            size.cx = Win32API.GetSystemMetrics
                      (Win32API.SM_CXSCREEN);

            size.cy = Win32API.GetSystemMetrics
                      (Win32API.SM_CYSCREEN);

            hBitmap = GDIAPI.CreateCompatibleBitmap(hDC, size.cx, size.cy);

            if (hBitmap != IntPtr.Zero) {
                IntPtr hOld = (IntPtr)GDIAPI.SelectObject
                                       (hMemDC, hBitmap);

                GDIAPI.BitBlt(hMemDC, 0, 0, size.cx, size.cy, hDC,
                                               0, 0, GDIAPI.SRCCOPY);

                GDIAPI.SelectObject(hMemDC, hOld);
                GDIAPI.DeleteDC(hMemDC);
                Win32API.ReleaseDC(Win32API.GetDesktopWindow(), hDC);
                Bitmap bmp = System.Drawing.Image.FromHbitmap(hBitmap);
                GDIAPI.DeleteObject(hBitmap);
                GC.Collect();
                return bmp;
            }
            return null;

        }


        static Bitmap Win32CaptureCursor(ref int x, ref int y) {
            Bitmap bmp;
            IntPtr hicon;
            CURSORINFO ci = new CURSORINFO();
            ICONINFO icInfo;
            ci.cbSize = Marshal.SizeOf(ci);
            if (Win32API.GetCursorInfo(out ci)) {
                if (ci.flags == Win32API.CURSOR_SHOWING) {
                    hicon = Win32API.CopyIcon(ci.hCursor);
                    if (Win32API.GetIconInfo(hicon, out icInfo)) {
                        x = ci.ptScreenPos.x - ((int)icInfo.xHotspot);
                        y = ci.ptScreenPos.y - ((int)icInfo.yHotspot);

                        Icon ic = Icon.FromHandle(hicon);
                        bmp = ic.ToBitmap();
                        return bmp;
                    }
                }
            }

            return null;
        }

        public static Bitmap Win32CaptureDesktopWithCursor() {
            int cursorX = 0;
            int cursorY = 0;
            Bitmap desktopBMP;
            Bitmap cursorBMP;
            Bitmap finalBMP;
            Graphics g;
            Rectangle r;

            desktopBMP = Win32CaptureDesktop();
            cursorBMP = Win32CaptureCursor(ref cursorX, ref cursorY);
            if (desktopBMP != null) {
                if (cursorBMP != null) {
                    r = new Rectangle(cursorX, cursorY, cursorBMP.Width, cursorBMP.Height);
                    g = Graphics.FromImage(desktopBMP);
                    g.DrawImage(cursorBMP, r);
                    g.Flush();

                    return desktopBMP;
                } else
                    return desktopBMP;
            }

            return null;

        }
        #endregion
    }
}

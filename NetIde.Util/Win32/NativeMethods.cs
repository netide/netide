using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NetIde.Util.Win32
{
    internal static class NativeMethods
    {
        public const int WM_NCHITTEST = 0x0084;
        public const int WM_PARENTNOTIFY = 0x0210;
        public const int WM_XBUTTONDOWN = 0x020b;
        public const int WM_APPCOMMAND = 0x0319;
        public const int WM_NCPAINT = 0x0085;
        public const int WM_ERASEBKGND = 0x0014;
        public const int WM_NOTIFY = 0x004E;

        public const int APPCOMMAND_BROWSER_BACKWARD = 1;
        public const int APPCOMMAND_BROWSER_FORWARD = 2;

        public const int XBUTTON1 = 0x1;
        public const int XBUTTON2 = 0x2;

        public const int SC_CLOSE = 0xF060;

        public const int MF_ENABLED = 0x0;
        public const int MF_GRAYED = 0x1;

        public const int HTBOTTOMLEFT = 16;
        public const int HTBOTTOMRIGHT = 17;
        public const int HTLEFT = 10;
        public const int HTBORDER = 18;

        public const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
        public const int TVM_GETEXTENDEDSTYLE = 0x1100 + 45;
        public const int TVS_EX_DOUBLEBUFFER = 0x0004;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport(ExternDll.User32)]
        public static extern bool GetMenuItemRect(HandleRef hWnd, HandleRef hMenu, uint uItem, out RECT lprcItem);

        [DllImport(ExternDll.User32)]
        public static extern int EnableMenuItem(HandleRef hMenu, int wIDEnableItem, int wEnable);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetSystemMenu(HandleRef hWnd, bool bRevert);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport(ExternDll.User32)]
        public static extern IntPtr SetForegroundWindow(HandleRef hWnd);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport(ExternDll.Uxtheme, CharSet = CharSet.Unicode)]
        public extern static int SetWindowTheme(HandleRef hWnd, string pszSubAppName, string pszSubIdList);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParram);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetActiveWindow();

        public static class Util
        {
            public static int LOWORD(IntPtr n)
            {
                return LOWORD(unchecked((int)(long)n));
            }

            public static int LOWORD(int n)
            {
                return n & 0xffff;
            }

            public static int HIWORD(int n)
            {
                return (n >> 16) & 0xffff;
            }

            public static int HIWORD(IntPtr n)
            {
                return HIWORD(unchecked((int)(long)n));
            }

            public static int SignedHIWORD(IntPtr n)
            {
                return SignedHIWORD(unchecked((int)(long)n));
            }
            public static int SignedLOWORD(IntPtr n)
            {
                return SignedLOWORD(unchecked((int)(long)n));
            }

            public static int SignedHIWORD(int n)
            {
                return (short)((n >> 16) & 0xffff);
            }

            public static int SignedLOWORD(int n)
            {
                return (short)(n & 0xFFFF);
            }
        }
    }
}

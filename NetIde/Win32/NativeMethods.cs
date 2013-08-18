using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Win32
{
    internal static class NativeMethods
    {
        public const int WM_PARENTNOTIFY = 0x0210;
        public const int WM_XBUTTONDOWN = 0x020b;
        public const int WM_APPCOMMAND = 0x0319;
        public const int WM_USER = 0x0400;

        public const uint PBM_SETSTATE = WM_USER + 16;

        public const int PBST_NORMAL = 0x1;
        public const int PBST_ERROR = 0x2;
        public const int PBST_PAUSED = 0x3;

        public const int APPCOMMAND_BROWSER_BACKWARD = 1;
        public const int APPCOMMAND_BROWSER_FORWARD = 2;

        public const int XBUTTON1 = 0x1;
        public const int XBUTTON2 = 0x2;

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetActiveWindow();

        [DllImport(ExternDll.User32)]
        public static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public static void SetProgressBarState(ProgressBar progressBar, int state)
        {
            if (progressBar == null)
                throw new ArgumentNullException("progressBar");

            SendMessage(new HandleRef(progressBar, progressBar.Handle), NativeMethods.PBM_SETSTATE, (IntPtr)state, IntPtr.Zero);
        }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NetIde.Shell
{
    internal static class NativeMethods
    {
        public const int SWP_NOACTIVATE = 0x0010;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_NOMOVE = 0x0002;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(HandleRef hWndChild, HandleRef hWndNewParent);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SetFocus(HandleRef hWnd);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(HandleRef hWnd, HandleRef hWndInsertAfter, int x, int y, int cx, int cy, int flags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();
    }
}

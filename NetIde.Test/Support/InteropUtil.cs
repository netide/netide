using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NetIde.Test.Support
{
    internal static class InteropUtil
    {
        public static IntPtr FindWindowInProcess(Process process, string title)
        {
            if (title == null)
                throw new ArgumentNullException("title");

            var windowHandle = IntPtr.Zero;

            foreach (ProcessThread thread in process.Threads)
            {
                windowHandle = FindWindowInThread(thread.Id, title);

                if (windowHandle != IntPtr.Zero)
                    break;
            }

            return windowHandle;
        }

        private static IntPtr FindWindowInThread(int threadId, string title)
        {
            var windowHandle = IntPtr.Zero;

            EnumThreadWindows(
                threadId,
                (hWnd, lParam) =>
                {
                    var text = new StringBuilder(200);

                    GetWindowText(hWnd, text, 200);

                    if (String.Equals(text.ToString(), title, StringComparison.OrdinalIgnoreCase))
                    {
                        windowHandle = hWnd;
                        return false;
                    }

                    windowHandle = FindChildWindow(hWnd, title);

                    return windowHandle == IntPtr.Zero;
                },
                IntPtr.Zero
            );

            return windowHandle;
        }

        private static IntPtr FindChildWindow(IntPtr hWnd, string title)
        {
            var windowHandle = IntPtr.Zero;

            EnumChildWindows(
                hWnd,
                (hChildWnd, lParam) =>
                {
                    var text = new StringBuilder(200);

                    GetWindowText(hChildWnd, text, 200);

                    if (String.Equals(text.ToString(), title, StringComparison.OrdinalIgnoreCase))
                    {
                        windowHandle = hChildWnd;
                        return false;
                    }

                    return true;
                },
                IntPtr.Zero
            );

            return windowHandle;
        }

        [DllImport("user32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool EnumThreadWindows(int threadId, EnumWindowsProc callback, IntPtr lParam);

        [DllImport("user32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        private extern static int GetWindowText(IntPtr hWnd, StringBuilder text, int maxCount);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace UIAutomationWrapper.Win32
{
    internal static class NativeMethods
    {
        internal static uint WM_MOUSEACTIVATE = 0x0021;
        internal static uint WM_MOUSEMOVE = 0x0200;
        internal static uint WM_LBUTTONDOWN = 0x0201;
        internal static uint WM_LBUTTONUP = 0x0202;
        internal static uint WM_LBUTTONDBLCLK = 0x0203;
        internal static uint WM_RBUTTONDOWN = 0x0204;
        internal static uint WM_RBUTTONUP = 0x0205;
        internal static uint WM_RBUTTONDBLCLK = 0x0206;
        internal static uint WM_MBUTTONDOWN = 0x0207;
        internal static uint WM_MBUTTONUP = 0x0208;
        internal static uint WM_MBUTTONDBLCLK = 0x0209;

        internal static uint MK_CONTROL = 0x0008;    // The CTRL key is down.
        internal static uint MK_LBUTTON = 0x0001;    // The left mouse button is down.
        internal static uint MK_MBUTTON = 0x0010;    // The middle mouse button is down.
        internal static uint MK_RBUTTON = 0x0002;    // The right mouse button is down.
        internal static uint MK_SHIFT = 0x0004;    // The SHIFT key is down.

        internal static byte VK_LSHIFT = 0xA0; // Left SHIFT key
        internal static byte VK_RSHIFT = 0xA1; // Right SHIFT key
        internal static byte VK_LCONTROL = 0xA2; // Left CONTROL key
        internal static byte VK_RCONTROL = 0xA3; // Right CONTROL key
        internal static byte VK_LMENU = 0xA4; // Left MENU key
        internal static byte VK_RMENU = 0xA5; // Right MENU key

        internal static uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        internal static uint KEYEVENTF_KEYUP = 0x0002;
        internal static uint KEYEVENTF_UNICODE = 0x0004;
        internal static uint KEYEVENTF_SCANCODE = 0x0008;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool PostMessage1(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "keybd")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "event")]
        internal static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
    }
}

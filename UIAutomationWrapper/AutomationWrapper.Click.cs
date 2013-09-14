using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using UIAutomationWrapper.Win32;

namespace UIAutomationWrapper
{
    partial class AutomationWrapper
    {
        public bool Click()
        {
            return Click(ClickType.Left);
        }

        public bool DoubleClick()
        {
            return DoubleClick(ClickType.Left);
        }

        public bool DoubleClick(ClickType clickType)
        {
            return Click(clickType | ClickType.DoubleClick);
        }

        public bool Click(ClickType clickType)
        {
            return Click(clickType, 5, 5);
        }

        public bool Click(ClickType clickType, int relativeX, int relativeY)
        {
            var whereToClick = AutomationElement;
            var whereTheHandle = whereToClick;

            if (whereToClick.Current.NativeWindowHandle == 0)
            {
                whereTheHandle = GetAncestorWithHandle(whereToClick);

                if (whereTheHandle.Current.NativeWindowHandle == 0)
                    throw new InvalidOperationException("The handle of this control equals to zero");
            }

            int x;
            int y;

            // These x and y are window-related coordinates
            if (relativeX != 0 && relativeY != 0)
            {
                x = relativeX + (int)whereToClick.Current.BoundingRectangle.X;
                y = relativeY + (int)whereToClick.Current.BoundingRectangle.Y;
            }
            else
            {
                // These x and y are for the SetCursorPos call. They are screen coordinates.

                x = (int)whereToClick.Current.BoundingRectangle.X + ((int)whereToClick.Current.BoundingRectangle.Width / 2);
                y = (int)whereToClick.Current.BoundingRectangle.Y + ((int)whereToClick.Current.BoundingRectangle.Height / 2);
            }

            // PostMessage's (click) second parameter
            uint uDown;
            uint uUp;

            // These relative coordinates for SendMessage/PostMessage
            relativeX = x - (int)whereTheHandle.Current.BoundingRectangle.X;
            relativeY = y - (int)whereTheHandle.Current.BoundingRectangle.Y;

            // PostMessage's (click) third and fourth parameters (the third will be re-assigned later)
            IntPtr wParamDown;
            IntPtr wParamUp;

            var lParam = new IntPtr(
                (new IntPtr(relativeX).ToInt32() & 0xFFFF) +
                ((new IntPtr(relativeY).ToInt32() & 0xFFFF) << 16)
            );

            // PostMessage's (activate) third parameter
            uint ulAct;
            uint uhAct;

            uint mask = 0;
            if (clickType.HasFlag(ClickType.Control))
                mask |= NativeMethods.MK_CONTROL;
            if (clickType.HasFlag(ClickType.Shift))
                mask |= NativeMethods.MK_SHIFT;

            bool doubleClick = clickType.HasFlag(ClickType.DoubleClick);
            var button = clickType & ClickType.ButtonMask;

            switch (button)
            {
                case ClickType.Right:
                    if (!doubleClick)
                    {
                        uhAct = uDown = NativeMethods.WM_RBUTTONDOWN;
                        uUp = NativeMethods.WM_RBUTTONUP;
                        wParamDown = new IntPtr(NativeMethods.MK_RBUTTON | mask);
                        wParamUp = new IntPtr(mask);
                        ulAct = NativeMethods.MK_RBUTTON;
                    }
                    else
                    {
                        uhAct = uDown = NativeMethods.WM_RBUTTONDBLCLK;
                        uUp = NativeMethods.WM_RBUTTONUP;
                        wParamDown = new IntPtr(NativeMethods.MK_RBUTTON | mask);
                        wParamUp = new IntPtr(mask);
                        ulAct = NativeMethods.MK_RBUTTON;
                    }
                    break;

                case ClickType.Middle:
                    if (!doubleClick)
                    {
                        uhAct = uDown = NativeMethods.WM_MBUTTONDOWN;
                        uUp = NativeMethods.WM_MBUTTONUP;
                        wParamDown = new IntPtr(NativeMethods.MK_MBUTTON | mask);
                        wParamUp = new IntPtr(mask);
                        ulAct = NativeMethods.MK_MBUTTON;
                    }
                    else
                    {
                        uhAct = uDown = NativeMethods.WM_MBUTTONDBLCLK;
                        uUp = NativeMethods.WM_MBUTTONUP;
                        wParamDown = new IntPtr(NativeMethods.MK_MBUTTON | mask);
                        wParamUp = new IntPtr(mask);
                        ulAct = NativeMethods.MK_MBUTTON;
                    }
                    break;

                default:
                    if (doubleClick)
                    {
                        uhAct = uDown = NativeMethods.WM_LBUTTONDBLCLK;
                        uUp = NativeMethods.WM_LBUTTONUP;
                        wParamDown = new IntPtr(NativeMethods.MK_LBUTTON | mask);
                        wParamUp = new IntPtr(mask);
                        ulAct = NativeMethods.MK_LBUTTON;
                    }
                    else
                    {
                        uhAct = uDown = NativeMethods.WM_LBUTTONDOWN;
                        uUp = NativeMethods.WM_LBUTTONUP;
                        wParamDown = new IntPtr(NativeMethods.MK_LBUTTON | mask);
                        wParamUp = new IntPtr(mask);
                        ulAct = NativeMethods.MK_LBUTTON;
                    }
                    break;
            }

            var handle = new IntPtr(whereTheHandle.Current.NativeWindowHandle);

            try
            {
                whereTheHandle.SetFocus();
            }
            catch
            {
            }

            NativeMethods.SetCursorPos(x, y);

            Thread.Sleep(0 /* Preferences.OnClickDelay */);

            // Trying to heal context menu clicks
            var mainWindow = Process.GetProcessById(whereTheHandle.Current.ProcessId).MainWindowHandle;

            if (mainWindow != IntPtr.Zero)
            {
                var lParam2 = new IntPtr(
                    (new IntPtr(ulAct).ToInt32() & 0xFFFF) +
                    ((new IntPtr(uhAct).ToInt32() & 0xFFFF) << 16)
                );

                NativeMethods.PostMessage1(
                    handle,
                    NativeMethods.WM_MOUSEACTIVATE,
                    mainWindow,
                    lParam2
                );
            }

            if (clickType.HasFlag(ClickType.Control))
                NativeMethods.keybd_event(NativeMethods.VK_LCONTROL, 0x45, NativeMethods.KEYEVENTF_EXTENDEDKEY, 0);
            if (clickType.HasFlag(ClickType.Shift))
                NativeMethods.keybd_event(NativeMethods.VK_LSHIFT, 0x45, NativeMethods.KEYEVENTF_EXTENDEDKEY, 0);

            bool postDownSuccess = NativeMethods.PostMessage1(handle, uDown, wParamDown, lParam);

            if (button == ClickType.Right || doubleClick)
                NativeMethods.PostMessage1(handle, NativeMethods.WM_MOUSEMOVE, wParamDown, lParam);

            bool postUpSuccess = NativeMethods.PostMessage1(handle, uUp, wParamUp, lParam);

            if (clickType.HasFlag(ClickType.Control))
                NativeMethods.keybd_event(NativeMethods.VK_LCONTROL, 0x45, NativeMethods.KEYEVENTF_EXTENDEDKEY | NativeMethods.KEYEVENTF_KEYUP, 0);
            if (clickType.HasFlag(ClickType.Shift))
                NativeMethods.keybd_event(NativeMethods.VK_LSHIFT, 0x45, NativeMethods.KEYEVENTF_EXTENDEDKEY | NativeMethods.KEYEVENTF_KEYUP, 0);

            return postDownSuccess && postUpSuccess;
        }

        private AutomationElement GetAncestorWithHandle(AutomationElement element)
        {
            try
            {
                var walker = new TreeWalker(Condition.TrueCondition);
                var testParent = walker.GetParent(element);

                while (testParent != null && testParent.Current.NativeWindowHandle == 0)
                {
                    testParent = walker.GetParent(testParent);

                    if (
                        testParent != null &&
                        testParent.Current.ProcessId > 0 &&
                        testParent.Current.NativeWindowHandle != 0
                    )
                        return testParent;
                }

                return
                    testParent.Current.NativeWindowHandle != 0
                    ? testParent
                    : null;
            }
            catch
            {
                return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NetIde.Util.Win32;

namespace NetIde.Util
{
    public class MouseWheelMessageFilter : IMessageFilter
    {
        private static readonly object _syncRoot = new object();
        private static bool _installed;

        public static void Install()
        {
            lock (_syncRoot)
            {
                if (!_installed)
                {
                    _installed = true;

                    Application.AddMessageFilter(new MouseWheelMessageFilter());
                }
            }
        }

        private MouseWheelMessageFilter()
        {
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg != NativeMethods.WM_MOUSEHWHEEL && m.Msg != NativeMethods.WM_MOUSEWHEEL)
                return false;

            // Here we redirect mouse wheel messages to the control the cursor
            // is currently over. This is more natural behavior and e.g. more
            // like how a browser works.
            //
            // Whether we target a window is based on whether the window has
            // the WM_HSCROLL or WM_VSCROLL window styles specified, which means
            // the window supports scrolling. If we find such a window, we
            // dispatch the message to that window instead of the one that
            // currently has focus.
            //
            // This algorithm does not match windows that have physical scrollbars
            // instead of the window styles. To have these behave correctly,
            // we dispatch the scroll messages to the original window if we
            // cannot find one under the cursor.

            var window = NativeMethods.WindowFromPoint(new Point(m.LParam.ToInt32()));

            while (window != IntPtr.Zero)
            {
                var styles = NativeMethods.GetWindowLong(window, NativeMethods.GWL_STYLE);

                if ((styles & (NativeMethods.WS_HSCROLL | NativeMethods.WS_VSCROLL)) != 0)
                {
                    // Are any of the scroll bars enabled on this window?

                    bool haveScroll = false;
                    if ((styles & NativeMethods.WS_VSCROLL) != 0)
                        haveScroll = haveScroll || IsScrollEnabled(window, NativeMethods.SB_VERT);
                    if ((styles & NativeMethods.WS_HSCROLL) != 0)
                        haveScroll = haveScroll || IsScrollEnabled(window, NativeMethods.SB_HORZ);

                    if (haveScroll)
                    {
                        NativeMethods.SendMessage(window, m.Msg, m.WParam, m.LParam);
                        return true;
                    }
                }

                window = NativeMethods.GetParent(window);
            }

            // Not all windows that support scrolling have WS_HSCROLL or
            // WS_VSCROLL set. These styles enable the built in scrollbars.
            // However, if a window implements scrolling by adding physical
            // scrollbars, the wheel events must still be send to the window.
            // We could just return false here, but that has the disadvantage
            // that all AppDomains are bothered with this message. Instead we
            // just dispatch the message right here and be done with it.

            NativeMethods.SendMessage(m.HWnd, m.Msg, m.WParam, m.LParam);

            return true;
        }

        private bool IsScrollEnabled(IntPtr window, int scrollBar)
        {
            var scrollInfo = new NativeMethods.SCROLLINFO
            {
                fMask = NativeMethods.SIF_ALL
            };

            // Lie and say yes if we cannot get the scroll info.

            if (!NativeMethods.GetScrollInfo(window, scrollBar, scrollInfo))
                return true;

            // Test whether the scroll bars are enabled. If the window
            // has SIF_DISABLENOSCROLL set, the scrollbar will be
            // disabled if there is nothing to scroll. In that case, we
            // try the parent. This prevents strange behavior with
            // nested scrollable controls.

            return (scrollInfo.nMax - scrollInfo.nMin) + 1 > scrollInfo.nPage;
        }
    }
}

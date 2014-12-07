// Taken from http://www.codeproject.com/Articles/13723/Themed-RichTextBox-A-RichTextBox-with-XP-styled-bo.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Util.Forms
{
    public class RichTextBox : System.Windows.Forms.RichTextBox
    {
        /// <summary>
        /// Contains the size of the visual style borders
        /// </summary>
        private RECT _borderRect;

        private bool _focused;
        private Padding _padding;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Category("Layout")]
        public new Padding Padding
        {
            get { return _padding; }
            set
            {
                if (_padding != value)
                {
                    _padding = value;

                    SetWindowPos(Handle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
                }
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            _focused = true;

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            _focused = false;

            base.OnLostFocus(e);
        }

        /// <summary>
        /// Filter some message we need to draw the border.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT: // the border painting is done here.
                    WmNcPaint(ref m);
                    break;
                case WM_NCCALCSIZE: // the size of the client area is calcuated here.
                    WmNcCalcSize(ref m);
                    break;
                case WM_THEMECHANGED: // Updates styles when the theme is changing.
                    UpdateStyles();
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// Calculates the size of the window frame and client area of the RichTextBox
        /// </summary>
        private void WmNcCalcSize(ref Message m)
        {
            // let the richtextbox control draw the scrollbar if necessary.
            base.WndProc(ref m);

            // we visual styles are not enabled and BorderStyle is not Fixed3D then we have nothing more to do.
            if (!RenderWithVisualStyles())
                return;

            // contains detailed information about WM_NCCALCSIZE message
            NCCALCSIZE_PARAMS par = new NCCALCSIZE_PARAMS();

            // contains the window frame RECT
            RECT windowRect;

            if (m.WParam == IntPtr.Zero) // LParam points to a RECT struct
            {
                windowRect = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));
            }
            else // LParam points to a NCCALCSIZE_PARAMS struct
            {
                par = (NCCALCSIZE_PARAMS)Marshal.PtrToStructure(m.LParam, typeof(NCCALCSIZE_PARAMS));
                windowRect = par.rgrc0;
            }

            // contains the client area of the control
            RECT contentRect;

            // get the DC
            IntPtr hDC = GetWindowDC(Handle);

            // open theme data
            IntPtr hTheme = OpenThemeData(Handle, "EDIT");

            // find out how much space the borders needs
            if (GetThemeBackgroundContentRect(hTheme, hDC, EP_EDITTEXT, GetState()
                , ref windowRect
                , out contentRect) == S_OK)
            {
                // shrink the client area the make more space for containing text.
                contentRect.Left += 1 + Padding.Left;
                contentRect.Top += 1 + Padding.Top;
                contentRect.Right -= 1 + Padding.Right;
                contentRect.Bottom -= 1 + Padding.Bottom;

                // remember the space of the borders
                _borderRect = new RECT(contentRect.Left - windowRect.Left
                    , contentRect.Top - windowRect.Top
                    , windowRect.Right - contentRect.Right
                    , windowRect.Bottom - contentRect.Bottom);

                // update LParam of the message with the new client area
                if (m.WParam == IntPtr.Zero)
                {
                    Marshal.StructureToPtr(contentRect, m.LParam, false);
                }
                else
                {
                    par.rgrc0 = contentRect;
                    Marshal.StructureToPtr(par, m.LParam, false);
                }

                // force the control to redraw it´s client area
                m.Result = new IntPtr(WVR_REDRAW);
            }

            // release theme data handle
            CloseThemeData(hTheme);

            // release DC
            ReleaseDC(Handle, hDC);
        }

        private int GetState()
        {
            if (!Enabled)
                return ETS_DISABLED;
            if (ReadOnly)
                return ETS_READONLY;
            if (_focused)
                return ETS_FOCUSED;
            return ETS_NORMAL;
        }

        /// <summary>
        /// The border painting is done here.
        /// </summary>
        private void WmNcPaint(ref Message m)
        {
            base.WndProc(ref m);

            if (!RenderWithVisualStyles())
            {
                return;
            }

            /////////////////////////////////////////////////////////////////////////////
            // Get the DC of the window frame and paint the border using uxTheme API´s
            /////////////////////////////////////////////////////////////////////////////

            // set the part id to TextBox
            int partId = EP_EDITTEXT;

            // define the windows frame rectangle of the TextBox
            RECT windowRect;
            GetWindowRect(Handle, out windowRect);
            windowRect.Right -= windowRect.Left; windowRect.Bottom -= windowRect.Top;
            windowRect.Top = windowRect.Left = 0;

            // get the device context of the window frame
            IntPtr hDC = GetWindowDC(Handle);

            // define a rectangle inside the borders and exclude it from the DC
            RECT clientRect = windowRect;
            clientRect.Left += _borderRect.Left;
            clientRect.Top += _borderRect.Top;
            clientRect.Right -= _borderRect.Right;
            clientRect.Bottom -= _borderRect.Bottom;
            ExcludeClipRect(hDC, clientRect.Left, clientRect.Top, clientRect.Right, clientRect.Bottom);

            // open theme data
            IntPtr hTheme = OpenThemeData(Handle, "EDIT");

            // make sure the background is updated when transparent background is used.
            if (IsThemeBackgroundPartiallyTransparent(hTheme
                , EP_EDITTEXT, ETS_NORMAL) != 0)
            {
                DrawThemeParentBackground(Handle, hDC, ref windowRect);
            }

            // draw background
            DrawThemeBackground(hTheme, hDC, partId, GetState(), ref windowRect, IntPtr.Zero);

            // close theme data
            CloseThemeData(hTheme);

            // release dc
            ReleaseDC(Handle, hDC);

            // we have processed the message so set the result to zero
            m.Result = IntPtr.Zero;
        }

        /// <summary>
        /// Returns true, when visual styles are enabled in this application.
        /// </summary>
        private bool VisualStylesEnabled()
        {
            // Check if RenderWithVisualStyles property is available in the Application class (New feature in NET 2.0)
            Type t = typeof(Application);
            PropertyInfo pi = t.GetProperty("RenderWithVisualStyles");

            if (pi == null)
            {
                // NET 1.1
                OperatingSystem os = Environment.OSVersion;
                if (os.Platform == PlatformID.Win32NT && (((os.Version.Major == 5) && (os.Version.Minor >= 1)) || (os.Version.Major > 5)))
                {
                    DLLVersionInfo version = new DLLVersionInfo();
                    version.cbSize = Marshal.SizeOf(typeof(DLLVersionInfo));
                    if (DllGetVersion(ref version) == 0)
                    {
                        return (version.dwMajorVersion > 5) && IsThemeActive() && IsAppThemed();
                    }
                }

                return false;
            }
            else
            {
                // NET 2.0
                return (bool)pi.GetValue(null, null);
            }
        }

        /// <summary>
        /// Return true, when this control should render with visual styles.
        /// </summary>
        /// <returns></returns>
        private bool RenderWithVisualStyles()
        {
            return (BorderStyle == BorderStyle.Fixed3D && VisualStylesEnabled());
        }

        /// <summary>
        /// Update the control parameters.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams p = base.CreateParams;

                // remove the Fixed3D border style
                if (RenderWithVisualStyles() && (p.ExStyle & WS_EX_CLIENTEDGE) == WS_EX_CLIENTEDGE)
                    p.ExStyle ^= WS_EX_CLIENTEDGE;

                return p;
            }
        }

        private const int S_OK = 0x0;

        private const int EP_EDITTEXT = 1;
        private const int ETS_NORMAL = 1;
        private const int ETS_HOT = 2;
        private const int ETS_SELECTED = 3;
        private const int ETS_DISABLED = 4;
        private const int ETS_FOCUSED = 5;
        private const int ETS_READONLY = 6;
        private const int ETS_ASSIST = 7;
        private const int ETS_CUEBANNER = 8;

        private const int WM_THEMECHANGED = 0x031A;
        private const int WM_NCPAINT = 0x85;
        private const int WM_NCCALCSIZE = 0x83;

        private const int WS_EX_CLIENTEDGE = 0x200;
        private const int WVR_HREDRAW = 0x100;
        private const int WVR_VREDRAW = 0x200;
        private const int WVR_REDRAW = (WVR_HREDRAW | WVR_VREDRAW);

        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_NOREDRAW = 0x0008;
        private const int SWP_NOACTIVATE = 0x0010;
        private const int SWP_FRAMECHANGED = 0x0020;
        private const int SWP_SHOWWINDOW = 0x0040;
        private const int SWP_HIDEWINDOW = 0x0080;
        private const int SWP_NOCOPYBITS = 0x0100;
        private const int SWP_NOOWNERZORDER = 0x0200;
        private const int SWP_NOSENDCHANGING = 0x0400;
        private const int SWP_DRAWFRAME = SWP_FRAMECHANGED;
        private const int SWP_NOREPOSITION = SWP_NOOWNERZORDER;
        private const int SWP_DEFERERASE = 0x2000;
        private const int SWP_ASYNCWINDOWPOS = 0x4000;

        [DllImport("User32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("gdi32.dll")]
        private static extern int ExcludeClipRect(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
        private static extern bool IsAppThemed();

        [DllImport("UxTheme.dll", CharSet = CharSet.Auto)]
        private static extern bool IsThemeActive();

        [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
        private static extern int DllGetVersion(ref DLLVersionInfo version);

        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr OpenThemeData(IntPtr hWnd, String classList);

        [DllImport("uxtheme.dll", ExactSpelling = true)]
        private extern static Int32 CloseThemeData(IntPtr hTheme);

        [DllImport("uxtheme", ExactSpelling = true)]
        private extern static Int32 DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId,
            int iStateId, ref RECT pRect, IntPtr pClipRect);

        [DllImport("uxtheme", ExactSpelling = true)]
        private extern static int IsThemeBackgroundPartiallyTransparent(IntPtr hTheme, int iPartId, int iStateId);

        [DllImport("uxtheme", ExactSpelling = true)]
        private extern static Int32 GetThemeBackgroundContentRect(IntPtr hTheme, IntPtr hdc
            , int iPartId, int iStateId, ref RECT pBoundingRect, out RECT pContentRect);

        [DllImport("uxtheme", ExactSpelling = true)]
        private extern static Int32 DrawThemeParentBackground(IntPtr hWnd, IntPtr hdc, ref RECT pRect);

        [DllImport("uxtheme", ExactSpelling = true)]
        private extern static Int32 DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId,
            int iStateId, ref RECT pRect, ref RECT pClipRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct DLLVersionInfo
        {
            public int cbSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformID;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NCCALCSIZE_PARAMS
        {
            public RECT rgrc0, rgrc1, rgrc2;
            public IntPtr lppos;
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left_, int top_, int right_, int bottom_)
            {
                Left = left_;
                Top = top_;
                Right = right_;
                Bottom = bottom_;
            }

            public int Height { get { return Bottom - Top + 1; } }
            public int Width { get { return Right - Left + 1; } }
            public Size Size { get { return new Size(Width, Height); } }

            public Point Location { get { return new Point(Left, Top); } }

            // Handy method for converting to a System.Drawing.Rectangle
            public Rectangle ToRectangle()
            { return Rectangle.FromLTRB(Left, Top, Right, Bottom); }

            public static RECT FromRectangle(Rectangle rectangle)
            {
                return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            }

            public void Inflate(int width, int height)
            {
                this.Left -= width;
                this.Top -= height;
                this.Right += width;
                this.Bottom += height;
            }

            public override int GetHashCode()
            {
                return Left ^ ((Top << 13) | (Top >> 0x13))
                    ^ ((Width << 0x1a) | (Width >> 6))
                    ^ ((Height << 7) | (Height >> 0x19));
            }

            #region Operator overloads

            public static implicit operator Rectangle(RECT rect)
            {
                return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }

            public static implicit operator RECT(Rectangle rect)
            {
                return new RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }

            #endregion
        }
    }
}

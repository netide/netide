using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NetIde.Util.Win32;

namespace NetIde.Util.Forms
{
    public class TextBox : System.Windows.Forms.TextBox
    {
        private string _cueBanner = String.Empty;
        private bool _focused;
        private bool _fakeCueBannerDrawn;
        private CueBannerVisibility _cueBannerVisibility = CueBannerVisibility.WhenNotFocused;
        private bool? _cueBannerOsSupport;

        [Category("Appearance")]
        [DefaultValue("")]
        public string CueBanner
        {
            get { return _cueBanner; }
            set
            {
                if (value == null)
                    value = String.Empty;

                if (_cueBanner != value)
                {
                    _cueBanner = value;

                    if (IsHandleCreated)
                        UpdateCueBanner();
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(CueBannerVisibility.WhenNotFocused)]
        public CueBannerVisibility CueBannerVisibility
        {
            get { return _cueBannerVisibility; }
            set
            {
                if (value != _cueBannerVisibility)
                {
                    _cueBannerVisibility = value;
                    _cueBannerOsSupport = null;

                    if (IsHandleCreated)
                        UpdateCueBanner();
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            UpdateCueBanner();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            _focused = true;

            UpdateFakeCueBanner();

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            _focused = false;

            UpdateFakeCueBanner();

            base.OnLostFocus(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            UpdateFakeCueBanner();

            base.OnTextChanged(e);
        }

        private void UpdateCueBanner()
        {
            if (CueBannerOsSupport())
            {
                if (_cueBannerVisibility == CueBannerVisibility.Never)
                {
                    NativeMethods.SendMessage(Handle, NativeMethods.EM_SETCUEBANNER, IntPtr.Zero, "");
                }
                else
                {
                    var wParam =
                        _cueBannerVisibility == CueBannerVisibility.Always
                        ? (IntPtr)1
                        : IntPtr.Zero;

                    NativeMethods.SendMessage(Handle, NativeMethods.EM_SETCUEBANNER, wParam, _cueBanner);
                }
            }
            else
            {
                UpdateFakeCueBanner();
            }
        }

        private void UpdateFakeCueBanner()
        {
            if (
                !CueBannerOsSupport() && (
                    _fakeCueBannerDrawn || (
                        _cueBannerVisibility != CueBannerVisibility.Never &&
                        !String.Empty.Equals(_cueBanner) &&
                        String.Empty.Equals(Text) &&
                        (!_focused || _cueBannerVisibility == CueBannerVisibility.Always)
                    )
                )
            )
            {
                _fakeCueBannerDrawn = false;

                Invalidate();
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == NativeMethods.WM_PAINT)
            {
                if (
                    !CueBannerOsSupport() &&
                    _cueBannerVisibility != CueBannerVisibility.Never &&
                    (!_focused || _cueBannerVisibility == CueBannerVisibility.Always) &&
                    String.Empty.Equals(Text) &&
                    !String.Empty.Equals(_cueBanner)
                )
                {
                    using (var graphics = CreateGraphics())
                    {
                        var bounds = ClientRectangle;

                        // Deflate the rectangle by 1 and include the left
                        // and right margins.

                        bounds = new Rectangle(
                            bounds.Left + 1,
                            bounds.Top + 1,
                            bounds.Width - 2,
                            bounds.Height - 2
                        );

                        TextRenderer.DrawText(
                            graphics,
                            _cueBanner,
                            SystemFonts.MessageBoxFont,
                            bounds,
                            SystemColors.GrayText,
                            TextFormatFlags.Left | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.TextBoxControl | TextFormatFlags.Top
                        );

                        _fakeCueBannerDrawn = true;
                    }
                }
            }
        }

        private bool CueBannerOsSupport()
        {
            if (!_cueBannerOsSupport.HasValue)
            {
                switch (_cueBannerVisibility)
                {
                    case CueBannerVisibility.Never:
                        _cueBannerOsSupport = true;
                        break;

                    case CueBannerVisibility.WhenNotFocused:
                        _cueBannerOsSupport = Application.RenderWithVisualStyles;
                        break;

                    default: // CueBannerVisibility.Always
                        // Test for Windows XP. Windows XP does not support showing
                        // the cue banner always.

                        _cueBannerOsSupport =
                            Application.RenderWithVisualStyles && !(
                                Environment.OSVersion.Platform == PlatformID.Win32NT &&
                                Environment.OSVersion.Version.Major == 5 &&
                                Environment.OSVersion.Version.Minor == 1
                            );
                        break;
                }
            }

            return _cueBannerOsSupport.Value;
        }
    }
}

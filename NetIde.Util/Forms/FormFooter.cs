using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using NetIde.Util.Win32;

namespace NetIde.Util.Forms
{
    public class FormFooter : FlowLayoutPanel
    {
        private static readonly Padding WizardPadding = new Padding(8, 8, 10, 8);
        private static readonly Padding DialogPadding = new Padding(4, 0, 4, 8);
        private const int SizeGripSize = 16;

        private Color _bumpLightColor = SystemColors.ControlDark;
        private Color _bumpDarkColor = SystemColors.ControlLightLight;
        private System.Windows.Forms.Form _form;
        private bool _renderSizeGrip;
        private VisualStyleRenderer _sizeGripRenderer;
        private FormFooterStyle _style;

        public FormFooter()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            Dock = DockStyle.Bottom;
            FlowDirection = FlowDirection.RightToLeft;
            AutoSize = true;
        }

        [Browsable(false)]
        public new Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }

        protected override Padding DefaultPadding
        {
            get { return Style == FormFooterStyle.Wizard ? WizardPadding : DialogPadding; }
        }

        [Category("Layout")]
        [Browsable(true)]
        [DefaultValue(true)]
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }

        [Category("Layout")]
        [Browsable(true)]
        [DefaultValue(typeof(FlowDirection), "RightToLeft")]
        public new FlowDirection FlowDirection
        {
            get { return base.FlowDirection; }
            set { base.FlowDirection = value; }
        }

        [Category("Layout")]
        [Browsable(true)]
        [DefaultValue(typeof(DockStyle), "Bottom")]
        public override DockStyle Dock
        {
            get { return base.Dock; }
            set { base.Dock = value; }
        }

        [Category("Appearance")]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "ControlDark")]
        public Color BumpLightColor
        {
            get { return _bumpLightColor; }
            set
            {
                _bumpLightColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "ControlLightLight")]
        public Color BumpDarkColor
        {
            get { return _bumpDarkColor; }
            set
            {
                _bumpDarkColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(FormFooterStyle.Wizard)]
        public FormFooterStyle Style
        {
            get { return _style; }
            set
            {
                if (_style != value)
                {
                    _style = value;
                    Padding = DefaultPadding;
                    PerformLayout();
                }
            }
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            // Repaint when the layout has changed.

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                e.Graphics.Clear(BackColor);

                base.OnPaint(e);

                if (Style == FormFooterStyle.Wizard)
                {
                    using (var pen = new Pen(_bumpLightColor))
                    {
                        e.Graphics.DrawLine(pen, 0, 0, Width - 1, 0);
                    }

                    using (var pen = new Pen(_bumpDarkColor))
                    {
                        e.Graphics.DrawLine(pen, 0, 1, Width - 1, 1);
                    }
                }

                if (_renderSizeGrip)
                {
                    Size sz = ClientSize;

                    if (Application.RenderWithVisualStyles)
                    {
                        if (_sizeGripRenderer == null)
                            _sizeGripRenderer = new VisualStyleRenderer(VisualStyleElement.Status.Gripper.Normal);

                        _sizeGripRenderer.DrawBackground(e.Graphics, new Rectangle(sz.Width - SizeGripSize, sz.Height - SizeGripSize, SizeGripSize, SizeGripSize));
                    }
                    else
                    {
                        ControlPaint.DrawSizeGrip(e.Graphics, BackColor, sz.Width - SizeGripSize, sz.Height - SizeGripSize, SizeGripSize, SizeGripSize);
                    }
                }
            }
            catch
            {
                // Suppress all exceptions coming from GDI actions.
            }
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            GetSizeGripDetails();
        }

        private void GetSizeGripDetails()
        {
            var form = FindForm();

            if (!ReferenceEquals(form, _form))
            {
                if (_form != null)
                    _form.StyleChanged -= Form_StyleChanged;

                _form = form;

                if (_form != null)
                    _form.StyleChanged += Form_StyleChanged;
            }

            _renderSizeGrip = false;

            if (_form != null)
            {
                switch (_form.FormBorderStyle)
                {
                    case FormBorderStyle.None:
                    case FormBorderStyle.FixedSingle:
                    case FormBorderStyle.Fixed3D:
                    case FormBorderStyle.FixedDialog:
                    case FormBorderStyle.FixedToolWindow:
                        _renderSizeGrip = false;
                        break;

                    case FormBorderStyle.Sizable:
                    case FormBorderStyle.SizableToolWindow:
                        switch (form.SizeGripStyle)
                        {
                            case SizeGripStyle.Show:
                                _renderSizeGrip = true;
                                break;

                            case SizeGripStyle.Hide:
                                _renderSizeGrip = false;
                                break;

                            case SizeGripStyle.Auto:
                                if (_form.Modal)
                                    _renderSizeGrip = true;
                                else
                                    _renderSizeGrip = false;
                                break;
                        }
                        break;
                }
            }
        }

        void Form_StyleChanged(object sender, EventArgs e)
        {
            GetSizeGripDetails();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_NCHITTEST:
                    WmNCHitTest(ref m);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void WmNCHitTest(ref Message m)
        {
            if (_renderSizeGrip)
            {
                int x = NativeMethods.Util.SignedLOWORD((int)m.LParam);
                int y = NativeMethods.Util.SignedHIWORD((int)m.LParam);

                // Convert to client coordinates
                //
                var pt = PointToClient(new Point(x, y));

                Size clientSize = ClientSize;

                // If the grip is not fully visible the grip area could overlap with the system control box; we need to disable
                // the grip area in this case not to get in the way of the control box.  We only need to check for the client's
                // height since the window width will be at least the size of the control box which is always bigger than the
                // grip width.
                if (pt.X >= (clientSize.Width - SizeGripSize) &&
                    pt.Y >= (clientSize.Height - SizeGripSize) &&
                    clientSize.Height >= SizeGripSize
                ) {
                    m.Result = IsMirrored ? (IntPtr)NativeMethods.HTBOTTOMLEFT : (IntPtr)NativeMethods.HTBOTTOMRIGHT;
                    return;
                }
            }

            base.WndProc(ref m);
        }
    }
}

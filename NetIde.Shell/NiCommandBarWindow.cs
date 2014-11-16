using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;

namespace NetIde.Shell
{
    public class NiCommandBarWindow : NiWindowHost
    {
        private readonly bool _designMode;
        private INiWindowPane _window;

        [Category("Behavior")]
        [Description("ID of the command bar to render.")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Guid CommandBarId { get; set; }

        public NiCommandBarWindow()
        {
            _designMode = ControlUtil.GetIsInDesignMode(this);
        }

        protected override INiIsolationClient CreateWindow()
        {
            var commandManager = (INiCommandManager)GetService(typeof(INiCommandManager));

            ErrorUtil.ThrowOnFailure(commandManager.CreateCommandBarWindow(CommandBarId, out _window));

            ErrorUtil.ThrowOnFailure(_window.Initialize());

            SetBoundsCore(0, 0, Width, Height, BoundsSpecified.Size);

            return _window;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if ((specified & BoundsSpecified.Size) != 0)
            {
                if (_window != null)
                {
                    Size size;
                    ErrorUtil.ThrowOnFailure(_window.GetPreferredSize(
                        new Size(width, height), out size
                    ));

                    width = size.Width;
                    height = size.Height;
                }
                else if (_designMode)
                {
                    height = 23;
                }
            }

            base.SetBoundsCore(x, y, width, height, specified);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!_designMode)
                return;

            using (var font = new Font(SystemFonts.MessageBoxFont, FontStyle.Italic))
            {
                TextRenderer.DrawText(
                    e.Graphics,
                    Labels.HostedToolStrip,
                    font,
                    ClientRectangle,
                    SystemColors.GrayText,
                    BackColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine
                );
            }
        }
    }
}

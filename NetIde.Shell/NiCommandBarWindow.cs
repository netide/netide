using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiCommandBarWindow : NiWindowHost<INiCommandBarWindow>
    {
        private INiCommandBarWindow _window;

        [Category("Behavior")]
        [Description("ID of the command bar to render.")]
        [Browsable(false)]
        public Guid CommandBarId { get; set; }

        protected override INiCommandBarWindow CreateWindow()
        {
            var commandManager = (INiCommandManager)GetService(typeof(INiCommandManager));

            ErrorUtil.ThrowOnFailure(commandManager.CreateCommandBarWindow(CommandBarId, out _window));

            ErrorUtil.ThrowOnFailure(_window.Initialize());

            SetBoundsCore(0, 0, Width, Height, BoundsSpecified.Size);

            return _window;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (_window != null && (specified & BoundsSpecified.Size) != 0)
            {
                Size size;
                ErrorUtil.ThrowOnFailure(_window.GetPreferredSize(
                    new Size(width, height), out size
                ));

                width = size.Width;
                height = size.Height;
            }

            base.SetBoundsCore(x, y, width, height, specified);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Services.CommandManager.Controls;
using NetIde.Services.MenuManager;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.CommandManager
{
    internal class CommandBarWindow : NiWindowPane
    {
        private readonly Guid _id;
        private BarControl _control;
        private bool _disposed;

        public CommandBarWindow(Guid id)
        {
            _id = id;
        }

        public ToolStrip ToolStrip
        {
            get { return (ToolStrip)Controls[0]; }
        }

        public override HResult Initialize()
        {
            try
            {
                var hr = base.Initialize();

                if (ErrorUtil.Failure(hr))
                    return hr;

                var commandManager = (INiCommandManager)GetService(typeof(INiCommandManager));
                var menuManager = (NiMenuManager)GetService(typeof(INiMenuManager));

                INiCommandBar toolbar;
                ErrorUtil.ThrowOnFailure(commandManager.FindCommandBar(_id, out toolbar));

                if (toolbar != null)
                {
                    _control = menuManager.CreateHost((NiCommandBar)toolbar);

                    var toolStrip = (ToolStrip)_control.Control;
                    toolStrip.GripStyle = ToolStripGripStyle.Hidden;

                    Controls.Add(toolStrip);
                }

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            return ToolStrip.GetPreferredSize(proposedSize);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_control != null)
                {
                    _control.Dispose();
                    _control = null;
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}

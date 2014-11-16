using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System;
using System.Windows.Forms;
using NetIde.Services.Shell;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.CommandManager.Controls
{
    internal class ToolStripBarControl : ToolStrip, IBarControl, INiObjectWithSite
    {
        private IServiceProvider _serviceProvider;
        private NiShell _shell;
        private bool _disposed;

        public HResult SetSite(IServiceProvider serviceProvider)
        {
            try
            {
                if (serviceProvider == null)
                    throw new ArgumentNullException("serviceProvider");

                _serviceProvider = serviceProvider;

                _shell = (NiShell)_serviceProvider.GetService(typeof(INiShell));
                _shell.RequerySuggested += _shell_RequerySuggested;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private void _shell_RequerySuggested(object sender, EventArgs e)
        {
            OnQueryStatus(EventArgs.Empty);
        }

        public HResult GetSite(out IServiceProvider serviceProvider)
        {
            serviceProvider = _serviceProvider;
            return HResult.OK;
        }

        Image IBarControl.Image { get; set; }

        public event EventHandler QueryStatus;

        protected virtual void OnQueryStatus(EventArgs e)
        {
            var ev = QueryStatus;
            if (ev != null)
                ev(this, e);
        }

        public ControlControl CreateButton(IServiceProvider serviceProvider, NiCommandBarButton button)
        {
            return new ButtonControl.Button(serviceProvider, button);
        }

        public ControlControl CreateComboBox(IServiceProvider serviceProvider, NiCommandBarComboBox comboBox)
        {
            return new ComboBoxControl(serviceProvider, comboBox);
        }

        public ControlControl CreateTextBox(IServiceProvider serviceProvider, NiCommandBarTextBox textBox)
        {
            return new TextBoxControl(serviceProvider, textBox);
        }

        public ControlControl CreatePopup(IServiceProvider serviceProvider, NiCommandBarPopup popup)
        {
            return new PopupControl<DropDownButtonBarControl>(serviceProvider, popup, ToolStripItemDisplayStyle.Image);
        }

        public ControlControl CreateLabel(IServiceProvider serviceProvider, NiCommandBarLabel label)
        {
            return new LabelControl(serviceProvider, label);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_shell != null)
                {
                    _shell.RequerySuggested -= _shell_RequerySuggested;
                    _shell = null;
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
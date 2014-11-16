using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Services.Env;
using NetIde.Services.MenuManager;
using NetIde.Shell;
using NetIde.Shell.Interop;
using log4net;

namespace NetIde.Services.CommandManager.Controls
{
    internal class BarControl : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BarControl));

        private bool _disposed;
        private GroupManager _groupManager;
        private readonly NiEnv _env;
        private readonly NiMenuManager _menuManager;

        public NiCommandBar Bar { get; private set; }
        public IBarControl Control { get; private set; }

        protected BarControl(IServiceProvider serviceProvider, NiCommandBar bar, IBarControl control)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");
            if (bar == null)
                throw new ArgumentNullException("bar");

            _menuManager = (NiMenuManager)serviceProvider.GetService(typeof(INiMenuManager));
            _env = (NiEnv)serviceProvider.GetService(typeof(INiEnv));

            Bar = bar;
            Bar.AppearanceChanged += Bar_AppearanceChanged;

            var objectWithSite = control as INiObjectWithSite;
            if (objectWithSite != null)
                ErrorUtil.ThrowOnFailure(objectWithSite.SetSite(serviceProvider));

            Control = control;
            Control.Tag = this;
            Control.QueryStatus += (s, e) => _menuManager.QueryStatus(Bar);

            UpdateItem();

            _groupManager = new GroupManager(Bar, serviceProvider, Control);
        }

        void Bar_AppearanceChanged(object sender, EventArgs e)
        {
            UpdateItem();
        }

        private void UpdateItem()
        {
            Control.Text = Bar.Text;
            Control.Enabled = Bar.IsEnabled;
            Control.Visible = Bar.IsVisible;

            if (Bar.DisplayStyle != NiCommandDisplayStyle.Default)
                Log.WarnFormat("DisplayStyle is not supported on '{0}'", Bar.GetType());

            if (Bar.Image != null)
                Control.Image = _env.ResourceManager.GetImage(Bar.Image);
            else
                Control.Image = Bar.Bitmap;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Bar.AppearanceChanged -= Bar_AppearanceChanged;

                if (_groupManager != null)
                {
                    _groupManager.Dispose();
                    _groupManager = null;
                }

                if (Control != null)
                {
                    Control.Dispose();
                    Control = null;
                }

                _disposed = true;
            }
        }
    }

    internal class BarControl<T> : BarControl
        where T : IBarControl, new()
    {
        public BarControl(IServiceProvider serviceProvider, NiCommandBar bar)
            : base(serviceProvider, bar, new T())
        {
        }
    }
}

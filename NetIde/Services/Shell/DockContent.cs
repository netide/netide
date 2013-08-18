using System.Text;
using System.Collections.Generic;
using System;
using System.Windows.Forms;
using NetIde.Services.Env;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.Shell
{
    internal class DockContent : Support.DockContent
    {
        private Proxy _proxy;
        private NativeWindowHost _host;
        private bool _disposed;

        public INiWindowPane WindowPane { get; private set; }
        public NiDockStyle DockStyle { get; private set; }
        public NiToolWindowOrientation Orientation { get; private set; }

        public DockContent(INiWindowPane windowPane, NiDockStyle dockStyle, NiToolWindowOrientation orientation)
        {
            if (windowPane == null)
                throw new ArgumentNullException("windowPane");

            WindowPane = windowPane;
            DockStyle = dockStyle;
            Orientation = orientation;

            switch (dockStyle)
            {
                case NiDockStyle.Document:
                    DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
                    break;

                case NiDockStyle.Float:
                    DockAreas =
                        WeifenLuo.WinFormsUI.Docking.DockAreas.Document |
                        WeifenLuo.WinFormsUI.Docking.DockAreas.Float;
                    break;

                case NiDockStyle.AlwaysFloat:
                    DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Float;
                    break;

                default:
                    DockAreas =
                        WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom |
                        WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft |
                        WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight |
                        WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop;
                    break;
            }

            _host = new NativeWindowHost
            {
                AcceptsTab = true,
                AcceptsArrows = true,
                Dock = System.Windows.Forms.DockStyle.Fill
            };

            _host.PreMessage += _host_PreMessage;

            Controls.Add(_host);
        }

        void _host_PreMessage(object sender, NiPreMessageEventArgs e)
        {
            var preMessageFilter = WindowPane as INiPreMessageFilter;

            if (preMessageFilter == null)
                return;

            var message = e.Message;

            bool handled;
            ErrorUtil.ThrowOnFailure(preMessageFilter.PreFilterMessage(ref message, out handled));

            e.Handled = handled;

            if (e.Handled)
                e.Message = message;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            _host.ChildHwnd = WindowPane.Handle;
        }

        public INiWindowFrame GetProxy()
        {
            return _proxy ?? (_proxy = new Proxy(this));
        }

        public void RaiseShow(NiWindowShow show)
        {
            _host.RaiseShow(show);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_host != null)
                {
                    _host.Dispose();
                    _host = null;
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        private class Proxy : ServiceObject, INiWindowFrame
        {
            private readonly DockContent _owner;
            private IResource _icon;
            private readonly NiEnv _env;
            private bool _shown;

            public Proxy(DockContent owner)
            {
                _owner = owner;
                _env = (NiEnv)_owner.GetService(typeof(INiEnv));
            }

            public bool IsVisible
            {
                get { return _owner.Visible; }
            }

            public string Caption
            {
                get { return _owner.TabText; }
                set { _owner.Text = _owner.TabText = value; }
            }

            public IResource Icon
            {
                get { return _icon; }
                set
                {
                    if (_icon != value)
                    {
                        _icon = value;

                        _owner.Icon =
                            value == null
                            ? null
                            : _env.ResourceManager.GetIcon(value);
                    }
                }
            }

            public HResult Show()
            {
                try
                {
                    if (_shown)
                    {
                        _owner.IsHidden = false;
                        _owner.Show();
                    }
                    else
                    {
                        _shown = true;

                        ((NiEnv)_owner.GetService(typeof(INiEnv))).MainForm.ShowContent(_owner);
                    }

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult Hide()
            {
                try
                {
                    _owner.IsHidden = true;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult Close()
            {
                try
                {
                    _owner.Close();

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult Advise(object sink, out int cookie)
            {
                return _owner._host.Advise(sink, out cookie);
            }

            public HResult Advise(INiWindowFrameNotify sink, out int cookie)
            {
                return Advise((object)sink, out cookie);
            }

            public HResult Unadvise(int cookie)
            {
                return _owner._host.Unadvise(cookie);
            }
        }
    }
}
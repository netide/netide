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
        private WindowHost _host;
        private bool _disposed;
        private bool _suppressClosing;

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
            ShowIcon = false;

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

            _host = new WindowHost(this)
            {
                Dock = System.Windows.Forms.DockStyle.Fill
            };

            Controls.Add(_host);
        }

        public INiWindowFrame GetProxy()
        {
            return _proxy ?? (_proxy = new Proxy(this));
        }

        public void RaiseShow(NiWindowShow show)
        {
            if (!_disposed)
                _host.RaiseShow(show);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!_suppressClosing)
            {
                bool cancel = false;
                RaiseClose(NiFrameCloseMode.PromptSave, ref cancel);

                if (cancel)
                    e.Cancel = true;
            }

            base.OnFormClosing(e);
        }

        public void RaiseClose(NiFrameCloseMode closeMode, ref bool cancel)
        {
            if (!_disposed)
                _host.RaiseClose(closeMode, ref cancel);
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

        private class WindowHost : NiWindowHost
        {
            private readonly DockContent _dockContent;

            public WindowHost(DockContent dockContent)
            {
                _dockContent = dockContent;
            }

            protected override INiIsolationClient CreateWindow()
            {
                return _dockContent.WindowPane;
            }
        }

        private class Proxy : ServiceObject, INiWindowFrame
        {
            private readonly DockContent _owner;
            private IResource _icon;
            private readonly NiEnv _env;
            private bool _shown;
            private readonly Dictionary<int, object> _properties = new Dictionary<int, object>();

            public Proxy(DockContent owner)
            {
                _owner = owner;
                _env = (NiEnv)_owner.GetService(typeof(INiEnv));

                if ((owner.DockAreas & WeifenLuo.WinFormsUI.Docking.DockAreas.Document) != 0)
                    _properties[(int)NiFrameProperty.Type] = NiFrameType.Document;
                else
                    _properties[(int)NiFrameProperty.Type] = NiFrameType.Tool;
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
                        _owner.ShowIcon = _owner.Icon != null;
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

            public HResult Close(NiFrameCloseMode closeMode)
            {
                try
                {
                    _owner._suppressClosing = true;

                    try
                    {
                        bool cancel = false;
                        _owner.RaiseClose(closeMode, ref cancel);

                        if (cancel)
                            return HResult.False;

                        _owner.Close();
                    }
                    finally
                    {
                        _owner._suppressClosing = false;
                    }

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

            public HResult GetProperty(int property, out object value)
            {
                value = null;

                try
                {
                    if (!_properties.TryGetValue(property, out value))
                        return HResult.False;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetProperty(int property, object value)
            {
                try
                {
                    if (value == null)
                        _properties.Remove(property);
                    else
                        _properties[property] = value;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }
        }
    }
}

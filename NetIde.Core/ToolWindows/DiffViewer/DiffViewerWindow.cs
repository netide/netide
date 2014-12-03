using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NetIde.Core.Support;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    [Guid("de634c48-d5d6-49d5-bd69-1ef6c7f082ed")]
    internal class DiffViewerWindow : EditorWindow, INiDiffViewerWindow
    {
        private readonly NiConnectionPoint<INiDiffViewerWindowNotify> _connectionPoint = new NiConnectionPoint<INiDiffViewerWindowNotify>();

        public HResult GetMode(out NiDiffViewerMode mode)
        {
            mode = 0;

            try
            {
                mode = Control.Mode;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult SetMode(NiDiffViewerMode mode)
        {
            try
            {
                Control.Mode = mode;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult SetReadOnly(bool readOnly)
        {
            try
            {
                Control.ReadOnly = readOnly;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetReadOnly(out bool readOnly)
        {
            readOnly = false;

            try
            {
                readOnly = Control.ReadOnly;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public DiffViewerControl Control
        {
            get { return (DiffViewerControl)Controls[0]; }
        }

        public HResult Advise(object sink, out int cookie)
        {
            return _connectionPoint.Advise(sink, out cookie);
        }

        public HResult Advise(INiDiffViewerWindowNotify sink, out int cookie)
        {
            return Advise((object)sink, out cookie);
        }

        public HResult Unadvise(int cookie)
        {
            return _connectionPoint.Unadvise(cookie);
        }

        protected override Control CreateClient()
        {
            var control = new DiffViewerControl();

            control.ModeChanged += control_ModeChanged;
            control.LeftUpdated += control_LeftUpdated;
            control.RightUpdated += control_RightUpdated;
            control.Site = new SiteProxy(this);

            return control;
        }

        void control_RightUpdated(object sender, EventArgs e)
        {
            _connectionPoint.ForAll(p => p.OnRightChanged());
        }

        void control_LeftUpdated(object sender, EventArgs e)
        {
            _connectionPoint.ForAll(p => p.OnLeftChanged());
        }

        void control_ModeChanged(object sender, EventArgs e)
        {
            _connectionPoint.ForAll(p => p.OnModeChanged());
        }

        public HResult Reset()
        {
            try
            {
                Control.Reset();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public new HResult Load(IStream left, IStream right)
        {
            try
            {
                Control.Load(left, right);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetLeft(out IStream stream)
        {
            stream = null;

            try
            {
                stream = Control.GetLeft();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetRight(out IStream stream)
        {
            stream = null;

            try
            {
                stream = Control.GetRight();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
    }
}

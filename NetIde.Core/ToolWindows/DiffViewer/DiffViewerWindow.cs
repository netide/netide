using System;
using System.Collections.Generic;
using System.IO;
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

        public HResult GetState(out NiDiffViewerState state)
        {
            state = 0;

            try
            {
                state = Control.State;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult SetState(NiDiffViewerState state)
        {
            try
            {
                Control.State = state;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public DiffViewerControl Control
        {
            get { return (DiffViewerControl)Window; }
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

        protected override Control CreateControl()
        {
            var control = new DiffViewerControl();

            control.StateChanged += control_StateChanged;
            control.Site = new SiteProxy(this);

            return control;
        }

        void control_StateChanged(object sender, EventArgs e)
        {
            _connectionPoint.ForAll(p => p.OnStateChanged());
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

        public HResult Load(IStream left, IStream right)
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
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    public partial class DiffViewerControl : NetIde.Util.Forms.UserControl
    {
        private Control _currentViewer;
        private NiDiffViewerState _state;

        public NiDiffViewerState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    _textViewer.UnifiedDiff = (value & NiDiffViewerState.Unified) != 0;

                    OnStateChanged(EventArgs.Empty);
                }
            }
        }

        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;

                foreach (Control control in Controls)
                {
                    control.Site = value;
                }
            }
        }

        public event EventHandler StateChanged;

        protected virtual void OnStateChanged(EventArgs e)
        {
            var ev = StateChanged;
            if (ev != null)
                ev(this, e);
        }

        public DiffViewerControl()
        {
            InitializeComponent();

            foreach (Control control in Controls)
            {
                control.Dock = DockStyle.Fill;
                control.SuspendLayout();
            }

            Reset();
        }

        public void Reset()
        {
            SelectViewer(_summaryViewer);

            ((IViewer)_currentViewer).LoadStreams(null, null, null, null, null, null);
        }

        private void SelectViewer(Control viewer)
        {
            if (viewer == _currentViewer)
                return;

            if (_currentViewer != null)
                _currentViewer.SuspendLayout();

            viewer.ResumeLayout();
            viewer.PerformLayout();
            viewer.BringToFront();

            _currentViewer = viewer;
        }

        public new void Load(IStream left, IStream right)
        {
            byte[] leftData = null;
            FileType leftFileType = null;

            if (left != null)
            {
                leftData = LoadStream(left);

                using (var leftStream = new MemoryStream(leftData))
                {
                    leftFileType = FileType.FromStream(leftStream, Path.GetExtension(left.Name));
                }
            }

            byte[] rightData = null;
            FileType rightFileType = null;

            if (right != null)
            {
                rightData = LoadStream(right);

                using (var rightStream = new MemoryStream(rightData))
                {
                    rightFileType = FileType.FromStream(rightStream, Path.GetExtension(right.Name));
                }
            }

            FileTypeType type;

            if (leftFileType == null)
            {
                if (rightFileType == null)
                    type = FileTypeType.Binary;
                else
                    type = rightFileType.Type;
            }
            else
            {
                if (rightFileType == null || leftFileType.Type == rightFileType.Type)
                    type = leftFileType.Type;
                else
                    type = FileTypeType.Binary;
            }

            switch (type)
            {
                case FileTypeType.Text:
                    SelectViewer(_textViewer);
                    break;

                case FileTypeType.Image:
                    SelectViewer(_imageViewer);
                    break;

                default:
                    SelectViewer(_summaryViewer);
                    break;
            }

            ((IViewer)_currentViewer).LoadStreams(left, leftFileType, leftData, right, rightFileType, rightData);
        }

        private byte[] LoadStream(IStream stream)
        {
            long length;
            ErrorUtil.ThrowOnFailure(stream.GetLength(out length));

            byte[] result;
            ErrorUtil.ThrowOnFailure(stream.Read((int)length, out result));

            return result;
        }

        private void _textViewer_UnifiedDiffChanged(object sender, EventArgs e)
        {
            if (_textViewer.UnifiedDiff)
                State |= NiDiffViewerState.Unified;
            else
                State &= ~NiDiffViewerState.Unified;
        }
    }
}

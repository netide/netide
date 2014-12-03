using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using NetIde.Core.Settings;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Shell.Settings;
using NetIde.Util;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    public partial class DiffViewerControl : NetIde.Util.Forms.UserControl
    {
        private Control _currentViewer;
        private NiDiffViewerMode _mode;
        private byte[] _leftData;
        private byte[] _rightData;

        public NiDiffViewerMode Mode
        {
            get { return _mode; }
            set
            {
                if (_mode != value)
                {
                    _mode = value;

                    if (value == NiDiffViewerMode.Default)
                        value = GetDefaultMode();

                    _textViewer.UnifiedDiff = value == NiDiffViewerMode.Unified;

                    OnModeChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(true)]
        [Browsable(false)]
        public bool ReadOnly
        {
            get { return _textViewer.ReadOnly; }
            set { _textViewer.ReadOnly = value; }
        }

        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;

                _textViewer.UnifiedDiff = GetDefaultMode() == NiDiffViewerMode.Unified;

                foreach (Control control in Controls)
                {
                    control.Site = value;
                }
            }
        }

        public event EventHandler ModeChanged;

        protected virtual void OnModeChanged(EventArgs e)
        {
            var ev = ModeChanged;
            if (ev != null)
                ev(this, e);
        }

        public event EventHandler LeftUpdated;

        protected virtual void OnLeftUpdated(EventArgs e)
        {
            var ev = LeftUpdated;
            if (ev != null)
                ev(this, e);
        }

        public event EventHandler RightUpdated;

        protected virtual void OnRightUpdated(EventArgs e)
        {
            var ev = RightUpdated;
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

            _textViewer.LeftUpdated += (s, e) => OnLeftUpdated(EventArgs.Empty);
            _textViewer.RightUpdated += (s, e) => OnRightUpdated(EventArgs.Empty);

            Reset();
        }

        public void Reset()
        {
            SelectViewer(_textViewer);

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
            _leftData = null;
            FileType leftFileType = null;

            if (left != null)
            {
                _leftData = LoadStream(left);

                using (var leftStream = new MemoryStream(_leftData))
                {
                    leftFileType = FileType.FromStream(leftStream, Path.GetExtension(left.Name));
                }
            }

            _rightData = null;
            FileType rightFileType = null;

            if (right != null)
            {
                _rightData = LoadStream(right);

                using (var rightStream = new MemoryStream(_rightData))
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

            ((IViewer)_currentViewer).LoadStreams(left, leftFileType, _leftData, right, rightFileType, _rightData);
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
            NiDiffViewerMode mode;

            if (_textViewer.UnifiedDiff)
                mode = NiDiffViewerMode.Unified;
            else
                mode = NiDiffViewerMode.SideBySide;

            if (Mode != NiDiffViewerMode.Default)
                Mode = mode;
            else
                SetDefaultMode(mode);
        }

        private NiDiffViewerMode GetDefaultMode()
        {
            return SettingsBuilder.GetSettings<IDiffViewerSettings>(Site).DefaultMode;
        }

        private void SetDefaultMode(NiDiffViewerMode mode)
        {
            SettingsBuilder.GetSettings<IDiffViewerSettings>(Site).DefaultMode = mode;
        }

        public IStream GetLeft()
        {
            Stream stream;

            if (_currentViewer == _textViewer)
                stream = _textViewer.GetLeft();
            else
                stream = new MemoryStream(_leftData);

            return StreamUtil.FromStream(stream);
        }

        public IStream GetRight()
        {
            Stream stream;

            if (_currentViewer == _textViewer)
                stream = _textViewer.GetRight();
            else
                stream = new MemoryStream(_rightData);

            return StreamUtil.FromStream(stream);
        }
    }
}

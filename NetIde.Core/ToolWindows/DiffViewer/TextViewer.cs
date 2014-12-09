using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Util.Forms;
using NGit.Diff;
using NetIde.Core.Support;
using NetIde.Shell.Interop;
using NetIde.Util;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal partial class TextViewer : NetIde.Util.Forms.UserControl, IViewer
    {
        private readonly bool _designing;
        private bool _unifiedDiff;
        private Control _selectedViewer;
        private byte[] _leftData;
        private TextFileType _leftFileType;
        private byte[] _rightData;
        private TextFileType _rightFileType;
        private Text _leftText;
        private Text _rightText;
        private EditList _editList;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UnifiedDiff
        {
            get { return _unifiedDiff; }
            set
            {
                if (_unifiedDiff != value)
                {
                    _unifiedDiff = value;
                    _unified.Checked = value;
                    _sideBySide.Checked = !value;

                    SelectViewer(value ? (Control)_unifiedViewer : _sideBySideViewer);
                    OnUnifiedDiffChanged(EventArgs.Empty);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(true)]
        public bool ReadOnly
        {
            get { return _sideBySideViewer.ReadOnly; }
            set { _sideBySideViewer.ReadOnly = value; }
        }

        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;

                if (_designing)
                    return;

                foreach (Control control in _container.Controls)
                {
                    control.Site = value;
                }
            }
        }

        [Category("Behavior")]
        [DefaultValue(false)]
        public bool IgnoreWhitespace
        {
            get { return _ignoreWhitespace.Checked; }
            set { _ignoreWhitespace.Checked = value; }
        }

        public event EventHandler UnifiedDiffChanged;

        protected virtual void OnUnifiedDiffChanged(EventArgs e)
        {
            var ev = UnifiedDiffChanged;
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

        public event CancelEventHandler LeftUpdating;

        protected virtual void OnLeftUpdating(CancelEventArgs e)
        {
            var handler = LeftUpdating;
            if (handler != null)
                handler(this, e);
        }

        public event CancelEventHandler RightUpdating;

        protected virtual void OnRightUpdating(CancelEventArgs e)
        {
            var handler = RightUpdating;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler IgnoreWhitespaceChanged;

        protected virtual void OnIgnoreWhitespaceChanged(EventArgs e)
        {
            var handler = IgnoreWhitespaceChanged;
            if (handler != null)
                handler(this, e);
        }

        private void SelectViewer(Control control)
        {
            if (_selectedViewer != null)
                _selectedViewer.SuspendLayout();

            control.ResumeLayout();
            control.BringToFront();

            _selectedViewer = control;

            if (_leftText != null && _rightText != null)
                ((ITextViewer)_selectedViewer).LoadDiff(_leftText, _rightText, _editList);
        }

        public void LoadStreams(IStream leftStream, FileType leftFileType, byte[] leftData, IStream rightStream, FileType rightFileType, byte[] rightData)
        {
            _unifiedViewer.SelectDetails(leftStream, leftFileType, rightStream, rightFileType);
            _sideBySideViewer.SelectDetails(leftStream, leftFileType, rightStream, rightFileType);

            _leftData = leftData;
            _leftFileType = (TextFileType)leftFileType;
            _rightData = rightData;
            _rightFileType = (TextFileType)rightFileType;

            int leftBomSize = _leftFileType != null ? _leftFileType.Encoding.GetPreamble().Length : 0;
            int rightBomSize = _rightFileType != null ? _rightFileType.Encoding.GetPreamble().Length : 0;

            _leftText = new Text(
                _leftData == null
                ? String.Empty
                : _leftFileType.Encoding.GetString(_leftData, leftBomSize, _leftData.Length - leftBomSize)
            );
            _rightText = new Text(
                _rightData == null
                ? String.Empty
                : _rightFileType.Encoding.GetString(_rightData, rightBomSize, _rightData.Length - rightBomSize)
            );

            LoadDiff();
        }

        private void LoadDiff()
        {
            if (_leftText == null && _rightText == null)
                return;

            BuildEditList();

            ((ITextViewer)_selectedViewer).LoadDiff(_leftText, _rightText, _editList);
        }

        private void BuildEditList()
        {
            _editList = DiffAlgorithm.GetAlgorithm(DiffAlgorithm.SupportedAlgorithm.HISTOGRAM).Diff(
                IgnoreWhitespace
                ? DiffViewer.Text.Comparator.WS_IGNORE_ALL
                : DiffViewer.Text.Comparator.DEFAULT,
                _leftText,
                _rightText
            );
        }

        void _sideBySideViewer_LeftUpdated(object sender, EventArgs e)
        {
            _leftText = new Text(_sideBySideViewer.GetLeftText());
            BuildEditList();
            OnLeftUpdated(EventArgs.Empty);
        }

        void _sideBySideViewer_RightUpdated(object sender, EventArgs e)
        {
            _rightText = new Text(_sideBySideViewer.GetRightText());
            BuildEditList();
            OnRightUpdated(EventArgs.Empty);
        }

        private void _sideBySideViewer_LeftUpdating(object sender, CancelEventArgs e)
        {
            OnLeftUpdating(e);
        }

        private void _sideBySideViewer_RightUpdating(object sender, CancelEventArgs e)
        {
            OnRightUpdating(e);
        }

        public TextViewer()
        {
            _designing = ControlUtil.GetIsInDesignMode(this);

            InitializeComponent();

            _toolStrip.Renderer = ToolStripSimpleRenderer.Instance;

            foreach (Control control in _container.Controls)
            {
                control.Dock = DockStyle.Fill;
                control.SuspendLayout();
            }

            _unifiedDiff = false;
            UnifiedDiff = true;
        }

        private void _unified_Click(object sender, EventArgs e)
        {
            UnifiedDiff = true;
        }

        private void _sideBySide_Click(object sender, EventArgs e)
        {
            UnifiedDiff = false;
        }

        public Stream GetLeft()
        {
            return BuildStream(_leftText, _leftFileType);
        }

        public Stream GetRight()
        {
            return BuildStream(_rightText, _rightFileType);
        }

        private Stream BuildStream(Text text, TextFileType fileType)
        {
            var preamble = fileType.Encoding.GetPreamble();

            var stream = new MemoryStream(text.Content.Length + preamble.Length);

            var data = fileType.Encoding.GetBytes(text.Content);

            stream.Write(preamble, 0, preamble.Length);
            stream.Write(data, 0, data.Length);
            stream.Position = 0;

            return stream;
        }

        private void _ignoreWhitespace_CheckedChanged(object sender, EventArgs e)
        {
            OnIgnoreWhitespaceChanged(EventArgs.Empty);

            LoadDiff();
        }
    }
}

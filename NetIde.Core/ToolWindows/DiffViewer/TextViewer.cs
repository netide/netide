using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NGit.Diff;
using NetIde.Core.Support;
using NetIde.Shell.Interop;
using NetIde.Util;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal partial class TextViewer : NetIde.Util.Forms.UserControl, IViewer
    {
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

                foreach (Control control in _container.Controls)
                {
                    control.Site = value;
                }
            }
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

            _leftText = new Text(_leftData == null ? String.Empty : _leftFileType.Encoding.GetString(_leftData, _leftFileType.BomSize, _leftData.Length - _leftFileType.BomSize));
            _rightText = new Text(_rightData == null ? String.Empty : _rightFileType.Encoding.GetString(_rightData, _rightFileType.BomSize, _rightData.Length - _rightFileType.BomSize));

            BuildEditList();

            ((ITextViewer)_selectedViewer).LoadDiff(_leftText, _rightText, _editList);
        }

        private void BuildEditList()
        {
            _editList = DiffAlgorithm.GetAlgorithm(DiffAlgorithm.SupportedAlgorithm.HISTOGRAM).Diff(
                DiffViewer.Text.Comparator.DEFAULT,
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

        public TextViewer()
        {
            InitializeComponent();

            _sideBySideViewer.LeftUpdated += _sideBySideViewer_LeftUpdated;
            _sideBySideViewer.RightUpdated += _sideBySideViewer_RightUpdated;

            toolStrip1.Renderer = ToolStripSimpleRenderer.Instance;

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
            var stream = new MemoryStream(text.Content.Length + fileType.BomSize);

            if (fileType.Bom != null)
                stream.Write(fileType.Bom, 0, fileType.Bom.Length);

            var data = fileType.Encoding.GetBytes(text.Content);
            stream.Write(data, 0, data.Length);
            stream.Position = 0;

            return stream;
        }
    }
}

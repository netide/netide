using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
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
        public event EventHandler UnifiedDiffChanged;
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

        protected virtual void OnUnifiedDiffChanged(EventArgs e)
        {
            var ev = UnifiedDiffChanged;
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

            if (_leftData == null || _rightData == null)
            {
                // Reset.

                _leftText = null;
                _rightText = null;
                _editList = null;
                _unifiedViewer.Reset();
                _sideBySideViewer.Reset();
                return;
            }

            _leftText = new Text(_leftFileType.Encoding.GetString(_leftData));
            _rightText = new Text(_rightFileType.Encoding.GetString(_rightData));

            _editList = DiffAlgorithm.GetAlgorithm(DiffAlgorithm.SupportedAlgorithm.HISTOGRAM).Diff(
                DiffViewer.Text.Comparator.DEFAULT,
                _leftText,
                _rightText
            );

            ((ITextViewer)_selectedViewer).LoadDiff(_leftText, _rightText, _editList);
        }

        public TextViewer()
        {
            InitializeComponent();

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
    }
}

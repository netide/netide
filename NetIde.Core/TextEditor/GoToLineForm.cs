using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Core.TextEditor
{
    internal partial class GoToLineForm : DialogForm
    {
        private readonly int _maxLine;

        public int? SelectedLine
        {
            get
            {
                if (_lineNumber.Value.HasValue)
                    return (int)_lineNumber.Value;

                return null;
            }
        }

        public GoToLineForm(int currentLine, int maxLine)
        {
            _maxLine = maxLine;

            InitializeComponent();

            _lineNumber.Value = currentLine;

            _lineNumberLabel.Text = String.Format(_lineNumberLabel.Text, maxLine);

            UpdateEnabled();
        }

        private void _acceptButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void _lineNumber_ValueChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            int selectedLine = SelectedLine.GetValueOrDefault(1);

            _acceptButton.Enabled = selectedLine >= 1 && selectedLine <= _maxLine;
        }
    }
}

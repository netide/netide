using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell.Interop;
using NetIde.Util;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal partial class SummaryViewer : NetIde.Util.Forms.UserControl, IViewer
    {
        public void LoadStreams(IStream leftStream, FileType leftFileType, byte[] leftData, IStream rightStream, FileType rightFileType, byte[] rightData)
        {
            _leftDetails.SelectDetails(leftStream, leftFileType);
            _rightDetails.SelectDetails(rightStream, rightFileType);

            if (leftStream == null)
                _summary.Text = null;
            else
                _summary.Text = Labels.BinaryFileSummary;
        }

        public SummaryViewer()
        {
            InitializeComponent();
        }
    }
}

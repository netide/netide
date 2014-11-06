using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell.Interop;
using NetIde.Util;
using log4net;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal partial class ImageViewer : NetIde.Util.Forms.UserControl, IViewer
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ImageViewer));

        public void LoadStreams(IStream leftStream, FileType leftFileType, byte[] leftData, IStream rightStream, FileType rightFileType, byte[] rightData)
        {
            _leftDetails.SelectDetails(leftStream, leftFileType);
            _rightDetails.SelectDetails(rightStream, rightFileType);

            _leftImage.Image = null;

            if (leftData != null)
            {
                try
                {
                    _leftImage.Image = Image.FromStream(new MemoryStream(leftData));
                }
                catch (Exception ex)
                {
                    Log.Warn("Could not load left image", ex);
                }
            }

            _leftImageContainer.Visible = _leftImage.Image != null;

            _rightImage.Image = null;

            if (rightData != null)
            {
                try
                {
                    _rightImage.Image = Image.FromStream(new MemoryStream(rightData));
                }
                catch (Exception ex)
                {
                    Log.Warn("Could not load right image", ex);
                }
            }

            _rightImageContainer.Visible = _rightImage.Image != null;
        }

        public ImageViewer()
        {
            InitializeComponent();
        }
    }
}

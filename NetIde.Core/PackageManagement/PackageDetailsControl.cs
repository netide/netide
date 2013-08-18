using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Xml.PackageMetadata;

namespace NetIde.Core.PackageManagement
{
    public partial class PackageDetailsControl : NetIde.Util.Forms.UserControl
    {
        private const int LineSpacing = 3;
        private static readonly Padding TextPadding = new Padding(7);
        private readonly List<DetailsLine> _lines = new List<DetailsLine>();

        private PackageMetadata _package;

        public PackageMetadata Package
        {
            get { return _package; }
            set
            {
                if (_package != value)
                {
                    _package = value;

                    _moreInformationLink.Visible =
                        _package != null &&
                        _package.GalleryDetailsUrl != null;

                    RebuildLines();
                }
            }
        }

        private void RebuildLines()
        {
            _lines.Clear();
            _lines.Add(new DetailsLine(Labels.CreatedBy, Package.Authors));
            _lines.Add(new DetailsLine(Labels.Id, Package.Id));
            _lines.Add(new DetailsLine(Labels.Version, Package.Version));

            if (Package.Published.HasValue)
                _lines.Add(new DetailsLine(Labels.LastPublished, Package.Published.Value.ToShortDateString()));

            if (Package.DownloadCount.HasValue)
                _lines.Add(new DetailsLine(Labels.Downloads, Package.DownloadCount.ToString()));

            _lines.Add(new DetailsLine(Labels.Tags, Package.Tags));

            PerformLayout();
            Invalidate();
        }

        public PackageDetailsControl()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (Package == null)
                return;

            int lineHeight = TextRenderer.MeasureText("W", Font).Height;

            using (var boldFont = new Font(Font, FontStyle.Bold))
            {
                int offset = 0;

                foreach (var line in _lines)
                {
                    string label = line.Label + ":";
                    int labelWidth = TextRenderer.MeasureText(label, boldFont).Width;

                    const TextFormatFlags format = TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

                    TextRenderer.DrawText(
                        e.Graphics,
                        label,
                        boldFont,
                        new Point(
                            TextPadding.Left,
                            TextPadding.Top + offset
                        ),
                        ForeColor,
                        BackColor,
                        format
                    );

                    TextRenderer.DrawText(
                        e.Graphics,
                        line.Value,
                        Font,
                        new Rectangle(
                            TextPadding.Left + labelWidth,
                            TextPadding.Top + offset,
                            Width - (TextPadding.Horizontal + labelWidth),
                            int.MaxValue
                        ),
                        ForeColor,
                        BackColor,
                        format
                    );

                    offset += lineHeight + LineSpacing;
                }
            }
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            int lineHeight = TextRenderer.MeasureText("W", Font).Height;

            _moreInformationLink.Top = (lineHeight + LineSpacing) * _lines.Count + TextPadding.Vertical;
        }

        private void _moreInformationLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Package != null)
            {
                try
                {
                    Process.Start(Package.GalleryDetailsUrl);
                }
                catch
                {
                    // Ignore exceptions.
                }
            }
        }

        private class DetailsLine
        {
            public string Label { get; private set; }
            public string Value { get; private set; }

            public DetailsLine(string label, string value)
            {
                Label = label;
                Value = value;
            }
        }
    }
}

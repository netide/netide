using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Document;
using NetIde.Core.Support;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal class StreamDetailsControl : ToolStrip
    {
        private static readonly string[] _highlighterNames = GetHighlighterNames();
        private static readonly string _defaultHighlighterName = HighlightingManager.Manager.DefaultHighlighting.Name;

        private static string[] GetHighlighterNames()
        {
            return HighlightingManager.Manager.HighlightingDefinitions
                .Cast<DictionaryEntry>()
                .Select(p => (string)p.Key)
                .OrderBy(p => p)
                .ToArray();
        }

        private readonly ToolStripLabel _lastWriteTime;
        private readonly ToolStripLabel _fileSize;
        private readonly ToolStripDropDownButton _contentType;
        private readonly ToolStripLabel _encoding;
        private readonly ToolStripLabel _bom;
        private readonly ToolStripLabel _lineEnding;

        public event EventHandler ContentTypeSelected;

        protected virtual void OnContentTypeSelected(EventArgs e)
        {
            var ev = ContentTypeSelected;
            if (ev != null)
                ev(this, e);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime? LastWriteTime
        {
            get { return (DateTime?)_lastWriteTime.Tag; }
            set
            {
                _lastWriteTime.Tag = value;
                _lastWriteTime.Visible = value.HasValue;

                if (value.HasValue)
                    _lastWriteTime.Text = value.Value.ToShortDateString() + " " + value.Value.ToShortTimeString();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long? FileSize
        {
            get { return (long?)_fileSize.Tag; }
            set
            {
                _fileSize.Tag = value;
                _fileSize.Visible = value.HasValue;

                if (!value.HasValue)
                    return;

                double size = value.Value;

                if (size > 1024)
                {
                    size /= 1024;

                    if (size > 1024)
                    {
                        size /= 1024;

                        if (size > 1024)
                        {
                            size /= 1024;

                            _fileSize.Text = String.Format(Labels.FileSizeGigaBytes, size);
                        }
                        else
                        {
                            _fileSize.Text = String.Format(Labels.FileSizeMegaBytes, size);
                        }
                    }
                    else
                    {
                        _fileSize.Text = String.Format(Labels.FileSizeKiloBytes, size);
                    }
                }
                else
                {
                    _fileSize.Text = String.Format(Labels.FileSizeBytes, size);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HaveBom
        {
            get { return _bom.Visible; }
            set { _bom.Visible = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LineTermination? LineTermination
        {
            get { return (LineTermination?)_lineEnding.Tag; }
            set
            {
                _lineEnding.Tag = value;
                _lineEnding.Visible = value.HasValue;

                if (value.HasValue)
                    _lineEnding.Text = value.ToString().ToUpper();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Encoding Encoding
        {
            get { return (Encoding)_encoding.Tag; }
            set
            {
                _encoding.Tag = value;
                _encoding.Visible = value != null;

                if (value != null)
                    _encoding.Text = value.HeaderName.ToUpper();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ContentType
        {
            get { return _contentType.Text; }
            set
            {
                _contentType.Visible = !String.IsNullOrEmpty(value);

                if (_contentType.Visible)
                {
                    foreach (ToolStripMenuItem item in _contentType.DropDownItems)
                    {
                        item.Checked = (string)item.Tag == value;

                        if (item.Checked)
                            _contentType.Text = item.Text;
                    }
                }
            }
        }

        public void SelectDetails(IStream stream, FileType fileType)
        {
            LastWriteTime = stream == null ? null : stream.LastWriteTime;

            if (stream == null)
            {
                FileSize = null;
            }
            else
            {
                long length;
                ErrorUtil.ThrowOnFailure(stream.GetLength(out length));

                FileSize = length;
            }

            var textFileType = fileType as TextFileType;

            HaveBom = textFileType != null && textFileType.Encoding.GetPreamble().Length > 0;
            LineTermination = textFileType == null ? (LineTermination?)null : textFileType.LineTermination;
            Encoding = textFileType == null ? null : textFileType.Encoding;

            if (textFileType == null)
            {
                ContentType = null;
            }
            else
            {
                var highlighter = HighlightingManager.Manager.FindHighlighterForFile(stream.Name);

                ContentType = highlighter == null ? null : highlighter.Name;
            }
        }

        public StreamDetailsControl()
        {
            SuspendLayout();

            _lastWriteTime = new ToolStripLabel();
            _fileSize = new ToolStripLabel();
            _contentType = new ToolStripDropDownButton
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };
            _encoding = new ToolStripLabel();
            _bom = new ToolStripLabel
            {
                Text = "BOM"
            };
            _lineEnding = new ToolStripLabel();

            Items.AddRange(new ToolStripItem[]
            {
                _lastWriteTime,
                _fileSize,
                _contentType,
                _encoding,
                _bom,
                _lineEnding
            });
            Renderer = ToolStripSimpleRenderer.Instance;

            LastWriteTime = null;
            FileSize = null;
            HaveBom = false;
            LineTermination = null;
            Encoding = null;
            ContentType = null;

            ResumeLayout(false);
            PerformLayout();

            foreach (string name in _highlighterNames)
            {
                var item = new ToolStripMenuItem
                {
                    Text = name == _defaultHighlighterName ? Labels.PlainText : name,
                    Tag = name
                };

                item.Click += item_Click;

                _contentType.DropDownItems.Add(item);
            }
        }

        void item_Click(object sender, EventArgs e)
        {
            var item = (ToolStripMenuItem)sender;

            ContentType = (string)item.Tag;

            OnContentTypeSelected(EventArgs.Empty);
        }
    }
}

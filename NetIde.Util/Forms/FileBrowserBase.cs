using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Util.Forms
{
    [DefaultEvent("PathChanged")]
    [ToolboxItem(false)]
    public class FileBrowserBase : BrowserControl
    {
        private bool _allowFiles;
        private bool _readOnly;
        private string _path;

        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Whether the browser allows files to be selected")]
        public bool AllowFiles
        {
            get { return _allowFiles; }
            set
            {
                if (_allowFiles != value)
                {
                    _allowFiles = value;
                    OnAllowFilesChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler AllowFilesChanged;

        protected virtual void OnAllowFilesChanged(EventArgs e)
        {
            var handler = AllowFilesChanged;
            if (handler != null)
                handler(this, e);
        }

        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Whether the user can enter file names manually")]
        public bool ReadOnly
        {
            get { return _readOnly; }
            set
            {
                if (_readOnly != value)
                {
                    _readOnly = value;
                    OnReadOnlyChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler ReadOnlyChanged;

        protected virtual void OnReadOnlyChanged(EventArgs e)
        {
            var handler = ReadOnlyChanged;
            if (handler != null)
                handler(this, e);
        }

        [DefaultValue(null)]
        [Category("Behavior")]
        [Description("Title of the open file or browse for folder dialog")]
        [Localizable(true)]
        public string Title { get; set; }

        [DefaultValue(null)]
        [Category("Behavior")]
        [Description("Filter used in the open file dialog")]
        [Localizable(true)]
        public string Filter { get; set; }

        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Check whether the file exists in the open file dialog")]
        public bool CheckFileExists { get; set; }

        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Check whether the path exists in the open file dialog")]
        public bool CheckPathExists { get; set; }

        [DefaultValue("")]
        [Category("Behavior")]
        [Description("Path of the browser")]
        public string Path
        {
            get { return _path; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    value = null;

                if (_path != value)
                {
                    _path = value;
                    OnPathChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler PathChanged;

        protected virtual void OnPathChanged(EventArgs e)
        {
            var handler = PathChanged;
            if (handler != null)
                handler(this, e);
        }

        public FileBrowserBase()
        {
            _allowFiles = true;
            CheckFileExists = true;
            CheckPathExists = true;
        }

        protected override void OnBrowse(EventArgs e)
        {
            base.OnBrowse(e);

            if (AllowFiles)
            {
                using (var form = new OpenFileDialog())
                {
                    form.AddExtension = true;
                    form.AutoUpgradeEnabled = true;
                    form.CheckFileExists = CheckFileExists;
                    form.CheckPathExists = CheckPathExists;
                    form.Filter = Filter;
                    form.RestoreDirectory = true;
                    form.Title = Title;

                    if (Path != null && File.Exists(Path))
                        form.FileName = Path;

                    if (form.ShowDialog(this) == DialogResult.OK)
                        Path = form.FileName;
                }
            }
            else
            {
                var browser = new FolderBrowser();

                browser.Title = Title;

                if (Path != null && Directory.Exists(Path))
                    browser.SelectedPath = Path;

                if (browser.ShowDialog(this) == DialogResult.OK)
                    Path = browser.SelectedPath;
            }
        }
    }
}

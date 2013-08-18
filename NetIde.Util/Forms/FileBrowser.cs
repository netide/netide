using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Util.Forms
{
    [DefaultEvent("PathChanged")]
    public partial class FileBrowser : BrowserControl
    {
        private bool _allowFiles;

        public event EventHandler PathChanged;

        protected virtual void OnPathChanged(EventArgs e)
        {
            var ev = PathChanged;
            if (ev != null)
                ev(this, e);
        }

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
                    _path.AutoCompleteSource =
                        value
                        ? AutoCompleteSource.FileSystem
                        : AutoCompleteSource.FileSystemDirectories;
                }
            }
        }

        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Whether the user can enter file names manually")]
        public bool ReadOnly
        {
            get { return _path.ReadOnly; }
            set { _path.ReadOnly = value; }
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
            get { return _path.Text.Length == 0 ? null : _path.Text; }
            set { _path.Text = value; }
        }

        public FileBrowser()
        {
            InitializeComponent();

            AllowFiles = true;
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
                    form.FileName = _path.Text;
                    form.Filter = Filter;
                    form.RestoreDirectory = true;
                    form.Title = Title;

                    if (form.ShowDialog(this) == DialogResult.OK)
                        _path.Text = form.FileName;
                }
            }
            else
            {
                var browser = new FolderBrowser();

                browser.Title = Title;
                browser.SelectedPath = _path.Text;

                if (browser.ShowDialog(this) == DialogResult.OK)
                    _path.Text = browser.SelectedPath;
            }
        }

        private void _path_TextChanged(object sender, EventArgs e)
        {
            OnPathChanged(EventArgs.Empty);
        }
    }
}

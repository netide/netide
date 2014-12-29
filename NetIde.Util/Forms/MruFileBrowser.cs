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
    public partial class MruFileBrowser : FileBrowserBase
    {
        public event MruHistoryEventHandler LoadHistory;

        protected virtual void OnLoadHistory(MruHistoryEventArgs e)
        {
            var handler = LoadHistory;
            if (handler != null)
                handler(this, e);
        }

        public event MruHistoryEventHandler SaveHistory;

        protected virtual void OnSaveHistory(MruHistoryEventArgs e)
        {
            var handler = SaveHistory;
            if (handler != null)
                handler(this, e);
        }

        public MruFileBrowser()
        {
            InitializeComponent();

            UpdateAutoCompleteSource();
        }

        protected override void OnAllowFilesChanged(EventArgs e)
        {
            UpdateAutoCompleteSource();

            base.OnAllowFilesChanged(e);
        }

        private void UpdateAutoCompleteSource()
        {
            _path.AutoCompleteSource = AllowFiles
                ? AutoCompleteSource.FileSystem
                : AutoCompleteSource.FileSystemDirectories;
        }

        protected override void OnReadOnlyChanged(EventArgs e)
        {
            _path.Enabled = !ReadOnly;

            base.OnReadOnlyChanged(e);
        }

        protected override void OnPathChanged(EventArgs e)
        {
            _path.Text = Path;

            base.OnPathChanged(e);
        }

        private void _path_TextChanged(object sender, EventArgs e)
        {
            Path = _path.Text;
        }

        public void AddTextToMru()
        {
            _path.AddTextToMru();
        }

        private void _path_LoadHistory(object sender, MruHistoryEventArgs e)
        {
            OnLoadHistory(e);
        }

        private void _path_SaveHistory(object sender, MruHistoryEventArgs e)
        {
            OnSaveHistory(e);
        }
    }
}

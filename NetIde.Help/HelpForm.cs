using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Util.Forms;

namespace NetIde.Help
{
    public partial class HelpForm : NetIde.Util.Forms.Form
    {
        private bool _isNavigating;

        public string Home { get; set; }

        public HelpForm()
        {
            InitializeComponent();

            UpdateEnabled();

            _webBrowser.CanGoBackChanged += (s, e) => UpdateEnabled();
            _webBrowser.CanGoForwardChanged += (s, e) => UpdateEnabled();
        }

        public event HelpFindEventHandler Find;

        protected virtual void OnFind(HelpFindEventArgs e)
        {
            var ev = Find;
            if (ev != null)
                ev(this, e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F5:
                    _refresh.PerformClick();
                    return true;

                case Keys.Home:
                    _home.PerformClick();
                    return true;

                case Keys.Control | Keys.F:
                    _search.Focus();
                    _search.SelectAll();
                    break;

                case Keys.Escape:
                    if (_isNavigating)
                        _stop.PerformClick();
                    else
                        Close();

                    return true;

                case Keys.Control | Keys.P:
                    _print.PerformClick();
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void NavigateTo(string url)
        {
            _webBrowser.Navigate(url);
        }

        private void _webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            _isNavigating = true;
            _statusStrip.Text = e.Url.ToString();

            UpdateEnabled();
        }

        private void _webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            _isNavigating = false;
            _statusStrip.Text = null;

            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            _back.Enabled = _webBrowser.CanGoBack;
            _forward.Enabled = _webBrowser.CanGoForward;
            _refresh.Visible = !_isNavigating;
            _stop.Visible = _isNavigating;
        }

        private void _back_Click(object sender, EventArgs e)
        {
            _webBrowser.GoBack();
        }

        private void _forward_Click(object sender, EventArgs e)
        {
            _webBrowser.GoForward();
        }

        private void _refresh_Click(object sender, EventArgs e)
        {
            _webBrowser.Refresh();
        }

        private void _stop_Click(object sender, EventArgs e)
        {
            _webBrowser.Stop();
        }

        private void _home_Click(object sender, EventArgs e)
        {
            if (Home != null)
                NavigateTo(Home);
        }

        private void _print_Click(object sender, EventArgs e)
        {
            _webBrowser.Print();
        }

        private void _find_Click(object sender, EventArgs e)
        {
            OnFind(new HelpFindEventArgs(_search.Text));
        }

        private void _search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                _find.PerformClick();
            }
        }

        private void HelpForm_BrowseButtonClick(object sender, BrowseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case BrowseButton.Forward:
                    e.Handled = true;

                    _forward.PerformClick();
                    break;

                case BrowseButton.Back:
                    e.Handled = true;

                    _back.PerformClick();
                    break;
            }
        }
    }
}

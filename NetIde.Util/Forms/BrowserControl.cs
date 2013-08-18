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
    [DefaultEvent("Browse")]
    public partial class BrowserControl : Panel
    {
        private readonly string _buttonText;
        private Image _buttonImage;

        protected Button BrowseButton
        {
            get { return _browseButton; }
        }

        [Category("Appearance")]
        [DefaultValue(null)]
        public Image ButtonImage
        {
            get { return _buttonImage; }
            set
            {
                if (_buttonImage != value)
                {
                    _buttonImage = value;

                    if (value == null)
                    {
                        _browseButton.FlatStyle = FlatStyle.System;
                        _browseButton.Image = null;
                        _browseButton.Text = _buttonText;
                    }
                    else
                    {
                        _browseButton.FlatStyle = FlatStyle.Standard;
                        _browseButton.Text = null;
                        _browseButton.Image = value;
                    }
                }
            }
        }

        public event EventHandler Browse;

        protected virtual void OnBrowse(EventArgs e)
        {
            var ev = Browse;
            if (ev != null)
                ev(this, EventArgs.Empty);
        }

        public BrowserControl()
        {
            InitializeComponent();

            _buttonText = _browseButton.Text;
        }

        private Control GetControl()
        {
            foreach (Control control in Controls)
            {
                if (control != _browseButton)
                    return control;
            }

            return null;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            var control = GetControl();

            if (control != null)
            {
                height = control.Height;

                specified |= BoundsSpecified.Height;
            }

            base.SetBoundsCore(x, y, width, height, specified);
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            var control = GetControl();

            if (control == null)
                return;

            _browseButton.SetBounds(
                Width - _browseButton.Width,
                -1,
                _browseButton.Width,
                control.Height + 2
            );

            control.SetBounds(
                0,
                0,
                Width - (_browseButton.Width + 3),
                control.Height
            );
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(EventArgs.Empty);

            Height = _browseButton.Height;
        }

        private void _browseButton_Click(object sender, EventArgs e)
        {
            OnBrowse(EventArgs.Empty);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NetIde.Util.Forms
{
    public class InformationItem
    {
        private InformationIcon _icon;
        private string _text;
        private bool _isLink;

        public event EventHandler Click;

        protected virtual void OnClick(EventArgs e)
        {
            var handler = Click;
            if (handler != null)
                handler(this, e);
        }

        internal InformationBar Bar { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Selected
        {
            get { return Bar != null && Bar.SelectedItem == this; }
            set
            {
                if (Bar != null)
                {
                    if (value)
                        Bar.SelectedItem = this;
                    else if (Bar.SelectedItem == this)
                        Bar.SelectedItem = null;
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(InformationIcon.None)]
        public InformationIcon Icon
        {
            get { return _icon; }
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    if (Selected)
                        Bar.SelectedItem = this;
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        public string Text
        {
            get { return _text; }
            set
            {
                if (value == null)
                    value = String.Empty;

                if (_text != value)
                {
                    _text = value;
                    if (Selected)
                        Bar.SelectedItem = this;
                }
            }
        }

        [Category("Behavior")]
        [DefaultValue(false)]
        public bool IsLink
        {
            get { return _isLink; }
            set
            {
                if (_isLink != value)
                {
                    _isLink = value;
                    if (Selected)
                        Bar.SelectedItem = this;
                }
            }
        }

        public InformationItem()
            : this(InformationIcon.None, null)
        {
        }

        public InformationItem(InformationIcon icon, string text)
        {
            _icon = icon;
            _text = text ?? String.Empty;
        }

        public void PerformClick()
        {
            OnClick(EventArgs.Empty);
        }
    }
}

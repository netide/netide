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

        public InformationItem()
            : this(InformationIcon.None, null)
        {
        }

        public InformationItem(InformationIcon icon, string text)
        {
            _icon = icon;
            _text = text ?? String.Empty;
        }
    }
}

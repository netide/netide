using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Util.Forms
{
    [DefaultProperty("Text")]
    public partial class TextBoxBrowser : BrowserControl
    {
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return _textBox.ReadOnly; }
            set { _textBox.ReadOnly = value; }
        }

        [Browsable(false)]
        public TextBox TextBox
        {
            get { return _textBox; }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Localizable(true)]
        public override string Text
        {
            get { return _textBox.Text; }
            set { _textBox.Text = value; }
        }

        public TextBoxBrowser()
        {
            InitializeComponent();
        }

        private void _textBox_TextChanged(object sender, EventArgs e)
        {
            OnTextChanged(EventArgs.Empty);
        }
    }
}

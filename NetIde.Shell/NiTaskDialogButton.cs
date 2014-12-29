using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Shell
{
    [DesignTimeVisible(false)]
    [ToolboxItem(false)]
    [DefaultProperty("Text")]
    public class NiTaskDialogButton : Component
    {
        private string _name;
        private readonly Formattable _text = new Formattable();

        [Description("Button name")]
        [Browsable(false)]
        public string Name
        {
            get
            {
                return WindowsFormsUtils.GetComponentName(this, _name);
            }
            set
            {
                _name = value ?? String.Empty;

                if (Site != null)
                    Site.Name = value;
            }
        }

        [Description("Text of the button")]
        [Category("Appearance")]
        [Localizable(true)]
        [Bindable(true)]
        [DefaultValue("")]
        public string Text
        {
            get { return _text.Text; }
            set { _text.Text = value; }
        }

        [Browsable(false)]
        internal string FormattedText
        {
            get { return _text.Formatted; }
        }

        [Description("Whether this button is the default button")]
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool Default { get; set; }

        [Description("Result returned from showing the dialog when this button is clicked")]
        [Category("Behavior")]
        [DefaultValue(DialogResult.None)]
        public DialogResult DialogResult { get; set; }

        [DefaultValue(null)]
        [TypeConverter(typeof(StringConverter))]
        [Bindable(true)]
        [Localizable(false)]
        public object Tag { get; set; }

        public NiTaskDialogButton()
        {
        }

        public NiTaskDialogButton(string text)
            : this(text, DialogResult.None)
        {
        }

        public NiTaskDialogButton(string text, DialogResult dialogResult)
        {
            Text = text;
            DialogResult = dialogResult;
        }

        public void FormatText(params object[] args)
        {
            _text.Format(args);
        }
    }
}

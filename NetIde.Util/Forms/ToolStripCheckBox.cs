using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Util.Forms
{
    public class ToolStripCheckBox : ToolStripControlHost
    {
        private CheckBox CheckBox
        {
            get { return (CheckBox)Control; }
        }

        public ToolStripCheckBox()
            : base(new CheckBox())
        {
            CheckBox.BackColor = Color.Transparent;
            CheckBox.Padding = new Padding(0, 1, 0, 0);
            //CheckBox.FlatStyle = FlatStyle.System;
        }

        [DefaultValue("")]
        [Localizable(true)]
        public override string Text
        {
            get { return CheckBox.Text; }
            set { CheckBox.Text = value; }
        }

        [DefaultValue(false)]
        [RefreshProperties(RefreshProperties.All)]
        [Bindable(true)]
        [SettingsBindable(true)]
        public bool Checked
        {
            get { return CheckBox.Checked; }
            set { CheckBox.Checked = value; }
        }

        [Bindable(true)]
        [DefaultValue(CheckState.Unchecked)]
        [RefreshProperties(RefreshProperties.All)]
        public CheckState CheckState
        {
            get { return CheckBox.CheckState; }
            set { CheckBox.CheckState = value; }
        }

        public event EventHandler CheckedChanged
        {
            add { CheckBox.CheckedChanged += value; }
            remove { CheckBox.CheckedChanged -= value; }
        }

        public event EventHandler CheckStateChanged
        {
            add { CheckBox.CheckStateChanged += value; }
            remove { CheckBox.CheckStateChanged -= value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Util;

namespace NetIde.Core.OptionPages.Environment
{
    internal class KeysTextBox : TextBox
    {
        private Keys _keys;
        private const Keys Backspace = (Keys)8;

        public event EventHandler KeysChanged;

        protected virtual void OnKeysChanged(EventArgs e)
        {
            var ev = KeysChanged;
            if (ev != null)
                ev(this, e);
        }

        public Keys Keys
        {
            get { return _keys; }
            set
            {
                if (_keys != value)
                {
                    _keys = value;

                    if (value == 0)
                        Text = null;
                    else
                        Text = ShortcutKeysUtil.ToDisplayString(value);

                    Select(Text.Length, 0);

                    OnKeysChanged(EventArgs.Empty);
                }
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            return base.IsInputKey(keyData) || ShortcutKeysUtil.IsValid(keyData) || keyData == Backspace;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyData == Backspace || !ShortcutKeysUtil.IsValid(e.KeyData))
                Keys = 0;
            else
                Keys = e.KeyData;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}

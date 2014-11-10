using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Services.CommandManager
{
    internal class NiCommandBarTextBox : NiCommandBarControl, INiCommandBarTextBox
    {
        private NiCommandBarTextBoxStyle _style;
        private string _value;

        public NiCommandBarTextBoxStyle Style
        {
            get { return _style; }
            set
            {
                if (_style != value)
                {
                    _style = value;
                    OnAppearanceChanged(EventArgs.Empty);
                }
            }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnAppearanceChanged(EventArgs.Empty);
                }
            }
        }

        public NiCommandBarTextBox(Guid id, int priority)
            : base(id, priority)
        {
        }
    }
}

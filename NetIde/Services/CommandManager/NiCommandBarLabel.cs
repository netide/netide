using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Services.CommandManager
{
    internal class NiCommandBarLabel : NiCommandBarControl, INiCommandBarLabel
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnAppearanceChanged(EventArgs.Empty);
                }
            }
        }
        
        public NiCommandBarLabel(Guid id, int priority)
            : base(id, priority)
        {
        }
    }
}

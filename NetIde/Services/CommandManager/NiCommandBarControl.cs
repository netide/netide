using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.CommandManager
{
    internal abstract class NiCommandBarControl : ServiceObject, INiCommandBarControl
    {
        private string _text;
        private bool _isVisible;
        private bool _isEnabled;
        private string _toolTip;

        public Guid Id { get; private set; }

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

        public string ToolTip
        {
            get { return _toolTip; }
            set
            {
                if (_toolTip != value)
                {
                    _toolTip = value;
                    OnAppearanceChanged(EventArgs.Empty);
                }
            }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    OnAppearanceChanged(EventArgs.Empty);
                }
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnAppearanceChanged(EventArgs.Empty);
                }
            }
        }

        public int Priority { get; private set; }

        internal event EventHandler AppearanceChanged;

        protected virtual void OnAppearanceChanged(EventArgs e)
        {
            var ev = AppearanceChanged;
            if (ev != null)
                ev(this, e);
        }

        protected NiCommandBarControl(Guid id, int priority)
        {
            Id = id;
            Priority = priority;
            IsVisible = true;
            IsEnabled = true;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}

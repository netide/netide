using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Services.CommandManager
{
    internal class NiCommandBarComboBox : NiCommandBarControl, INiCommandBarComboBox
    {
        private NiCommandComboBoxStyle _style;
        private string _selectedValue;

        public Guid FillCommand { get; private set; }

        public NiCommandComboBoxStyle Style
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

        public string[] Values { get; set; }

        public string SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                if (_selectedValue != value)
                {
                    _selectedValue = value;
                    OnAppearanceChanged(EventArgs.Empty);
                }
            }
        }

        public NiCommandBarComboBox(Guid id, Guid fillCommand, int priority)
            : base(id, priority)
        {
            FillCommand = fillCommand;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Core.Settings;
using NetIde.Shell.Settings;

namespace NetIde.Core.OptionPages.Environment
{
    internal partial class FontOptionControl : OptionPageControl
    {
        private IFontSettings _settings;

        public FontOptionControl()
        {
            InitializeComponent();
        }

        private void FontsControl_Load(object sender, EventArgs e)
        {
            _settings = SettingsBuilder.GetSettings<IFontSettings>(Site);

            InitializeFontControl(_font, _settings.CodeFont ?? Constants.DefaultCodeFont);
        }

        private void FontsControl_Apply(object sender, EventArgs e)
        {
            _settings.CodeFont = (Font)_font.Tag;
        }
    }
}

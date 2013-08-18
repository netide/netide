using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System;
using System.Windows.Forms;

namespace NetIde.Services.CommandManager.Controls
{
    internal interface IBarControl : IDisposable
    {
        object Tag { get; set; }
        string Text { get; set; }
        bool Visible { get; set; }
        bool Enabled { get; set; }
        Image Image { get; set; }
        ToolStripItemCollection Items { get; }

        event EventHandler DropDownOpening;

        ControlControl CreateButton(IServiceProvider serviceProvider, NiCommandBarButton button);
        ControlControl CreateComboBox(IServiceProvider serviceProvider, NiCommandBarComboBox comboBox);
        ControlControl CreatePopup(IServiceProvider serviceProvider, NiCommandBarPopup popup);
    }
}

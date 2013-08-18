using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiCommandManager : INiCommandTarget
    {
        HResult CreateCommandBar(Guid id, NiCommandBarKind kind, int priority, out INiCommandBar commandBar);
        HResult CreateCommandBarGroup(Guid id, int priority, out INiCommandBarGroup group);
        HResult CreateCommandBarButton(Guid id, int priority, out INiCommandBarButton button);
        HResult CreateCommandBarComboBox(Guid id, Guid fillCommand, int priority, out INiCommandBarComboBox comboBox);
        HResult CreateCommandBarPopup(Guid id, int priority, out INiCommandBarPopup popup);
        HResult RegisterCommandTarget(INiCommandTarget commandTarget, out int cookie);
        HResult UnregisterCommandTarget(int cookie);
        HResult FindCommandBar(Guid id, out INiCommandBar commandBar);
        HResult FindCommandBarGroup(Guid id, out INiCommandBarGroup group);
        HResult FindCommandBarControl(Guid id, out INiCommandBarControl command);
        HResult FindCommandBarPopup(Guid id, out INiCommandBarPopup popup);
    }
}

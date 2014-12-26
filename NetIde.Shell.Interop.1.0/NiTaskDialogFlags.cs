using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    [Flags]
    public enum NiTaskDialogFlags
    {
        None = 0,
        EnableHyperlinks = 1 << 0,
        AllowDialogCancellation = 1 << 1,
        UseCommandLinks = 1 << 2,
        UseCommandLinksNoIcon = 1 << 3,
        ExpandFooterArea = 1 << 4,
        ExpandedByDefault = 1 << 5,
        VerificationFlagChecked = 1 << 6,
        ShowProgressBar = 1 << 7,
        ShowMarqueeProgressBar = 1 << 8,
        CallbackTimer = 1 << 9,
        PositionRelativeToWindow = 1 << 10,
        RightToLeftLayout = 1 << 11,
        NoDefaultRadioButton = 1 << 12,
        CanBeMinimized = 1 << 13
    }
}

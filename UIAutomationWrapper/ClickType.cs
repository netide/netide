using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIAutomationWrapper
{
    [Flags]
    public enum ClickType
    {
        Left = 1,
        Middle = 2,
        Right = 3,
        ButtonMask = 3,
        DoubleClick = 4,
        Control = 8,
        Alt = 16,
        Shift = 32
    }
}

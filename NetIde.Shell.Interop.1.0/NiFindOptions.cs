using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    [Flags]
    public enum NiFindOptions
    {
        OptionsMask =       0xff,
        MatchCase =         0x01,
        WholeWord =         0x02,
        Backwards =         0x04,
        Selection =         0x08,
        KeepCase =          0x10,
        SubFolders =        0x20,
        KeepOpen =          0x40,
        SyntaxMark =        0xf00,
        Plain =             0x100,
        RegExp =            0x200,
        TargetMask =        0xf000,
        Document =          0x1000,
        OpenDocument =      0x2000,
        Files =             0x4000,
        Project =           0x8000,
        ActionMask =        0xf0000,
        Find =              0x10000,
        FindAll =           0x20000,
        Replace =           0x40000,
        ReplaceAll =        0x80000
    }
}

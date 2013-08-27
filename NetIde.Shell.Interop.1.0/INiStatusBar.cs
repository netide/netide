using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiStatusBar
    {
        HResult Clear();
        HResult GetText(out string text);
        HResult IsCurrentUser(INiStatusBarUser user, out bool isCurrent);
        HResult CreateProgress(out INiStatusBarProgress progress);
        HResult SetInsertMode(NiInsertMode mode);
        HResult HideLineColChar();
        HResult SetLineChar(int line, int charIndex);
        HResult SetLineColChar(int line, int index, int charIndex);
        HResult SetText(string text);
    }
}

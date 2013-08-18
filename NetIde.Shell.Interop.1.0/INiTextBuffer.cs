using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiTextBuffer
    {
        HResult InitializeContent(string text);
        HResult GetStateFlags(out NiTextBufferState flags);
        HResult SetStateFlags(NiTextBufferState flags);
        HResult GetPositionOfLine(int line, out int position);
        HResult GetPositionOfLineIndex(int line, int index, out int position);
        HResult GetLineIndexOfPosition(int position, out int line, out int column);
        HResult GetLengthOfLine(int line, out int length);
        HResult GetLineCount(out int lineCount);
        HResult GetSize(out int length);
        HResult GetLanguageServiceID(out Guid languageService);
        HResult SetLanguageServiceID(Guid languageService);
        HResult GetLastLineIndex(out int line, out int index);
    }
}

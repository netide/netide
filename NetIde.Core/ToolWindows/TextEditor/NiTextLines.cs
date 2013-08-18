using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Core.ToolWindows.TextEditor
{
    [Guid(NiConstants.TextLines)]
    [ProvideObject(typeof(NiTextLines))]
    internal class NiTextLines : NiTextBuffer, INiTextLines
    {
        public HResult GetLineText(int startLine, int startIndex, int endLine, int endIndex, out string result)
        {
            result = null;

            try
            {
                var range = TranslateOffset(startLine, startIndex, endLine, endIndex);

                result = Document.GetText(range.Offset, range.Length);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult ReplaceLines(int startLine, int startIndex, int endLine, int endIndex, string text)
        {
            try
            {
                var range = TranslateOffset(startLine, startIndex, endLine, endIndex);

                bool readOnly = Document.ReadOnly;

                OnBeginUpdate(EventArgs.Empty);
                Document.ReadOnly = false;

                try
                {
                    Document.Replace(range.Offset, range.Length, text);
                }
                finally
                {
                    Document.ReadOnly = readOnly;
                    OnEndUpdate(EventArgs.Empty);
                }

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private Range TranslateOffset(int startLine, int startIndex, int endLine, int endIndex)
        {
            if (startLine < 0 || startLine >= Document.TotalNumberOfLines)
                throw new ArgumentOutOfRangeException("startLine");
            if (endLine < startLine || endLine >= Document.TotalNumberOfLines)
                throw new ArgumentOutOfRangeException("endLine");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex");
            if (endIndex < 0 || (startLine == endLine && endIndex < startIndex))
                throw new ArgumentOutOfRangeException("endIndex");

            var startSegment = Document.GetLineSegment(startLine);
            var endSegment = Document.GetLineSegment(endLine);

            if (startIndex > startSegment.Length)
                throw new ArgumentOutOfRangeException("startIndex");
            if (endIndex > endSegment.Length)
                throw new ArgumentOutOfRangeException("endIndex");

            int absoluteStart = startSegment.Offset + startIndex;
            int absoluteEnd = endSegment.Offset + endIndex;

            return new Range(absoluteStart, absoluteEnd - absoluteStart);
        }

        private class Range
        {
            public int Offset { get; private set; }
            public int Length { get; private set; }

            public Range(int offset, int length)
            {
                Offset = offset;
                Length = length;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ICSharpCode.TextEditor.Document;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util;

namespace NetIde.Core.ToolWindows.TextEditor
{
    [Guid(NiConstants.TextLines)]
    [ProvideObject(typeof(NiTextLines))]
    internal class NiTextLines : NiTextBuffer, INiTextLines
    {
        private readonly NiConnectionPoint<INiTextLinesEvents> _connectionPoint = new NiConnectionPoint<INiTextLinesEvents>();

        public NiTextLines()
        {
            Document.DocumentChanged += Document_DocumentChanged;
        }

        void Document_DocumentChanged(object sender, DocumentEventArgs e)
        {
            int startOffset = e.Offset;
            int endOffset = e.Offset + e.Length;

            var segment = Document.GetLineSegmentForOffset(startOffset);

            int startLine = segment.LineNumber;
            int startIndex = segment.Offset - startOffset;

            segment = Document.GetLineSegmentForOffset(endOffset);

            int endLine = segment.LineNumber;
            int endIndex = segment.Offset - endOffset;

            _connectionPoint.ForAll(p => p.OnChanged(startLine, startIndex, endLine, endIndex));
        }

        public HResult Advise(object sink, out int cookie)
        {
            return _connectionPoint.Advise(sink, out cookie);
        }

        public HResult Advise(INiTextLinesEvents sink, out int cookie)
        {
            return Advise((object)sink, out cookie);
        }

        public HResult Unadvise(int cookie)
        {
            return _connectionPoint.Unadvise(cookie);
        }

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

            if (startIndex > startSegment.Length + 1)
                throw new ArgumentOutOfRangeException("startIndex");
            if (endIndex > endSegment.Length + 1)
                throw new ArgumentOutOfRangeException("endIndex");

            int absoluteStart = startSegment.Offset + startIndex;
            int absoluteEnd = endSegment.Offset + endIndex;

            return new Range(absoluteStart, absoluteEnd - absoluteStart);
        }

        public HResult CreateTextMarker(NiTextMarkerType type, NiTextMarkerHatchStyle hatchStyle, bool extendToBorder, int color, int foreColor, int startLine, int startIndex, int endLine, int endIndex, out INiTextMarker textMarker)
        {
            textMarker = null;

            try
            {
                var range = TranslateOffset(startLine, startIndex, endLine, endIndex);
                var nativeType = GetTextMarkerType(type, extendToBorder);
                var drawingColor = Color.FromArgb(color);
                var drawingForeColor = Color.FromArgb(foreColor);
                
                TextMarker nativeMarker;

                // The TextMarker interface is crap. We cannot simply call
                // the constructor with all the arguments, because calling
                // the constructor determines whether a property is available.

                if (hatchStyle != NiTextMarkerHatchStyle.Default)
                {
                    nativeMarker = new TextMarker(
                        range.Offset,
                        range.Length,
                        nativeType,
                        drawingColor,
                        drawingForeColor,
                        Enum<HatchStyle>.Parse(hatchStyle.ToString())
                    );
                }
                else if (drawingForeColor.A != 0)
                {
                    nativeMarker = new TextMarker(
                        range.Offset,
                        range.Length,
                        nativeType,
                        drawingColor,
                        drawingForeColor
                    );
                }
                else if (drawingColor.A != 0)
                {
                    nativeMarker = new TextMarker(
                        range.Offset,
                        range.Length,
                        nativeType,
                        drawingColor
                    );
                }
                else
                {
                    nativeMarker = new TextMarker(
                        range.Offset,
                        range.Length,
                        nativeType
                    );
                }

                OnBeginUpdate(EventArgs.Empty);

                try
                {
                    Document.MarkerStrategy.AddMarker(nativeMarker);
                }
                finally
                {
                    OnEndUpdate(EventArgs.Empty);
                }

                textMarker = new NiTextMarker(this, nativeMarker);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private TextMarkerType GetTextMarkerType(NiTextMarkerType type, bool extendToBorder)
        {
            TextMarkerType result;

            switch (type)
            {
                case NiTextMarkerType.Invisible: result = TextMarkerType.Invisible; break;
                case NiTextMarkerType.SolidBlock: result = TextMarkerType.SolidBlock; break;
                case NiTextMarkerType.Underline: result = TextMarkerType.Underlined; break;
                case NiTextMarkerType.WaveLine: result = TextMarkerType.WaveLine; break;
                default: throw new ArgumentOutOfRangeException("type");
            }

            if (extendToBorder)
                result |= TextMarkerType.ExtendToBorder;

            return result;
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

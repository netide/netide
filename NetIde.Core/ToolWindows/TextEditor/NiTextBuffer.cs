using System.Runtime.InteropServices;
using ICSharpCode.TextEditor.Document;
using NetIde.Shell;
using NetIde.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Core.ToolWindows.TextEditor
{
    [Guid(NiConstants.TextBuffer)]
    [ProvideObject(typeof(NiTextBuffer))]
    internal partial class NiTextBuffer : ServiceObject, INiTextBuffer
    {
        private bool _dirty;
        private Guid _languageService;

        public IDocument Document { get; private set; }

        public event EventHandler BeginUpdate;

        protected virtual void OnBeginUpdate(EventArgs e)
        {
            var ev = BeginUpdate;
            if (ev != null)
                ev(this, e);
        }

        public event EventHandler EndUpdate;

        protected virtual void OnEndUpdate(EventArgs e)
        {
            var ev = EndUpdate;
            if (ev != null)
                ev(this, e);
        }

        public NiTextBuffer()
            : this(new DocumentFactory().CreateDocument())
        {
        }

        public NiTextBuffer(IDocument document)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            _languageService = new Guid(NiConstants.LanguageServiceDefault);

            Document = document;
            Document.DocumentChanged += (s, e) => _dirty = true;
        }

        public HResult InitializeContent(string text)
        {
            try
            {
                OnBeginUpdate(EventArgs.Empty);

                try
                {
                    Document.TextContent = text;
                }
                finally
                {
                    OnEndUpdate(EventArgs.Empty);
                }

                _dirty = false;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetStateFlags(out NiTextBufferState flags)
        {
            flags = 0;

            try
            {
                flags =
                    (_dirty ? NiTextBufferState.Dirty : 0) |
                    (Document.ReadOnly ? NiTextBufferState.ReadOnly : 0);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult SetStateFlags(NiTextBufferState flags)
        {
            try
            {
                Document.ReadOnly = (flags & NiTextBufferState.ReadOnly) != 0;
                _dirty = (flags & NiTextBufferState.Dirty) != 0;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetPositionOfLine(int line, out int position)
        {
            position = 0;

            try
            {
                if (line < 0 || line >= Document.TotalNumberOfLines)
                    throw new ArgumentOutOfRangeException("line");

                position = Document.GetLineSegment(line).Offset;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetPositionOfLineIndex(int line, int index, out int position)
        {
            position = 0;

            try
            {
                if (line < 0 || line >= Document.TotalNumberOfLines)
                    throw new ArgumentOutOfRangeException("line");
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index");

                var segment = Document.GetLineSegment(line);

                if (index >= segment.Length)
                    throw new ArgumentOutOfRangeException("index");

                position = segment.Offset + index;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetLineIndexOfPosition(int position, out int line, out int column)
        {
            line = 0;
            column = 0;

            try
            {
                if (position < 0 || position >= Document.TextLength)
                    throw new ArgumentOutOfRangeException("position");

                var segment = Document.GetLineSegmentForOffset(position);

                line = segment.LineNumber;
                column = position - segment.Offset;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetLengthOfLine(int line, out int length)
        {
            length = 0;

            try
            {
                if (line < 0 || line >= Document.TotalNumberOfLines)
                    throw new ArgumentOutOfRangeException("line");

                var segment = Document.GetLineSegment(line);

                length = segment.Length;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetLineCount(out int lineCount)
        {
            lineCount = 0;

            try
            {
                lineCount = Document.TotalNumberOfLines;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetSize(out int length)
        {
            length = 0;

            try
            {
                length = Document.TextLength;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetLanguageServiceID(out Guid languageService)
        {
            languageService = _languageService;

            return HResult.OK;
        }

        public HResult SetLanguageServiceID(Guid languageService)
        {
            try
            {
                if (LanguageServiceMapper.GetHighlighterFromLanguageService(languageService) == null)
                    throw new ArgumentOutOfRangeException("languageService");

                _languageService = languageService;

                string highlighter = LanguageServiceMapper.GetHighlighterFromLanguageService(languageService);

                Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter(highlighter);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetLastLineIndex(out int line, out int index)
        {
            line = 0;
            index = 0;

            try
            {
                var segment = Document.GetLineSegment(Document.TotalNumberOfLines - 1);

                line = segment.LineNumber;
                index = segment.Length;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
    }
}

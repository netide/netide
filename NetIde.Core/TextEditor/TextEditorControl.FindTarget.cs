using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using NetIde.Core.ToolWindows.TextEditor;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Core.TextEditor
{
    partial class TextEditorControl
    {
        private class FindTargetImpl : ServiceObject, INiFindTarget
        {
            private object _findState;
            private readonly TextAreaControl _textAreaControl;
            private INiTextBuffer _textBufferShim;

            public FindTargetImpl(TextAreaControl textAreaControl)
            {
                _textAreaControl = textAreaControl;
            }

            public HResult GetTextBuffer(out INiTextBuffer textBuffer)
            {
                textBuffer = null;

                try
                {
                    if (_textBufferShim == null)
                        _textBufferShim = new NiTextLines(_textAreaControl.Document);

                    textBuffer = _textBufferShim;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult Find(string search, NiFindOptions options, bool resetStartPoint, INiFindHelper helper, out NiFindResult result)
            {
                return Replace(search, null, options, resetStartPoint, helper, out result);
            }

            public HResult Replace(string search, string replace, NiFindOptions options, bool resetStartPoint, INiFindHelper helper, out NiFindResult result)
            {
                result = NiFindResult.NotFound;

                try
                {
                    if (search == null)
                        throw new ArgumentNullException("search");
                    if (helper == null)
                        throw new ArgumentNullException("helper");

                    var document = _textAreaControl.Document;
                    string text = document.TextContent;

                    int offset;

                    if (options.HasFlag(NiFindOptions.Backwards))
                    {
                        if (resetStartPoint)
                            offset = text.Length;
                        else
                            offset = _textAreaControl.Caret.Offset;
                    }
                    else
                    {
                        if (resetStartPoint)
                            offset = 0;
                        else if (_textAreaControl.SelectionManager.HasSomethingSelected)
                            offset = _textAreaControl.SelectionManager.SelectionCollection.Last().EndOffset;
                        else
                            offset = _textAreaControl.Caret.Offset;
                    }

                    int matchLength;
                    string replacement;
                    bool found;
                    ErrorUtil.ThrowOnFailure(helper.FindInText(
                        search,
                        replace,
                        options,
                        text,
                        offset,
                        out offset,
                        out matchLength,
                        out replacement,
                        out found
                    ));

                    if (found)
                    {
                        MarkSpan(GetTextSpan(document, offset, matchLength));
                        result = replace == null ? NiFindResult.Found : NiFindResult.Replaced;
                    }
                    else
                    {
                        result = NiFindResult.NotFound;
                    }

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            private NiTextSpan GetTextSpan(IDocument document, int offset, int length)
            {
                var start = document.OffsetToPosition(offset);
                var end = document.OffsetToPosition(offset + length);

                return new NiTextSpan(start.Line, start.Column, end.Line, end.Column);
            }

            public HResult GetCurrentSpan(out NiTextSpan span)
            {
                span = new NiTextSpan();

                try
                {
                    var caretPosition = _textAreaControl.Caret.Position;

                    span = new NiTextSpan(caretPosition.Line, caretPosition.Column, caretPosition.Line, caretPosition.Column);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetFindState(out object state)
            {
                state = _findState;
                return HResult.OK;
            }

            public HResult SetFindState(object state)
            {
                _findState = state;
                return HResult.OK;
            }

            public HResult MarkSpan(NiTextSpan span)
            {
                try
                {
                    var selectionManager = _textAreaControl.SelectionManager;

                    var start = new TextLocation(span.StartIndex, span.StartLine);

                    _textAreaControl.Caret.Position = start;

                    selectionManager.SetSelection(
                        start,
                        new TextLocation(span.EndIndex, span.EndLine)
                    );

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult NavigateTo(NiTextSpan span)
            {
                try
                {
                    _textAreaControl.Caret.Position = new TextLocation(
                        span.StartIndex,
                        span.StartLine
                    );

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult NotifyFindTarget(NiFindTargetNotify notify)
            {
                return HResult.OK;
            }

            public HResult GetCapabilities(out NiFindCapabilities options)
            {
                options = NiFindCapabilities.All;
                return HResult.OK;
            }

            public HResult GetInitialPattern(out string pattern)
            {
                pattern = null;

                try
                {
                    var selectionManager = _textAreaControl.SelectionManager;

                    if (selectionManager.HasSomethingSelected)
                        pattern = selectionManager.SelectedText;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }
        }
    }
}

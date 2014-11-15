using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;

namespace NetIde.Core.ToolWindows.FindResults
{
    [Guid("ec2b6efd-fd81-4334-9165-84f2047badbd")]
    internal class FindResultsWindow : NiWindowPane
    {
        private Listener _listener;
        private bool _disposed;
        private NiEditor _editor;
        private List<FindResult> _results;

        public override HResult Initialize()
        {
            try
            {
                var hr = base.Initialize();

                if (ErrorUtil.Failure(hr))
                    return hr;

                Frame.Caption = Labels.FindResults;

                _editor = new NiEditor
                {
                    Site = new SiteProxy(this),
                    Dock = DockStyle.Fill
                };

                ErrorUtil.ThrowOnFailure(_editor.TextBuffer.SetStateFlags(
                    NiTextBufferState.ReadOnly
                ));
                ErrorUtil.ThrowOnFailure(_editor.TextBuffer.SetLanguageServiceID(
                    new Guid(NiConstants.LanguageServiceDefault)
                ));

                _listener = new Listener(this);

                Controls.Add(_editor);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public void ResetResults()
        {
            _results = new List<FindResult>();
            _editor.TextBuffer.InitializeContent(String.Empty);
        }

        public void AddResult(FindResult result, int line, string text)
        {
            if (result == null)
                throw new ArgumentNullException("result");
            if (text == null)
                throw new ArgumentNullException("text");

            int outputLine = _results.Count;

            _results.Add(result);

            string append = String.Format(
                "{0}({1}): {2}" + Environment.NewLine,
                result.FileName,
                line + 1,
                text
            );

            _editor.TextBuffer.ReplaceLines(
                outputLine,
                0,
                outputLine,
                0,
                append
            );
        }

        private void ProcessDoubleClick()
        {
            int line;
            int index;
            ErrorUtil.ThrowOnFailure(_editor.Window.GetCaretPosition(out line, out index));

            if (line < 0 || line >= _results.Count)
                return;

            var result = _results[line];

            INiHierarchy hier;
            INiWindowFrame windowFrame;

            ErrorUtil.ThrowOnFailure(((INiOpenDocumentManager)GetService(typeof(INiOpenDocumentManager))).IsDocumentOpen(
                result.FileName,
                out hier,
                out windowFrame
            ));

            // First check whether this is part of the project.

            if (windowFrame == null)
            {
                var activeProject = ((INiProjectManager)GetService(typeof(INiProjectManager))).ActiveProject;

                if (activeProject != null)
                {
                    hier = activeProject.FindByDocument(result.FileName);

                    if (hier != null)
                        ErrorUtil.ThrowOnFailure(activeProject.OpenItem(hier, out windowFrame));
                }
            }

            // Else, we're opening a document from outside of the project.

            if (windowFrame == null)
            {
                ErrorUtil.ThrowOnFailure(((INiOpenDocumentManager)GetService(typeof(INiOpenDocumentManager))).OpenStandardEditor(
                    null,
                    result.FileName,
                    null,
                    this,
                    out windowFrame
                ));
            }

            if (windowFrame == null)
                return;

            ErrorUtil.ThrowOnFailure(windowFrame.Show());

            var docView = windowFrame.GetPropertyEx(NiFrameProperty.DocView);

            var findTarget = docView as INiFindTarget;
            if (findTarget == null)
                return;

            INiTextBuffer textBuffer;
            ErrorUtil.ThrowOnFailure(findTarget.GetTextBuffer(out textBuffer));

            if (textBuffer == null)
                return;

            int startLine;
            int startIndex;
            int endLine;
            int endIndex;

            ErrorUtil.ThrowOnFailure(textBuffer.GetLineIndexOfPosition(
                result.Offset, out startLine, out startIndex
            ));
            ErrorUtil.ThrowOnFailure(textBuffer.GetLineIndexOfPosition(
                result.Offset + result.Length, out endLine, out endIndex
            ));

            ErrorUtil.ThrowOnFailure(findTarget.MarkSpan(
                new NiTextSpan(startLine, startIndex, endLine, endIndex)
            ));
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_listener != null)
                {
                    _listener.Dispose();
                    _listener = null;
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        private class Listener : NiEventSink, INiMouseEventNotify
        {
            private readonly FindResultsWindow _window;

            public Listener(FindResultsWindow window)
                : base((INiConnectionPoint)window._editor.Window)
            {
                _window = window;
            }

            public void OnMouseDown(MouseButtons button, int x, int y)
            {
            }

            public void OnMouseUp(MouseButtons button, int x, int y)
            {
            }

            public void OnMouseClick(MouseButtons button, int x, int y)
            {
            }

            public void OnMouseDoubleClick(MouseButtons button, int x, int y)
            {
                if (button == MouseButtons.Left)
                    _window.ProcessDoubleClick();
            }

            public void OnMouseWheel(int delta)
            {
            }
        }
    }
}

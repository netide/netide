using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiEditor : NiWindowHost<INiCodeWindow>
    {
        private string _text;
        private INiTextLines _textBuffer;
        private bool _disposed;

        public INiTextLines TextBuffer
        {
            get
            {
                if (_textBuffer == null)
                    CreateHandle();

                return _textBuffer;
            }
        }

        public override string Text
        {
            get
            {
                if (_textBuffer == null)
                    return _text;
                else
                    return GetText();
            }
            set
            {
                if (_textBuffer == null)
                    _text = value ?? String.Empty;
                else
                    SetText(value);
            }
        }

        public new NiTextChangedEventHandler TextChanged;
        private Listener _listener;

        public virtual void OnTextChanged(NiTextChangedEventArgs e)
        {
            var ev = TextChanged;
            if (ev != null)
                ev(this, e);
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

        protected override INiCodeWindow CreateWindow()
        {
            var manager = (INiOpenDocumentManager)GetService(typeof(INiOpenDocumentManager));
            var registry = (INiLocalRegistry)GetService(typeof(INiLocalRegistry));

            INiEditorFactory editorFactory;
            ErrorUtil.ThrowOnFailure(manager.GetStandardEditorFactory(new Guid(NiConstants.TextEditor), null, out editorFactory));

            string unused;
            INiWindowPane windowPane;
            ErrorUtil.ThrowOnFailure(editorFactory.CreateEditor(null, null, out unused, out windowPane));

            var window = (INiCodeWindow)windowPane;

            ErrorUtil.ThrowOnFailure(window.Initialize());

            object instance;
            ErrorUtil.ThrowOnFailure(registry.CreateInstance(new Guid(NiConstants.TextLines), windowPane, out instance));

            _textBuffer = (INiTextLines)instance;

            ErrorUtil.ThrowOnFailure(window.SetBuffer(TextBuffer));

            if (!String.IsNullOrEmpty(_text))
            {
                SetText(_text);
                _text = null;
            }

            _listener = new Listener(this);

            return window;
        }

        private string GetText()
        {
            int line;
            int index;
            ErrorUtil.ThrowOnFailure(TextBuffer.GetLastLineIndex(out line, out index));

            string result;
            ErrorUtil.ThrowOnFailure(TextBuffer.GetLineText(0, 0, line, index, out result));

            return result;
        }

        private void SetText(string value)
        {
            int line;
            int index;
            ErrorUtil.ThrowOnFailure(TextBuffer.GetLastLineIndex(out line, out index));

            ErrorUtil.ThrowOnFailure(TextBuffer.ReplaceLines(0, 0, line, index, value ?? String.Empty));
        }

        private class Listener : NiEventSink, INiTextLinesEvents
        {
            private readonly NiEditor _owner;

            public Listener(NiEditor owner)
                : base(owner._textBuffer)
            {
                _owner = owner;
            }

            public void OnChanged(int startLine, int startIndex, int endLine, int endIndex)
            {
                _owner.OnTextChanged(new NiTextChangedEventArgs(startLine, startIndex, endLine, endIndex));
            }
        }
    }
}

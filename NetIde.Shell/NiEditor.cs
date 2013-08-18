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

        public INiTextLines TextBuffer
        {
            get
            {
                if (_textBuffer == null)
                    CreateHandle();

                return _textBuffer;
            }
        }

        [DefaultValue(true)]
        public override bool AcceptsArrows
        {
            get { return base.AcceptsArrows; }
            set { base.AcceptsArrows = value; }
        }

        [DefaultValue(true)]
        public override bool AcceptsTab
        {
            get { return base.AcceptsTab; }
            set { base.AcceptsTab = value; }
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

        public NiEditor()
        {
            AcceptsArrows = true;
            AcceptsTab = true;
        }

        protected override INiCodeWindow CreateWindow()
        {
            var registry = ((INiLocalRegistry)GetService(typeof(INiLocalRegistry)));

            object instance;
            ErrorUtil.ThrowOnFailure(registry.CreateInstance(new Guid(NiConstants.TextEditor), out instance));

            var window = (INiCodeWindow)instance;

            ErrorUtil.ThrowOnFailure(window.Initialize());

            ErrorUtil.ThrowOnFailure(registry.CreateInstance(new Guid(NiConstants.TextLines), out instance));

            _textBuffer = (INiTextLines)instance;

            ErrorUtil.ThrowOnFailure(window.SetBuffer(TextBuffer));

            if (!String.IsNullOrEmpty(_text))
            {
                SetText(_text);
                _text = null;
            }

            return window;
        }
    }
}

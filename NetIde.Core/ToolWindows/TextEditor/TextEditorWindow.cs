using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using NetIde.Core.Settings;
using NetIde.Core.Support;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Shell.Settings;

namespace NetIde.Core.ToolWindows.TextEditor
{
    [Guid("9e5f682c-1308-4d12-9136-05b820a78989")]
    internal class TextEditorWindow : EditorWindow, INiCodeWindow
    {
        private NiTextLines _textLines;

        protected TextEditorControl Control
        {
            get { return (TextEditorControl)Window; }
        }

        protected override Control CreateControl()
        {
            var fontSettings = SettingsBuilder.GetSettings<IFontSettings>(this);

            return new TextEditorControl
            {
                Font = fontSettings.CodeFont ?? Constants.DefaultCodeFont,
                ConvertTabsToSpaces = true
            };
        }

        public HResult GetBuffer(out INiTextLines textBuffer)
        {
            textBuffer = _textLines;

            return HResult.OK;
        }

        public HResult SetBuffer(INiTextLines textBuffer)
        {
            try
            {
                if (textBuffer == null)
                    throw new ArgumentNullException("textBuffer");

                if (_textLines != null)
                {
                    _textLines.BeginUpdate -= _textLines_BeginUpdate;
                    _textLines.EndUpdate -= _textLines_EndUpdate;
                }

                _textLines = (NiTextLines)textBuffer;

                if (_textLines != null)
                {
                    Control.Document = _textLines.Document;

                    _textLines.BeginUpdate += _textLines_BeginUpdate;
                    _textLines.EndUpdate += _textLines_EndUpdate;
                }

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        void _textLines_BeginUpdate(object sender, EventArgs e)
        {
            Control.ActiveTextAreaControl.TextArea.BeginUpdate();
        }

        void _textLines_EndUpdate(object sender, EventArgs e)
        {
            var textArea = Control.ActiveTextAreaControl.TextArea;

            textArea.Caret.ValidateCaretPos();
            textArea.EndUpdate();
            textArea.Refresh();
        }
    }
}

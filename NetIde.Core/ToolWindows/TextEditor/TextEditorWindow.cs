using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using NetIde.Core.Settings;
using NetIde.Core.Support;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Shell.Settings;

namespace NetIde.Core.ToolWindows.TextEditor
{
    [Guid("9e5f682c-1308-4d12-9136-05b820a78989")]
    internal partial class TextEditorWindow : EditorWindow, INiCodeWindow
    {
        private readonly NiCommandMapper _commandMapper = new NiCommandMapper();
        private NiTextLines _textLines;

        protected TextEditorControl Control
        {
            get { return (TextEditorControl)Window; }
        }

        protected SelectionManager SelectionManager
        {
            get { return Control.ActiveTextAreaControl.SelectionManager; }
        }

        public TextEditorWindow()
        {
            BuildCommands();
        }

        protected override Control CreateControl()
        {
            var fontSettings = SettingsBuilder.GetSettings<IFontSettings>(this);

            var control = new TextEditorControl
            {
                Font = fontSettings.CodeFont ?? Constants.DefaultCodeFont,
                ConvertTabsToSpaces = true
            };

            control.ActiveTextAreaControl.Caret.CaretModeChanged += (s, e) => UpdateCaretMode();
            control.ActiveTextAreaControl.Caret.PositionChanged += (s, e) => UpdateCaretPosition();

            return control;
        }

        private void UpdateCaretMode()
        {
            var statusBar = (INiStatusBar)GetService(typeof(INiStatusBar));

            ErrorUtil.ThrowOnFailure(statusBar.SetInsertMode(
                Control.ActiveTextAreaControl.Caret.CaretMode == CaretMode.InsertMode
                ? NiInsertMode.Insert
                : NiInsertMode.Overwrite
            ));
        }

        private void UpdateCaretPosition()
        {
            var statusBar = (INiStatusBar)GetService(typeof(INiStatusBar));

            ErrorUtil.ThrowOnFailure(statusBar.SetLineChar(
                Control.ActiveTextAreaControl.Caret.Line + 1,
                Control.ActiveTextAreaControl.Caret.Column + 1
            ));
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

        public HResult QueryStatus(Guid command, out NiCommandStatus status)
        {
            return _commandMapper.QueryStatus(command, out status);
        }

        public HResult Exec(Guid command, object argument, out object result)
        {
            return _commandMapper.Exec(command, argument, out result);
        }

        public HResult GetCaretPosition(out int line, out int index)
        {
            line = -1;
            index = -1;

            try
            {
                var position = Control.ActiveTextAreaControl.Caret.Position;

                line = position.Line;
                index = position.Column;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult SetCaretPosition(int line, int index)
        {
            try
            {
                Control.ActiveTextAreaControl.Caret.Position = new TextLocation(index, line);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetSelectionCount(out int count)
        {
            count = 0;

            try
            {
                count = SelectionManager.SelectionCollection.Count;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetSelection(int index, out int startLine, out int startIndex, out int endLine, out int endIndex)
        {
            startLine = -1;
            startIndex = -1;
            endLine = -1;
            endIndex = -1;

            try
            {
                var selection = SelectionManager.SelectionCollection;

                if (index < 0 || index >= selection.Count)
                    throw new ArgumentOutOfRangeException("index");

                var selectionItem = selection[index];

                startLine = selectionItem.StartPosition.Line;
                startIndex = selectionItem.StartPosition.Column;
                endLine = selectionItem.EndPosition.Line;
                endIndex = selectionItem.EndPosition.Column;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult SetSelection(int startLine, int startIndex, int endLine, int endIndex)
        {
            try
            {
                SelectionManager.SetSelection(
                    new TextLocation(startIndex, startLine),
                    new TextLocation(endIndex, endLine)
                );

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult ClearSelection()
        {
            try
            {
                SelectionManager.ClearSelection();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult SetInfo()
        {
            try
            {
                UpdateCaretMode();
                UpdateCaretPosition();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
    }
}

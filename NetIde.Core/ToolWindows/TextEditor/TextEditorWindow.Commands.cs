using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Core.ToolWindows.TextEditor
{
    partial class TextEditorWindow
    {
        private void BuildCommands()
        {
            _commandMapper.Add(
                Shell.NiResources.Edit_Copy,
                e => Control.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(null, EventArgs.Empty),
                e => e.Status = Control.ActiveTextAreaControl.SelectionManager.HasSomethingSelected ? NiCommandStatus.Enabled : NiCommandStatus.Supported
            );
            _commandMapper.Add(
                Shell.NiResources.Edit_Cut,
                e => Control.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(null, EventArgs.Empty),
                e => e.Status = Control.ActiveTextAreaControl.SelectionManager.HasSomethingSelected ? NiCommandStatus.Enabled : NiCommandStatus.Supported
            );
            _commandMapper.Add(
                Shell.NiResources.Edit_Paste,
                e => Control.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(null, EventArgs.Empty)
            );
            _commandMapper.Add(
                Shell.NiResources.Edit_Undo,
                e => Control.Document.UndoStack.Undo(),
                e => e.Status = Control.Document.UndoStack.CanUndo ? NiCommandStatus.Enabled : NiCommandStatus.Supported
            );
            _commandMapper.Add(
                Shell.NiResources.Edit_Redo,
                e => Control.Document.UndoStack.Redo(),
                e => e.Status = Control.Document.UndoStack.CanRedo ? NiCommandStatus.Enabled : NiCommandStatus.Supported
            );
        }
    }
}

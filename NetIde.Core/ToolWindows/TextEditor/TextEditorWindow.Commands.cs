using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TextEditor.Actions;
using NetIde.Shell.Interop;

namespace NetIde.Core.ToolWindows.TextEditor
{
    partial class TextEditorWindow
    {
        private void BuildCommands()
        {
            _commandMapper.Add(
                Shell.NiResources.Edit_Copy,
                e => new Copy().Execute(Control.ActiveTextAreaControl.TextArea),
                e => e.Status = Control.ActiveTextAreaControl.SelectionManager.HasSomethingSelected ? NiCommandStatus.Enabled : NiCommandStatus.Supported
            );
            _commandMapper.Add(
                Shell.NiResources.Edit_Cut,
                e => new Cut().Execute(Control.ActiveTextAreaControl.TextArea),
                e => e.Status = Control.ActiveTextAreaControl.SelectionManager.HasSomethingSelected ? NiCommandStatus.Enabled : NiCommandStatus.Supported
            );
            _commandMapper.Add(
                Shell.NiResources.Edit_Paste,
                e => new Paste().Execute(Control.ActiveTextAreaControl.TextArea)
            );
            _commandMapper.Add(
                Shell.NiResources.Edit_Undo,
                e => new Undo().Execute(Control.ActiveTextAreaControl.TextArea),
                e => e.Status = Control.Document.UndoStack.CanUndo ? NiCommandStatus.Enabled : NiCommandStatus.Supported
            );
            _commandMapper.Add(
                Shell.NiResources.Edit_Redo,
                e => new Redo().Execute(Control.ActiveTextAreaControl.TextArea),
                e => e.Status = Control.Document.UndoStack.CanRedo ? NiCommandStatus.Enabled : NiCommandStatus.Supported
            );
        }
    }
}

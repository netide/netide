using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Actions;
using NetIde.Core.ToolWindows.TextEditor;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Core.TextEditor
{
    partial class TextEditorControl
    {
        private void BuildCommands()
        {
            CommandMapper.Add(
                NiResources.TextEditor_Copy,
                e => new Copy().Execute(ActiveTextAreaControl.TextArea),
                e => e.Status = ActiveTextAreaControl.SelectionManager.HasSomethingSelected ? NiCommandStatus.Enabled : 0
            );
            CommandMapper.Add(
                NiResources.TextEditor_Cut,
                e => new Cut().Execute(ActiveTextAreaControl.TextArea),
                e => e.Status = !Document.ReadOnly && ActiveTextAreaControl.SelectionManager.HasSomethingSelected ? NiCommandStatus.Enabled : 0
            );
            CommandMapper.Add(
                NiResources.TextEditor_Paste,
                e => new Paste().Execute(ActiveTextAreaControl.TextArea),
                e => e.Status = !Document.ReadOnly ? NiCommandStatus.Enabled : 0
            );
            CommandMapper.Add(
                NiResources.TextEditor_Undo,
                e => new Undo().Execute(ActiveTextAreaControl.TextArea),
                e => e.Status = !Document.ReadOnly && Document.UndoStack.CanUndo ? NiCommandStatus.Enabled : 0
            );
            CommandMapper.Add(
                NiResources.TextEditor_Redo,
                e => new Redo().Execute(ActiveTextAreaControl.TextArea),
                e => e.Status = !Document.ReadOnly && Document.UndoStack.CanRedo ? NiCommandStatus.Enabled : 0
            );
            CommandMapper.Add(
                NiResources.TextEditor_GoToLine,
                e => GoToLine()
            );
            CommandMapper.Add(
                NiResources.TextEditor_Find,
                e => OpenFindWindow(NiFindOptions.Find)
            );
            CommandMapper.Add(
                NiResources.TextEditor_FindAndReplace,
                e => OpenFindWindow(NiFindOptions.Replace),
                e => e.Status = !Document.ReadOnly ? NiCommandStatus.Enabled : 0
            );
        }

        private void OpenFindWindow(NiFindOptions options)
        {
            FindControl.Show(Site, ActiveTextAreaControl.TextArea, options, NiFindOptions.ActionMask, FindTarget);
        }

        private void GoToLine()
        {
            int line = ActiveTextAreaControl.Caret.Line + 1;
            int maxLine = Document.TotalNumberOfLines;

            using (var form = new GoToLineForm(line, maxLine))
            {
                if (form.ShowDialog(Site) == DialogResult.OK)
                    ActiveTextAreaControl.Caret.Line = form.SelectedLine.Value - 1;
            }
        }
    }
}

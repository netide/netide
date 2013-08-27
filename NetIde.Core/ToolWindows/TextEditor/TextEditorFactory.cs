using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Core.ToolWindows.TextEditor
{
    [Guid(NiConstants.TextEditor)]
    internal class TextEditorFactory : NiEditorFactory
    {
        public override HResult CreateEditor(string document, INiHierarchy hier, out string editorCaption, out INiWindowPane editor)
        {
            editor = null;
            editorCaption = null;

            try
            {
                editor = new TextEditorWindow();
                editor.SetSite(this);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
    }
}

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
        public override HResult CreateEditor(string document, out string editorCaption, out INiWindowPane editor)
        {
            editor = null;
            editorCaption = null;

            try
            {
                INiTextLines textLines = null;

                // If we were opened from a real document or hier, set a text buffer.
                // NiOpenDocumentManager will initialize this.

                if (document != null)
                {
                    var registry = (INiLocalRegistry)GetService(typeof(INiLocalRegistry));

                    object instance;
                    var hr = registry.CreateInstance(new Guid(NiConstants.TextLines), this, out instance);
                    if (ErrorUtil.Failure(hr))
                        return hr;

                    textLines = (INiTextLines)instance;

                    Guid languageServiceId;
                    hr = ((INiLanguageServiceRegistry)GetService(typeof(INiLanguageServiceRegistry))).FindForFileName(
                        document, out languageServiceId
                    );
                    if (ErrorUtil.Failure(hr))
                        return hr;

                    hr = textLines.SetLanguageServiceID(languageServiceId);
                    if (ErrorUtil.Failure(hr))
                        return hr;
                }

                editor = new TextEditorWindow(textLines);
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

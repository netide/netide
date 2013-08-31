using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TextEditor.Document;
using NetIde.Core.ToolWindows.TextEditor;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Core
{
    internal sealed class ProvideAllEditorExtensionsAttribute : RegistrationAttribute
    {
        public override void Register(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
            foreach (var entry in HighlightingManager.Manager.HighlightingDefinitions.Values.OfType<DictionaryEntry>())
            {
                var syntaxMode = (SyntaxMode)entry.Key;

                foreach (string extension in syntaxMode.Extensions)
                {
                    var attribute = new ProvideEditorExtensionAttribute(typeof(TextEditorFactory), extension, 100)
                    {
                        DefaultName = syntaxMode.Name
                    };

                    attribute.Register(package, context, packageKey);
                }
            }
        }

        public override void Unregister(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
        }
    }
}

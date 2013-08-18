using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ICSharpCode.TextEditor.Document;
using NetIde.Core.ToolWindows.TextEditor;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Core.Services.LanguageServiceRegistry
{
    internal class NiLanguageServiceRegistry : ServiceBase, INiLanguageServiceRegistry
    {
        public Guid DefaultLanguageServiceID { get; private set; }

        public NiLanguageServiceRegistry(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            DefaultLanguageServiceID = LanguageServiceMapper.GetLanguageServiceFromHighlighter(
                HighlightingManager.Manager.DefaultHighlighting.Name
            ).Value;
        }

        public HResult FindForFileName(string fileName, out Guid languageServiceId)
        {
            languageServiceId = new Guid();

            try
            {
                if (fileName == null)
                    throw new ArgumentNullException("fileName");

                var strategy = HighlightingManager.Manager.FindHighlighterForFile(fileName);

                if (strategy == null)
                    return HResult.False;

                var result = LanguageServiceMapper.GetLanguageServiceFromHighlighter(strategy.Name);

                Debug.Assert(result.HasValue);

                if (!result.HasValue)
                    return HResult.False;

                languageServiceId = result.Value;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetDefault(out Guid languageService)
        {
            languageService = DefaultLanguageServiceID;

            return HResult.OK;
        }
    }
}

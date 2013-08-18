using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell;

namespace NetIde.Core.ToolWindows.TextEditor
{
    internal static class LanguageServiceMapper
    {
        private static readonly Dictionary<Guid, string> _languageServiceMapping = new Dictionary<Guid, string>
        {
            { new Guid(NiConstants.LanguageServiceXml), "XML" },
            { new Guid(NiConstants.LanguageServiceHtml), "HTML" },
            { new Guid(NiConstants.LanguageServiceCppNet), "C++.NET" },
            { new Guid(NiConstants.LanguageServiceBat), "BAT" },
            { new Guid(NiConstants.LanguageServiceCoco), "Coco" },
            { new Guid(NiConstants.LanguageServicePhp), "PHP" },
            { new Guid(NiConstants.LanguageServiceCSharp), "C#" },
            { new Guid(NiConstants.LanguageServicePatch), "Patch" },
            { new Guid(NiConstants.LanguageServiceBoo), "Boo" },
            { new Guid(NiConstants.LanguageServiceDefault), "Default" },
            { new Guid(NiConstants.LanguageServiceVbNet), "VBNET" },
            { new Guid(NiConstants.LanguageServiceTex), "TeX" },
            { new Guid(NiConstants.LanguageServiceAspXHtml), "ASP/XHTML" },
            { new Guid(NiConstants.LanguageServiceJavaScript), "JavaScript" },
            { new Guid(NiConstants.LanguageServiceJava), "Java" }
        };

        private static readonly Dictionary<string, Guid> _languageNameMapping = BuildLanguageNameMapping();

        private static Dictionary<string, Guid> BuildLanguageNameMapping()
        {
            var result = new Dictionary<string, Guid>();

            foreach (var item in _languageServiceMapping)
            {
                result.Add(item.Value, item.Key);
            }

            return result;
        }

        public static string GetHighlighterFromLanguageService(Guid languageService)
        {
            string highlighter;
            _languageServiceMapping.TryGetValue(languageService, out highlighter);

            return highlighter;
        }

        public static Guid? GetLanguageServiceFromHighlighter(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            Guid languageServiceId;
            if (_languageNameMapping.TryGetValue(name, out languageServiceId))
                return languageServiceId;

            return null;
        }
    }
}

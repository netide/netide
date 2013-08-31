using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Core.Services.Finder
{
    [Guid(NiConstants.FindHelper)]
    [ProvideObject(typeof(NiFindHelper))]
    internal class NiFindHelper : ServiceObject, INiFindHelper
    {
        public HResult FindInText(string find, string replace, NiFindOptions options, string text, int offset, out int found, out int matchLength, out string replacementText, out bool isFound)
        {
            found = 0;
            matchLength = 0;
            replacementText = null;
            isFound = false;

            try
            {
                if (find == null)
                    throw new ArgumentNullException("find");
                if (text == null)
                    throw new ArgumentNullException("text");

                string pattern =
                    options.HasFlag(NiFindOptions.RegExp)
                    ? find
                    : Regex.Escape(find);

                if (options.HasFlag(NiFindOptions.WholeWord))
                    pattern = @"\b" + pattern + @"\b";

                var regexOptions = RegexOptions.Multiline;

                if (!options.HasFlag(NiFindOptions.MatchCase))
                    regexOptions |= RegexOptions.IgnoreCase;
                if (options.HasFlag(NiFindOptions.Backwards))
                    regexOptions |= RegexOptions.RightToLeft;

                var match = new Regex(pattern, regexOptions).Match(text, offset);

                isFound = match.Success;

                if (isFound)
                {
                    found = match.Index;
                    matchLength = match.Length;
                }

                if (replace != null)
                    throw new NotImplementedException();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
    }
}

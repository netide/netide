using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NetIde.Core
{
    internal static class Constants
    {
        public const string CorePackageGuid = "129646dc-01e6-4443-abac-453f731ea7f5";

        public static readonly Font DefaultCodeFont = GetDefaultCodeFont();

        private static Font GetDefaultCodeFont()
        {
            var font = new Font("Consolas", 10);

            if (font.FontFamily.Name == "Consolas")
                return font;

            font.Dispose();

            return new Font("Courier New", 10);
        }
    }
}

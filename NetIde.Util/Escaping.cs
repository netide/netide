using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Util
{
    public static class Escaping
    {
        public static string ShellArgument(string argument)
        {
            if (argument == null)
                argument = String.Empty;

            return "\"" + argument.Replace("\"", "\"\"") + "\"";
        }
    }
}

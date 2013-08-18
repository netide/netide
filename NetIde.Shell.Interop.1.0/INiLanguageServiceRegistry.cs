using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiLanguageServiceRegistry
    {
        Guid DefaultLanguageServiceID { get; }

        HResult FindForFileName(string fileName, out Guid languageServiceId);
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetIde.Help
{
    internal interface IHelpSourceEntry
    {
        string Name { get;  }

        Stream GetInputStream();
    }
}

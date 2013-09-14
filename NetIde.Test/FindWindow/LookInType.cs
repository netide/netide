using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NetIde.Test.FindWindow
{
    internal enum LookInType
    {
        Unknown,
        [Description("Current Document")]
        CurrentDocument,
        [Description("All Open Documents")]
        AllOpenDocuments,
        [Description("Entire Project")]
        EntireProject
    }
}

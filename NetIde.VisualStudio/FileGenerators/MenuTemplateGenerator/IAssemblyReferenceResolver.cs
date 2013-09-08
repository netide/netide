using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetIde.VisualStudio.FileGenerators.MenuTemplateGenerator
{
    internal interface IAssemblyReferenceResolver
    {
        string ResolveAssemblyReference(string assemblyName);
    }
}

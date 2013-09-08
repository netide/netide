using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;

namespace NetIde.VisualStudio.FileGenerators.MenuTemplateGenerator
{
    [ComVisible(true)]
    [Guid("663ee67a-e45a-4bc0-bc15-2ab0d0c282a1")]
    [CodeGeneratorRegistration(typeof(NetIdeResourceTransformer), "Net IDE Resource Transformer", "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}", GeneratesDesignTimeSource = true)]
    [ProvideObject(typeof(NetIdeResourceTransformer))]
    public class NetIdeResourceTransformer : BaseCodeGeneratorWithSite
    {
        protected override byte[] GenerateCode(string inputFileContent)
        {
            string result = IsolatedResourceTransformer.Transform(
                inputFileContent,
                new AssemblyReferenceResolver(this)
            );

            return Encoding.UTF8.GetBytes(result);
        }

        protected override string GetDefaultExtension()
        {
            return ".Generated.xml";
        }

        private class AssemblyReferenceResolver : MarshalByRefObject, IAssemblyReferenceResolver
        {
            private readonly NetIdeResourceTransformer _transformer;

            public AssemblyReferenceResolver(NetIdeResourceTransformer transformer)
            {
                _transformer = transformer;
            }

            public string ResolveAssemblyReference(string assemblyName)
            {
                foreach (VSLangProj.Reference reference in _transformer.VSProject.References)
                {
                    if (
                        !String.IsNullOrEmpty(reference.Path) &&
                        String.Equals(assemblyName, reference.Name, StringComparison.OrdinalIgnoreCase)
                    )
                        return Path.GetFullPath(reference.Path);
                }

                throw new ArgumentException(String.Format(Labels.CanotResolveAssemblyReference, assemblyName), "assemblyName");
            }
        }
    }
}

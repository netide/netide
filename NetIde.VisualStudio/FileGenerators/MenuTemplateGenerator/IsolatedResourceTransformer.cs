using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetIde.VisualStudio.FileGenerators.MenuTemplateGenerator
{
    public class IsolatedResourceTransformer : MarshalByRefObject
    {
        internal static string Transform(string xml, IAssemblyReferenceResolver resolver)
        {
            if (xml == null)
                throw new ArgumentNullException("xml");

            var setup = new AppDomainSetup
            {
                ApplicationBase = Path.GetDirectoryName(typeof(IsolatedResourceTransformer).Assembly.Location),
                ApplicationName = "Isolated Transformer"
            };

            var appDomain = AppDomain.CreateDomain(
                setup.ApplicationName,
                AppDomain.CurrentDomain.Evidence,
                setup
            );

            try
            {
                using (new AssemblyResolver())
                {
                    var transformer = (IsolatedResourceTransformer)appDomain.CreateInstanceAndUnwrap(
                        typeof(IsolatedResourceTransformer).Assembly.FullName,
                        typeof(IsolatedResourceTransformer).FullName
                    );

                    return transformer.PerformTransform(xml, resolver);
                }
            }
            finally
            {
                AppDomain.Unload(appDomain);
            }
        }

        private string PerformTransform(string xml, IAssemblyReferenceResolver resolver)
        {
            return ResourceTransformer.Transform(xml, resolver);
        }

        private class AssemblyResolver : IDisposable
        {
            private bool _disposed;

            public AssemblyResolver()
            {
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            }

            Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
            {
                return AppDomain.CurrentDomain.GetAssemblies()
                    .SingleOrDefault(p => p.FullName == args.Name);
            }

            public void Dispose()
            {
                if (!_disposed)
                {
                    AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;

                    _disposed = true;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetIde.BuildTasks
{
    internal class VersionRetriever : MarshalByRefObject
    {
        public static string GetVersion(string fileName)
        {
            var setup = new AppDomainSetup
            {
                ApplicationBase = Path.GetDirectoryName(typeof(VersionRetriever).Assembly.Location),
                ApplicationName = "Version Retriever"
            };

            var appDomain = AppDomain.CreateDomain(
                setup.ApplicationName,
                AppDomain.CurrentDomain.Evidence,
                setup
            );

            try
            {
                var retriever = (VersionRetriever)appDomain.CreateInstanceAndUnwrap(
                    typeof(VersionRetriever).Assembly.GetName().Name,
                    typeof(VersionRetriever).FullName
                );

                return retriever.GetVersionFromAssembly(fileName);
            }
            finally
            {
                AppDomain.Unload(appDomain);
            }
        }

        private string GetVersionFromAssembly(string fileName)
        {
            return Assembly.ReflectionOnlyLoadFrom(fileName).GetName().Version.ToString();
        }
    }
}

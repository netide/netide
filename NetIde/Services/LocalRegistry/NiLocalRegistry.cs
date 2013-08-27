using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Util;
using NetIde.Services.PackageManager;
using NetIde.Shell;
using NetIde.Shell.Interop;
using log4net;

namespace NetIde.Services.LocalRegistry
{
    internal class NiLocalRegistry : ServiceBase, INiLocalRegistry
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NiLocalRegistry));

        private static readonly Dictionary<Type, Func<NiLocalRegistry, IRegistration, object>> _builders = new Dictionary<Type, Func<NiLocalRegistry, IRegistration, object>>
        {
            { typeof(ToolWindowRegistration), (s, r) => s.CreateToolWindow((ToolWindowRegistration)r) },
            { typeof(TypeRegistration), (s, r) => s.CreateType((ITypeRegistration)r) },
            { typeof(OptionPageRegistration), (s, r) => s.CreateType((ITypeRegistration)r) },
        };

        private readonly RegistrationCollection _registrations = new RegistrationCollection();

        public IKeyedCollection<Guid, IRegistration> Registrations { get; private set; }

        public NiLocalRegistry(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            Registrations = ReadOnlyKeyedCollection.Create(_registrations);

            LoadProjectFactories();
            LoadToolWindows();
            LoadClasses();
            LoadOptionPages();
        }

        private void LoadProjectFactories()
        {
            RegistryUtil.ForEachPackage(this, "Projects", (packageId, key) =>
            {
                foreach (string projectId in key.GetSubKeyNames())
                {
                    using (var subKey = key.OpenSubKey(projectId))
                    {
                        Log.InfoFormat("Loading project factory {0}", projectId);

                        try
                        {
                            _registrations.Add(new ProjectFactoryRegistration(
                                packageId,
                                Guid.Parse(projectId),
                                (string)subKey.GetValue("DisplayName"),
                                (string)subKey.GetValue("ProjectFileExtensions"),
                                (string)subKey.GetValue("DefaultProjectExtension"),
                                (string)subKey.GetValue("PossibleProjectExtensions")
                            ));
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Could not load editor", ex);
                        }
                    }
                }
            });
        }

        private void LoadToolWindows()
        {
            RegistryUtil.ForEachPackage(this, "ToolWindows", (packageId, key) =>
            {
                foreach (string toolWindowId in key.GetSubKeyNames())
                {
                    using (var subKey = key.OpenSubKey(toolWindowId))
                    {
                        Log.InfoFormat("Loading tool window {0}", toolWindowId);

                        try
                        {
                            _registrations.Add(new ToolWindowRegistration(
                                packageId,
                                Guid.Parse(toolWindowId),
                                RegistryUtil.GetBool(subKey.GetValue("MultipleInstances")),
                                Enum<NiToolWindowOrientation>.Parse((string)subKey.GetValue("Orientation")),
                                Enum<NiDockStyle>.Parse((string)subKey.GetValue("Style")),
                                RegistryUtil.GetBool(subKey.GetValue("Transient"))
                            ));
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Could not load tool window", ex);
                        }
                    }
                }
            });
        }

        private void LoadClasses()
        {
            RegistryUtil.ForEachPackage(this, "CLSID", (packageId, key) =>
            {
                foreach (string classId in key.GetSubKeyNames())
                {
                    using (var subKey = key.OpenSubKey(classId))
                    {
                        Log.InfoFormat("Loading CLSID {0}", classId);

                        try
                        {
                            _registrations.Add(new TypeRegistration(
                                packageId,
                                Guid.Parse(classId),
                                (string)subKey.GetValue("Class")
                            ));
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Could not load CLSID", ex);
                        }
                    }
                }
            });
        }

        private void LoadOptionPages()
        {
            RegistryUtil.ForEachPackage(this, "OptionPages", (packageId, key) =>
            {
                foreach (string pageId in key.GetSubKeyNames())
                {
                    using (var subKey = key.OpenSubKey(pageId))
                    {
                        Log.InfoFormat("Loading page {0}", pageId);

                        try
                        {
                            _registrations.Add(new OptionPageRegistration(
                                packageId,
                                Guid.Parse(pageId),
                                (string)subKey.GetValue("Class"),
                                (string)subKey.GetValue("CategoryName"),
                                (string)subKey.GetValue("PageName"),
                                (string)subKey.GetValue("CategoryNameResourceId"),
                                (string)subKey.GetValue("PageNameResourceId")
                            ));
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Could not load page", ex);
                        }
                    }
                }
            });
        }

        public HResult CreateInstance(Guid guid, out object instance)
        {
            instance = null;

            try
            {
                IRegistration registration;

                if (!_registrations.TryGetValue(guid, out registration))
                    return HResult.False;

                instance = _builders[registration.GetType()](this, registration);

                return instance != null ? HResult.OK : HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private object CreateToolWindow(ToolWindowRegistration registration)
        {
            var packageManager = (NiPackageManager)GetService(typeof(INiPackageManager));
            var package = packageManager.Packages[registration.Package];

            INiWindowPane windowPane;
            ErrorUtil.ThrowOnFailure(package.Package.CreateToolWindow(registration.Id, out windowPane));

            return windowPane;
        }

        private object CreateType(ITypeRegistration registration)
        {
            var packageManager = (NiPackageManager)GetService(typeof(INiPackageManager));
            var package = packageManager.Packages[registration.Package];

            return package.CreateInstanceAndUnwrap(registration.Type);
        }

        private class RegistrationCollection : KeyedCollection<Guid, IRegistration>
        {
            protected override Guid GetKeyForItem(IRegistration item)
            {
                return item.Id;
            }
        }
    }
}

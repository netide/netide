using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util;
using log4net;

namespace NetIde.Services.EditorFactoryRegistry
{
    internal class NiEditorFactoryRegistry : ServiceBase, INiEditorFactoryRegistry
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NiEditorFactoryRegistry));

        private readonly Dictionary<Guid, INiEditorFactory> _factories = new Dictionary<Guid, INiEditorFactory>();

        public IKeyedCollection<string, ExtensionRegistration> DefaultRegistry { get; private set; }
        public IKeyedCollection<Guid, IKeyedCollection<string, ExtensionRegistration>> ProjectRegistries { get; private set; }

        public NiEditorFactoryRegistry(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            LoadExtensions();
        }

        public HResult RegisterEditorFactory(Guid guid, INiEditorFactory editorFactory)
        {
            try
            {
                if (editorFactory == null)
                    throw new ArgumentNullException("editorFactory");

                if (_factories.ContainsKey(guid))
                    throw new InvalidOperationException(Labels.EditorFactoryAlreadyRegistered);

                _factories.Add(guid, editorFactory);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public bool TryGetEditorFactory(Guid guid, out INiEditorFactory editorFactory)
        {
            return _factories.TryGetValue(guid, out editorFactory);
        }

        private void LoadExtensions()
        {
            var projectRegistry = new ProjectExtensionRegistry();

            RegistryUtil.ForEachPackage(this, "Projects", (packageId, key) =>
            {
                foreach (string projectId in key.GetSubKeyNames())
                {
                    using (var subKey = key.OpenSubKey(projectId + "\\Extensions"))
                    {
                        if (subKey != null)
                            projectRegistry.Add(LoadExtensionRegistry(new Guid(projectId), subKey));
                    }
                }
            });

            ProjectRegistries = new ReadOnlyKeyedCollection<Guid, IKeyedCollection<string, ExtensionRegistration>>(projectRegistry);

            RegistryUtil.ForEachPackage(this, "Extensions", (packageId, key) =>
            {
                DefaultRegistry = new ReadOnlyKeyedCollection<string, ExtensionRegistration>(LoadExtensionRegistry(null, key)); 
            });
        }

        private ExtensionRegistry LoadExtensionRegistry(Guid? projectGuid, RegistryKey key)
        {
            var registry =
                projectGuid.HasValue
                ? new ProjectExtensionRegistration(projectGuid.Value)
                : new ExtensionRegistry();

            foreach (string extension in key.GetSubKeyNames())
            {
                using (var subKey = key.OpenSubKey(extension))
                {
                    Log.InfoFormat("Loading editor extension {0}", extension);

                    try
                    {
                        registry.Add(new ExtensionRegistration(
                            new Guid((string)subKey.GetValue("FactoryType")),
                            extension,
                            (int)subKey.GetValue("Priority"),
                            (string)subKey.GetValue("DefaultName"),
                            (string)subKey.GetValue("TemplateResource")
                        ));
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Could not load editor extension", ex);
                    }
                }
            }

            return registry;
        }

        private class ProjectExtensionRegistration : ExtensionRegistry
        {
            public Guid ProjectGuid { get; private set; }

            public ProjectExtensionRegistration(Guid projectGuid)
            {
                ProjectGuid = projectGuid;
            }
        }

        private class ProjectExtensionRegistry : KeyedCollection<Guid, IKeyedCollection<string, ExtensionRegistration>>
        {
            protected override Guid GetKeyForItem(IKeyedCollection<string, ExtensionRegistration> item)
            {
                return ((ProjectExtensionRegistration)item).ProjectGuid;
            }
        }
    }
}

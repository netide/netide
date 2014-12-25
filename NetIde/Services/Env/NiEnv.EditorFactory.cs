using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NetIde.Project;
using NetIde.Project.Interop;
using NetIde.Services.EditorFactoryRegistry;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util;

namespace NetIde.Services.Env
{
    partial class NiEnv
    {
        public HResult GetStandardEditorFactory(Guid? editorGuid, string document, out INiEditorFactory editorFactory)
        {
            Guid resolvedEditorFactory;
            return GetStandardEditorFactory(editorGuid, document, out editorFactory, out resolvedEditorFactory);
        }

        public HResult GetStandardEditorFactory(Guid? editorGuid, string document, out INiEditorFactory editorFactory, out Guid resolvedEditorGuid)
        {
            editorFactory = null;
            resolvedEditorGuid = Guid.Empty;

            try
            {
                if (editorGuid.HasValue == (document != null))
                    throw new ArgumentOutOfRangeException("editorGuid", NeutralResources.SpecifyEitherDocumentOrEditorGuid);

                var editorFactoryRegistry = (NiEditorFactoryRegistry)GetService(typeof(INiEditorFactoryRegistry));

                if (document != null)
                {
                    string extension = Path.GetExtension(document);

                    if (!String.IsNullOrEmpty(extension))
                    {
                        var activeProject = ((INiProjectManager)GetService(typeof(INiProjectManager))).ActiveProject;
                        ExtensionRegistration registration = null;

                        if (activeProject != null)
                        {
                            var projectGuid = (Guid?)activeProject.GetPropertyEx(NiHierarchyProperty.OwnerType);

                            if (projectGuid.HasValue)
                            {
                                IKeyedCollection<string, ExtensionRegistration> registry;
                                if (editorFactoryRegistry.ProjectRegistries.TryGetValue(projectGuid.Value, out registry))
                                    registry.TryGetValue(extension, out registration);
                            }
                        }

                        if (registration == null)
                            editorFactoryRegistry.DefaultRegistry.TryGetValue(extension, out registration);

                        if (registration != null)
                            editorGuid = registration.FactoryType;
                    }

                    if (!editorGuid.HasValue)
                    {
                        // If we cannot find an editor for the extension, we fall
                        // back to the default editor which opens the document as
                        // plain text.

                        editorGuid = new Guid(NiConstants.TextEditor);
                    }
                }

                if (!editorFactoryRegistry.TryGetEditorFactory(editorGuid.Value, out editorFactory))
                    return HResult.False;

                resolvedEditorGuid = editorGuid.Value;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NetIde.Services.EditorFactoryRegistry;
using NetIde.Services.Shell;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util;
using log4net;

namespace NetIde.Services.OpenDocumentManager
{
    internal class NiOpenDocumentManager : ServiceBase, INiOpenDocumentManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NiOpenDocumentManager));

        private readonly RegistrationCollection _registrations = new RegistrationCollection();
        private readonly Dictionary<string, OpenDocument> _openDocuments = new Dictionary<string, OpenDocument>(StringComparer.OrdinalIgnoreCase);

        public bool HaveOpenDocuments
        {
            get { return _openDocuments.Count > 0; }
        }

        public NiOpenDocumentManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            LoadEditorFactories();
        }

        private void LoadEditorFactories()
        {
            RegistryUtil.ForEachPackage(this, "Editors", (packageId, key) =>
            {
                foreach (string editorId in key.GetSubKeyNames())
                {
                    using (var subKey = key.OpenSubKey(editorId))
                    {
                        Log.InfoFormat("Loading editor {0}", editorId);

                        try
                        {
                            _registrations.Add(new EditorFactoryRegistration(
                                packageId,
                                Guid.Parse(editorId),
                                (string)subKey.GetValue("DisplayName")
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

        public HResult GetStandardEditorFactory(Guid? editorGuid, string document, out INiEditorFactory editorFactory)
        {
            Guid resolvedEditorFactory;
            return GetStandardEditorFactory(editorGuid, document, out editorFactory, out resolvedEditorFactory);
        }

        private HResult GetStandardEditorFactory(Guid? editorGuid, string document, out INiEditorFactory editorFactory, out Guid resolvedEditorGuid)
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

        public HResult IsDocumentOpen(string document, out INiHierarchy hier, out INiWindowFrame windowFrame)
        {
            hier = null;
            windowFrame = null;

            try
            {
                if (document == null)
                    throw new ArgumentNullException("document");

                OpenDocument openDocument;
                if (_openDocuments.TryGetValue(document, out openDocument))
                {
                    hier = openDocument.Item;
                    windowFrame = openDocument.WindowFrame;

                    return HResult.OK;
                }

                return HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult OpenStandardEditor(Guid? editorGuid, string document, INiHierarchy hier, IServiceProvider serviceProvider, out INiWindowFrame windowFrame)
        {
            windowFrame = null;

            try
            {
                if (document == null)
                    throw new ArgumentNullException("document");
                if (serviceProvider == null)
                    throw new ArgumentNullException("serviceProvider");

                INiEditorFactory editorFactory;
                Guid resolvedEditorGuid;
                var hr = GetStandardEditorFactory(null, document, out editorFactory, out resolvedEditorGuid);

                if (ErrorUtil.Failure(hr))
                    return hr;
                if (editorFactory == null)
                    return HResult.False;

                return OpenSpecificEditor(
                    document,
                    resolvedEditorGuid,
                    editorFactory,
                    hier,
                    serviceProvider,
                    out windowFrame
                );
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult OpenSpecificEditor(string document, Guid editorType, INiHierarchy hier, IServiceProvider serviceProvider, out INiWindowFrame windowFrame)
        {
            windowFrame = null;

            try
            {
                var manager = (INiOpenDocumentManager)GetService(typeof(INiOpenDocumentManager));
                INiEditorFactory editorFactory;
                var hr = manager.GetStandardEditorFactory(editorType, null, out editorFactory);

                if (ErrorUtil.Failure(hr))
                    return hr;

                return OpenSpecificEditor(document, editorType, editorFactory, hier, serviceProvider, out windowFrame);
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private HResult OpenSpecificEditor(string document, Guid editorType, INiEditorFactory editorFactory, INiHierarchy hier, IServiceProvider serviceProvider, out INiWindowFrame windowFrame)
        {
            windowFrame = null;

            try
            {
                if (document == null)
                    throw new ArgumentNullException("document");
                if (editorFactory == null)
                    throw new ArgumentNullException("editorFactory");
                if (serviceProvider == null)
                    throw new ArgumentNullException("serviceProvider");

                OpenDocument openDocument;
                if (_openDocuments.TryGetValue(document, out openDocument))
                {
                    ErrorUtil.ThrowOnFailure(openDocument.WindowFrame.Show());
                    return HResult.OK;
                }

                string editorCaption;
                INiWindowPane windowPane;
                var hr = editorFactory.CreateEditor(document, hier, out editorCaption, out windowPane);

                if (ErrorUtil.Failure(hr))
                    return hr;
                if (windowPane == null)
                    return HResult.False;

                hr = windowPane.SetSite(serviceProvider);

                if (ErrorUtil.Failure(hr))
                    return hr;

                hr = ((INiShell)GetService(typeof(INiShell))).CreateToolWindow(
                    windowPane,
                    NiDockStyle.Document,
                    NiToolWindowOrientation.Top,
                    out windowFrame
                );

                if (ErrorUtil.Failure(hr))
                    return hr;

                ErrorUtil.ThrowOnFailure(windowFrame.SetCaption(
                    editorCaption ?? Path.GetFileName(document)
                ));

                hr = windowPane.Initialize();

                if (ErrorUtil.Failure(hr))
                    return hr;

                var docData = windowPane as INiPersistDocData;

                if (docData == null)
                {
                    var textBufferProvider = windowPane as INiTextBufferProvider;

                    if (textBufferProvider != null)
                    {
                        INiTextBuffer textBuffer;
                        hr = textBufferProvider.GetTextBuffer(out textBuffer);

                        if (ErrorUtil.Failure(hr))
                            return hr;

                        docData = textBuffer;
                    }
                }

                int rdtCooke = -1;

                if (docData != null)
                {
                    hr = docData.LoadDocData(document);
                    if (ErrorUtil.Failure(hr))
                        return hr;

                    hr = ((INiRunningDocumentTable)GetService(typeof(INiRunningDocumentTable))).Register(
                        document, hier, docData, out rdtCooke
                    );
                    if (ErrorUtil.Failure(hr))
                        return hr;
                }

                windowFrame.SetPropertyEx(NiFrameProperty.DocCookie, rdtCooke);
                windowFrame.SetPropertyEx(NiFrameProperty.DocView, windowPane);
                windowFrame.SetPropertyEx(NiFrameProperty.DocData, docData);
                windowFrame.SetPropertyEx(NiFrameProperty.Document, document);
                windowFrame.SetPropertyEx(NiFrameProperty.EditorType, editorType);
                windowFrame.SetPropertyEx(NiFrameProperty.Hierarchy, hier);
                windowFrame.SetPropertyEx(NiFrameProperty.Type, NiFrameType.Document);

                _openDocuments.Add(document, new OpenDocument(
                    this,
                    document,
                    hier,
                    windowFrame,
                    rdtCooke,
                    docData
                ));

                return windowFrame.Show();
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private void RemoveDocument(OpenDocument openDocument)
        {
            bool removed = _openDocuments.Remove(openDocument.Document);

            Debug.Assert(removed);
        }

        private class OpenDocument : IDisposable
        {
            private readonly NiOpenDocumentManager _manager;
            private int _rdtCookie;
            private readonly INiPersistDocData _docData;
            private bool _disposed;
            private readonly string _initialCaption;
            private bool _wasDirty;

            public string Document { get; private set; }
            public INiHierarchy Item { get; private set; }
            public INiWindowFrame WindowFrame { get; private set; }

            public OpenDocument(NiOpenDocumentManager manager, string document, INiHierarchy item, INiWindowFrame windowFrame, int rdtCookie, INiPersistDocData docData)
            {
                _manager = manager;
                _rdtCookie = rdtCookie;
                _docData = docData;
                Document = document;
                Item = item;
                WindowFrame = windowFrame;

                new Listener(this);

                if (docData != null)
                {
                    ErrorUtil.ThrowOnFailure(windowFrame.GetCaption(out _initialCaption));

                    UpdateDirtyFlag();

                    ((NiShell)manager.GetService(typeof(INiShell))).RequerySuggested += OpenDocument_RequerySuggested;
                }
            }

            private void UpdateDirtyFlag()
            {
                bool isDirty;
                ErrorUtil.ThrowOnFailure(_docData.IsDocDataDirty(out isDirty));

                if (_wasDirty != isDirty)
                {
                    _wasDirty = isDirty;

                    string caption;
                    if (isDirty)
                        caption = _initialCaption + "*";
                    else
                        caption = _initialCaption;

                    ErrorUtil.ThrowOnFailure(WindowFrame.SetCaption(caption));
                }
            }

            void OpenDocument_RequerySuggested(object sender, EventArgs e)
            {
                UpdateDirtyFlag();
            }

            public void Dispose()
            {
                if (!_disposed)
                {
                    _manager.RemoveDocument(this);

                    if (_docData != null)
                        ((NiShell)_manager.GetService(typeof(INiShell))).RequerySuggested -= OpenDocument_RequerySuggested;

                    if (_rdtCookie != -1)
                    {
                        var runningDocumentTable = (INiRunningDocumentTable)_manager.GetService(typeof(INiRunningDocumentTable));
                        ErrorUtil.ThrowOnFailure(runningDocumentTable.Unregister(
                            _rdtCookie
                        ));

                        _rdtCookie = -1;
                    }

                    _disposed = true;
                }
            }

            private class Listener : NiEventSink, INiWindowFrameNotify
            {
                private static readonly ILog Log = LogManager.GetLogger(typeof(Listener));
                    
                private readonly OpenDocument _owner;

                public Listener(OpenDocument owner)
                    : base(owner.WindowFrame)
                {
                    _owner = owner;
                }

                public void OnShow(NiWindowShow action)
                {
                    if (action == NiWindowShow.Close)
                    {
                        _owner.Dispose();
                        Dispose();
                    }
                }

                public void OnSize()
                {
                }

                public void OnClose(NiFrameCloseMode closeMode, ref bool cancel)
                {
                    try
                    {
                        if (_owner._docData == null)
                            return;

                        NiSaveMode saveMode;

                        switch (closeMode)
                        {
                            case NiFrameCloseMode.PromptSave: saveMode = NiSaveMode.Save; break;
                            case NiFrameCloseMode.SaveIfDirty: saveMode = NiSaveMode.SilentSave; break;
                            default: return;
                        }

                        string document;
                        bool saved;
                        ErrorUtil.ThrowOnFailure(_owner._docData.SaveDocData(saveMode, out document, out saved));

                        cancel = !saved;
                    }
                    catch (Exception ex)
                    {
                        Log.Warn("Failed to save document", ex);
                    }
                }
            }
        }

        private class RegistrationCollection : KeyedCollection<Guid, Registration>
        {
            protected override Guid GetKeyForItem(Registration item)
            {
                return item.Id;
            }
        }

        private abstract class Registration
        {
            public Guid PackageId { get; private set; }
            public Guid Id { get; private set; }

            protected Registration(Guid packageId, Guid id)
            {
                PackageId = packageId;
                Id = id;
            }
        }

        private class EditorFactoryRegistration : Registration
        {
            public string DisplayName { get; private set; }

            public EditorFactoryRegistration(Guid packageId, Guid id, string displayName)
                : base(packageId, id)
            {
                DisplayName = displayName;
            }
        }
    }
}

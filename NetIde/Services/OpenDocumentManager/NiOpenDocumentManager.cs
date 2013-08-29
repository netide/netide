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
            if (editorGuid.HasValue == (document != null))
                throw new InvalidOperationException(Labels.SpecifyEitherDocumentOrEditorGuid);

            if (document != null)
            {
                // Editor extension registration has not yet been implemented.
                throw new NotImplementedException();
            }
            else
            {
                var registry = (NiEditorFactoryRegistry)GetService(typeof(INiEditorFactoryRegistry));
                if (!registry.TryGetEditorFactory(editorGuid.Value, out editorFactory))
                    return HResult.False;

                return HResult.OK;
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
                if (hier == null)
                    throw new ArgumentNullException("hier");
                if (serviceProvider == null)
                    throw new ArgumentNullException("serviceProvider");

                INiEditorFactory editorFactory;
                var hr = GetStandardEditorFactory(null, document, out editorFactory);

                if (ErrorUtil.Failure(hr))
                    return hr;
                if (editorFactory == null)
                    return HResult.False;

                return OpenSpecificEditor(
                    document,
                    editorFactory.GetType().GUID,
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
                if (document == null)
                    throw new ArgumentNullException("document");
                if (hier == null)
                    throw new ArgumentNullException("hier");
                if (serviceProvider == null)
                    throw new ArgumentNullException("serviceProvider");

                OpenDocument openDocument;
                if (_openDocuments.TryGetValue(document, out openDocument))
                {
                    ErrorUtil.ThrowOnFailure(openDocument.WindowFrame.Show());
                    return HResult.OK;
                }

                var manager = (INiOpenDocumentManager)GetService(typeof(INiOpenDocumentManager));
                INiEditorFactory editorFactory;
                var hr = manager.GetStandardEditorFactory(editorType, null, out editorFactory);

                if (ErrorUtil.Failure(hr))
                    return hr;

                string editorCaption;
                INiWindowPane windowPane;
                hr = editorFactory.CreateEditor(document, hier, out editorCaption, out windowPane);

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

                windowFrame.Caption = editorCaption ?? Path.GetFileName(document);

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
                    _initialCaption = windowFrame.Caption;

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

                    if (isDirty)
                        WindowFrame.Caption = _initialCaption + "*";
                    else
                        WindowFrame.Caption = _initialCaption;
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

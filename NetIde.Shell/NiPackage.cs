using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;
using NetIde.Shell.Interop;
using NetIde.Util;
using NetIde.Util.Forms;
using ListView = System.Windows.Forms.ListView;

namespace NetIde.Shell
{
    public abstract class NiPackage : ServiceObject, INiPackage, INiPreMessageFilter
    {
        private System.Resources.ResourceManager _stringResourceManager;
        private NiServiceContainer _serviceContainer;
        private IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, NiWindowPane> _toolWindows = new Dictionary<Type, NiWindowPane>();
        private int _commandTargetCookie;
        private int _preMessageFilterRecursion;
        private bool _disposed;

        public event CancelEventHandler PackageClosing;

        protected virtual void OnPackageClosing(CancelEventArgs e)
        {
            var ev = PackageClosing;
            if (ev != null)
                ev(this, e);
        }

        protected NiPackage()
        {
            AppDomainSetup.Setup();
        }

        public HResult GetStringResource(string key, out string value)
        {
            value = null;

            try
            {
                if (key == null)
                    throw new ArgumentNullException("key");

                EnsureStringResources();

                value = _stringResourceManager.GetString(key);

                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException(String.Format(NeutralResources.InvalidResource, key));

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private void EnsureStringResources()
        {
            if (_stringResourceManager != null)
                return;

            var attribute = GetType().GetCustomAttributes(typeof(NiStringResourcesAttribute), false);

            if (attribute.Length == 0)
                throw new InvalidOperationException(NeutralResources.CouldNotFindStringResourcesAttribute);

            string resourceName = GetType().Namespace + "." + ((NiStringResourcesAttribute)attribute[0]).ResourceName;

            _stringResourceManager = new System.Resources.ResourceManager(resourceName, GetType().Assembly);
        }

        public HResult GetNiResources(out IResource value)
        {
            value = null;

            try
            {
                var attribute = GetType().GetCustomAttributes(typeof(NiResourcesAttribute), false);

                if (attribute.Length == 0)
                    throw new InvalidOperationException(NeutralResources.CouldNotFindResourcesAttribute);

                string resourceName = GetType().Namespace + "." + ((NiResourcesAttribute)attribute[0]).ResourceName + ".resources";

                if (!GetType().Assembly.GetManifestResourceNames().Contains(resourceName))
                    return HResult.False;

                value = ResourceUtil.FromManifestResourceStream(
                    GetType().Assembly,
                    resourceName
                );

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public virtual HResult Initialize()
        {
            try
            {
                var commandTarget = this as INiCommandTarget;

                if (commandTarget != null)
                    ((INiCommandManager)GetService(typeof(INiCommandManager))).RegisterCommandTarget(commandTarget, out _commandTargetCookie);

                RegisterEditorFactories();

                ToolStripManager.Renderer = new VS2012ToolStripRenderer();

                Application.AddMessageFilter(new MessageFilter(this));

                MouseWheelMessageFilter.Install();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult PreFilterMessage(ref NiMessage message)
        {
            try
            {
                if (_preMessageFilterRecursion > 0)
                    return HResult.False;

                _preMessageFilterRecursion++;

                try
                {
                    return MessageFilterUtil.InvokeMessageFilter(ref message)
                        ? HResult.OK
                        : HResult.False;
                }
                finally
                {
                    _preMessageFilterRecursion--;
                }
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private void RegisterEditorFactories()
        {
            var registry = (INiEditorFactoryRegistry)GetService(typeof(INiEditorFactoryRegistry));

            foreach (ProvideEditorFactoryAttribute attribute in GetType().GetCustomAttributes(typeof(ProvideEditorFactoryAttribute), true))
            {
                var editorFactory = (INiEditorFactory)Activator.CreateInstance(attribute.FactoryType);

                editorFactory.SetSite(this);

                ErrorUtil.ThrowOnFailure(registry.RegisterEditorFactory(
                    attribute.FactoryType.GUID,
                    editorFactory
                ));
            }
        }

        public void RegisterProjectFactory(INiProjectFactory projectFactory)
        {
            if (projectFactory == null)
                throw new ArgumentNullException("projectFactory");

            var projectManager = (INiProjectManager)GetService(typeof(INiProjectManager));

            projectFactory.SetSite(this);

            ErrorUtil.ThrowOnFailure(projectManager.RegisterProjectFactory(
                projectFactory.GetType().GUID,
                projectFactory
            ));
        }

        public HResult SetSite(IServiceProvider serviceProvider)
        {
            try
            {
                if (serviceProvider == null)
                    throw new ArgumentNullException("serviceProvider");

                // This is the first time we get access to an IServiceProvider
                // from a new AppDomain. Install logging redirection now.

                LoggingRedirection.Install(serviceProvider);

                _serviceProvider = serviceProvider;
                _serviceContainer = new NiServiceContainer(serviceProvider);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetSite(out IServiceProvider serviceProvider)
        {
            serviceProvider = _serviceProvider;
            return HResult.OK;
        }

        [DebuggerStepThrough]
        public object GetService(Type serviceType)
        {
            return _serviceContainer.GetService(serviceType);
        }

        public NiWindowPane FindToolWindow(Type toolType, bool create)
        {
            if (toolType == null)
                throw new ArgumentNullException("toolType");

            NiWindowPane result;

            if (!_toolWindows.TryGetValue(toolType, out result) && create)
                result = CreateToolWindow(toolType);

            return result;
        }

        public HResult CreateToolWindow(Guid guid, out INiWindowPane toolWindow)
        {
            toolWindow = null;

            try
            {
                var registration = GetType()
                    .GetCustomAttributes(typeof(ProvideToolWindowAttribute), true)
                    .Cast<ProvideToolWindowAttribute>()
                    .SingleOrDefault(p => p.ToolType.GUID == guid);

                if (registration != null)
                {
                    toolWindow = CreateToolWindow(registration.ToolType);
                    return HResult.OK;
                }

                return HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public NiWindowPane CreateToolWindow(Type toolType)
        {
            if (toolType == null)
                throw new ArgumentNullException("toolType");

            var registration = GetType()
                .GetCustomAttributes(typeof(ProvideToolWindowAttribute), true)
                .Cast<ProvideToolWindowAttribute>()
                .SingleOrDefault(p => p.ToolType == toolType);

            if (registration == null)
                throw new InvalidOperationException(NeutralResources.ToolWindowNotRegistered);

            var toolWindow = (NiWindowPane)Activator.CreateInstance(toolType);

            ErrorUtil.ThrowOnFailure(toolWindow.SetSite(this));

            var shell = ((INiShell)GetService(typeof(INiShell)));

            INiWindowFrame frame;
            ErrorUtil.ThrowOnFailure(shell.CreateToolWindow(
                toolWindow,
                registration.Style,
                registration.Orientation,
                out frame
            ));

            toolWindow.Frame = frame;

            ErrorUtil.ThrowOnFailure(toolWindow.Initialize());

            _toolWindows[toolType] = toolWindow;

            new ToolWindowListener(toolWindow).Closed += toolWindow_Closed;

            return toolWindow;
        }

        void toolWindow_Closed(object sender, EventArgs e)
        {
            _toolWindows.Remove(((ToolWindowListener)sender).Window.GetType());
        }

        public HResult QueryClose(out bool canClose)
        {
            canClose = false;

            try
            {
                var ev = new CancelEventArgs();

                OnPackageClosing(ev);

                canClose = !ev.Cancel;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult Register(INiRegistrationContext registrationContext)
        {
            try
            {
                if (registrationContext == null)
                    throw new ArgumentNullException("registrationContext");

                var packageGuid = GetType().GUID.ToString("B").ToUpperInvariant();

                var descriptionAttribute = GetType().GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>().SingleOrDefault();
                string description = descriptionAttribute != null ? descriptionAttribute.Description : registrationContext.PackageId;

                using (var key = registrationContext.CreateKey("Packages\\" + packageGuid))
                {
                    key.SetValue(null, this.ResolveStringResource(description));

                    foreach (var type in GetType().Assembly.GetTypes())
                    {
                        foreach (RegistrationAttribute attribute in type.GetCustomAttributes(typeof(RegistrationAttribute), true))
                        {
                            attribute.Register(this, registrationContext, key);
                        }
                    }
                }

                using (var key = registrationContext.CreateKey("InstalledProducts\\" + registrationContext.PackageId))
                {
                    key.SetValue(null, description);
                    key.SetValue("Package", packageGuid);
                    key.SetValue("Version", GetType().Assembly.GetName().Version.ToString());
                }

                // Add the installed products key.

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult Unregister(INiRegistrationContext registrationContext)
        {
            try
            {
                if (registrationContext == null)
                    throw new ArgumentNullException("registrationContext");

                var packageGuid = GetType().GUID.ToString("B").ToUpperInvariant();

                using (var key = registrationContext.CreateKey("Packages\\" + packageGuid))
                {
                    foreach (var type in GetType().Assembly.GetTypes())
                    {
                        foreach (RegistrationAttribute attribute in type.GetCustomAttributes(typeof(RegistrationAttribute), true))
                        {
                            attribute.Unregister(this, registrationContext, key);
                        }
                    }
                }

                // Un-registration allows attributes to perform cleanup other
                // than from inside the package namespace. The package namespace
                // itself is unconditionally deleted anyway, so the attributes
                // don't have to clean up that.

                registrationContext.RemoveKey("Packages\\" + packageGuid);
                registrationContext.RemoveKey("InstalledProducts\\" + registrationContext.PackageId);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_commandTargetCookie != 0)
                {
                    ((INiCommandManager)GetService(typeof(INiCommandManager))).UnregisterCommandTarget(_commandTargetCookie);
                    _commandTargetCookie = 0;
                }

                _disposed = true;
            }
        }

        private class ToolWindowListener : NiEventSink, INiWindowFrameNotify
        {
            private static readonly ILog Log = LogManager.GetLogger(typeof(ToolWindowListener));

            public NiWindowPane Window { get; private set; }

            public event EventHandler Closed;

            private void OnClosed(EventArgs e)
            {
                var ev = Closed;
                if (ev != null)
                    ev(this, e);
            }

            public ToolWindowListener(NiWindowPane owner)
                : base(owner.Frame)
            {
                Window = owner;
            }

            public void OnShow(NiWindowShow action)
            {
                try
                {
                    if (action == NiWindowShow.Close)
                    {
                        OnClosed(EventArgs.Empty);

                        Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Log.Warn("Failed to publish close of tool window", ex);
                }
            }

            public void OnSize()
            {
            }

            public void OnClose(NiFrameCloseMode closeMode, ref bool cancel)
            {
            }
        }

        private class MessageFilter : IMessageFilter
        {
            private readonly NiPackage _package;
            private readonly INiShell _shell;
            private readonly HashSet<ListView> _installedListViews = new HashSet<ListView>();

            public MessageFilter(NiPackage package)
            {
                _package = package;
                _shell = (INiShell)package.GetService(typeof(INiShell));
            }

            public bool PreFilterMessage(ref Message m)
            {
                if (_package._preMessageFilterRecursion > 0)
                    return false;

                _package._preMessageFilterRecursion++;

                try
                {
                    InstallListViewListener(ref m);

                    NiMessage message = m;
                    bool processed = ErrorUtil.ThrowOnFailure(_shell.BroadcastPreMessageFilter(ref message));
                    m = message;

                    return processed;
                }
                finally
                {
                    _package._preMessageFilterRecursion--;
                }
            }

            private void InstallListViewListener(ref Message m)
            {
                // This is a hack. To implement updating command states, NiShell
                // executes a requery when certain window messages arrive.
                // One of the messages on which this is triggered is the
                // WM_L/RBUTTONUP. The problem is that the ListView does not send
                // these; it only sends the WM_L/RBUTTONDOWN, and we cannot use
                // these because the state of the list view may be updated in
                // a manner that is necessary for the commands to be updated
                // correctly (e.g. a change of selection). To work around this,
                // we add a MouseUp event to every list view that we see.

                var listView = Control.FromHandle(m.HWnd) as ListView;
                if (listView == null || !_installedListViews.Add(listView))
                    return;

                listView.MouseUp += listView_MouseUp;
                listView.Disposed += listView_Disposed;
            }

            void listView_MouseUp(object sender, MouseEventArgs e)
            {
                if (_shell != null && (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right))
                    ErrorUtil.ThrowOnFailure(_shell.InvalidateRequerySuggested());
            }

            void listView_Disposed(object sender, EventArgs e)
            {
                _installedListViews.Remove((ListView)sender);
            }
        }
    }
}

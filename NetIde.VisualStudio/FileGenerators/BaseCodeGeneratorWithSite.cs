using System.Text;
using System.Collections.Generic;
using System;
using System.CodeDom.Compiler;
using System.Runtime.InteropServices;
using EnvDTE;
using VSLangProj;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Designer.Interfaces;
using IServiceProvider = System.IServiceProvider;

namespace NetIde.VisualStudio.FileGenerators
{
    [ComVisible(true)]
    public abstract class BaseCodeGeneratorWithSite : BaseCodeGenerator, IObjectWithSite, IServiceProvider
    {
        private object _site;
        private CodeDomProvider _codeDomProvider;
        private Microsoft.VisualStudio.Shell.ServiceProvider _serviceProvider;

        public Project Project
        {
            get { return ProjectItem.ContainingProject; }
        }

        public VSProjectItem VSProjectItem
        {
            get { return (VSProjectItem)ProjectItem.Object; }
        }

        public VSProject VSProject
        {
            get { return (VSProject)Project.Object; }
        }

        public ProjectItem ProjectItem
        {
            get { return (ProjectItem)GetService(typeof(ProjectItem)); }
        }

        public CodeDomProvider CodeProvider
        {
            get
            {
                if (_codeDomProvider == null)
                {
                    var provider = GetService(typeof(SVSMDCodeDomProvider)) as IVSMDCodeDomProvider;
                    if (provider != null)
                        _codeDomProvider = provider.CodeDomProvider as CodeDomProvider;
                    else
                        _codeDomProvider = CodeDomProvider.CreateProvider("C#");
                }

                return _codeDomProvider;
            }
        }

        void IObjectWithSite.GetSite(ref Guid riid, out IntPtr ppvSite)
        {
            if (_site == null)
                throw new COMException(Labels.ObjectNotSited, VSConstants.E_FAIL);

            var pUnknownPointer = Marshal.GetIUnknownForObject(_site);

            IntPtr intPointer;
            Marshal.QueryInterface(pUnknownPointer, ref riid, out intPointer);

            if (intPointer == IntPtr.Zero)
                throw new COMException(Labels.InterfaceNotSupported, VSConstants.E_NOINTERFACE);

            ppvSite = intPointer;
        }

        void IObjectWithSite.SetSite(object pUnkSite)
        {
            _site = pUnkSite;

            _serviceProvider =
                _site == null
                ? null
                : new Microsoft.VisualStudio.Shell.ServiceProvider(_site as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);

            _codeDomProvider = null;
        }

        public object GetService(Guid serviceGuid)
        {
            return _serviceProvider.GetService(serviceGuid);
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        protected override string GetDefaultExtension()
        {
            string extension = CodeProvider.FileExtension;

            if (String.IsNullOrEmpty(extension))
                extension = "." + extension.TrimStart('.');

            return extension;
        }
    }
}

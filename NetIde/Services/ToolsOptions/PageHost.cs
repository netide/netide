using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.ToolsOptions
{
    internal class PageHost : NiWindowHost<INiWindowPane>
    {
        [Browsable(false)]
        [DefaultValue(null)]
        public INiOptionPage Page
        {
            get { return ((Proxy)Window).Page; }
            set { ((Proxy)Window).Page = value; }
        }

        protected override INiWindowPane CreateWindow()
        {
            var window = new Proxy(this);

            ErrorUtil.ThrowOnFailure(window.Initialize());

            return window;
        }

        private class Proxy : INiWindowPane, INiMessageFilter
        {
            private IServiceProvider _site;
            private readonly PageHost _host;
            private INiOptionPage _page;

            public INiOptionPage Page
            {
                get { return _page; }
                set
                {
                    if (_page != value)
                    {
                        _page = value;
                        _host.ChildHwnd = Handle;
                    }
                }
            }

            public Proxy(PageHost host)
            {
                _host = host;
            }

            public HResult Initialize()
            {
                return HResult.OK;
            }

            public object GetService(Type serviceType)
            {
                return _site.GetService(serviceType);
            }

            public void Dispose()
            {
            }

            public IntPtr Handle
            {
                get { return _page == null ? IntPtr.Zero : _page.Handle; }
            }

            public HResult SetSite(IServiceProvider serviceProvider)
            {
                _site = serviceProvider;
                return HResult.OK;
            }

            public HResult GetSite(out IServiceProvider serviceProvider)
            {
                serviceProvider = _site;
                return HResult.OK;
            }

            public HResult IsInputKey(Keys keyData, out bool result)
            {
                result = false;

                try
                {
                    if (_page == null)
                        return HResult.OK;

                    return _page.IsInputKey(keyData, out result);
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult IsInputChar(char charCode, out bool result)
            {
                result = false;

                try
                {
                    if (_page == null)
                        return HResult.OK;

                    return _page.IsInputChar(charCode, out result);
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult PreFilterMessage(ref NiMessage message, out bool processed)
            {
                processed = false;

                try
                {
                    if (_page == null)
                        return HResult.OK;

                    return _page.PreFilterMessage(ref message, out processed);
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }
        }
    }
}

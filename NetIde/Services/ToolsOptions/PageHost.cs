using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.ToolsOptions
{
    internal class PageHost : NiWindowHost
    {
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public INiOptionPage Page
        {
            get { return ((Proxy)Window).Page; }
            set { ((Proxy)Window).Page = value; }
        }

        protected override INiIsolationClient CreateWindow()
        {
            var window = new Proxy(this);

            ErrorUtil.ThrowOnFailure(window.Initialize());

            return window;
        }

        private class Proxy : INiWindowPane
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
                        ErrorUtil.ThrowOnFailure(_page.SetHost(_host));
                        _host.SetChildHwnd(Handle);
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

            public HResult SetHost(INiIsolationHost host)
            {
                try
                {
                    Debug.Assert(_host == host);

                    if (_page != null)
                        return _page.SetHost(host);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult PreviewKeyDown(Keys keyData)
            {
                try
                {
                    if (_page != null)
                        return _page.PreviewKeyDown(keyData);

                    return HResult.False;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult PreProcessMessage(ref NiMessage message, out PreProcessMessageResult preProcessMessageResult)
            {
                preProcessMessageResult = 0;

                try
                {
                    if (_page != null)
                        return _page.PreProcessMessage(ref message, out preProcessMessageResult);

                    return HResult.False;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult ProcessMnemonic(char charCode)
            {
                try
                {
                    if (_page != null)
                        return _page.ProcessMnemonic(charCode);

                    return HResult.False;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SelectNextControl(bool forward)
            {
                try
                {
                    if (_page != null)
                        return _page.SelectNextControl(forward);

                    return HResult.False;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetPreferredSize(Size proposedSize, out Size preferredSize)
            {
                preferredSize = new Size();

                try
                {
                    if (_page != null)
                        return _page.GetPreferredSize(proposedSize, out preferredSize);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Services.Env;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde
{
    partial class MainForm
    {
        private INiWindow _proxy;

        public INiWindow GetProxy()
        {
            return _proxy ?? (_proxy = new Proxy(this));
        }

        private class Proxy : ServiceObject, INiWindow
        {
            private readonly MainForm _owner;
            private IResource _icon;

            public Proxy(MainForm owner)
            {
                _owner = owner;
            }

            public IntPtr Handle
            {
                get { return _owner.Handle; }
            }

            public HResult Close()
            {
                try
                {
                    _owner.Close();

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public string Caption
            {
                get { return _owner.Text; }
                set { _owner.Text = value; }
            }

            public IResource Icon
            {
                get { return _icon; }
                set
                {
                    if (_icon != value)
                    {
                        _icon = value;

                        _owner.Icon =
                            value == null
                            ? null
                            : ((NiEnv)_owner.GetService(typeof(INiEnv))).ResourceManager.GetIcon(value);
                    }
                }
            }
        }
    }
}

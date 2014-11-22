using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using NetIde.Util.Win32;

namespace NetIde.Util.Forms
{
    internal class ActivationContext : IDisposable
    {
        private static readonly object _syncRoot = new object();

        private uint _cookie;
        private static NativeMethods.ACTCTX _enableThemingActivationContext;
        private static IntPtr _hActCtx;
        private static bool _contextCreationSucceeded;
        private bool _disposed;

        public ActivationContext()
        {
            _cookie = 0;

            if (EnsureActivateContextCreated())
            {
                if (!NativeMethods.ActivateActCtx(_hActCtx, out _cookie))
                {
                    // Be sure cookie always zero if activation failed
                    _cookie = 0;
                }
            }
        }

        ~ActivationContext()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_cookie != 0)
                {
                    if (NativeMethods.DeactivateActCtx(0, _cookie))
                    {
                        // deactivation succeeded...
                        _cookie = 0;
                    }
                }

                _disposed = true;
            }
        }

        private bool EnsureActivateContextCreated()
        {
            lock (_syncRoot)
            {
                if (!_contextCreationSucceeded)
                {
                    // Pull manifest from the .NET Framework install
                    // directory

                    string assemblyLoc = null;

                    FileIOPermission fiop = new FileIOPermission(PermissionState.None);
                    fiop.AllFiles = FileIOPermissionAccess.PathDiscovery;
                    fiop.Assert();
                    try
                    {
                        assemblyLoc = typeof(Object).Assembly.Location;
                    }
                    finally
                    {
                        CodeAccessPermission.RevertAssert();
                    }

                    string manifestLoc = null;
                    string installDir = null;
                    if (assemblyLoc != null)
                    {
                        installDir = Path.GetDirectoryName(assemblyLoc);
                        const string manifestName = "XPThemes.manifest";
                        manifestLoc = Path.Combine(installDir, manifestName);
                    }

                    if (manifestLoc != null && installDir != null)
                    {
                        _enableThemingActivationContext = new NativeMethods.ACTCTX();
                        _enableThemingActivationContext.cbSize = Marshal.SizeOf(typeof(NativeMethods.ACTCTX));
                        _enableThemingActivationContext.lpSource = manifestLoc;

                        // Set the lpAssemblyDirectory to the install
                        // directory to prevent Win32 Side by Side from
                        // looking for comctl32 in the application
                        // directory, which could cause a bogus dll to be
                        // placed there and open a security hole.
                        _enableThemingActivationContext.lpAssemblyDirectory = installDir;
                        _enableThemingActivationContext.dwFlags = NativeMethods.ACTCTX_FLAG_ASSEMBLY_DIRECTORY_VALID;

                        // Note this will fail gracefully if file specified
                        // by manifestLoc doesn't exist.
                        _hActCtx = NativeMethods.CreateActCtx(ref _enableThemingActivationContext);
                        _contextCreationSucceeded = (_hActCtx != new IntPtr(-1));
                    }
                }

                // If we return false, we'll try again on the next call into
                // EnsureActivateContextCreated(), which is fine.
                return _contextCreationSucceeded;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiEventSink : ServiceObject
    {
        private bool _disposed;
        private INiConnectionPoint _connectionPoint;
        private readonly int _cookie;

        public NiEventSink(INiConnectionPoint connectionPoint)
        {
            if (connectionPoint == null)
                throw new ArgumentNullException("connectionPoint");

            _connectionPoint = connectionPoint;

            ErrorUtil.ThrowOnFailure(_connectionPoint.Advise(this, out _cookie));
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_connectionPoint != null)
                {
                    ErrorUtil.ThrowOnFailure(_connectionPoint.Unadvise(_cookie));
                    _connectionPoint = null;
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public abstract class NiIterator<TIn, TOut> : ServiceObject, INiIterator<TOut>
    {
        private IEnumerator<TIn> _enumerator;
        private bool _disposed;

        protected NiIterator(IEnumerator<TIn> enumerator)
        {
            _enumerator = enumerator;
        }

        public HResult GetCurrent(out TOut current)
        {
            current = default(TOut);

            try
            {
                current = GetCurrentFromInput(_enumerator.Current);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        protected abstract TOut GetCurrentFromInput(TIn current);

        public HResult Next(out bool available)
        {
            available = false;

            try
            {
                available = _enumerator.MoveNext();

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
                if (_enumerator != null)
                {
                    _enumerator.Dispose();
                    _enumerator = null;
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }

    public abstract class NiIterator<T> : NiIterator<T, T>
    {
        protected NiIterator(IEnumerator<T> enumerator)
            : base(enumerator)
        {
        }

        protected override T GetCurrentFromInput(T current)
        {
            return current;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;
using log4net;

namespace NetIde.Shell
{
    public class NiConnectionPoint : INiConnectionPoint
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NiConnectionPoint));

        private readonly Dictionary<int, object> _listeners = new Dictionary<int, object>();
        private int _nextCookie;

        public HResult Advise(object sink, out int cookie)
        {
            cookie = 0;

            try
            {
                if (sink == null)
                    throw new ArgumentNullException("sink");

                cookie = ++_nextCookie;

                _listeners.Add(cookie, sink);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult Unadvise(int cookie)
        {
            try
            {
                return _listeners.Remove(cookie) ? HResult.OK : HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public void ForAll<T>(Action<T> callback)
        {
            foreach (var listener in _listeners.Values.ToArray())
            {
                try
                {
                    if (listener is T)
                        callback((T)listener);
                }
                catch (Exception ex)
                {
                    Log.Warn("Exception thrown from connection point callback", ex);
                }
            }
        }
    }

    public class NiConnectionPoint<T> : NiConnectionPoint
    {
        public void ForAll(Action<T> callback)
        {
            ForAll<T>(callback);
        }
    }
}

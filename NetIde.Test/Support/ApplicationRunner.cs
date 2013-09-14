using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NetIde.Test.Support
{
    internal class ApplicationRunner : MarshalByRefObject, IDisposable
    {
        private NetIdeApplication _application;
        private Thread _thread;
        private ManualResetEvent _stopEvent = new ManualResetEvent(false);
        private bool _disposed;

        public IntPtr Handle { get; private set; }

        public void Start()
        {
            Start(null);
        }

        public void Start(string[] args)
        {
            using (var @event = new ManualResetEvent(false))
            {
                _application = new NetIdeApplication(args);

                _application.HandleAvailable += (s, e) =>
                {
                    Handle = _application.Handle;
                    @event.Set();
                };

                _thread = new Thread(ThreadProc);

                _thread.SetApartmentState(ApartmentState.STA);

                _thread.Start();

                @event.WaitOne();
            }
        }

        private void ThreadProc()
        {
            _application.Run();

            _stopEvent.Set();
        }

        public void WaitForExit(TimeSpan timeout)
        {
            bool success = _stopEvent.WaitOne(timeout);

            if (!success)
                throw new ExitTimeoutException();
        }

        public void Stop()
        {
            _application.Stop();

            if (!_thread.Join(TimeSpan.FromSeconds(3)))
                _thread.Abort();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_stopEvent != null)
                {
                    _stopEvent.Dispose();
                    _stopEvent = null;
                }

                _disposed = true;
            }
        }
    }
}

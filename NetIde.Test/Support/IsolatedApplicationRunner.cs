using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Test.Support
{
    public class IsolatedApplicationRunner : IDisposable
    {
        private ApplicationRunner _application;
        private bool _disposed;
        private AppDomain _appDomain;

        public IsolatedApplicationRunner()
        {
            var setup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                ApplicationName = "Net IDE"
            };

            var appDomain = AppDomain.CreateDomain(
                setup.ApplicationName,
                AppDomain.CurrentDomain.Evidence,
                setup
            );

            try
            {
                _application = (ApplicationRunner)appDomain.CreateInstanceAndUnwrap(
                    typeof(ApplicationRunner).Assembly.FullName,
                    typeof(ApplicationRunner).FullName
                );

                _appDomain = appDomain;
            }
            catch
            {
                AppDomain.Unload(appDomain);

                throw;
            }
        }

        public IntPtr Handle
        {
            get { return _application.Handle; }
        }

        public void Start()
        {
            Start(null);
        }

        public void Start(string[] args)
        {
            _application.Start(args);
        }

        public void Stop()
        {
            _application.Stop();
        }

        public void WaitForExit(TimeSpan timeout)
        {
            _application.WaitForExit(timeout);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_application != null)
                {
                    _application.Dispose();
                    _application = null;
                }

                if (_appDomain != null)
                {
                    AppDomain.Unload(_appDomain);
                    _appDomain = null;
                }

                _disposed = true;
            }
        }
    }
}

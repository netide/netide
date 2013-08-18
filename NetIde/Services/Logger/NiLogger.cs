using System.IO;
using NetIde.Shell;
using NetIde.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Services.Logger
{
    internal class NiLogger : ServiceBase, INiLogger
    {
        private readonly object _syncRoot = new object();
        private StreamWriter _logStream;
        private bool _disposed;

        public bool Enabled { get; private set; }

        public NiLogger(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            var commandLine = ((INiCommandLine)GetService(typeof(INiCommandLine)));

            string logTarget;
            bool present;
            ErrorUtil.ThrowOnFailure(commandLine.GetOption("/log", out present, out logTarget));

            bool enabled = present && !String.IsNullOrEmpty(logTarget);

            if (!enabled)
                return;

            try
            {
                _logStream = new StreamWriter(
                    File.Open(logTarget, FileMode.Append, FileAccess.Write, FileShare.Read)
                );

                _logStream.AutoFlush = true;

                Enabled = true;
            }
            catch
            {
                // Ignore exceptions and just fail.
            }
        }

        public HResult Append(INiLogEvent logEvent)
        {
            try
            {
                if (logEvent == null)
                    throw new ArgumentNullException("logEvent");

                if (!Enabled)
                    return HResult.OK;

                var sb = new StringBuilder();

                sb.AppendLine(String.Format(
                    "[{0}] [{1}] {2}",
                    logEvent.TimeStamp.ToString("s"),
                    ((NiConstants.Severity)logEvent.Severity).ToString().ToUpperInvariant(),
                    logEvent.Message
                ));

                if (!String.IsNullOrEmpty(logEvent.Content))
                {
                    sb.AppendLine();
                    sb.AppendLine(logEvent.Content.TrimEnd());
                    sb.AppendLine();
                }

                lock (_syncRoot)
                {
                    _logStream.Write(sb.ToString());
                }

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_logStream != null)
                {
                    _logStream.Dispose();
                    _logStream = null;
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}

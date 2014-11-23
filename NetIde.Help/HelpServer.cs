using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Common.Logging;
using NHttp;

namespace NetIde.Help
{
    public class HelpServer : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HelpServer));

        private bool _disposed;
        private HttpServer _server;

        public IPEndPoint EndPoint
        {
            get { return _server.EndPoint; }
        }

        public HelpManager Manager { get; private set; }

        public HelpServer()
        {
            Manager = new HelpManager();

            _server = new HttpServer();
            _server.RequestReceived += _server_RequestReceived;
            _server.UnhandledException += _server_UnhandledException;
            _server.Start();
        }

        void _server_UnhandledException(object sender, HttpExceptionEventArgs e)
        {
            e.Handled = true;

            Log.Warn("Unhandled Help Server exception", e.Exception);
        }

        void _server_RequestReceived(object sender, HttpRequestEventArgs e)
        {
            try
            {
                using (var stream = Manager.Load(e.Request.RawUrl))
                {
                    if (stream != null)
                    {
                        e.Response.Status = "200 OK";

                        stream.CopyTo(e.Response.OutputStream);
                    }
                    else
                    {
                        e.Response.Status = "404 Not Found";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warn("Problem while loading help page", ex);

                e.Response.Status = "500 Internal Server Error";
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_server != null)
                {
                    _server.Dispose();
                    _server = null;
                }

                _disposed = true;
            }
        }
    }
}

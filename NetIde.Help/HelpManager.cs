using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace NetIde.Help
{
    public class HelpManager : IDisposable
    {
        private readonly ConcurrentDictionary<HelpRegistration, Resolver> _resolvers = new ConcurrentDictionary<HelpRegistration, Resolver>();
        private bool _disposed;

        public IList<HelpRegistration> Registrations { get; private set; }

        public event HelpResolveEventHandler Resolve;

        protected virtual void OnResolve(HelpResolveEventArgs e)
        {
            var ev = Resolve;
            if (ev != null)
                ev(this, e);
        }

        public HelpManager()
        {
            Registrations = new List<HelpRegistration>();
        }

        public Stream Load(string url)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            // Take out the path part.

            var e = new HelpResolveEventArgs(url);
            OnResolve(e);
            if (e.Stream != null)
                return e.Stream;

            // Extract the root.

            string root = null;
            string path = url.TrimStart('/');
            int index = path.IndexOf('/');

            if (index != -1)
            {
                root = path.Substring(0, index);
                path = path.Substring(index + 1);
            }

            // Find the registration for this root.

            foreach (var registration in Registrations)
            {
                if (String.Equals(registration.Root, root, StringComparison.OrdinalIgnoreCase))
                    return Load(registration, path);
            }

            return null;
        }

        private Stream Load(HelpRegistration registration, string path)
        {
            return _resolvers.GetOrAdd(registration, p => new Resolver(p)).Load(path);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                foreach (var resolver in _resolvers.Values)
                {
                    resolver.Dispose();
                }

                _resolvers.Clear();

                _disposed = true;
            }
        }

        private class Resolver : IDisposable
        {
            private bool _disposed;
            private HelpSource _source;

            public Resolver(HelpRegistration registration)
            {
                _source = HelpSource.FromSource(registration.Source);
            }

            public Stream Load(string path)
            {
                if (String.IsNullOrEmpty(path))
                    path = "index.html";

                var entry = _source.FindEntry(path);
                if (entry != null)
                    return entry.GetInputStream();

                return null;
            }

            public void Dispose()
            {
                if (!_disposed)
                {
                    if (_source != null)
                    {
                        _source.Dispose();
                        _source = null;
                    }

                    _disposed = true;
                }
            }
        }
    }
}

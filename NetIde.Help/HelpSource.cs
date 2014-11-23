using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace NetIde.Help
{
    internal abstract class HelpSource : IDisposable, IEnumerable<IHelpSourceEntry>
    {
        public static HelpSource FromSource(string source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (File.Exists(source))
                return new ZipFileSource(source);
            if (Directory.Exists(source))
                return new DirectorySource(source);
            throw new FileNotFoundException(String.Format("Source \"{0}\" does not exist", source));
        }

        public abstract IEnumerator<IHelpSourceEntry> GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract IHelpSourceEntry FindEntry(string path);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        private class DirectorySource : HelpSource
        {
            private readonly string _source;

            public DirectorySource(string source)
            {
                _source = source;
            }

            public override IEnumerator<IHelpSourceEntry> GetEnumerator()
            {
                foreach (string path in Directory.GetFiles(_source, "*", SearchOption.AllDirectories))
                {
                    Debug.Assert(path.StartsWith(_source));

                    yield return new Entry(
                        _source,
                        path.Substring(_source.Length).TrimStart(Path.DirectorySeparatorChar)
                    );
                }
            }

            public override IHelpSourceEntry FindEntry(string path)
            {
                if (path == null)
                    throw new ArgumentNullException("path");

                string fileName = Path.Combine(_source, path.Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(fileName))
                    return new Entry(_source, path);

                return null;
            }

            private class Entry : IHelpSourceEntry
            {
                private readonly string _basePath;
                private readonly string _path;

                public Entry(string basePath, string path)
                {
                    _basePath = basePath;
                    _path = path;
                }

                public string Name
                {
                    get { return _path.Replace(Path.DirectorySeparatorChar, '/'); }
                }

                public Stream GetInputStream()
                {
                    return File.OpenRead(Path.Combine(_basePath, _path));
                }
            }
        }

        private class ZipFileSource : HelpSource
        {
            private ZipFile _zipFile;
            private bool _disposed;

            public ZipFileSource(string source)
            {
                _zipFile = new ZipFile(source);
            }

            public override IEnumerator<IHelpSourceEntry> GetEnumerator()
            {
                foreach (ZipEntry entry in _zipFile)
                {
                    yield return new Entry(_zipFile, entry);
                }
            }

            public override IHelpSourceEntry FindEntry(string path)
            {
                if (path == null)
                    throw new ArgumentNullException("path");

                int entry = _zipFile.FindEntry(path, true);
                if (entry != -1)
                    return new Entry(_zipFile, _zipFile[entry]);

                return null;
            }

            protected override void Dispose(bool disposing)
            {
                if (!_disposed && disposing)
                {
                    if (_zipFile != null)
                    {
                        _zipFile.Close();
                        _zipFile = null;
                    }

                    _disposed = true;
                }

                base.Dispose(disposing);
            }

            private class Entry : IHelpSourceEntry
            {
                private readonly ZipFile _file;
                private ZipEntry _entry;

                public Entry(ZipFile file, ZipEntry entry)
                {
                    _file = file;
                    _entry = entry;
                }

                public string Name
                {
                    get { return _entry.Name; }
                }

                public Stream GetInputStream()
                {
                    return _file.GetInputStream(_entry);
                }
            }
        }
    }
}

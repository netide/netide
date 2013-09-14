using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetIde.Test.Support
{
    public class Project : IDisposable
    {
        private bool _disposed;

        public string Path { get; private set; }
        public string ProjectFileName { get; private set; }
        public string ProjectFilePath { get; private set; }

        public Project(string path, string projectFileName)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (projectFileName == null)
                throw new ArgumentNullException("projectFileName");

            Path = path;
            ProjectFileName = projectFileName;
            ProjectFilePath = System.IO.Path.Combine(path, projectFileName);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Directory.Delete(Path, true);

                _disposed = true;
            }
        }
    }
}

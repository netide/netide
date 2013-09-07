using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetIde.VisualStudio.Wizard
{
    internal static class CurrentDirectorySetter
    {
        public static IDisposable SetCurrentDirectory(string currentDirectory)
        {
            if (currentDirectory == null)
                throw new ArgumentNullException("currentDirectory");

            string previousDirectory = Environment.CurrentDirectory;

            Environment.CurrentDirectory = currentDirectory;

            return new Restorer(previousDirectory);
        }

        private class Restorer : IDisposable
        {
            private bool _disposed;
            private readonly string _directory;

            public Restorer(string directory)
            {
                _directory = directory;
            }

            public void Dispose()
            {
                if (!_disposed)
                {
                    Environment.CurrentDirectory = _directory;
                    _disposed = true;
                }
            }
        }
    }
}

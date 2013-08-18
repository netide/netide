using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface IStream : IDisposable
    {
        string Name { get; }
        string DisplayName { get; }
        StreamState State { get; }
        DateTime? CreationTime { get; }
        DateTime? LastWriteTime { get; }

        HResult GetLength(out long length);
        HResult SetLength(long length);
        HResult GetPosition(out long position);
        HResult SetPosition(long position, SeekOrigin origin);
        HResult Read(int length, out byte[] buffer);
        HResult Write(byte[] buffer);
        HResult Flush();
        HResult Close();
    }
}

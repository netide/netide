using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NetIde.Shell.Interop;
using log4net;

namespace NetIde.Shell
{
    public static class StreamUtil
    {
        public static IStream FromManifestResourceStream(Assembly assembly, string resourceName)
        {
            return FromManifestResourceStream(assembly, resourceName, null);
        }

        public static IStream FromManifestResourceStream(Assembly assembly, string resourceName, StreamInfo streamInfo)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            if (resourceName == null)
                throw new ArgumentNullException("resourceName");

            var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new IOException(String.Format(NeutralResources.ResourceNotFound, resourceName, assembly.FullName));

            if (streamInfo == null)
            {
                streamInfo = new StreamInfo
                {
                    Name = resourceName
                };
            }

            return FromStream(stream, streamInfo);
        }

        public static IStream FromFile(string fileName, FileMode mode)
        {
            return FromFile(fileName, mode, null);
        }

        public static IStream FromFile(string fileName, FileMode mode, StreamInfo streamInfo)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            return FromStream(File.Open(fileName, mode), streamInfo ?? StreamInfo.FromFile(fileName));
        }

        public static IStream FromByteArray(byte[] byteArray)
        {
            return FromByteArray(byteArray, null);
        }

        public static IStream FromByteArray(byte[] byteArray, StreamInfo streamInfo)
        {
            if (byteArray == null)
                throw new ArgumentNullException("byteArray");

            return FromStream(new MemoryStream(byteArray), streamInfo);
        }

        public static IStream FromStream(Stream stream)
        {
            return FromStream(stream, null);
        }

        public static IStream FromStream(Stream stream, StreamInfo streamInfo)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            return new StreamWrapper(stream, streamInfo);
        }

        public static Stream ToStream(IStream stream)
        {
            return ToStream(stream, true);
        }

        public static Stream ToStream(IStream stream, bool closeBaseStream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            return new InteropStreamWrapper(stream, closeBaseStream);
        }

        public static Stream ToStream(IResource resource)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");

            // This is a hack for performance reasons. When the resource
            // represents a file, there really is no reason why we can't just
            // go to disk ourselves instead of going over AppDomain boundaries.

            string fileName = resource.FileName;

            if (fileName != null && File.Exists(fileName))
                return File.OpenRead(fileName);

            IStream stream;
            ErrorUtil.ThrowOnFailure(resource.Open(out stream));

            if (stream == null)
                return null;

            return ToStream(stream);
        }

        private class StreamWrapper : ServiceObject, IStream
        {
            private static readonly ILog Log = LogManager.GetLogger(typeof(StreamWrapper));

            private Stream _stream;
            private bool _disposed;

            public string Name { get; private set; }
            public string DisplayName { get; private set; }
            public DateTime? CreationTime { get; private set; }
            public DateTime? LastWriteTime { get; private set; }
            public StreamState State { get; private set; }

            public StreamWrapper(Stream stream, StreamInfo streamInfo)
            {
                _stream = stream;

                if (streamInfo != null)
                {
                    Name = streamInfo.Name;
                    DisplayName = streamInfo.DisplayName;
                    CreationTime = streamInfo.CreationTime;
                    LastWriteTime = streamInfo.LastWriteTime;
                }

                if (_stream.CanRead)
                    State |= StreamState.Readable;
                if (_stream.CanWrite)
                    State |= StreamState.Writable;
                if (_stream.CanSeek)
                    State |= StreamState.Seekable;
            }

            public HResult GetLength(out long length)
            {
                length = 0;

                try
                {
                    length = _stream.Length;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetLength(long length)
            {
                try
                {
                    _stream.SetLength(length);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetPosition(out long position)
            {
                position = 0;

                try
                {
                    position = _stream.Position;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetPosition(long position, SeekOrigin origin)
            {
                try
                {
                    _stream.Seek(position, origin);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult Read(int length, out byte[] buffer)
            {
                buffer = null;

                try
                {
                    buffer = new byte[length];

                    if (length == 0)
                        return HResult.OK;

                    int read = _stream.Read(buffer, 0, length);

                    if (read == 0)
                    {
                        buffer = null;

                        return HResult.False;
                    }

                    if (read != length)
                    {
                        Debug.Assert(read < length);

                        Array.Resize(ref buffer, read);
                    }

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult Write(byte[] buffer)
            {
                try
                {
                    _stream.Write(buffer, 0, buffer.Length);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult Flush()
            {
                try
                {
                    _stream.Flush();

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult Close()
            {
                try
                {
                    _stream.Close();

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
                    try
                    {
                        if (_stream != null)
                        {
                            _stream.Dispose();
                            _stream = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Warn("Exception while disposing stream", ex);
                    }

                    _disposed = true;
                }

                base.Dispose(disposing);
            }
        }

        private class InteropStreamWrapper : Stream
        {
            private IStream _stream;
            private readonly bool _closeBaseStream;
            private StreamState? _streamState;
            private bool _disposed;

            public override bool CanRead
            {
                get { return (State & StreamState.Readable) != 0; }
            }

            public override bool CanSeek
            {
                get { return (State & StreamState.Seekable) != 0; }
            }

            public override bool CanWrite
            {
                get { return (State & StreamState.Writable) != 0; }
            }

            private StreamState State
            {
                get
                {
                    if (!_streamState.HasValue)
                        _streamState = _stream.State;

                    return _streamState.Value;
                }
            }

            public InteropStreamWrapper(IStream stream, bool closeBaseStream)
            {
                _stream = stream;
                _closeBaseStream = closeBaseStream;
            }

            public override void Flush()
            {
                ErrorUtil.ThrowOnFailure(_stream.Flush());
            }

            public override long Length
            {
                get
                {
                    long result;
                    ErrorUtil.ThrowOnFailure(_stream.GetLength(out result));

                    return result;
                }
            }

            public override long Position
            {
                get
                {
                    long result;
                    ErrorUtil.ThrowOnFailure(_stream.GetPosition(out result));

                    return result;
                }
                set
                {
                    Seek(value, SeekOrigin.Begin);
                }
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (buffer == null)
                    throw new ArgumentNullException("buffer");
                if (offset < 0)
                    throw new ArgumentOutOfRangeException("offset");
                if (count <= 0 || buffer.Length < offset + count)
                    throw new ArgumentOutOfRangeException("count");

                byte[] marshalBuffer;
                ErrorUtil.ThrowOnFailure(_stream.Read(count, out marshalBuffer));

                if (marshalBuffer == null)
                    return 0;

                Array.Copy(marshalBuffer, 0, buffer, offset, Math.Min(marshalBuffer.Length, count));

                return marshalBuffer.Length;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                ErrorUtil.ThrowOnFailure(_stream.SetPosition(offset, origin));

                return Position;
            }

            public override void SetLength(long value)
            {
                ErrorUtil.ThrowOnFailure(_stream.SetLength(value));
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                if (buffer == null)
                    throw new ArgumentNullException("buffer");
                if (offset < 0)
                    throw new ArgumentOutOfRangeException("offset");
                if (count <= 0 || buffer.Length < offset + count)
                    throw new ArgumentOutOfRangeException("count");

                var marshalBuffer = buffer;

                if (offset != 0 || buffer.Length != count)
                {
                    marshalBuffer = new byte[count];

                    Array.Copy(buffer, offset, marshalBuffer, 0, count);
                }

                ErrorUtil.ThrowOnFailure(_stream.Write(marshalBuffer));
            }

            public override void Close()
            {
                // The base Dispose calls Close, so the != null check.

                if (_stream != null && _closeBaseStream)
                    ErrorUtil.ThrowOnFailure(_stream.Close());

                base.Close();
            }

            protected override void Dispose(bool disposing)
            {
                if (!_disposed && disposing)
                {
                    if (_stream != null && _closeBaseStream)
                    {
                        _stream.Dispose();
                        _stream = null;
                    }

                    _disposed = true;
                }

                base.Dispose(disposing);
            }
        }
    }
}

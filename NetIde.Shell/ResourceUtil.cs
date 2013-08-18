using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public static class ResourceUtil
    {
        public static IResource FromManifestResourceStream(Assembly assembly, string resourceName)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            if (resourceName == null)
                throw new ArgumentNullException("resourceName");

            return new AssemblyResource(assembly, resourceName);
        }

        public static IResource FromFile(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            return new FileResource(fileName);
        }

        public static IResource FromByteArray(byte[] byteArray)
        {
            if (byteArray == null)
                throw new ArgumentNullException("byteArray");

            return new ByteArrayResource(byteArray);
        }

        private class AssemblyResource : ServiceObject, IResource
        {
            private readonly Assembly _assembly;
            private readonly string _resourceName;

            public string FileName
            {
                get { return null; }
            }

            public AssemblyResource(Assembly assembly, string resourceName)
            {
                _assembly = assembly;
                _resourceName = resourceName;
            }

            public HResult Open(out IStream stream)
            {
                stream = null;

                try
                {
                    stream = StreamUtil.FromManifestResourceStream(_assembly, _resourceName);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public string Key
            {
                get { return "assembly://" + _assembly.GetName().Name + "/" + _resourceName; }
            }
        }

        private class FileResource : ServiceObject, IResource
        {
            public string FileName { get; private set; }

            public FileResource(string fileName)
            {
                FileName = fileName;
            }

            public HResult Open(out IStream stream)
            {
                stream = null;

                try
                {
                    stream = StreamUtil.FromFile(FileName, FileMode.Open);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public string Key
            {
                get { return "file:///" + Path.GetFullPath(FileName); }
            }
        }

        private class ByteArrayResource : ServiceObject, IResource
        {
            private readonly byte[] _byteArray;

            public string FileName
            {
                get { return null; }
            }

            public ByteArrayResource(byte[] byteArray)
            {
                _byteArray = byteArray;
            }

            public HResult Open(out IStream stream)
            {
                stream = null;

                try
                {
                    stream = StreamUtil.FromByteArray(_byteArray);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public string Key
            {
                get { return "object:///" + _byteArray.GetHashCode().ToString(CultureInfo.InvariantCulture); }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Update
{
    internal class NiRegistrationContext : MarshalByRefObject, INiRegistrationContext
    {
        public string Context { get; private set; }
        public string PackageId { get; private set; }
        public string FileSystemRoot { get; private set; }
        public string RegistryRoot { get; private set; }

        public NiRegistrationContext(string context, string packageId, string fileSystemRoot)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (packageId == null)
                throw new ArgumentNullException("packageId");
            if (fileSystemRoot == null)
                throw new ArgumentNullException("fileSystemRoot");

            Context = context;
            PackageId = packageId;
            FileSystemRoot = fileSystemRoot;
            RegistryRoot = context;
        }

        public INiRegistrationKey CreateKey(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            using (var contextKey = PackageRegistry.OpenRegistryRoot(true, RegistryRoot))
            {
                return new NiRegistrationKey(contextKey.CreateSubKey(name));
            }
        }

        public void RemoveKey(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            using (var contextKey = PackageRegistry.OpenRegistryRoot(true, RegistryRoot))
            {
                contextKey.DeleteSubKeyTree(name);
            }
        }

        public void RemoveKeyIfEmpty(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            using (var contextKey = PackageRegistry.OpenRegistryRoot(false, RegistryRoot))
            using (var key = contextKey.OpenSubKey(name))
            {
                if (key == null)
                    return;

                int values = key.GetValueNames().Length;
                int subKeys = key.GetSubKeyNames().Length;

                if (values > 0 || subKeys > 0)
                    return;
            }

            RemoveKey(name);
        }

        public void RemoveValue(string key, string value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (value == null)
                throw new ArgumentNullException("value");

            using (var contextKey = PackageRegistry.OpenRegistryRoot(true, RegistryRoot))
            using (var registryKey = contextKey.OpenSubKey(key, true))
            {
                if (registryKey != null)
                    registryKey.DeleteValue(value);
            }
        }
    }
}

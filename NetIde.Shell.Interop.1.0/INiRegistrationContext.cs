using System.Text;
using System.Collections.Generic;
using System;

namespace NetIde.Shell.Interop
{
    public interface INiRegistrationContext
    {
        string Context { get; }
        string FileSystemRoot { get; }
        string PackageId { get; }

        INiRegistrationKey CreateKey(string name);
        void RemoveKey(string name);
        void RemoveKeyIfEmpty(string name);
        void RemoveValue(string key, string value);
    }
}
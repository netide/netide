using System.Text;
using System.Collections.Generic;
using System;

namespace NetIde.Services.PackageManager
{
    internal class PendingUpdate
    {
        public string PackageId { get; private set; }
        public PendingUpdateType Type { get; private set; }

        public PendingUpdate(string packageId, PendingUpdateType type)
        {
            PackageId = packageId;
            Type = type;
        }
    }
}
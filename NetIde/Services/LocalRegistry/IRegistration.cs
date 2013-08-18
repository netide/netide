using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Services.LocalRegistry
{
    internal interface IRegistration
    {
        Guid Package { get; }
        Guid Id { get; }
    }
}

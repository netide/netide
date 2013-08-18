using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Services.LocalRegistry
{
    internal interface ITypeRegistration : IRegistration
    {
        string Type { get; }
    }
}

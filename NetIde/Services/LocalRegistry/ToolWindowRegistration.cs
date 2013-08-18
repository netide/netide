using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Services.LocalRegistry
{
    internal class ToolWindowRegistration : IRegistration
    {
        public Guid Package { get; private set; }
        public Guid Id { get; private set; }
        public bool MultipleInstances { get; private set; }
        public NiToolWindowOrientation Orientation { get; private set; }
        public NiDockStyle DockStyle { get; private set; }
        public bool Transient { get; private set; }

        public ToolWindowRegistration(Guid package, Guid id, bool multipleInstances, NiToolWindowOrientation orientation, NiDockStyle dockStyle, bool transient)
        {
            Package = package;
            Id = id;
            MultipleInstances = multipleInstances;
            Orientation = orientation;
            DockStyle = dockStyle;
            Transient = transient;
        }
    }
}

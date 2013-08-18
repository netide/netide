using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public sealed class ProvideToolWindowAttribute : RegistrationAttribute
    {
        public Type ToolType { get; private set; }
        public NiDockStyle Style { get; set; }
        public NiToolWindowOrientation Orientation { get; set; }
        public bool MultipleInstances { get; set; }
        public bool Transient { get; set; }

        public ProvideToolWindowAttribute(Type toolType)
        {
            if (toolType == null)
                throw new ArgumentNullException("toolType");

            ToolType = toolType;
        }

        public override void Register(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
            string toolWindowGuid = ToolType.GUID.ToString("B").ToUpperInvariant();

            using (var key = packageKey.CreateSubKey("ToolWindows\\" + toolWindowGuid))
            {
                key.SetValue("Style", Style.ToString());
                key.SetValue("Orientation", Orientation.ToString());
                key.SetValue("MultipleInstances", MultipleInstances ? 1 : 0);
                key.SetValue("Transient", Transient ? 1 : 0);
            }
        }

        public override void Unregister(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
        }
    }
}

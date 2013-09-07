using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetIde.VisualStudio.Wizard
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    internal sealed class WizardStepAttribute : Attribute
    {
        public WizardStep Step { get; private set; }

        public WizardStepAttribute(WizardStep step)
        {
            Step = step;
        }
    }
}

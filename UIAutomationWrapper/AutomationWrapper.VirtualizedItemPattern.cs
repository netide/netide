using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace UIAutomationWrapper
{
    partial class AutomationWrapper
    {
        public VirtualizedItemWrapper VirtualizedItem
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new VirtualizedItemWrapper(this, GetPattern<VirtualizedItemPattern>(VirtualizedItemPattern.Pattern));
            }
        }

        public class VirtualizedItemWrapper : PatternWrapper<VirtualizedItemPattern>
        {
            public VirtualizedItemWrapper(AutomationWrapper automationWrapper, VirtualizedItemPattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public void Realize()
            {
                Pattern.Realize();
            }
        }
    }
}

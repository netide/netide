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
        public DockWrapper Dock
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new DockWrapper(this, GetPattern<DockPattern>(DockPattern.Pattern));
            }
        }

        public class DockWrapper : PatternWrapper<DockPattern>
        {
            public DockWrapper(AutomationWrapper automationWrapper, DockPattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public DockPosition DockPosition
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.DockPosition;
                }
                set { Pattern.SetDockPosition(value); }
            }
        }
    }
}

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
        public ToggleWrapper Toggle
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new ToggleWrapper(this, GetPattern<TogglePattern>(TogglePattern.Pattern));
            }
        }

        public class ToggleWrapper : PatternWrapper<TogglePattern>
        {
            public ToggleWrapper(AutomationWrapper automationWrapper, TogglePattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public void Toggle()
            {
                Pattern.Toggle();
            }

            public ToggleState ToggleState
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.ToggleState;
                }
            }
        }
    }
}

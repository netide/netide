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
        public ScrollItemWrapper ScrollItem
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new ScrollItemWrapper(this, GetPattern<ScrollItemPattern>(ScrollItemPattern.Pattern));
            }
        }

        public class ScrollItemWrapper : PatternWrapper<ScrollItemPattern>
        {
            public ScrollItemWrapper(AutomationWrapper automationWrapper, ScrollItemPattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public void ScrollIntoView()
            {
                Pattern.ScrollIntoView();
            }
        }
    }
}

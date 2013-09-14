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
        public ExpandCollapseWrapper ExpandCollapse
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new ExpandCollapseWrapper(this, GetPattern<ExpandCollapsePattern>(ExpandCollapsePattern.Pattern));
            }
        }

        public class ExpandCollapseWrapper : PatternWrapper<ExpandCollapsePattern>
        {
            public ExpandCollapseWrapper(AutomationWrapper automationWrapper, ExpandCollapsePattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public ExpandCollapseState ExpandCollapseState
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.ExpandCollapseState;
                }
            }

            public void Expand()
            {
                Pattern.Expand();
            }

            public void Collapse()
            {
                Pattern.Collapse();
            }
        }
    }
}

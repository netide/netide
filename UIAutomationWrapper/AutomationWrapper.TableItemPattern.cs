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
        public TableItemWrapper TableItem
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new TableItemWrapper(this, GetPattern<TableItemPattern>(TableItemPattern.Pattern));
            }
        }

        public class TableItemWrapper : GridItemWrapper
        {
            public new TableItemPattern Pattern
            {
                get { return (TableItemPattern)base.Pattern; }
            }

            public TableItemWrapper(AutomationWrapper automationWrapper, GridItemPattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public AutomationWrapper[] GetRowHeaderItems()
            {
                return Wrap(Pattern.Current.GetRowHeaderItems());
            }

            public AutomationWrapper[] GetColumnHeaderItems()
            {
                return Wrap(Pattern.Current.GetColumnHeaderItems());
            }
        }
    }
}

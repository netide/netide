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
        public TableWrapper Table
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new TableWrapper(this, GetPattern<TablePattern>(TablePattern.Pattern));
            }
        }

        public class TableWrapper : GridWrapper
        {
            public new TablePattern Pattern
            {
                get { return (TablePattern)base.Pattern; }
            }

            public TableWrapper(AutomationWrapper automationWrapper, GridPattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public AutomationWrapper[] GetRowHeaders()
            {
                return Wrap(Pattern.Current.GetRowHeaders());
            }

            public AutomationWrapper[] GetColumnHeaders()
            {
                return Wrap(Pattern.Current.GetColumnHeaders());
            }

            public RowOrColumnMajor RowOrColumnMajor
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.RowOrColumnMajor;
                }
            }
        }
    }
}

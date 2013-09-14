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
        public GridWrapper Grid
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new GridWrapper(this, GetPattern<GridPattern>(GridPattern.Pattern));
            }
        }

        public class GridWrapper : PatternWrapper<GridPattern>
        {
            public GridWrapper(AutomationWrapper automationWrapper, GridPattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public int RowCount
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.RowCount;
                }
            }

            public int ColumnCount
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.ColumnCount;
                }
            }

            public AutomationWrapper GetItem(int row, int column)
            {
                return Wrap(Pattern.GetItem(row, column));
            }
        }
    }
}

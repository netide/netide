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
        public GridItemWrapper GridItem
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new GridItemWrapper(this, GetPattern<GridItemPattern>(GridItemPattern.Pattern));
            }
        }

        public class GridItemWrapper : PatternWrapper<GridItemPattern>
        {
            public GridItemWrapper(AutomationWrapper automationWrapper, GridItemPattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public int Row
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.Row;
                }
            }

            public int RowSpan
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.RowSpan;
                }
            }

            public int Column
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.Column;
                }
            }

            public int ColumnSpan
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.ColumnSpan;
                }
            }

            public AutomationWrapper ContainingGrid
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Wrap(Pattern.Current.ContainingGrid);
                }
            }
        }
    }
}

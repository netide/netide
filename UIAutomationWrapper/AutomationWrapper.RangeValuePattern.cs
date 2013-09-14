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
        public RangeValueWrapper RangeValue
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new RangeValueWrapper(this, GetPattern<RangeValuePattern>(RangeValuePattern.Pattern));
            }
        }

        public class RangeValueWrapper : PatternWrapper<RangeValuePattern>
        {
            public RangeValueWrapper(AutomationWrapper automationWrapper, RangeValuePattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public bool IsReadOnly
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.IsReadOnly;
                }
            }

            public double LargeChange
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.LargeChange;
                }
            }

            public double Maximum
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.Maximum;
                }
            }

            public double Minimum
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.Minimum;
                }
            }

            public double SmallChange
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.SmallChange;
                }
            }

            public double Value
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.Value;
                }
                set { Pattern.SetValue(value); }
            }
        }
    }
}

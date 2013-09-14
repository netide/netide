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
        public ValueWrapper Value
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new ValueWrapper(this, GetPattern<ValuePattern>(ValuePattern.Pattern));
            }
        }

        public class ValueWrapper : PatternWrapper<ValuePattern>
        {
            public ValueWrapper(AutomationWrapper automationWrapper, ValuePattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public string Value
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.Value;
                }
                set { Pattern.SetValue(value); }
            }

            public bool IsReadOnly
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.IsReadOnly;
                }
            }
        }
    }
}

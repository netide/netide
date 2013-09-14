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
        public MultipleViewWrapper MultipleView
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new MultipleViewWrapper(this, GetPattern<MultipleViewPattern>(MultipleViewPattern.Pattern));
            }
        }

        public class MultipleViewWrapper : PatternWrapper<MultipleViewPattern>
        {
            public MultipleViewWrapper(AutomationWrapper automationWrapper, MultipleViewPattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public int CurrentView
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.CurrentView;
                }
                set { Pattern.SetCurrentView(value); }
            }

            public int[] GetSupportedViews()
            {
                return Pattern.Current.GetSupportedViews();
            }

            public string GetViewName(int viewId)
            {
                return Pattern.GetViewName(viewId);
            }
        }
    }
}

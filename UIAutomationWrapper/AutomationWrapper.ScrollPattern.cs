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
        public ScrollWrapper Scroll
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new ScrollWrapper(this, GetPattern<ScrollPattern>(ScrollPattern.Pattern));
            }
        }

        public class ScrollWrapper : PatternWrapper<ScrollPattern>
        {
            public ScrollWrapper(AutomationWrapper automationWrapper, ScrollPattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public bool HorizontallyScrollable
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.HorizontallyScrollable;
                }
            }

            public double HorizontalScrollPercent
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.HorizontalScrollPercent;
                }
            }

            public double HorizontalViewSize
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.HorizontalViewSize;
                }
            }

            public bool VerticallyScrollable
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.VerticallyScrollable;
                }
            }

            public double VerticalScrollPercent
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.VerticalScrollPercent;
                }
            }

            public double VerticalViewSize
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.VerticalViewSize;
                }
            }

            public void Scroll(ScrollAmount horizontalAmount, ScrollAmount verticalAmount)
            {
                Pattern.Scroll(horizontalAmount, verticalAmount);
            }

            public void ScrollHorizontal(ScrollAmount amount)
            {
                Pattern.ScrollHorizontal(amount);
            }

            public void ScrollVertical(ScrollAmount amount)
            {
                Pattern.ScrollVertical(amount);
            }

            public void SetScrollPercent(double horizontalPercent, double verticalPercent)
            {
                Pattern.SetScrollPercent(horizontalPercent, verticalPercent);
            }
        }
    }
}

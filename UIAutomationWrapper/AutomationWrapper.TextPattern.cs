using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Automation;
using System.Windows.Automation.Text;

namespace UIAutomationWrapper
{
    partial class AutomationWrapper
    {
        public TextWrapper Text
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new TextWrapper(this, GetPattern<TextPattern>(TextPattern.Pattern));
            }
        }

        public class TextWrapper : PatternWrapper<TextPattern>
        {
            public TextWrapper(AutomationWrapper automationWrapper, TextPattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public TextPatternRange DocumentRange
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.DocumentRange;
                }
            }

            public SupportedTextSelection SupportedTextSelection
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.SupportedTextSelection;
                }
            }

            public TextPatternRange[] GetSelection()
            {
                return Pattern.GetSelection();
            }

            public TextPatternRange[] GetVisibleRanges()
            {
                return Pattern.GetVisibleRanges();
            }

            public TextPatternRange RangeFromChild(AutomationWrapper automationWrapper)
            {
                if (automationWrapper == null)
                    throw new ArgumentNullException("automationWrapper");

                return Pattern.RangeFromChild(automationWrapper.AutomationElement);
            }

            public TextPatternRange RangeFromPoint(Point screenLocation)
            {
                return Pattern.RangeFromPoint(new System.Windows.Point(screenLocation.X, screenLocation.Y));
            }
        }
    }
}

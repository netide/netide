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
        public SelectionWrapper Selection
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new SelectionWrapper(this, GetPattern<SelectionPattern>(SelectionPattern.Pattern));
            }
        }

        public class SelectionWrapper : PatternWrapper<SelectionPattern>
        {
            public SelectionWrapper(AutomationWrapper automationWrapper, SelectionPattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public bool CanSelectMultiple
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.CanSelectMultiple;
                }
            }

            public bool IsSelectionRequired
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.IsSelectionRequired;
                }
            }

            public AutomationWrapper[] GetSelection()
            {
                return Wrap(Pattern.Current.GetSelection());
            }

            public event AutomationEventHandler Invalidated
            {
                add { AutomationWrapper.AddEvent(SelectionPattern.InvalidatedEvent, value); }
                remove { AutomationWrapper.RemoveEvent(value); }
            }
        }
    }
}

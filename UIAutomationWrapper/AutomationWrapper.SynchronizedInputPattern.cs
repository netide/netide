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
        public SynchronizedInputWrapper SynchronizedInput
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new SynchronizedInputWrapper(this, GetPattern<SynchronizedInputPattern>(SynchronizedInputPattern.Pattern));
            }
        }

        public class SynchronizedInputWrapper : PatternWrapper<SynchronizedInputPattern>
        {
            public SynchronizedInputWrapper(AutomationWrapper automationWrapper, SynchronizedInputPattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public void StartListening(SynchronizedInputType inputType)
            {
                Pattern.StartListening(inputType);
            }

            public void Cancel()
            {
                Pattern.Cancel();
            }

            public event AutomationEventHandler InputReachedTarget
            {
                add { AutomationWrapper.AddEvent(SynchronizedInputPattern.InputReachedTargetEvent, value); }
                remove { AutomationWrapper.RemoveEvent(value); }
            }

            public event AutomationEventHandler InputDiscarded
            {
                add { AutomationWrapper.AddEvent(SynchronizedInputPattern.InputDiscardedEvent, value); }
                remove { AutomationWrapper.RemoveEvent(value); }
            }

            public event AutomationEventHandler InputReachedOtherElement
            {
                add { AutomationWrapper.AddEvent(SynchronizedInputPattern.InputReachedOtherElementEvent, value); }
                remove { AutomationWrapper.RemoveEvent(value); }
            }
        }
    }
}

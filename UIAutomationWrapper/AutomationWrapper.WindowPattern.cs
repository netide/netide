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
        public WindowWrapper Window
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new WindowWrapper(this, GetPattern<WindowPattern>(WindowPattern.Pattern));
            }
        }

        public class WindowWrapper : PatternWrapper<WindowPattern>
        {
            public WindowWrapper(AutomationWrapper automationWrapper, WindowPattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public WindowVisualState WindowVisualState
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.WindowVisualState;
                }
                set { Pattern.SetWindowVisualState(value); }
            }

            public bool CanMaximize
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.CanMaximize;
                }
            }

            public bool CanMinimize
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.CanMinimize;
                }
            }

            public bool IsModal
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.IsModal;
                }
            }

            public bool IsTopmost
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.IsTopmost;
                }
            }

            public WindowInteractionState WindowInteractionState
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.WindowInteractionState;
                }
            }

            public void Close()
            {
                Pattern.Close();
            }

            public void WaitForInputIdle(int milliseconds)
            {
                Pattern.WaitForInputIdle(milliseconds);
            }
        }
    }
}

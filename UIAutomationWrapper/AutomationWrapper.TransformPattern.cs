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
        public TransformWrapper Transform
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new TransformWrapper(this, GetPattern<TransformPattern>(TransformPattern.Pattern));
            }
        }

        public class TransformWrapper : PatternWrapper<TransformPattern>
        {
            public TransformWrapper(AutomationWrapper automationWrapper, TransformPattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public void Move(double x, double y)
            {
                Pattern.Move(x, y);
            }

            public void Resize(double width, double height)
            {
                Pattern.Resize(width, height);
            }

            public void Rotate(double degrees)
            {
                Pattern.Rotate(degrees);
            }

            public bool CanMove
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.CanMove;
                }
            }

            public bool CanResize
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.CanResize;
                }
            }

            public bool CanRotate
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.CanRotate;
                }
            }
        }
    }
}

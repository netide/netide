using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIAutomationWrapper;

namespace NetIde.Test.Support
{
    internal static class AutomationWrapperExtensions
    {
        public static AutomationWrapper GetNestedChild(this AutomationWrapper self, Func<AutomationWrapper, bool> condition)
        {
            if (self == null)
                throw new ArgumentNullException("self");
            if (condition == null)
                throw new ArgumentNullException("condition");

            while (true)
            {
                var children = self.Children.Where(condition).ToArray();

                if (children.Length == 1)
                    return children[0];

                if (self.Children.Count != 1)
                    throw new ArgumentException("Cannot find element", "condition");

                self = self.Children[0];
            }
        }
    }
}

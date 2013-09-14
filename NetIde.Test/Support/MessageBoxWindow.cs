using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UIAutomationWrapper;

namespace NetIde.Test.Support
{
    internal class MessageBoxWindow
    {
        public AutomationWrapper Window { get; private set; }

        public string Title
        {
            get { return Window.Name; }
        }

        public string Text
        {
            get { return Window.Children[ControlType.Text].Name; }
        }

        public void Click(MessageBoxButton dialogResult)
        {
            Window.FindDescendantByAutomationId(((int)dialogResult).ToString(CultureInfo.InvariantCulture)).Invoke.Invoke();
        }

        public MessageBoxWindow(AutomationWrapper window)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            Window = window;
        }
    }
}

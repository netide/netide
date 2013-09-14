using NUnit.Framework;
using NetIde.Test.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIAutomationWrapper;

namespace NetIde.Test.BasicTests
{
    public class OpenAndCloseFindWindowFixture : TestBase
    {
        [Test]
        public void OpenFindWindow()
        {
            InvokeMainMenuItem("Edit", "Find in Files");

            MainWindow.Children["Find and Replace", ControlType.Window].Window.Close();
        }
    }
}

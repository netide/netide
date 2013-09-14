using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NUnit.Framework;
using NetIde.Test.Support;

namespace NetIde.Test.BasicTests
{
    public class ShutdownTestFixture : TestBase
    {
        [Test]
        public void SuccessfulShutdown()
        {
            InvokeMainMenuItem("File", "Exit");

            Application.WaitForExit(TimeSpan.FromSeconds(3));
        }
    }
}

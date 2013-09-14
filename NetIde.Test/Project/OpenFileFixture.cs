using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NUnit.Framework;
using NetIde.Test.Support;

namespace NetIde.Test.Project
{
    public class OpenFileFixture : ProjectTestBase
    {
        [Test]
        public void OpenAndVerifyFile()
        {
            using (OpenTestProject(true))
            {
                FindProjectNode("Project", "FileA").DoubleClick();

                var editor = FindDockPanel("FileA");

                Assert.IsNotNull(editor);

                editor.Window.Close();

                CloseProject();
            }
        }
    }
}

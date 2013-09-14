using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NUnit.Framework;
using NetIde.Test.Support;
using UIAutomationWrapper;

namespace NetIde.Test.Project
{
    public class OpenProjectFixture : ProjectTestBase
    {
        [Test]
        public void OpenProject()
        {
            using (OpenTestProject(true))
            {
                VerifyAndCloseOpenedProject();
            }
        }

        [Test]
        public void CreateProject()
        {
            using (var project = OpenTestProject(false))
            {
                VerifyAndCloseOpenedProject();

                // Verify that the project file was created.

                Assert.IsTrue(File.Exists(project.ProjectFilePath));
            }
        }

        private void VerifyAndCloseOpenedProject()
        {
            // Verify that the project has been loaded.

            Assert.IsNotNull(FindProjectNode("Project"));

            // Close the project.

            CloseProject();

            // Verify the project has been closed.

            var projectTreeView = FindDockPanel(DockPanelType.ProjectExplorer).GetNestedChild(p => p.ControlType == ControlType.Tree);

            Assert.AreEqual(0, projectTreeView.Children.Count);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using UIAutomationWrapper;
using ControlType = UIAutomationWrapper.ControlType;

namespace NetIde.Test.Support
{
    public abstract class ProjectTestBase : TestBase
    {
        protected Project OpenTestProject(bool createProjectFile)
        {
            var project = ProjectCreator.CreateTestProject(createProjectFile);

            if (!createProjectFile)
            {
                // Create the project.

                InvokeMainMenuItem("File", "New Project");

                var window = MainWindow.Children["Create Project", ControlType.Window];

                window.Descendants()
                    .Single(p => p.Name == "File name:" && p.ControlType == ControlType.ComboBox)
                    .Value.Value = project.ProjectFilePath;

                window.Children["Save"].Invoke.Invoke();
            }
            else
            {
                // Open the project.

                InvokeMainMenuItem("File", "Open Project");

                var window = MainWindow.Children["Open Project", ControlType.Window];

                window.Children["File name:", ControlType.ComboBox].Value.Value = project.ProjectFilePath;
                window.Children["Open"].Invoke.Invoke();
            }

            return project;
        }

        protected void CloseProject()
        {
            InvokeMainMenuItem("File", "Close Project");
        }

        protected AutomationWrapper FindProjectNode(params string[] path)
        {
            var element = FindDockPanel(DockPanelType.ProjectExplorer).GetNestedChild(p => p.ControlType == ControlType.Tree);

            foreach (string part in path)
            {
                element = element.Children[part, ControlType.TreeItem];

                if (element == null)
                    throw new ArgumentException(String.Format("Cannot find project node '{0}'", part), "path");

                var expandCollapse = element.ExpandCollapse;

                if (expandCollapse.ExpandCollapseState == ExpandCollapseState.Collapsed)
                    expandCollapse.Expand();
            }

            return element;
        }
    }
}

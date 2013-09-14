using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using NUnit.Framework;
using UIAutomationWrapper;

namespace NetIde.Test.Support
{
    [TestFixture]
    public abstract class TestBase
    {
        private string _previousCurrentDirectory;
        private AutomationWrapper _requeryElement;

        protected IsolatedApplicationRunner Application { get; private set; }

        protected virtual string[] GetArguments()
        {
            return null;
        }

        public virtual string TestContext
        {
            get { return "NetIdeUnitTests"; }
        }

        protected AutomationWrapper MainWindow { get; private set; }

        protected AutomationWrapper MainMenu { get; private set; }

        protected bool InvokeMainMenuItem(params string[] path)
        {
            RequeryCommandStatuses();

            var element = MainMenu;

            foreach (string part in path)
            {
                element = element.Children.Single(
                    p => p.Name == part && p.ControlType == ControlType.MenuItem
                );

                if (!element.IsEnabled)
                    return false;

                element.Invoke.Invoke();
            }

            return true;
        }

        protected AutomationWrapper GetDockPanelContainer()
        {
            return MainWindow.Children
                .Single(p => p.AutomationId == "_toolStripContainer")
                .GetNestedChild(p => p.AutomationId == "_dockPanel");
        }

        protected AutomationWrapper FindDockPanel(DockPanelType type)
        {
            return FindDockPanel(type.GetDescription());
        }

        protected AutomationWrapper FindDockPanel(string name)
        {
            // The first and second level child panes are just containers.

            foreach (var outerContainer in GetDockPanelContainer().Children)
            {
                foreach (var innerContainer in outerContainer.Children)
                {
                    var pane = innerContainer.Children[name, ControlType.Window];

                    if (pane != null)
                        return pane;
                }
            }

            throw new ArgumentException(String.Format("Dock panel '{0}' not found", name), "name");
        }

        [TestFixtureSetUp]
        protected virtual void TestFixtureSetUp()
        {
            string installationPath = GetInstallationPath();

            _previousCurrentDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = installationPath;

            Application = new IsolatedApplicationRunner();
            Application.Start(GetArguments());

            MainWindow = AutomationWrapper.FromHandle(Application.Handle);
            MainMenu = MainWindow.Children.Single(p => p.ControlType == ControlType.MenuBar);

            // Because we're accessing the application through automation,
            // the requery mechanism doesn't work anymore (it depends on mouse
            // and keyboard actions to be triggered). To work around this, Net IDE
            // exposes a Button with the Guid below as the name of the control.
            // When we click this, a require is fired immediately.

            var handle = InteropUtil.FindWindowInProcess(
                Process.GetCurrentProcess(),
                "89db2dd3-10f4-43f7-a09c-8b1d1038f137"
            );
            _requeryElement = AutomationWrapper.FromHandle(handle);
        }

        private string GetInstallationPath()
        {
            using (var key = Registry.CurrentUser.OpenSubKey("Software\\Net IDE\\" + TestContext))
            {
                return (string)key.GetValue("InstallationPath");
            }
        }

        [TestFixtureTearDown]
        protected virtual void TestFixtureTearDown()
        {
            Application.Stop();

            Environment.CurrentDirectory = _previousCurrentDirectory;
        }

        protected void RequeryCommandStatuses()
        {
            _requeryElement.Invoke.Invoke();
        }

        protected AutomationWrapper FindActiveDocument()
        {
            // The first and second level child panes are just containers.

            foreach (var outerContainer in GetDockPanelContainer().Children)
            {
                foreach (var innerContainer in outerContainer.Children)
                {
                    foreach (var child in innerContainer.Children)
                    {
                        if (
                            child.ControlType == ControlType.Window &&
                            child.Window.IsTopmost
                        )
                            return child;
                    }
                }
            }

            return null;
        }
    }
}

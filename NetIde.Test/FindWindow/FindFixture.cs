using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NUnit.Framework;
using NetIde.Test.Support;
using UIAutomationWrapper;

namespace NetIde.Test.FindWindow
{
    public class FindFixture : ProjectTestBase
    {
        [Test]
        public void FindInCurrentDocument()
        {
            using (OpenTestProject(true))
            {
                Assert.IsFalse(InvokeMainMenuItem("Edit", "Find"));

                FindProjectNode("Project", "FileA").DoubleClick();

                var editorPanel = FindDockPanel("FileA");
                var editor = editorPanel.Children[0].Children[0];

                Assert.IsTrue(InvokeMainMenuItem("Edit", "Find"));

                var findWindow = new FindWindow(MainWindow.Children["Find and Replace", ControlType.Window]);

                Assert.AreEqual(LookInType.CurrentDocument, findWindow.LookInType);

                findWindow.FindWhat = "Aa";
                findWindow.MatchCase = true;
                findWindow.MatchWholeWord = true;
                findWindow.UseRegularExpressions = false;
                findWindow.LookInFileTypes = null;

                findWindow.FindNext();

                var selection = editor.Text.GetSelection();

                Assert.AreEqual(1, selection.Length);
                Assert.AreEqual("Aa", selection[0].GetText(int.MaxValue));

                findWindow.Window.Window.Close();

                editorPanel.Window.Close();

                CloseProject();
            }
        }

        [Test]
        public void FindInAllDocuments()
        {
            using (OpenTestProject(true))
            {
                Assert.IsTrue(InvokeMainMenuItem("Edit", "Find in Files"));

                var findWindow = new FindWindow(MainWindow.Children["Find and Replace", ControlType.Window]);

                Assert.AreEqual(LookInType.EntireProject, findWindow.LookInType);

                findWindow.FindWhat = "A\\da";
                findWindow.MatchCase = true;
                findWindow.MatchWholeWord = true;
                findWindow.UseRegularExpressions = true;
                findWindow.LookInFileTypes = null;

                for (int i = 0; i < 4; i++)
                {
                    findWindow.FindNext();

                    var messageBoxControl = findWindow.Window.Children[ControlType.Window];

                    if (messageBoxControl != null)
                    {
                        Assert.AreEqual(3, i);

                        new MessageBoxWindow(messageBoxControl).Click(MessageBoxButton.Cancel);
                        break;
                    }

                    var editorPanel = FindActiveDocument();

                    Assert.IsNotNull(editorPanel);

                    var editor = editorPanel.Children[0].Children[0];

                    var selection = editor.Text.GetSelection();

                    Assert.AreEqual(1, selection.Length);
                    Assert.AreEqual("A" + (i + 1) + "a", selection[0].GetText(int.MaxValue));
                }

                findWindow.Window.Window.Close();

                CloseProject();
            }
        }

        [Test]
        public void ReplaceAllInCurrentWindow()
        {
            using (OpenTestProject(true))
            {
                Assert.IsFalse(InvokeMainMenuItem("Edit", "Replace"));

                FindProjectNode("Project", "FileA").DoubleClick();

                var editorPanel = FindDockPanel("FileA");
                var editor = editorPanel.Children[0].Children[0];

                Assert.IsTrue(InvokeMainMenuItem("Edit", "Replace"));

                var findWindow = new FindWindow(MainWindow.Children["Find and Replace", ControlType.Window]);

                Assert.AreEqual(LookInType.CurrentDocument, findWindow.LookInType);

                findWindow.FindWhat = "A";
                findWindow.ReplaceWith = "B";
                findWindow.MatchCase = true;
                findWindow.MatchWholeWord = false;
                findWindow.UseRegularExpressions = false;
                findWindow.LookInFileTypes = null;

                findWindow.ReplaceAll();

                MessageBox.Show("Hi");

                Assert.AreEqual(
@"Ba
Bb
Bc
",
                    editor.Value.Value
                );

                findWindow.Window.Window.Close();

                editorPanel.Window.Close();

                CloseProject();
            }
        }
    }
}

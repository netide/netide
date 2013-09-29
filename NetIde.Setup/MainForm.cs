using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using NetIde.Setup.Pages;
using NetIde.Setup.Support;

namespace NetIde.Setup
{
    public partial class MainForm : NetIde.Util.Forms.Form
    {
        private static readonly Page[] _pages = (Page[])Enum.GetValues(typeof(Page));
        private Page _page = Page.Initializing;

        public SetupConfiguration SetupConfiguration { get; private set; }

        public MainForm()
        {
            InitializeComponent();

            Icon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);

            Text = String.Format(Text, Program.Configuration.Title);
        }

        public void SetStep(int offset)
        {
            int index = Array.IndexOf(_pages, _page) + offset;

            if (index >= _pages.Length)
            {
                Dispose();
                return;
            }

            if (!Program.Configuration.ShowPage(_pages[index]))
            {
                if (offset > 0)
                    offset++;
                else
                    offset--;

                SetStep(offset);
                return;
            }

            SetStep(_pages[index]);
        }

        private void SetStep(Page step)
        {
            SetStep(step, null);
        }

        private void SetStep(Page step, Action<PageControl> initializer)
        {
            _page = step;
            
            var field = typeof(Page).GetField(step.ToString());
            var attributes = field.GetCustomAttributes(typeof(PageAttribute), true);
            var pageType = ((PageAttribute)attributes[0]).Type;
            var page = (PageControl)Activator.CreateInstance(pageType);

            if (initializer != null)
                initializer(page);

            page.Dock = DockStyle.Fill;

            var currentControls = new ArrayList(Controls);

            Controls.Add(page);

            foreach (Control control in currentControls)
            {
                Controls.Remove(control);

                control.Dispose();
            }

            AcceptButton = page.AcceptButton;
            CancelButton = page.CancelButton;

            if (IsHandleCreated)
            {
                var acceptButton = AcceptButton as Button;

                if (acceptButton != null)
                    acceptButton.Focus();
                else
                    page.SelectNextControl(page, true, true, true, false);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
                return;

            var page = Controls.Cast<PageControl>().SingleOrDefault();

            if (page != null && !page.CanClose)
            {
                MessageBox.Show(
                    this,
                    Labels.CannotAbortSetup,
                    Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                e.Cancel = true;
            }
            else
            {
                var result = MessageBox.Show(
                    this,
                    String.Format(Labels.AreYouSureQuit, Program.Configuration.Title),
                    Text,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result != DialogResult.Yes)
                    e.Cancel = true;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetupConfiguration = new SetupConfiguration();

            // Only create shortcuts when we don't have an existing context.

            SetupConfiguration.CreateDesktopShortcut =
            SetupConfiguration.CreateStartMenuShortcut =
                Program.Configuration.InstallationPath == null;

            if (Program.Configuration.InstallationPath != null)
            {
                SetupConfiguration.InstallationPath = Program.Configuration.InstallationPath;
            }
            else
            {
                SetupConfiguration.InstallationPath = Path.Combine(
                    NativeMethods.SHGetFolderPath(
                        Handle,
                        NativeMethods.SpecialFolderCSIDL.CSIDL_LOCAL_APPDATA,
                        IntPtr.Zero,
                        0
                    ),
                    "Net IDE",
                    Program.Configuration.Context.Name
                );
            }

            SetupConfiguration.StartMenu = Program.Configuration.StartMenu ?? Program.Configuration.Context.Name;

            SetStep(0);
        }

        public void ShowMessage(MessageSeverity severity, string message)
        {
            SetStep(Page.Message, p => ((MessagePage)p).SetMessage(severity, message));
        }
    }
}

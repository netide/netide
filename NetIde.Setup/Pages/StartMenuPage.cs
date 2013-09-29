using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Setup.Support;

namespace NetIde.Setup.Pages
{
    public partial class StartMenuPage : PageControl
    {
        public override IButtonControl AcceptButton
        {
            get { return _acceptButton; }
        }

        public override IButtonControl CancelButton
        {
            get { return _cancelButton; }
        }

        public StartMenuPage()
        {
            InitializeComponent();

            _formHeader.SubText = String.Format(_formHeader.SubText, Program.Configuration.Title);
        }

        private void StartMenuPage_Load(object sender, EventArgs e)
        {
            _startMenuFolder.Text = SetupConfiguration.StartMenu;
            _createStartMenu.Checked = SetupConfiguration.CreateStartMenuShortcut;
            _createOnDesktop.Checked = SetupConfiguration.CreateDesktopShortcut;

            var startMenuFolders = new List<string>();

            AddStartMenuFolders(startMenuFolders, NativeMethods.SpecialFolderCSIDL.CSIDL_PROGRAMS);
            AddStartMenuFolders(startMenuFolders, NativeMethods.SpecialFolderCSIDL.CSIDL_COMMON_PROGRAMS);

            startMenuFolders.Sort((a, b) => String.Compare(a, b, StringComparison.OrdinalIgnoreCase));

            foreach (string directory in startMenuFolders)
            {
                _startMenuFolders.Items.Add(directory);
            }
        }

        private void AddStartMenuFolders(List<string> startMenuFolders, NativeMethods.SpecialFolderCSIDL specialFolderCSIDL)
        {
            string startMenuFolder = NativeMethods.SHGetFolderPath(
                Handle,
                specialFolderCSIDL,
                IntPtr.Zero,
                0
            );

            foreach (string directory in Directory.GetDirectories(startMenuFolder))
            {
                string path = Path.GetFileName(directory);

                if (!startMenuFolders.Contains(path))
                    startMenuFolders.Add(path);
            }
        }

        private void _previousButton_Click(object sender, EventArgs e)
        {
            MainForm.SetStep(-1);
        }

        private void _acceptButton_Click(object sender, EventArgs e)
        {
            MainForm.SetStep(1);
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            MainForm.Close();
        }

        private void _startMenuFolder_TextChanged(object sender, EventArgs e)
        {
            SetupConfiguration.StartMenu = _startMenuFolder.Text;
        }

        private void _createStartMenu_CheckedChanged(object sender, EventArgs e)
        {
            SetupConfiguration.CreateStartMenuShortcut = _createStartMenu.Checked;
        }

        private void _createOnDesktop_CheckedChanged(object sender, EventArgs e)
        {
            SetupConfiguration.CreateDesktopShortcut = _createOnDesktop.Checked;
        }
    }
}

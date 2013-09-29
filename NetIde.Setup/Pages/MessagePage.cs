using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Setup.Pages
{
    public partial class MessagePage : PageControl
    {
        public override IButtonControl AcceptButton
        {
            get { return _acceptButton; }
        }

        public MessagePage()
        {
            InitializeComponent();
        }

        private void _acceptButton_Click(object sender, EventArgs e)
        {
            MainForm.Dispose();
        }

        public void SetMessage(MessageSeverity severity, string message)
        {
            switch (severity)
            {
                case MessageSeverity.Error:
                    _pictureBox.Image = NeutralResources.error;
                    break;

                case MessageSeverity.Warning:
                    _pictureBox.Image = NeutralResources.warning;
                    break;

                case MessageSeverity.Information:
                    _pictureBox.Image = NeutralResources.information;
                    break;
            }

            _message.Text = message;
        }
    }
}

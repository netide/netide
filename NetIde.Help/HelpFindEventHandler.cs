using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Help
{
    public class HelpFindEventArgs : EventArgs
    {
        public string Text { get; private set; }

        public HelpFindEventArgs(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            Text = text;
        }
    }

    public delegate void HelpFindEventHandler(object sender, HelpFindEventArgs e);
}

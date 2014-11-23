using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetIde.Help
{
    public class HelpResolveEventArgs : EventArgs
    {
        public string Url { get; private set; }
        public Stream Stream { get; set; }

        public HelpResolveEventArgs(string url)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            Url = url;
        }
    }

    public delegate void HelpResolveEventHandler(object sender, HelpResolveEventArgs e);
}

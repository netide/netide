using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Help
{
    public class HelpRegistration
    {
        public string Root { get; private set; }
        public string Source { get; private set; }

        public HelpRegistration(string root, string source)
        {
            if (root == null)
                throw new ArgumentNullException("root");
            if (source == null)
                throw new ArgumentNullException("source");

            Root = root;
            Source = source;
        }
    }
}

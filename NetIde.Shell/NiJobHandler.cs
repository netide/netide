using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public abstract class NiJobHandler : ServiceObject, INiJobHandler
    {
        public string Title { get; private set; }

        protected NiJobHandler(string title)
        {
            if (title == null)
                throw new ArgumentNullException("title");

            Title = title;
        }

        public abstract HResult Perform(INiJob job);
    }
}

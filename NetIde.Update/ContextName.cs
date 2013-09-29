using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Update
{
    public class ContextName
    {
        public string Name { get; private set; }

        public bool Experimental { get; private set; }

        public ContextName(string name)
            : this(name, false)
        {
        }

        public ContextName(string name, bool experimental)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            Name = name;
            Experimental = experimental;
        }
    }
}

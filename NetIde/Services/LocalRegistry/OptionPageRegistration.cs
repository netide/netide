using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Services.LocalRegistry
{
    internal class OptionPageRegistration : ITypeRegistration
    {
        public Guid Package { get; private set; }
        public Guid Id { get; private set; }
        public string Type { get; set; }
        public string CategoryName { get; private set; }
        public string PageName { get; private set; }
        public string CategoryNameResourceKey { get; private set; }
        public string PageNameResourceKey { get; private set; }

        public OptionPageRegistration(Guid package, Guid id, string type, string categoryName, string pageName, string categoryNameResourceKey, string pageNameResourceKey)
        {
            if (categoryName == null)
                throw new ArgumentNullException("categoryName");
            if (pageName == null)
                throw new ArgumentNullException("pageName");

            Package = package;
            Id = id;
            Type = type;
            CategoryName = categoryName;
            PageName = pageName;
            CategoryNameResourceKey = categoryNameResourceKey;
            PageNameResourceKey = pageNameResourceKey;
        }
    }
}

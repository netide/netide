using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public sealed class ProvideOptionPageAttribute : RegistrationAttribute
    {
        public Type PageType { get; private set; }
        public string CategoryName { get; private set; }
        public string PageName { get; private set; }
        public string CategoryNameResourceKey { get; private set; }
        public string PageNameResourceKey { get; private set; }

        public ProvideOptionPageAttribute(Type pageType, string categoryName, string pageName, string categoryNameResourceKey, string pageNameResourceKey)
        {
            if (pageType == null)
                throw new ArgumentNullException("pageType");
            if (categoryName == null)
                throw new ArgumentNullException("categoryName");
            if (pageName == null)
                throw new ArgumentNullException("pageName");

            PageType = pageType;
            CategoryName = categoryName;
            PageName = pageName;
            CategoryNameResourceKey = categoryNameResourceKey;
            PageNameResourceKey = pageNameResourceKey;
        }

        public override void Register(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
            string optionPageGuid = PageType.GUID.ToString("B").ToUpperInvariant();

            using (var key = packageKey.CreateSubKey("OptionPages\\" + optionPageGuid))
            {
                key.SetValue("Class", PageType.AssemblyQualifiedName);
                key.SetValue("CategoryName", CategoryName);
                key.SetValue("PageName", PageName);

                if (CategoryNameResourceKey != null)
                    key.SetValue("CategoryNameResourceKey", CategoryNameResourceKey);
                if (PageNameResourceKey != null)
                    key.SetValue("PageNameResourceKey", PageNameResourceKey);
            }
        }

        public override void Unregister(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NetIde.Shell
{
    internal class WindowsFormsUtils
    {
        public static string GetComponentName(IComponent component, string defaultNameValue)
        {
            if (component == null)
                throw new ArgumentNullException("component");

            if (!String.IsNullOrEmpty(defaultNameValue))
                return defaultNameValue;

            if (component.Site != null)
                return component.Site.Name ?? String.Empty;

            return String.Empty;
        }
    }
}
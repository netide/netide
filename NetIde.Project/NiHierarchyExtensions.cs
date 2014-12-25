using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Project.Interop;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Project
{
    public static class NiHierarchyExtensions
    {
        public static object GetPropertyEx(this INiHierarchy self, int property)
        {
            object result;
            ErrorUtil.ThrowOnFailure(self.GetProperty(property, out result));

            return result;
        }

        public static object GetPropertyEx(this INiHierarchy self, NiHierarchyProperty property)
        {
            return GetPropertyEx(self, (int)property);
        }

        public static void SetPropertyEx(this INiHierarchy self, int property, object value)
        {
            ErrorUtil.ThrowOnFailure(self.SetProperty(property, value));
        }

        public static void SetPropertyEx(this INiHierarchy self, NiHierarchyProperty property, object value)
        {
            SetPropertyEx(self, (int)property, value);
        }

        public static IEnumerable<INiHierarchy> GetChildren(this INiHierarchy self)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            var child = (INiHierarchy)self.GetPropertyEx(NiHierarchyProperty.FirstChild);

            while (child != null)
            {
                yield return child;

                child = (INiHierarchy)child.GetPropertyEx(NiHierarchyProperty.NextSibling);
            }
        }

        public static INiHierarchy FindNext(this INiHierarchy self)
        {
            // Descend into children.

            var firstChild = (INiHierarchy)self.GetPropertyEx(NiHierarchyProperty.FirstChild);

            if (firstChild != null)
                return firstChild;

            // Find the next sibling.

            var nextSibling = (INiHierarchy)self.GetPropertyEx(NiHierarchyProperty.NextSibling);

            if (nextSibling != null)
                return nextSibling;

            // Look at the parents.

            var root = self;
            var parent = (INiHierarchy)self.GetPropertyEx(NiHierarchyProperty.Parent);

            while (parent != null)
            {
                // If the parent has a next sibling, return that.

                nextSibling = (INiHierarchy)parent.GetPropertyEx(NiHierarchyProperty.NextSibling);

                if (nextSibling != null)
                    return nextSibling;

                // Else, look at the parent of the parent.

                root = parent;
                parent = (INiHierarchy)parent.GetPropertyEx(NiHierarchyProperty.Parent);
            }

            // If the root didn't have a next sibling, we re-start from the root.
            // If we started at the root (i.e. the root is the only hierarchy),
            // we don't return it.

            if (root != self)
                return root;

            return null;
        }

        public static INiHierarchy FindPrevious(this INiHierarchy self)
        {
            var parent = (INiHierarchy)self.GetPropertyEx(NiHierarchyProperty.Parent);

            // Look at the parent.

            if (parent != null)
            {
                // Find the previous sibling of the parent.

                INiHierarchy previousSibling = null;

                foreach (var child in parent.GetChildren())
                {
                    if (child == self)
                        break;

                    previousSibling = child;
                }

                // If the parent had a previous sibling, find the last child
                // if that sibling.

                if (previousSibling != null)
                    return GetLastChild(previousSibling);

                // Else, return the parent.

                return parent;
            }

            // We're at the root. In that case, descend into the last child of the root.

            var lastChild = GetLastChild(self);

            // Don't return the last child if it's the same as the root
            // (i.e. is the only node).

            if (self == lastChild)
                return null;

            return lastChild;
        }

        private static INiHierarchy GetLastChild(INiHierarchy hier)
        {
            var lastChild = hier.GetChildren().LastOrDefault();

            while (lastChild != null)
            {
                hier = lastChild;
                lastChild = hier.GetChildren().LastOrDefault();
            }

            return hier;
        }

        public static INiHierarchy FindByDocument(this INiHierarchy self, string document)
        {
            var projectItem = self as INiProjectItem;

            if (projectItem != null)
            {
                string fileName;
                ErrorUtil.ThrowOnFailure(projectItem.GetFileName(out fileName));

                if (fileName != null && String.Equals(document, fileName, StringComparison.OrdinalIgnoreCase))
                    return self;
            }

            foreach (var child in self.GetChildren())
            {
                var result = FindByDocument(child, document);

                if (result != null)
                    return result;
            }

            return null;
        }
    }
}

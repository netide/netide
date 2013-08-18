using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using NetIde.Util.Win32;

namespace NetIde.Util.Forms
{
    public static class VisualStyleUtil
    {
        public static void ApplyExplorerTheme(this System.Windows.Forms.TreeView treeView)
        {
            if (treeView == null)
                throw new ArgumentNullException("treeView");

            if (Application.VisualStyleState != VisualStyleState.NoneEnabled)
                NativeMethods.SetWindowTheme(new HandleRef(treeView, treeView.Handle), "explorer", null);
        }

        public static void ApplyExplorerTheme(this ListView listView)
        {
            if (listView == null)
                throw new ArgumentNullException("listView");

            if (Application.VisualStyleState != VisualStyleState.NoneEnabled)
                NativeMethods.SetWindowTheme(new HandleRef(listView, listView.Handle), "explorer", null);
        }
    }
}

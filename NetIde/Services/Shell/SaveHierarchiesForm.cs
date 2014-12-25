using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Project;
using NetIde.Project.Interop;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.Shell
{
    internal partial class SaveHierarchiesForm : DialogForm
    {
        private SaveHierarchiesForm()
        {
            InitializeComponent();
        }

        public static DialogResult ShowDialog(IServiceProvider serviceProvider, IEnumerable<INiHierarchy> hierarchies)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");
            if (hierarchies == null)
                throw new ArgumentNullException("hierarchies");

            var root = new Node(null);

            foreach (var hier in hierarchies)
            {
                InsertNode(root, hier);
            }

            using (var dialog = new SaveHierarchiesForm())
            {
                AddNodes(dialog._listBox, root, 0);

                return dialog.ShowDialog(serviceProvider);
            }
        }

        private static void AddNodes(ListBox listBox, Node node, int indent)
        {
            foreach (var child in node.Children.OrderBy(p => p.Value))
            {
                listBox.Items.Add(new string(' ', indent * 4) + child.Value.Name);

                AddNodes(listBox, child.Value, indent + 1);
            }
        }

        private static Node InsertNode(Node node, INiHierarchy hier)
        {
            var parent = (INiHierarchy)hier.GetPropertyEx(NiHierarchyProperty.Parent);

            if (parent != null)
                node = InsertNode(node, parent);

            Node result;

            if (!node.Children.TryGetValue(hier, out result))
            {
                result = new Node(hier);

                node.Children.Add(hier, result);
            }

            return result;
        }

        private void _yes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }

        private void _no_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        private void _cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private class Node : IComparable<Node>
        {
            public string Name { get; private set; }
            public Dictionary<INiHierarchy, Node> Children { get; private set; }

            public Node(INiHierarchy hierarchy)
            {
                if (hierarchy != null)
                    Name = (string)hierarchy.GetPropertyEx(NiHierarchyProperty.Name);

                Children = new Dictionary<INiHierarchy, Node>();
            }

            public int CompareTo(Node other)
            {
                if ((Children.Count != 0) != (other.Children.Count != 0))
                    return Children.Count == 0 ? 1 : -1;

                return String.Compare(Name, other.Name, StringComparison.CurrentCultureIgnoreCase);
            }
        }
    }
}

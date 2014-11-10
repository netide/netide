using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell.Interop;

namespace NetIde.Services.CommandManager.Controls
{
    internal class GroupControl : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private bool _disposed;

        public IBarControl Bar { get; private set; }
        public NiCommandBarGroup Group { get; private set; }
        public ToolStripSeparator Separator { get; private set; }

        public GroupControl(IServiceProvider serviceProvider, IBarControl bar, NiCommandBarGroup group)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");
            if (bar == null)
                throw new ArgumentNullException("bar");
            if (group == null)
                throw new ArgumentNullException("group");

            _serviceProvider = serviceProvider;
            Bar = bar;
            Group = group;
            Group.CommandsChanged += Group_CommandsChanged;
            Group.AppearanceChanged += Group_AppearanceChanged;

            Separator = new ToolStripSeparator
            {
                Tag = this,
                Alignment = GetAlignment()
            };

            var items = Bar.Items;
            int insertIndex = items.Count;

            for (int i = 0; i < items.Count; i++)
            {
                var separator = items[i] as ToolStripSeparator;

                if (separator == null)
                    continue;

                var otherGroup = (GroupControl)separator.Tag;

                if (Group.Priority < otherGroup.Group.Priority)
                {
                    insertIndex = i;
                    break;
                }
            }

            items.Insert(insertIndex, Separator);

            foreach (NiCommandBarControl command in Group.Controls)
            {
                CreateControl(command);
            }

            Bar.UpdateSeparatorVisibility();
        }

        void Group_AppearanceChanged(object sender, EventArgs e)
        {
            var items = Bar.Items;
            int offset = items.IndexOf(Separator) + 1;
            var count = Group.Controls.Count;
            var alignment = GetAlignment();

            Separator.Alignment = alignment;

            for (int i = 0; i < count; i++)
            {
                Bar.Items[offset + i].Alignment = alignment;
            }
        }

        private ToolStripItemAlignment GetAlignment()
        {
            return Group.Align == NiCommandBarGroupAlign.Left
                ? ToolStripItemAlignment.Left
                : ToolStripItemAlignment.Right;
        }

        void Group_CommandsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                RemoveAllItems();
            }
            else
            {
                if (e.OldItems != null)
                {
                    foreach (NiCommandBarControl command in e.OldItems)
                    {
                        RemoveItem(IndexOfCommand(command));
                    }
                }

                if (e.NewItems != null)
                {
                    foreach (NiCommandBarControl command in e.NewItems)
                    {
                        CreateControl(command);
                    }
                }
            }
        }

        private int IndexOfCommand(NiCommandBarControl command)
        {
            var items = Bar.Items;

            for (int i = 0; i < items.Count; i++)
            {
                var control = items[i].Tag as ControlControl;

                if (control != null && control.NiCommand == command)
                    return i;
            }

            throw new InvalidOperationException();
        }

        private void CreateControl(NiCommandBarControl command)
        {
            ControlControl control;

            if (command is NiCommandBarButton)
                control = Bar.CreateButton(_serviceProvider, (NiCommandBarButton)command);
            else if (command is NiCommandBarComboBox)
                control = Bar.CreateComboBox(_serviceProvider, (NiCommandBarComboBox)command);
            else if (command is NiCommandBarTextBox)
                control = Bar.CreateTextBox(_serviceProvider, (NiCommandBarTextBox)command);
            else if (command is NiCommandBarPopup)
                control = Bar.CreatePopup(_serviceProvider, (NiCommandBarPopup)command);
            else if (command is NiCommandBarLabel)
                control = Bar.CreateLabel(_serviceProvider, (NiCommandBarLabel)command);
            else
                throw new NotSupportedException();

            command.AppearanceChanged += command_AppearanceChanged;

            var items = Bar.Items;
            int insertIndex = items.Count;

            for (int i = items.IndexOf(Separator) + 1; i < items.Count; i++)
            {
                var otherControl = items[i].Tag as ControlControl;

                if (
                    items[i] is ToolStripSeparator ||
                    (otherControl != null && command.Priority < otherControl.NiCommand.Priority)
                ) {
                    insertIndex = i;
                    break;
                }
            }

            control.Item.Alignment = GetAlignment();

            items.Insert(insertIndex, control.Item);

            Bar.UpdateSeparatorVisibility();
        }

        void command_AppearanceChanged(object sender, EventArgs e)
        {
            Bar.UpdateSeparatorVisibility();
        }

        private void RemoveAllItems()
        {
            var items = Bar.Items;
            int index = items.IndexOf(Separator) + 1;

            while (
                index < items.Count &&
                !(items[index] is ToolStripSeparator)
            )
                RemoveItem(index);
        }

        private void RemoveItem(int index)
        {
            var control = (ControlControl)Bar.Items[index].Tag;

            control.NiCommand.AppearanceChanged += command_AppearanceChanged;
            control.Dispose();

            Bar.UpdateSeparatorVisibility();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (Separator != null)
                {
                    RemoveAllItems();

                    Bar.Items.Remove(Separator);

                    Group.CommandsChanged -= Group_CommandsChanged;
                    Group.AppearanceChanged -= Group_AppearanceChanged;

                    Separator = null;
                }

                _disposed = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Services.CommandManager.Controls
{
    internal class GroupManager : IDisposable
    {
        private bool _disposed;
        private ICommandBarContainer _container;
        private readonly IServiceProvider _serviceProvider;
        private readonly IBarControl _control;

        public GroupManager(ICommandBarContainer container, IServiceProvider serviceProvider, IBarControl control)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");
            if (control == null)
                throw new ArgumentNullException("control");

            _container = container;
            _serviceProvider = serviceProvider;
            _control = control;

            foreach (NiCommandBarGroup group in _container.Controls)
            {
                new GroupControl(_serviceProvider, _control, group);
            }

            _container.ItemsChanged += _container_ItemsChanged;
        }

        void _container_ItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                var separators = _control.Items.OfType<ToolStripSeparator>().ToArray();

                foreach (var separator in separators)
                {
                    ((GroupControl)separator.Tag).Dispose();
                }
            }
            else
            {
                if (e.OldItems != null)
                {
                    foreach (NiCommandBarGroup group in e.OldItems)
                    {
                        GetGroupControl(group).Dispose();
                    }
                }

                if (e.NewItems != null)
                {
                    foreach (NiCommandBarGroup group in e.NewItems)
                    {
                        new GroupControl(_serviceProvider, _control, group);
                    }
                }
            }
        }

        private GroupControl GetGroupControl(NiCommandBarGroup group)
        {
            return _control.Items
                .OfType<ToolStripSeparator>()
                .Select(p => (GroupControl)p.Tag)
                .Single(p => p.Group == group);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_container != null)
                {
                    _container.ItemsChanged -= _container_ItemsChanged;
                    _container = null;
                }

                _disposed = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.CommandManager
{
    internal sealed class NiCommandBarGroup : ServiceObject, INiCommandBarGroup
    {
        private NiCommandBarGroupAlign _align;

        public Guid Id { get; private set; }

        public int Priority { get; private set; }

        public NiCommandBarGroupAlign Align
        {
            get { return _align; }
            set
            {
                if (_align != value)
                {
                    _align = value;
                    OnAppearanceChanged(EventArgs.Empty);
                }
            }
        }

        public INiList<INiCommandBarControl> Controls { get; private set; }

        internal event NotifyCollectionChangedEventHandler CommandsChanged;

        private void OnCommandsChanged(NotifyCollectionChangedEventArgs e)
        {
            var ev = CommandsChanged;
            if (ev != null)
                ev(this, e);
        }

        internal event EventHandler AppearanceChanged;

        private void OnAppearanceChanged(EventArgs e)
        {
            var ev = AppearanceChanged;
            if (ev != null)
                ev(this, e);
        }

        public NiCommandBarGroup(Guid id, int priority)
        {
            Id = id;
            Priority = priority;

            var collection = new NiList<INiCommandBarControl>();

            collection.CollectionChanged += (s, e) => OnCommandsChanged(e);

            Controls = collection;
        }
    }
}

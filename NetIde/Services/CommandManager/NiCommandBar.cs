using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetIde.Services.CommandManager.Controls;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.CommandManager
{
    internal class NiCommandBar : NiCommandBarControl, INiCommandBar, ICommandBarContainer
    {
        private IResource _image;
        private Image _bitmap;
        private NiCommandDisplayStyle _displayStyle;

        public INiList<INiCommandBarGroup> Controls { get; private set; }

        public NiCommandBarKind Kind { get; private set; }

        public NiCommandDisplayStyle DisplayStyle
        {
            get { return _displayStyle; }
            set
            {
                if (_displayStyle != value)
                {
                    _displayStyle = value;
                    OnAppearanceChanged(EventArgs.Empty);
                }
            }
        }

        public IResource Image
        {
            get { return _image; }
            set
            {
                if (_image != value)
                {
                    _image = value;
                    OnAppearanceChanged(EventArgs.Empty);
                }
            }
        }

        public Image Bitmap
        {
            get { return _bitmap; }
            set
            {
                if (_bitmap != value)
                {
                    _bitmap = value;
                    OnAppearanceChanged(EventArgs.Empty);
                }
            }
        }

        public event NotifyCollectionChangedEventHandler ItemsChanged;

        private void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            var ev = ItemsChanged;

            if (ev != null)
                ev(this, e);
        }

        public BarControl Control { get; set; }

        public NiCommandBar(Guid id, NiCommandBarKind kind, int priority)
            : base(id, priority)
        {
            Kind = kind;

            var collection = new NiList<INiCommandBarGroup>();

            collection.CollectionChanged += (s, e) => OnItemsChanged(e);

            Controls = collection;
        }
    }
}

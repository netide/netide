using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetIde.Shell.Interop;

namespace NetIde.Services.CommandManager
{
    internal class NiCommandBarButton : NiCommandBarControl, INiCommandBarButton
    {
        private NiCommandDisplayStyle _displayStyle;
        private IResource _image;
        private Image _bitmap;
        private Keys _shortcutKeys;
        private bool _isLatched;

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

        public Keys ShortcutKeys
        {
            get { return _shortcutKeys; }
            set
            {
                if (_shortcutKeys != value)
                {
                    _shortcutKeys = value;
                    OnAppearanceChanged(EventArgs.Empty);
                }
            }
        }

        public bool IsLatched
        {
            get { return _isLatched; }
            set
            {
                if (_isLatched != value)
                {
                    _isLatched = value;
                    OnAppearanceChanged(EventArgs.Empty);
                }
            }
        }

        public NiCommandBarButton(Guid id, int priority)
            : base(id, priority)
        {
        }
    }
}

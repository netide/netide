using NetIde.Shell;
using NetIde.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Services.WindowPaneSelection
{
    internal class NiWindowPaneSelection : ServiceBase, INiWindowPaneSelection
    {
        private readonly NiConnectionPoint<INiWindowPaneSelectionNotify> _connectionPoint = new NiConnectionPoint<INiWindowPaneSelectionNotify>();
        private INiWindowPane _activeDocument;

        public INiWindowPane ActiveDocument
        {
            get { return _activeDocument; }
            set
            {
                if (_activeDocument != value)
                {
                    _activeDocument = value;
                    _connectionPoint.ForAll(p => p.OnActiveDocumentChanged());
                }
            }
        }

        public NiWindowPaneSelection(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public HResult Advise(object sink, out int cookie)
        {
            return _connectionPoint.Advise(sink, out cookie);
        }

        public HResult Advise(INiWindowPaneSelectionNotify sink, out int cookie)
        {
            return Advise((object)sink, out cookie);
        }

        public HResult Unadvise(int cookie)
        {
            return _connectionPoint.Unadvise(cookie);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal class DiffEditorButtonEventArgs : EventArgs
    {
        public IDiffMarker Marker { get; private set; }
        public DiffEditorButtonType Type { get; private set; }

        public DiffEditorButtonEventArgs(IDiffMarker marker, DiffEditorButtonType type)
        {
            Marker = marker;
            Type = type;
        }
    }

    internal delegate void DiffEditorButtonEventHandler(object sender, DiffEditorButtonEventArgs e);
}

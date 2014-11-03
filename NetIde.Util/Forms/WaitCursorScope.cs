using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Util.Forms
{
    public class WaitCursorScope : IDisposable
    {
        private bool _disposed;

        public WaitCursorScope()
        {
            // Only setting both the current cursor and UseWaitCursor works
            // dependently.

            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Application.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;

                _disposed = true;
            }
        }
    }
}

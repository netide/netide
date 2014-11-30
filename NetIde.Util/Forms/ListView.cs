using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Util.Win32;

namespace NetIde.Util.Forms
{
    public class ListView : System.Windows.Forms.ListView
    {
        public event ContextEventHandler Context;

        [Category("Behavior")]
        [DefaultValue(false)]
        public new bool DoubleBuffered
        {
            get { return base.DoubleBuffered; }
            set { base.DoubleBuffered = value; }
        }

        protected virtual void OnContext(ContextEventArgs e)
        {
            var ev = Context;
            if (ev != null)
                ev(this, e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_CONTEXTMENU)
            {
                int x = NativeMethods.Util.SignedLOWORD(m.LParam);
                int y = NativeMethods.Util.SignedHIWORD(m.LParam);

                Point point;
                if ((int)m.LParam == -1)
                    point = new Point(Width / 2, Height / 2);
                else
                    point = PointToClient(new Point(x, y));

                if (ClientRectangle.Contains(point))
                {
                    var e = new ContextEventArgs(point);
                    OnContext(e);
                    if (e.Handled)
                        return;
                }
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// Removes all list view items without calling Items.Clear.
        /// </summary>
        /// <remarks>
        /// The reason for using this method is that the scroll position is maintained
        /// when clearing a list view in this manner. Items.Clear resets the
        /// scroll position to the top of the list view.
        /// </remarks>
        public void ClearItems()
        {
            while (Items.Count > 0)
            {
                Items.RemoveAt(Items.Count - 1);
            }
        }
    }
}

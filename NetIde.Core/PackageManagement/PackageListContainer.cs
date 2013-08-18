using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Core.PackageManagement
{
    internal class PackageListContainer : Panel
    {
        protected override void OnLayout(LayoutEventArgs e)
        {
            int offset = AutoScrollPosition.Y;

            foreach (Control control in Controls)
            {
                var size = control.GetPreferredSize(new Size(ClientSize.Width, int.MaxValue));

                control.SetBounds(
                    0,
                    offset,
                    ClientSize.Width,
                    size.Height
                );

                offset += size.Height;
            }

            bool vScroll = VScroll;

            AdjustFormScrollbars(AutoScroll);

            if (vScroll != VScroll)
                BeginInvoke(new Action(PerformLayout));
        }
    }
}

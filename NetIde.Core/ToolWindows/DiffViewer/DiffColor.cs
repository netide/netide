using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal class DiffColor
    {
        public static DiffColor Added = new DiffColor(Color.FromArgb(200, 255, 200), Color.Green);
        public static DiffColor Removed = new DiffColor(Color.FromArgb(255, 200, 200), Color.Red);
        public static DiffColor Changed = new DiffColor(Color.FromArgb(202, 194, 255), Color.Blue);

        public Color LightColor { get; private set; }
        public Color DarkColor { get; private set; }
        public Pen LightPen { get; private set; }
        public Pen DarkPen { get; private set; }
        public Brush LightBrush { get; private set; }
        public Brush DarkBrush { get; private set; }

        private DiffColor(Color lightColor, Color darkColor)
        {
            LightColor = lightColor;
            DarkColor = darkColor;
            LightPen = new Pen(lightColor);
            DarkPen = new Pen(darkColor);
            LightBrush = new SolidBrush(lightColor);
            DarkBrush = new SolidBrush(darkColor);
        }
    }
}

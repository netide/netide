using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Util.Forms;

namespace NetIde.Core.OptionPages
{
    internal class OptionPageControl : NetIde.Util.Forms.UserControl
    {
        public event EventHandler Activate;

        protected virtual void OnActivate(EventArgs e)
        {
            var ev = Activate;
            if (ev != null)
                ev(this, e);
        }

        public void RaiseActivate()
        {
            OnActivate(EventArgs.Empty);
        }

        public event CancelEventHandler Deactivate;

        protected virtual void OnDeactivate(CancelEventArgs e)
        {
            var ev = Deactivate;
            if (ev != null)
                ev(this, e);
        }

        public bool RaiseDeactivate()
        {
            var e = new CancelEventArgs();

            OnDeactivate(e);

            return e.Cancel;
        }

        public event EventHandler Apply;

        protected virtual void OnApply(EventArgs e)
        {
            var ev = Apply;
            if (ev != null)
                ev(this, e);
        }

        public void RaiseApply()
        {
            OnApply(EventArgs.Empty);
        }

        public event EventHandler Cancel;

        protected virtual void OnCancel(EventArgs e)
        {
            var ev = Cancel;
            if (ev != null)
                ev(this, e);
        }

        public void RaiseCancel()
        {
            OnCancel(EventArgs.Empty);
        }

        protected void InitializeFontControl(TextBoxBrowser control, Font font)
        {
            InitializeFontControl(control, font, false);
        }

        protected void InitializeFontControl(TextBoxBrowser control, Font font, bool fixedPitchOnly)
        {
            control.ReadOnly = true;
            control.Browse += (s, e) =>
            {
                using (var dialog = new FontDialog())
                {
                    dialog.Font = (Font)control.Tag;
                    dialog.FixedPitchOnly = fixedPitchOnly;

                    if (dialog.ShowDialog(this) == DialogResult.OK)
                        LoadFontControl(control, dialog.Font);
                }
            };

            LoadFontControl(control, font);
        }

        private void LoadFontControl(TextBoxBrowser control, Font font)
        {
            control.Tag = font;
            control.Text = String.Format(
                "{0}, {1}pt, {2}",
                font.FontFamily.Name,
                font.Size,
                GetFontStyle(font.Style)
            );
        }

        private string GetFontStyle(FontStyle fontStyle)
        {
            var styles = new List<string>();

            if ((fontStyle & FontStyle.Bold) != 0)
                styles.Add(Labels.FontBold);
            if ((fontStyle & FontStyle.Italic) != 0)
                styles.Add(Labels.FontItalic);
            if ((fontStyle & FontStyle.Underline) != 0)
                styles.Add(Labels.FontUnderline);
            if ((fontStyle & FontStyle.Strikeout) != 0)
                styles.Add(Labels.FontStrikeout);
            if (styles.Count == 0)
                styles.Add(Labels.FontRegular);

            return String.Join(", ", styles);
        }

        protected void InitializeColorControl(TextBoxBrowser control, Color color)
        {
            control.ReadOnly = true;
            control.Browse += (s, e) =>
            {
                using (var dialog = new ColorDialog())
                {
                    dialog.Color = (Color)control.Tag;

                    if (dialog.ShowDialog(this) == DialogResult.OK)
                        LoadColorControl(control, dialog.Color);
                }
            };

            LoadColorControl(control, color);
        }

        private void LoadColorControl(TextBoxBrowser control, Color color)
        {
            control.TextBox.BackColor = color;
            control.Tag = color;
            control.Text = String.Format("{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        }
    }
}

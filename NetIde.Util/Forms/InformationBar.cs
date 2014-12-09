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
    public class InformationBar : Control
    {
        private static readonly Random Random = new Random();
        private static readonly Image InfoIcon = NeutralResources.information;
        private static readonly Image WarnIcon = NeutralResources.warning;
        private static readonly Image ErrorIcon = NeutralResources.error;
        private static readonly Image LeftImage = NeutralResources.InformationBarLeft;
        private static readonly Image RightImage = NeutralResources.InformationBarRight;
        private static readonly Image CloseImage = NeutralResources.InformationBarClose;
        private const TextFormatFlags FormatFlags = TextFormatFlags.NoPrefix | TextFormatFlags.WordBreak;
        private const int TextOffset = 4;
        private const int ButtonSize = 17;

        private InformationIcon _icon = InformationIcon.None;
        private bool _border = true;
        private bool _randomItemSelected;
        private InformationItem _selectedItem;
        private readonly List<Button> _buttons = new List<Button>();
        private bool _canClose;
        private bool _buttonsMeasureValid;
        private bool _isLink;
        private ButtonState _linkState;
        private Rectangle _textBounds;
        private Font _underlineFont;
        private bool _designMode;

        [Category("Appearance")]
        [DefaultValue(InformationIcon.None)]
        public InformationIcon Icon
        {
            get { return _icon; }
            set
            {
                if (_icon != value)
                {
                    _icon = value;

                    Height = 0;
                    Invalidate();
                }
            }
        }

        public bool IsLink
        {
            get { return _isLink; }
            set
            {
                if (_isLink != value)
                {
                    _isLink = value;

                    Invalidate();
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(true)]
        public bool Border
        {
            get { return _border; }
            set
            {
                if (_border != value)
                {
                    _border = value;

                    Height = 0;
                    Invalidate();
                }
            }
        }

        [Category("Behavior")]
        [DefaultValue(false)]
        public bool CanClose
        {
            get { return _canClose; }
            set
            {
                if (_canClose != value)
                {
                    _canClose = value;
                    RebuildButtons();
                }
            }
        }

        protected override Padding DefaultPadding
        {
            get { return new Padding(4); }
        }

        public InformationItem SelectedItem
        {
            get { return _selectedItem; }
            internal set
            {
                if (_designMode)
                    return;

                // We specifically do not check whether the item has changed. The
                // reason for this is that InformationItem uses this setter to
                // signal that something has changed in the item.

                _selectedItem = value;

                if (value == null)
                {
                    Icon = InformationIcon.None;
                    Text = null;
                    IsLink = false;
                }
                else
                {
                    Icon = value.Icon;
                    Text = value.Text;
                    IsLink = value.IsLink;
                }
            }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public InformationCollection Items { get; private set; }

        [Category("Behavior")]
        [DefaultValue(false)]
        public bool ShowRandomItem { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(false)]
        public new bool TabStop
        {
            get { return base.TabStop; }
            set { base.TabStop = value; }
        }

        public event EventHandler Closed;

        protected virtual void OnClosed(EventArgs e)
        {
            var ev = Closed;
            if (ev != null)
                ev(this, e);
        }

        public event EventHandler ItemClick;

        protected virtual void OnItemClick(EventArgs e)
        {
            var handler = ItemClick;
            if (handler != null)
                handler(this, e);
        }

        public InformationBar()
        {
            _designMode = ControlUtil.GetIsInDesignMode(this);

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);

            Items = new InformationCollection(this);

            TabStop = false;

            UpdateFont();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            UpdateFont();
        }

        private void UpdateFont()
        {
            _underlineFont = new Font(Font, FontStyle.Underline);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            // Select the random item to show the first time we become visible.

            if (Visible && ShowRandomItem && !_randomItemSelected)
            {
                _randomItemSelected = true;

                if (Items.Count > 0)
                    Items[Random.Next(Items.Count)].Selected = true;
            }

            base.OnVisibleChanged(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            Height = 0;
            Invalidate();
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, CalculateHeight(width), specified);
        }
        
        private int CalculateHeight(int width)
        {
            _buttonsMeasureValid = false;

            EnsureButtonMeasureValid();

            int right = Padding.Right;
            if (_buttons.Count > 0)
                right = width - _buttons[0].Bounds.Left;

            width -= Padding.Left + right;

            if (Icon != InformationIcon.None)
                width -= TextOffset + 16;

            var size = TextRenderer.MeasureText(
                Text.Length == 0 ? "W" : Text,
                Font,
                new Size(width, int.MaxValue),
                FormatFlags
            );

            int height = size.Height;
            if (Icon != InformationIcon.None && height < 16)
                height = 16;
            if (_border)
                height++;

            return height + Padding.Vertical;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            EnsureButtonMeasureValid();

            e.Graphics.Clear(SystemColors.Info);

            Image icon = null;

            switch (_icon)
            {
                case InformationIcon.Info:
                    icon = InfoIcon;
                    break;

                case InformationIcon.Warning:
                    icon = WarnIcon;
                    break;

                case InformationIcon.Error:
                    icon = ErrorIcon;
                    break;
            }

            int left = Padding.Left;

            if (icon != null)
            {
                e.Graphics.DrawImage(icon, new Point(left, Padding.Top));
                left += 16 + TextOffset;
            }

            if (Text.Length > 0)
            {
                var height = Height - Padding.Vertical;
                if (_border)
                    height--;

                int right = Padding.Right;
                if (_buttons.Count > 0)
                    right = Width - _buttons[0].Bounds.Left;

                var font =
                    (_linkState & ButtonState.Over) != 0
                    ? _underlineFont
                    : Font;

                var size = TextRenderer.MeasureText(
                    e.Graphics,
                    Text,
                    font,
                    new Size(Width - (left + right), height),
                    FormatFlags
                );

                _textBounds = new Rectangle(
                    left,
                    Padding.Top + (height - size.Height) / 2,
                    size.Width,
                    size.Height
                );

                TextRenderer.DrawText(
                    e.Graphics,
                    Text,
                    font,
                    _textBounds,
                    SystemColors.InfoText,
                    SystemColors.Info,
                    FormatFlags
                );
            }

            if (_border)
            {
                e.Graphics.DrawLine(
                    SystemPens.ControlDark,
                    0, Height - 1, Width, Height - 1
                );
            }

            foreach (var button in _buttons)
            {
                var bounds = button.Bounds;

                e.Graphics.DrawImage(
                    button.Image,
                    new Point(
                        bounds.Left + (bounds.Width - button.Image.Width) / 2, 
                        bounds.Top + (bounds.Height - button.Image.Height) / 2
                    )
                );

                if (button.State.HasFlag(ButtonState.Over))
                {
                    var nwPen = button.State.HasFlag(ButtonState.Down)
                        ? SystemPens.ControlDark
                        : SystemPens.ControlLight;
                    var sePen = button.State.HasFlag(ButtonState.Down)
                        ? SystemPens.ControlLight
                        : SystemPens.ControlDark;

                    e.Graphics.DrawLine(
                        nwPen,
                        bounds.Left, bounds.Top, bounds.Right - 1, bounds.Top
                    );
                    e.Graphics.DrawLine(
                        nwPen,
                        bounds.Left, bounds.Top, bounds.Left, bounds.Bottom - 1
                    );
                    e.Graphics.DrawLine(
                        sePen,
                        bounds.Right - 1, bounds.Top, bounds.Right - 1, bounds.Bottom - 1
                    );
                    e.Graphics.DrawLine(
                        sePen,
                        bounds.Left, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1
                    );
                }
            }
        }

        private void EnsureButtonMeasureValid()
        {
            if (!_buttonsMeasureValid)
            {
                int right = Width - Padding.Right;

                _buttonsMeasureValid = true;

                for (int i = _buttons.Count - 1; i >= 0; i--)
                {
                    _buttons[i].Bounds = new Rectangle(
                        right - ButtonSize,
                        Padding.Top,
                        ButtonSize,
                        ButtonSize
                    );

                    right -= ButtonSize;
                }
            }
        }

        internal void RebuildButtons()
        {
            _buttons.Clear();

            if (Items.Count > 1)
            {
                _buttons.Add(new Button
                {
                    Image = LeftImage,
                    Action = () => SelectNextItem(-1)
                });
                _buttons.Add(new Button
                {
                    Image = RightImage,
                    Action = () => SelectNextItem(1)
                });
            }

            if (CanClose)
            {
                _buttons.Add(new Button
                {
                    Image = CloseImage,
                    Action = () =>
                    {
                        Visible = false;
                        OnClosed(EventArgs.Empty);
                    }
                });
            }

            _buttonsMeasureValid = false;

            Height = 0;
            Invalidate();
        }

        private void SelectNextItem(int offset)
        {
            int index = _selectedItem == null
                ? 0
                : Items.IndexOf(_selectedItem);

            index = (index + offset + Items.Count) % Items.Count;

            Items[index].Selected = true;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            EnsureButtonMeasureValid();

            _linkState &= ~ButtonState.Down;

            foreach (var button in _buttons)
            {
                if (button.Bounds.Contains(e.Location))
                {
                    button.State |= ButtonState.Down;
                    Invalidate(button.Bounds);
                    Capture = true;
                    return;
                }
            }

            if (IsLink && _textBounds.Contains(e.Location))
                _linkState |= ButtonState.Down;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            Button wasOver = null;
            Button over = null;
            Button down = null;

            foreach (var button in _buttons)
            {
                if (button.State.HasFlag(ButtonState.Over))
                    wasOver = button;
                if (button.Bounds.Contains(e.Location))
                    over = button;
                if (button.State.HasFlag(ButtonState.Down))
                    down = button;
            }

            var previousLinkState = _linkState;

            if (IsLink && _textBounds.Contains(e.Location))
                _linkState |= ButtonState.Over;
            else
                _linkState &= ~ButtonState.Over;

            if (previousLinkState != _linkState)
                Invalidate(_textBounds);

            Button newOver = null;

            if (down != null)
            {
                if (down == over)
                    newOver = over;
            }
            else 
            {
                newOver = over;
            }

            if (wasOver != null)
                wasOver.State &= ~ButtonState.Over;
            if (newOver != null)
                newOver.State |= ButtonState.Over;

            if (wasOver != newOver)
            {
                if (wasOver != null)
                    Invalidate(wasOver.Bounds);
                if (newOver != null)
                    Invalidate(newOver.Bounds);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_linkState.HasFlag(ButtonState.Down))
            {
                _linkState &= ~ButtonState.Down;
                Invalidate(_textBounds);
                if (_linkState.HasFlag(ButtonState.Over))
                    PerformItemClick();
                return;
            }

            foreach (var button in _buttons)
            {
                if (button.State.HasFlag(ButtonState.Down))
                {
                    button.State &= ~ButtonState.Down;
                    Capture = false;
                    Invalidate(button.Bounds);

                    if (button.State.HasFlag(ButtonState.Over))
                        button.Action();
                    return;
                }
            }
        }

        private void PerformItemClick()
        {
            OnItemClick(EventArgs.Empty);

            if (SelectedItem != null)
                SelectedItem.PerformClick();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_SETCURSOR && IsLink)
            {
                var location = PointToClient(Cursor.Position);
                if (_textBounds.Contains(location))
                {
                    Cursor.Current = Cursors.Hand;
                    m.Result = (IntPtr)1;
                    return;
                }
            }

            base.WndProc(ref m);
        }

        private class Button
        {
            public Image Image { get; set; }
            public Action Action { get; set; }
            public Rectangle Bounds { get; set; }
            public ButtonState State { get; set; }
        }

        [Flags]
        private enum ButtonState
        {
            None = 0,
            Over = 1,
            Down = 2
        }
    }
}

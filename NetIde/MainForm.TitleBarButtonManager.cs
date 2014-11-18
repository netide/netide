using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CustomChrome;
using NetIde.Services;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde
{
    partial class MainForm
    {
        private class NiTitleBarButtonManager : ServiceBase, INiTitleBarButtonManager
        {
            private static readonly Point ButtonImageOffset = new Point(12, 7);

            private readonly NiMainWindowChrome _mainWindowChrome;
            private readonly Dictionary<int, TitleBarButton> _buttonMap = new Dictionary<int, TitleBarButton>();
            private int _nextCookie = 1;
            private readonly List<TitleBarButton> _buttons = new List<TitleBarButton>();
            private readonly NiConnectionPoint<INiTitleBarButtonManagerNotify> _connectionPoint = new NiConnectionPoint<INiTitleBarButtonManagerNotify>();
            private bool _disposed;

            public NiTitleBarButtonManager(IServiceProvider serviceProvider)
                : base(serviceProvider)
            {
                _mainWindowChrome = (NiMainWindowChrome)serviceProvider.GetService(typeof(INiMainWindowChrome));
            }

            public HResult Advise(object sink, out int cookie)
            {
                return _connectionPoint.Advise(sink, out cookie);
            }

            public HResult Advise(INiTitleBarButtonManagerNotify sink, out int cookie)
            {
                return Advise((object)sink, out cookie);
            }

            public HResult Unadvise(int cookie)
            {
                return _connectionPoint.Unadvise(cookie);
            }

            public HResult AddButton(INiTitleBarButton button, out int cookie)
            {
                cookie = 0;

                try
                {
                    if (button == null)
                        throw new ArgumentNullException("button");

                    cookie = _nextCookie++;

                    var wrapper = new TitleBarButton(this, cookie);
                    wrapper.Update(button);

                    _buttonMap.Add(cookie, wrapper);
                    _buttons.Add(wrapper);

                    RebuildButtons();

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult UpdateButton(int cookie, INiTitleBarButton button)
            {
                try
                {
                    if (button == null)
                        throw new ArgumentNullException("button");

                    TitleBarButton wrapper;
                    if (!_buttonMap.TryGetValue(cookie, out wrapper))
                        return HResult.False;

                    wrapper.Update(button);

                    var formChrome = _mainWindowChrome.FormChrome;
                    if (formChrome != null)
                        formChrome.PaintNonClientArea();

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult RemoveButton(int cookie)
            {
                try
                {
                    TitleBarButton button;
                    if (!_buttonMap.TryGetValue(cookie, out button))
                        return HResult.False;

                    _buttonMap.Remove(cookie);
                    _buttons.Remove(button);

                    button.Dispose();

                    RebuildButtons();

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public void RebuildButtons()
            {
                var formChrome = _mainWindowChrome.FormChrome;
                if (formChrome == null)
                    return;

                var buttons = new List<TitleBarButton>(_buttons);

                buttons.Sort((a, b) =>
                {
                    int result = a.Priority.CompareTo(b.Priority);
                    if (result != 0)
                        return result;

                    return _buttons.IndexOf(a).CompareTo(_buttons.IndexOf(b));
                });

                formChrome.Buttons.Clear();

                foreach (var button in _buttons)
                {
                    button.EnsureImage();

                    button.ChromeButton = new VisualStudioButton
                    {
                        Enabled = button.Enabled,
                        Visible = button.Visible,
                        Tag = button
                    };

                    button.ChromeButton.Click += chromeButton_Click;
                    button.ChromeButton.Paint += chromeButton_Paint;

                    formChrome.Buttons.Add(button.ChromeButton);
                }
            }

            void chromeButton_Click(object sender, EventArgs e)
            {
                var chromeButton = (VisualStudioButton)sender;
                int cookie = ((TitleBarButton)chromeButton.Tag).Cookie;

                _connectionPoint.ForAll(p => p.OnClicked(cookie));
            }

            void chromeButton_Paint(object sender, VisualStudioButtonPaintEventArgs e)
            {
                var chromeButton = (VisualStudioButton)sender;
                var button = (TitleBarButton)chromeButton.Tag;

                if (chromeButton.Enabled && !chromeButton.IsOver && !chromeButton.IsDown && button.BackColor.HasValue)
                {
                    using (var brush = new SolidBrush(button.BackColor.Value))
                    {
                        e.Graphics.FillRectangle(brush, e.Bounds);
                    }
                }
                else
                {
                    e.PaintBackground();
                }

                if (button.Image != null)
                {
                    Bitmap image;

                    if (!chromeButton.Enabled)
                        image = button.DisabledImage;
                    else if (chromeButton.IsOver)
                        image = button.OverImage;
                    else if (chromeButton.IsDown)
                        image = button.DownImage;
                    else
                        image = button.EnabledImage;

                    e.Graphics.DrawImageUnscaled(
                        image,
                        e.Bounds.Left + ButtonImageOffset.X,
                        e.Bounds.Top + ButtonImageOffset.Y
                    );
                }
            }

            protected override void Dispose(bool disposing)
            {
                if (!_disposed && disposing)
                {
                    foreach (var button in _buttons)
                    {
                        button.Dispose();
                    }

                    _buttons.Clear();
                    _buttonMap.Clear();

                    _disposed = true;
                }

                base.Dispose(disposing);
            }

            private class TitleBarButton : IDisposable
            {
                private IResource _resource;
                private readonly NiTitleBarButtonManager _manager;
                private Color? _cachedImageForeColor;
                private bool _disposed;

                public int Cookie { get; private set; }

                public VisualStudioButton ChromeButton { get; set; }

                public Bitmap Image { get; private set; }

                public Bitmap EnabledImage { get; private set; }

                public Bitmap DisabledImage { get; private set; }

                public Bitmap OverImage { get; private set; }

                public Bitmap DownImage { get; private set; }

                public int Priority { get; private set; }

                public Color? ForeColor { get; private set; }

                public Color? BackColor { get; private set; }

                public bool Enabled { get; private set; }

                public bool Visible { get; private set; }

                public TitleBarButton(NiTitleBarButtonManager manager, int cookie)
                {
                    Cookie = cookie;
                    _manager = manager;
                }

                public void Update(INiTitleBarButton button)
                {
                    int priority;
                    ErrorUtil.ThrowOnFailure(button.GetPriority(out priority));
                    Priority = priority;

                    Color? foreColor;
                    ErrorUtil.ThrowOnFailure(button.GetForeColor(out foreColor));
                    ForeColor = foreColor;

                    Color? backColor;
                    ErrorUtil.ThrowOnFailure(button.GetBackColor(out backColor));
                    BackColor = backColor;

                    bool enabled;
                    ErrorUtil.ThrowOnFailure(button.GetEnabled(out enabled));
                    Enabled = enabled;
                    
                    bool visible;
                    ErrorUtil.ThrowOnFailure(button.GetVisible(out visible));
                    Visible = visible;

                    IResource resource;
                    ErrorUtil.ThrowOnFailure(button.GetImage(out resource));
                    SetImage(resource);

                    if (ChromeButton != null)
                    {
                        ChromeButton.Enabled = enabled;
                        ChromeButton.Visible = visible;
                    }
                }

                private void SetImage(IResource resource)
                {
                    if (_resource == resource && ForeColor == _cachedImageForeColor)
                        return;

                    _resource = resource;
                    _cachedImageForeColor = ForeColor;

                    if (Image != null)
                    {
                        EnabledImage.Dispose();
                        EnabledImage = null;

                        DisabledImage.Dispose();
                        DisabledImage = null;

                        OverImage.Dispose();
                        OverImage = null;

                        DownImage.Dispose();
                        DownImage = null;

                        Image.Dispose();
                        Image = null;
                    }

                    EnsureImage();
                }

                public void Dispose()
                {
                    if (!_disposed)
                    {
                        SetImage(null);

                        _disposed = true;
                    }
                }

                public void EnsureImage()
                {
                    var formChrome = _manager._mainWindowChrome.FormChrome;
                    if (formChrome == null)
                        return;

                    if (_resource == null || Image != null)
                        return;

                    Image = (Bitmap)System.Drawing.Image.FromStream(_resource.ToStream());

                    if (Image != null)
                    {
                        EnabledImage = ImageUtil.GetImage(Image, ForeColor.GetValueOrDefault(Color.Black));
                        DisabledImage = ImageUtil.GetImage(Image, SystemColors.ControlDark);
                        OverImage = ImageUtil.GetImage(Image, formChrome.PrimaryColor);
                        DownImage = ImageUtil.GetImage(Image, Color.White);
                    }
                }
            }
        }
    }
}

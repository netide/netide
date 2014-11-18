using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using log4net;
using NetIde.Core.ToolWindows.Notifications;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Core.Services.NotificationManager
{
    internal class NiNotificationManager : ServiceBase, INiNotificationManager
    {
        private int _nextCookie = 1;
        private readonly Dictionary<int, NotificationItem> _itemMap = new Dictionary<int, NotificationItem>();
        private readonly NiConnectionPoint<INiNotificationManagerNotify> _connectionPoint = new NiConnectionPoint<INiNotificationManagerNotify>();
        private readonly List<NotificationItem> _items = new List<NotificationItem>();
        private NiTitleBarButton _button;
        private int _buttonCookie;
        private readonly INiTitleBarButtonManager _titleBarButtonManager;
        private NotificationsWindow _window;
        private readonly CorePackage _package;
        private ButtonListener _buttonListener;
        private bool _disposed;

        public NiNotificationManager(CorePackage package)
            : base(package)
        {
            _package = package;

            _button = new NiTitleBarButton
            {
                Priority = 100,
                Image = Resources.NotificationsInactive,
                ForeColor = SystemColors.ControlDark
            };

            _titleBarButtonManager = ((INiTitleBarButtonManager)GetService(typeof(INiTitleBarButtonManager)));

            ErrorUtil.ThrowOnFailure(_titleBarButtonManager.AddButton(_button, out _buttonCookie));

            _buttonListener = new ButtonListener(this);
        }

        public void Activate(int cookie)
        {
            _connectionPoint.ForAll(p => p.OnClicked(cookie));
        }

        public void Dismiss(int cookie)
        {
            ErrorUtil.ThrowOnFailure(RemoveItem(cookie));

            _connectionPoint.ForAll(p => p.OnDismissed(cookie));
        }

        public void DismissAll()
        {
            var cookies = _itemMap.Keys.ToList();

            foreach (int cookie in cookies)
            {
                ErrorUtil.ThrowOnFailure(RemoveItem(cookie));
            }

            _connectionPoint.ForAll(p =>
            {
                foreach (int cookie in cookies)
                {
                    p.OnDismissed(cookie);
                }
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_button != null)
                {
                    _titleBarButtonManager.RemoveButton(_buttonCookie);
                    _buttonCookie = 0;
                    _button = null;
                }

                if (_buttonListener != null)
                {
                    _buttonListener.Dispose();
                    _buttonListener = null;
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        public HResult Show()
        {
            try
            {
                if (_window == null)
                {
                    _window = (NotificationsWindow)_package.CreateToolWindow(typeof(NotificationsWindow));
                    new WindowListener(this);

                    _window.RedrawItems(_items);
                }

                return _window.Frame.Show();
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult Hide()
        {
            try
            {
                if (_window != null)
                    return _window.Frame.Hide();

                return HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private void ToggleToolWindow()
        {
            bool visible = false;
            if (_window != null)
                ErrorUtil.ThrowOnFailure(_window.Frame.GetIsVisible(out visible));

            if (visible)
                ErrorUtil.ThrowOnFailure(Hide());
            else
                ErrorUtil.ThrowOnFailure(Show());
        }

        private void UpdateButton()
        {
            if (_items.Count == 0)
            {
                _button.BackColor = null;
                _button.Image = Resources.NotificationsInactive;
                _button.ForeColor = SystemColors.ControlDark;
            }
            else
            {
                var priority = NiNotificationItemPriority.Normal;

                foreach (var item in _items)
                {
                    priority = (NiNotificationItemPriority)Math.Max((int)priority, (int)item.Priority);
                }

                _button.ForeColor = null;

                switch (priority)
                {
                    case NiNotificationItemPriority.Informational:
                        _button.BackColor = NotificationsControl.WarningColor;
                        break;

                    case NiNotificationItemPriority.Critical:
                        _button.BackColor = NotificationsControl.ErrorColor;
                        _button.ForeColor = Color.White;
                        break;

                    default:
                        _button.BackColor = null;
                        break;
                }

                _button.Image = Resources.NotificationsActive;
            }

            ErrorUtil.ThrowOnFailure(_titleBarButtonManager.UpdateButton(_buttonCookie, _button));
        }

        public HResult Advise(object sink, out int cookie)
        {
            return _connectionPoint.Advise(sink, out cookie);
        }

        public HResult Advise(INiNotificationManagerNotify sink, out int cookie)
        {
            return Advise((object)sink, out cookie);
        }

        public HResult Unadvise(int cookie)
        {
            return _connectionPoint.Unadvise(cookie);
        }

        public HResult AddItem(INiNotificationItem item, out int cookie)
        {
            cookie = 0;

            try
            {
                if (item == null)
                    throw new ArgumentNullException("item");

                cookie = _nextCookie++;

                var wrapper = new NotificationItem(cookie);
                wrapper.Update(item);

                _itemMap.Add(cookie, wrapper);
                _items.Add(wrapper);

                UpdateButton();

                if (_window != null)
                    _window.RedrawItems(_items);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult UpdateItem(int cookie, INiNotificationItem item)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException("item");

                NotificationItem wrapper;
                if (!_itemMap.TryGetValue(cookie, out wrapper))
                    return HResult.False;

                wrapper.Update(item);

                UpdateButton();

                if (_window != null)
                    _window.RedrawItems(_items);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult RemoveItem(int cookie)
        {
            try
            {
                NotificationItem wrapper;
                if (!_itemMap.TryGetValue(cookie, out wrapper))
                    return HResult.False;

                _itemMap.Remove(cookie);
                _items.Remove(wrapper);

                UpdateButton();

                if (_window != null)
                    _window.RedrawItems(_items);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private class ButtonListener : NiEventSink, INiTitleBarButtonManagerNotify
        {
            private static readonly ILog Log = LogManager.GetLogger(typeof(ButtonListener));

            private readonly NiNotificationManager _manager;

            public ButtonListener(NiNotificationManager manager)
                : base(manager._titleBarButtonManager)
            {
                _manager = manager;
            }

            public void OnClicked(int cookie)
            {
                try
                {
                    if (_manager._buttonCookie == cookie)
                        _manager.ToggleToolWindow();
                }
                catch (Exception ex)
                {
                    Log.Warn("Failed to toggle notifications tool window", ex);
                }
            }
        }

        private class WindowListener : NiEventSink, INiWindowFrameNotify
        {
            private static readonly ILog Log = LogManager.GetLogger(typeof(WindowListener));

            private readonly NiNotificationManager _manager;

            public WindowListener(NiNotificationManager manager)
                : base(manager._window.Frame)
            {
                _manager = manager;
            }

            public void OnShow(NiWindowShow action)
            {
            }

            public void OnClose(NiFrameCloseMode closeMode, ref bool cancel)
            {
                try
                {
                    _manager._window = null;

                    Dispose();
                }
                catch (Exception ex)
                {
                    Log.Warn("Failed to handle close of notifications tool window", ex);
                }
            }

            public void OnSize()
            {
            }
        }
    }
}

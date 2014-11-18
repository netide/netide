using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GdiPresentation;
using NetIde.Core.Services.NotificationManager;
using NetIde.Shell.Interop;
using Brush = GdiPresentation.Brush;
using Color = System.Drawing.Color;
using Cursor = GdiPresentation.Cursor;
using FontStyle = GdiPresentation.FontStyle;
using Image = System.Drawing.Image;
using MouseEventArgs = GdiPresentation.MouseEventArgs;
using Orientation = GdiPresentation.Orientation;
using SolidBrush = GdiPresentation.SolidBrush;

namespace NetIde.Core.ToolWindows.Notifications
{
    internal partial class NotificationsControl : NetIde.Util.Forms.UserControl
    {
        public static readonly Color ErrorColor = Color.FromArgb(229, 20, 0);
        public static readonly Color WarningColor = Color.FromArgb(255, 204, 0);
        private static readonly Image NotificationClose = NeutralResources.NotificationClose;

        private TextBlock _totalNotifications;
        private StackPanel _notifications;
        private NiNotificationManager _notificationManager;

        public NotificationsControl()
        {
            InitializeComponent();

            BuildElement();
        }

        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;
                _notificationManager = (NiNotificationManager)GetService(typeof(INiNotificationManager));
            }
        }

        private void BuildElement()
        {
            _elementControl.BackColor = SystemColors.Window;
            _elementControl.Content = BuildContainer();
        }

        private Element BuildContainer()
        {
            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(new GridLength(GridUnitType.Star, 1))
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(new GridLength(GridUnitType.Star, 1)),
                    new ColumnDefinition(GridLength.Auto)
                }
            };

            _totalNotifications = new TextBlock
            {
                Margin = new Thickness(3, 5)
            };

            grid.Children.Add(_totalNotifications);

            var dismissAll = new Link(Labels.DismissAll);

            dismissAll.Click += dismissAll_Click;

            var dismissAllBlock = new TextBlock
            {
                Margin = new Thickness(3, 5),
                Runs =
                {
                    dismissAll
                }
            };

            Grid.SetColumn(dismissAllBlock, 1);
            grid.Children.Add(dismissAllBlock);

            UpdateTotalNotifications(0);

            _notifications = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            var notificationsHost = new ElementControlHost();
            notificationsHost.Control.AllowedScrollBars = ScrollBars.Vertical;
            notificationsHost.Control.Content = _notifications;

            RedrawItems(new List<NotificationItem>());

            Grid.SetColumnSpan(notificationsHost, 2);
            Grid.SetRow(notificationsHost, 1);
            grid.Children.Add(notificationsHost);

            return grid;
        }

        void dismissAll_Click(object sender, EventArgs e)
        {
            _notificationManager.DismissAll();
        }

        private void UpdateTotalNotifications(int count)
        {
            _totalNotifications.Runs.Clear();
            _totalNotifications.Runs.Add(new Run(String.Format(Labels.TotalNotifications, count)));
        }

        public void RedrawItems(List<NotificationItem> items)
        {
            UpdateTotalNotifications(items.Count);

            _notifications.Children.Clear();

            foreach (var item in items)
            {
                if (_notifications.Children.Count > 0)
                {
                    _notifications.Children.Add(new Border
                    {
                        BorderThickness = new Thickness(0, 4)
                    });
                }

                _notifications.Children.Add(new Item(this, item));
            }
        }

        public void Dismiss(int cookie)
        {
            _notificationManager.Dismiss(cookie);
        }

        public void Activate(int cookie)
        {
            _notificationManager.Activate(cookie);
        }

        private class Item : Border
        {
            private readonly NotificationsControl _control;
            private readonly NotificationItem _item;
            private readonly TextBlock _subject;
            private readonly TextBlock _message;
            private readonly TextBlock _created;
            private GdiPresentation.Image _close;

            public Item(NotificationsControl control, NotificationItem item)
            {
                _control = control;
                _item = item;

                BorderThickness = new Thickness(4, 0, 0, 0);
                Background = Brush.Transparent;
                Padding = new Thickness(5);
                Cursor = Cursor.Hand;

                switch (item.Priority)
                {
                    case NiNotificationItemPriority.Informational:
                        BorderBrush = new SolidBrush((GdiPresentation.Color)WarningColor);
                        break;

                    case NiNotificationItemPriority.Critical:
                        BorderBrush = new SolidBrush((GdiPresentation.Color)ErrorColor);
                        break;

                    default:
                        BorderBrush = Brush.Transparent;
                        break;
                }

                var grid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition(new GridLength(GridUnitType.Star, 1)),
                        new ColumnDefinition(GridLength.Auto)
                    },
                    RowDefinitions =
                    {
                        new RowDefinition(GridLength.Auto)
                    }
                };

                Content = grid;

                var information = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };
                grid.Children.Add(information);

                _subject = new TextBlock
                {
                    FontStyle = FontStyle.Bold,
                    TextTrimming = TextTrimming.CharacterEllipsis
                };
                information.Children.Add(_subject);

                _message = new TextBlock
                {
                    Wrap = true
                };
                Grid.SetRow(_message, 1);
                information.Children.Add(_message);

                _created = new TextBlock
                {
                    ForeColor = (GdiPresentation.Color)SystemColors.GrayText,
                    TextTrimming = TextTrimming.CharacterEllipsis,
                    Margin = new Thickness(0, 2, 0, 0)
                };
                Grid.SetColumnSpan(_created, 2);
                Grid.SetRow(_created, 2);
                information.Children.Add(_created);

                _close = new GdiPresentation.Image
                {
                    Margin = new Thickness(3),
                    Bitmap = NotificationClose,
                    Cursor = Cursor.Arrow,
                    Visibility = Visibility.Hidden,
                    Background = Brush.Transparent,
                    VerticalAlignment = VerticalAlignment.Top
                };
                _close.MouseUp += _close_MouseUp;
                Grid.SetColumn(_close, 1);
                grid.Children.Add(_close);

                UpdateFromItem();
            }

            void _close_MouseUp(object sender, MouseEventArgs e)
            {
                e.PreventBubble();
                _control.Dismiss(_item.Cookie);
            }

            protected override void OnMouseEnter(EventArgs e)
            {
                Background = new SolidBrush((GdiPresentation.Color)SystemColors.Highlight);

                var highlightText = (GdiPresentation.Color)SystemColors.HighlightText;

                _subject.ForeColor = highlightText;
                _subject.FontStyle = FontStyle.Bold | FontStyle.Underline;

                _message.ForeColor = highlightText;
                _created.ForeColor = highlightText;

                _close.Visibility = Visibility.Visible;
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                Background = Brush.Transparent;

                _subject.ForeColor = null;
                _subject.FontStyle = FontStyle.Bold;

                _message.ForeColor = null;
                _created.ForeColor = (GdiPresentation.Color)SystemColors.GrayText;

                _close.Visibility = Visibility.Hidden;
            }

            protected override void OnMouseUp(MouseEventArgs e)
            {
                e.PreventBubble();
                _control.Activate(_item.Cookie);
            }

            private void UpdateFromItem()
            {
                _subject.Runs.Clear();
                _subject.Runs.Add(new Run(_item.Subject));

                _message.Runs.Clear();
                _message.Runs.Add(new Run(_item.Message));

                var now = DateTime.Now;

                _created.Runs.Clear();
                if (_item.Created.HasValue)
                    _created.Runs.Add(new Run(UpperCaseFirst(FormatTimeSpan(_item.Created.Value - now))));
            }

            private string UpperCaseFirst(string value)
            {
                if (String.IsNullOrEmpty(value))
                    return value;

                return value.Substring(0, 1).ToUpper() + value.Substring(1);
            }

            private string FormatTimeSpan(TimeSpan timeSpan)
            {
                if (timeSpan.Ticks < 0)
                {
                    timeSpan = -timeSpan;

                    double minutes = timeSpan.TotalMinutes;
                    if (minutes < 1)
                        return Labels.LessThanOneMinuteAgo;
                    if (minutes < 2)
                        return Labels.OneMinuteAgo;
                    if (minutes < 60)
                        return String.Format(Labels.MinutesAgo, (int)minutes);
                    double hours = timeSpan.TotalHours;
                    if (hours < 2)
                        return Labels.OneHourAgo;
                    if (hours < 24)
                        return String.Format(Labels.HoursAgo, (int)hours);
                    double days = timeSpan.TotalDays;
                    if (days < 2)
                        return Labels.OneDayAgo;
                    return String.Format(Labels.DaysAgo, (int)days);
                }
                else
                {
                    double minutes = timeSpan.TotalMinutes;
                    if (minutes < 1)
                        return Labels.LessThanOneMinute;
                    if (minutes < 2)
                        return Labels.OneMinute;
                    if (minutes < 60)
                        return String.Format(Labels.Minutes, (int)minutes);
                    double hours = timeSpan.TotalHours;
                    if (hours < 2)
                        return Labels.OneHour;
                    if (hours < 24)
                        return String.Format(Labels.Hours, (int)hours);
                    double days = timeSpan.TotalDays;
                    if (days < 2)
                        return Labels.OneDay;
                    return String.Format(Labels.Days, (int)days);
                }
            }
        }

        private class ElementControlHost : ControlHost<ElementControl>
        {
            public ElementControlHost()
                : base(new ElementControl())
            {
            }
        }
    }
}

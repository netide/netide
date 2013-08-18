using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using NetIde.Util.Win32;

namespace NetIde.Util.Forms
{
    public class FormHelper
    {
        private const string RegistryBaseStr = "Software\\SystemEx\\Interface";

        public static string GlobalKeyAddition { get; set; }

        private readonly System.Windows.Forms.Form _form;
        private Point _normalLocation;
        private Size _normalSize;
        private List<PropertyTracker> _propertyTrackers;
        private bool _initializeFormCalled;
        private readonly string _defaultFontName;
        private readonly string _correctFontName;
        private readonly float _correctFontSize;
        private readonly float _defaultFontSize;
        private int? _mainMenuHeight;
        private readonly Control _control;

        public bool InDesignMode { get; private set; }

        public bool EnableBoundsTracking { get; set; }

        public FormHelper(Control control)
        {
            _control = control;
            _form = control as System.Windows.Forms.Form;

            InDesignMode = ControlUtil.GetIsInDesignMode(control);

            if (InDesignMode)
                return;

            _defaultFontName = _control.Font.Name;
            _defaultFontSize = _control.Font.Size;

            var correctFont = SystemFonts.MessageBoxFont;

            if (!(_control is UserControl))
                _control.Font = correctFont;

            _correctFontName = correctFont.Name;
            _correctFontSize = correctFont.Size;
        }

        public void InitializeForm()
        {
            if (!InDesignMode && !_initializeFormCalled)
            {
                FixFonts();

                if (_form != null)
                    RestoreUserSettings();

                _initializeFormCalled = true;
            }
        }

        private void FixFonts()
        {
            var userControl = _control as UserControl;

            if (userControl != null)
            {
                if (userControl.IsFixed)
                    return;

                userControl.IsFixed = true;
            }

            FixFonts(_control.Controls);
        }

        private void FixFonts(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                FixFont(control);

                FixFonts(control.Controls);
            }
        }

        private void FixFont(Control control)
        {
            var userControl = control as UserControl;

            if (userControl != null)
            {
                if (userControl.IsFixed)
                    return;

                userControl.IsFixed = true;
            }

            string fontName = control.Font.Name;

            if (fontName != _correctFontName)
            {
                float fontSize = control.Font.Size;

                if (fontName == _defaultFontName)
                    fontName = _correctFontName;
                if (fontSize == _defaultFontSize)
                    fontSize = _correctFontSize;

                control.Font = new Font(
                    fontName,
                    fontSize,
                    control.Font.Style,
                    control.Font.Unit,
                    control.Font.GdiCharSet);
            }
        }

        public string KeyAddition { get; set; }

        public void StoreUserSettings()
        {
            RegistryKey key = null;

            if (EnableBoundsTracking && _form.FormBorderStyle != FormBorderStyle.None)
            {
                key = FormKey;

                StoreDWord(key, "WindowState", (int)_form.WindowState);
                StorePoint(key, "Location", _normalLocation);

                var size = _normalSize;

                if (_form.Menu != null)
                {
                    size = new Size(
                        size.Width,
                        size.Height - SystemInformation.MenuHeight
                    );
                }

                StoreSize(key, "Size", size);
            }

            // Track all properties

            if (_propertyTrackers != null)
            {
                foreach (var item in _propertyTrackers)
                {
                    if (key == null)
                    {
                        key = FormKey;
                    }

                    item.Store(key);
                }
            }
        }

        public bool RestoreUserSettings()
        {
            RegistryKey key = null;
            bool restored = FormKeyExists;

            if (EnableBoundsTracking && _form.FormBorderStyle != FormBorderStyle.None)
            {
                key = FormKey;

                switch (_form.StartPosition)
                {
                    case FormStartPosition.CenterParent:
                    case FormStartPosition.CenterScreen:
                        // Do not restore the location
                        break;

                    default:
                        Point location = _form.Location;

                        if (RestorePoint(key, "Location", ref location))
                        {
                            _form.Location = location;
                            _form.StartPosition = FormStartPosition.Manual;
                        }
                        break;
                }

                switch (_form.FormBorderStyle)
                {
                    case FormBorderStyle.None:
                    case FormBorderStyle.FixedSingle:
                    case FormBorderStyle.Fixed3D:
                    case FormBorderStyle.FixedDialog:
                    case FormBorderStyle.FixedToolWindow:
                        // Do not restore the size
                        break;

                    default:
                        Size size = _form.Size;

                        if (RestoreSize(key, "Size", ref size))
                        {
                            if (_form.Menu != null)
                            {
                                // When we store the height, the MainMenu
                                // has already been removed, so this height
                                // won't include the height of the menu.
                                // We add it back here.

                                size = new Size(
                                    size.Width,
                                    size.Height + SystemInformation.MenuHeight
                                );
                            }

                            _form.Size = size;
                        }
                        break;
                }

                int windowState = (int)_form.WindowState;

                if (RestoreDWord(key, "WindowState", ref windowState))
                {
                    switch ((FormWindowState)windowState)
                    {
                        case FormWindowState.Minimized:
                            if (_form.MinimizeBox)
                                _form.WindowState = FormWindowState.Minimized;
                            break;

                        case FormWindowState.Maximized:
                            if (_form.MaximizeBox)
                                _form.WindowState = FormWindowState.Maximized;
                            break;

                        case FormWindowState.Normal:
                            _form.WindowState = FormWindowState.Normal;
                            break;
                    }
                }
            }

            // Restore all properties

            if (_propertyTrackers != null)
            {
                foreach (var item in _propertyTrackers)
                {
                    if (key == null)
                    {
                        key = FormKey;
                    }

                    item.Restore(key);
                }
            }

            if (key != null)
                key.Dispose();

            return restored;
        }

        private void StoreSize(RegistryKey key, string name, Size value)
        {
            key.SetValue(name, String.Format("{0}x{1}", value.Width, value.Height));
        }

        private bool RestoreSize(RegistryKey key, string name, ref Size value)
        {
            object data = key.GetValue(name);

            if (data is string)
            {
                string[] parts = (data as string).Split(new[] { 'x' });

                if (parts.Length == 2)
                {
                    value.Width = int.Parse(parts[0]);
                    value.Height = int.Parse(parts[1]);

                    return true;
                }
            }

            return false;
        }

        private void StorePoint(RegistryKey key, string name, Point value)
        {
            key.SetValue(name, String.Format("{0}x{1}", value.X, value.Y));
        }

        private bool RestorePoint(RegistryKey key, string name, ref Point value)
        {
            object data = key.GetValue(name);

            if (data is string)
            {
                string[] parts = (data as string).Split(new[] { 'x' });

                if (parts.Length == 2)
                {
                    value.X = int.Parse(parts[0]);
                    value.Y = int.Parse(parts[1]);

                    return true;
                }
            }

            return false;
        }

        private void StoreDWord(RegistryKey key, string name, int value)
        {
            key.SetValue(name, value);
        }

        private bool RestoreDWord(RegistryKey key, string name, ref int value)
        {
            object data = key.GetValue(name);

            if (data is int)
            {
                value = (int)data;

                return true;
            }

            return false;
        }

        private RegistryKey FormKey
        {
            get
            {
                return UserSettingsKey.CreateSubKey(FormKeyName);
            }
        }

        private string FormKeyName
        {
            get
            {
                string key = _form.GetType().FullName;

                if (KeyAddition != null)
                    key += "$" + KeyAddition;

                return key;
            }
        }

        private bool FormKeyExists
        {
            get
            {
                using (var key = UserSettingsKey.OpenSubKey(FormKeyName))
                {
                    return key != null;
                }
            }
        }

        private RegistryKey UserSettingsKey
        {
            get
            {
                string key = RegistryBaseStr;

                if (GlobalKeyAddition != null)
                    key += "\\" + GlobalKeyAddition;

                key += "\\" + GetScreenDimensionsKey();

                return Registry.CurrentUser.CreateSubKey(key);
            }
        }

        private string GetScreenDimensionsKey()
        {
            var sb = new StringBuilder();

            sb.Append(SerializeScreen(Screen.PrimaryScreen));

            foreach (var screen in Screen.AllScreens)
            {
                if (!screen.Primary)
                {
                    sb.Append("+");

                    sb.Append(SerializeScreen(screen));
                }
            }

            return sb.ToString();
        }

        private string SerializeScreen(Screen screen)
        {
            var bounds = screen.Bounds;

            return String.Format("{0}x{1}-{2}x{3}",
                    bounds.Left, bounds.Top, bounds.Width, bounds.Height);
        }

        public void OnSizeChanged(EventArgs e)
        {
            if (!InDesignMode)
            {
                if (_form.WindowState == FormWindowState.Normal)
                    _normalSize = GetNormalSize();
            }
        }

        private Size GetNormalSize()
        {
            var size = _form.Size;

            if (_form.Menu != null && !_mainMenuHeight.HasValue)
            {
                // We need to subtract the height of the main menu because
                // when the main menu is made visible after the size has been
                // restored.

                NativeMethods.RECT rect;

                NativeMethods.GetMenuItemRect(
                    new HandleRef(this, _form.Handle),
                    new HandleRef(this, _form.Menu.Handle),
                    0,
                    out rect
                );

                // We need to store the main menu height because when disposing
                // the form, the main menu is removed and becomes Null.
                // Once we've seen a main menu, we always include it in the
                // calculations.

                _mainMenuHeight = rect.bottom - rect.top + 1;
            }

            size = new Size(
                size.Width,
                size.Height - _mainMenuHeight.GetValueOrDefault()
            );

            return size;
        }

        public void OnLocationChanged(EventArgs e)
        {
            if (!InDesignMode)
            {
                if (_form.WindowState == FormWindowState.Normal)
                {
                    _normalLocation = _form.Location;
                }
            }
        }

        public void CenterOverParent(double relativeSize)
        {
            if (!InDesignMode)
            {
                if (_form.Owner != null)
                {
                    _form.WindowState = FormWindowState.Normal;

                    int width = _form.Owner.Width;
                    int height = _form.Owner.Height;
                    double relativeLocation = (1.0 - relativeSize) / 2.0;

                    _form.Location = new Point(
                        _form.Owner.Location.X + (int)(width * relativeLocation),
                        _form.Owner.Location.Y + (int)(height * relativeLocation)
                    );

                    _form.Size = new Size(
                        (int)(width * relativeSize),
                        (int)(height * relativeSize)
                    );
                }
            }
        }

        public void TrackProperty(Control control, string property)
        {
            if (_propertyTrackers == null)
                _propertyTrackers = new List<PropertyTracker>();

            _propertyTrackers.Add(new PropertyTracker(control, property));
        }

        private class PropertyTracker
        {
            private readonly Control _control;
            private readonly string _property;

            public PropertyTracker(Control control, string property)
            {
                _control = control;
                _property = property;
            }

            public void Store(RegistryKey key)
            {
                var controlKey = key.CreateSubKey("Controls\\" + _control.Name);

                var property = _control.GetType().GetProperty(_property);

                controlKey.SetValue(_property, property.GetValue(_control, null).ToString());
            }

            public void Restore(RegistryKey key)
            {
                var controlKey = key.OpenSubKey("Controls\\" + _control.Name);

                if (controlKey != null)
                {
                    object value = controlKey.GetValue(_property);

                    if (value != null)
                    {
                        var property = _control.GetType().GetProperty(_property);

                        try
                        {
                            property.SetValue(
                                _control,
                                Convert.ChangeType(value, property.PropertyType),
                                null);
                        }
                        catch
                        {
                            // The store value could be illegal. In this case,
                            // we just ignore the exception.
                        }
                    }
                }
            }
        }
    }
}

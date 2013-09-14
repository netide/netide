using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Automation;
using System.Windows.Automation.Provider;
using System.Windows.Forms;

namespace UIAutomationWrapper
{
    [ComVisible(true)]
    public abstract class ElementProvider : IRawElementProviderSimple
    {
        private static readonly Dictionary<AutomationPattern, Type> _patterns = new Dictionary<AutomationPattern, Type>
        {
            { DockPattern.Pattern, typeof(IDockProvider) },
            { ExpandCollapsePattern.Pattern, typeof(IExpandCollapseProvider) },
            { GridItemPattern.Pattern, typeof(IGridItemProvider) },
            { GridPattern.Pattern, typeof(IGridProvider) },
            { InvokePattern.Pattern, typeof(IInvokeProvider) },
            { ItemContainerPattern.Pattern, typeof(IItemContainerProvider) },
            { MultipleViewPattern.Pattern, typeof(IMultipleViewProvider) },
            { RangeValuePattern.Pattern, typeof(IRangeValueProvider) },
            { ScrollItemPattern.Pattern, typeof(IScrollItemProvider) },
            { ScrollPattern.Pattern, typeof(IScrollProvider) },
            { SelectionItemPattern.Pattern, typeof(ISelectionItemProvider) },
            { SelectionPattern.Pattern, typeof(ISelectionProvider) },
            { SynchronizedInputPattern.Pattern, typeof(ISynchronizedInputProvider) },
            { TextPattern.Pattern, typeof(ITextProvider) },
            { TogglePattern.Pattern, typeof(IToggleProvider) },
            { TransformPattern.Pattern, typeof(ITransformProvider) },
            { ValuePattern.Pattern, typeof(IValueProvider) },
            { VirtualizedItemPattern.Pattern, typeof(IVirtualizedItemProvider) },
            { WindowPattern.Pattern, typeof(IWindowProvider) }
        };

        private static readonly Dictionary<AutomationProperty, Func<ElementProvider, object>> _valueGetters = new Dictionary<AutomationProperty, Func<ElementProvider, object>>
        {
            { AutomationElement.LocalizedControlTypeProperty, p => p.LocalizedControlType },
            { AutomationElement.ControlTypeProperty, p => (p.ControlType ?? System.Windows.Automation.ControlType.Pane).Id },
            { AutomationElement.IsContentElementProperty, p => p.IsContentElement },
            { AutomationElement.NameProperty, p => p.Name },
            { AutomationElement.AccessKeyProperty, p => p.GetAccessKey() },
            { AutomationElement.IsEnabledProperty, p => p.Control.Enabled },
            { AutomationElement.IsKeyboardFocusableProperty, p => p.IsKeyboardFocusable },
            { AutomationElement.ProcessIdProperty, p => Process.GetCurrentProcess().Id },
            { AutomationElement.ClickablePointProperty, p => (POINT)p.ClickablePoint },
            { AutomationElement.HasKeyboardFocusProperty, p => p.Control.Focused },
            { AutomationElement.AutomationIdProperty, p => p.GetAutomationId() },
            { AutomationElement.IsOffscreenProperty, p => p.GetIsOffscreen() },
            { AutomationElement.HelpTextProperty, p => p.HelpText },
            { AutomationElement.FrameworkIdProperty, p => "WinForms" }
            // Missing: NativeWindowHandleProperty, ClassNameProperty
        };

        public static void Install(ElementProvider elementProvider)
        {
            if (elementProvider == null)
                throw new ArgumentNullException("elementProvider");

            new ElementProviderHook(elementProvider);
        }

        public Control Control { get; private set; }

        IRawElementProviderSimple IRawElementProviderSimple.HostRawElementProvider
        {
            get { return AutomationInteropProvider.HostProviderFromHandle(Control.Handle); }
        }

        public virtual ProviderOptions ProviderOptions
        {
            get { return ProviderOptions.ServerSideProvider | ProviderOptions.UseComThreading; }
        }

        public virtual string LocalizedControlType
        {
            get { return null; }
        }

        public virtual System.Windows.Automation.ControlType ControlType
        {
            get { return System.Windows.Automation.ControlType.Pane; }
        }

        public virtual bool IsContentElement
        {
            get { return true; }
        }

        public virtual string Name
        {
            get { return Control.Text; }
        }

        public virtual bool IsKeyboardFocusable
        {
            get
            {
                if (Control.Focused)
                    return true;

                if (Control.Visible && Control.Enabled)
                    return Control.TabStop;

                return false;
            }
        }

        public virtual string HelpText
        {
            get { return null; }
        }

        protected ElementProvider(Control control)
        {
            if (control == null)
                throw new ArgumentNullException("control");

            Control = control;
        }

        object IRawElementProviderSimple.GetPatternProvider(int patternId)
        {
            var pattern = AutomationPattern.LookupById(patternId);
            if (pattern != null)
                return GetPatternProvider(pattern);

            return null;
        }

        protected virtual object GetPatternProvider(AutomationPattern automationPattern)
        {
            Type type;
            if (_patterns.TryGetValue(automationPattern, out type) && type.IsInstanceOfType(this))
                return this;

            return null;
        }

        object IRawElementProviderSimple.GetPropertyValue(int propertyId)
        {
            var property = AutomationProperty.LookupById(propertyId);
            if (property != null)
                return GetPropertyValue(property);

            return null;
        }

        protected virtual object GetPropertyValue(AutomationProperty automationProperty)
        {
            Func<ElementProvider, object> func;
            if (_valueGetters.TryGetValue(automationProperty, out func))
                return func(this);

            return null;
        }

        private object GetAccessKey()
        {
            string text = Control.Text;

            if (!String.IsNullOrEmpty(text))
            {
                int pos = text.IndexOf('&');
                if (pos != -1 && pos < text.Length)
                    return "Alt+" + text[pos];
            }

            return null;
        }

        public virtual Point ClickablePoint
        {
            get
            {
                var clientBounds = Control.ClientRectangle;

                var bounds = new Rectangle(
                    Control.PointToScreen(clientBounds.Location),
                    clientBounds.Size
                );

                return new Point((bounds.Left + bounds.Right) / 2, (bounds.Top + bounds.Bottom) / 2);
            }
        }

        private object GetAutomationId()
        {
            if (!String.IsNullOrEmpty(Control.Name))
                return Control.Name;

            return Control.Handle.ToInt32().ToString(CultureInfo.InvariantCulture);
        }

        private object GetIsOffscreen()
        {
            var bounds = (RECT)new Rectangle(Control.PointToScreen(new Point(0, 0)), Control.Size);
            
            return MonitorFromRect(ref bounds, MONITOR_DEFAULTTONULL) == IntPtr.Zero;
        }

        private class ElementProviderHook : NativeWindow
        {
            private readonly ElementProvider _elementProvider;

            public ElementProviderHook(ElementProvider elementProvider)
            {
                _elementProvider = elementProvider;
                _elementProvider.Control.HandleCreated += Control_HandleCreated;
                _elementProvider.Control.HandleDestroyed += Control_HandleDestroyed;

                if (_elementProvider.Control.IsHandleCreated)
                    AssignHandle(_elementProvider.Control.Handle);
            }

            void Control_HandleCreated(object sender, EventArgs e)
            {
                AssignHandle(_elementProvider.Control.Handle);
            }

            void Control_HandleDestroyed(object sender, EventArgs e)
            {
                ReleaseHandle();
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_GETOBJECT)
                {
                    if (m.LParam.ToInt32() == AutomationInteropProvider.RootObjectId)
                    {
                        m.Result = AutomationInteropProvider.ReturnRawElementProvider(m.HWnd, m.WParam, m.LParam, _elementProvider);
                        return;
                    }
                    DefWndProc(ref m);
                    return;
                }

                base.WndProc(ref m);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public static explicit operator RECT(Rectangle rect)
            {
                return new RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;

            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public static explicit operator POINT(Point pt)
            {
                return new POINT(pt.X, pt.Y);
            }
        }

        private const uint MONITOR_DEFAULTTONULL = 0;
        private const int WM_GETOBJECT = 0x3D;

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromRect([In] ref RECT lprc, uint dwFlags);
    }
}

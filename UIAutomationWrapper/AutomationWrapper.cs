using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Provider;
using Point = System.Drawing.Point;

namespace UIAutomationWrapper
{
    public partial class AutomationWrapper
    {
        private static readonly Dictionary<System.Windows.Automation.ControlType, ControlType> _controlTypeMap = new Dictionary<System.Windows.Automation.ControlType, ControlType>
        {
            { System.Windows.Automation.ControlType.Button, ControlType.Button },
            { System.Windows.Automation.ControlType.Calendar, ControlType.Calendar },
            { System.Windows.Automation.ControlType.CheckBox, ControlType.CheckBox },
            { System.Windows.Automation.ControlType.ComboBox, ControlType.ComboBox },
            { System.Windows.Automation.ControlType.Custom, ControlType.Custom },
            { System.Windows.Automation.ControlType.DataGrid, ControlType.DataGrid },
            { System.Windows.Automation.ControlType.DataItem, ControlType.DataItem },
            { System.Windows.Automation.ControlType.Document, ControlType.Document },
            { System.Windows.Automation.ControlType.Edit, ControlType.Edit },
            { System.Windows.Automation.ControlType.Group, ControlType.Group },
            { System.Windows.Automation.ControlType.Header, ControlType.Header },
            { System.Windows.Automation.ControlType.HeaderItem, ControlType.HeaderItem },
            { System.Windows.Automation.ControlType.Hyperlink, ControlType.Hyperlink },
            { System.Windows.Automation.ControlType.Image, ControlType.Image },
            { System.Windows.Automation.ControlType.List, ControlType.List },
            { System.Windows.Automation.ControlType.ListItem, ControlType.ListItem },
            { System.Windows.Automation.ControlType.Menu, ControlType.Menu },
            { System.Windows.Automation.ControlType.MenuBar, ControlType.MenuBar },
            { System.Windows.Automation.ControlType.MenuItem, ControlType.MenuItem },
            { System.Windows.Automation.ControlType.Pane, ControlType.Pane },
            { System.Windows.Automation.ControlType.ProgressBar, ControlType.ProgressBar },
            { System.Windows.Automation.ControlType.RadioButton, ControlType.RadioButton },
            { System.Windows.Automation.ControlType.ScrollBar, ControlType.ScrollBar },
            { System.Windows.Automation.ControlType.Separator, ControlType.Separator },
            { System.Windows.Automation.ControlType.Slider, ControlType.Slider },
            { System.Windows.Automation.ControlType.Spinner, ControlType.Spinner },
            { System.Windows.Automation.ControlType.SplitButton, ControlType.SplitButton },
            { System.Windows.Automation.ControlType.StatusBar, ControlType.StatusBar },
            { System.Windows.Automation.ControlType.Tab, ControlType.Tab },
            { System.Windows.Automation.ControlType.TabItem, ControlType.TabItem },
            { System.Windows.Automation.ControlType.Table, ControlType.Table },
            { System.Windows.Automation.ControlType.Text, ControlType.Text },
            { System.Windows.Automation.ControlType.Thumb, ControlType.Thumb },
            { System.Windows.Automation.ControlType.TitleBar, ControlType.TitleBar },
            { System.Windows.Automation.ControlType.ToolBar, ControlType.ToolBar },
            { System.Windows.Automation.ControlType.ToolTip, ControlType.ToolTip },
            { System.Windows.Automation.ControlType.Tree, ControlType.Tree },
            { System.Windows.Automation.ControlType.TreeItem, ControlType.TreeItem },
            { System.Windows.Automation.ControlType.Window, ControlType.Window }
        };

        private static readonly Dictionary<ControlType, System.Windows.Automation.ControlType> _controlTypeReverseMap = _controlTypeMap.ToDictionary(p => p.Value, p => p.Key);

        private readonly Dictionary<AutomationEventHandler, EventWrapper> _registeredEvents = new Dictionary<AutomationEventHandler,EventWrapper>();
        private readonly string _cachedName;
        private readonly ControlType _cachedControlType;
        private readonly string _cachedAutomationId;

        public static AutomationWrapper FocusedElement
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new AutomationWrapper(AutomationElement.FocusedElement);
            }
        }

        public static AutomationWrapper RootElement
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new AutomationWrapper(AutomationElement.RootElement);
            }
        }

        public static AutomationWrapper FromHandle(IntPtr handle)
        {
            return new AutomationWrapper(AutomationElement.FromHandle(handle));
        }

        public static AutomationWrapper FromLocalProvider(IRawElementProviderSimple localImpl)
        {
            return new AutomationWrapper(AutomationElement.FromLocalProvider(localImpl));
        }

        public static AutomationWrapper FromPoint(Point pt)
        {
            return new AutomationWrapper(AutomationElement.FromPoint(new System.Windows.Point(pt.X, pt.Y)));
        }

        public AutomationElement AutomationElement { get; private set; }

        public AutomationWrapper Parent
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return FindFirst(TreeScope.Parent);
            }
        }

        public string AcceleratorKey
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.AcceleratorKey;
            }
        }

        public string AccessKey
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.AccessKey;
            }
        }

        public string AutomationId
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.AutomationId;
            }
        }

        public Rect BoundingRectangle
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.BoundingRectangle;
            }
        }

        public string ClassName
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.ClassName;
            }
        }

        public ControlType ControlType
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return _controlTypeMap[AutomationElement.Current.ControlType];
            }
        }

        public string FrameworkId
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.FrameworkId;
            }
        }

        public bool HasKeyboardFocus
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.HasKeyboardFocus;
            }
        }

        public string HelpText
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.HelpText;
            }
        }

        public bool IsContentElement
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.IsContentElement;
            }
        }

        public bool IsControlElement
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.IsControlElement;
            }
        }

        public bool IsEnabled
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.IsEnabled;
            }
        }

        public bool IsKeyboardFocusable
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.IsKeyboardFocusable;
            }
        }

        public bool IsOffscreen
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.IsOffscreen;
            }
        }

        public bool IsPassword
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.IsPassword;
            }
        }

        public bool IsRequiredForForm
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.IsRequiredForForm;
            }
        }

        public string ItemStatus
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.ItemStatus;
            }
        }

        public string ItemType
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.ItemType;
            }
        }

        public AutomationWrapper LabeledBy
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return Wrap(AutomationElement.Current.LabeledBy);
            }
        }

        public string LocalizedControlType
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.LocalizedControlType;
            }
        }

        public string Name
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.Name;
            }
        }

        public int NativeWindowHandle
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.NativeWindowHandle;
            }
        }

        public OrientationType Orientation
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.Orientation;
            }
        }

        public int ProcessId
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return AutomationElement.Current.ProcessId;
            }
        }

        public event AutomationEventHandler AsyncContentLoaded
        {
            add { AddEvent(AutomationElement.AsyncContentLoadedEvent, value); }
            remove { RemoveEvent(value); }
        }

        public event AutomationEventHandler AutomationFocusChanged
        {
            add { AddEvent(AutomationElement.AutomationFocusChangedEvent, value); }
            remove { RemoveEvent(value); }
        }

        public event AutomationEventHandler AutomationPropertyChanged
        {
            add { AddEvent(AutomationElement.AutomationPropertyChangedEvent, value); }
            remove { RemoveEvent(value); }
        }

        public event AutomationEventHandler LayoutInvalidated
        {
            add { AddEvent(AutomationElement.LayoutInvalidatedEvent, value); }
            remove { RemoveEvent(value); }
        }

        public event AutomationEventHandler MenuClosed
        {
            add { AddEvent(AutomationElement.MenuClosedEvent, value); }
            remove { RemoveEvent(value); }
        }

        public event AutomationEventHandler MenuOpened
        {
            add { AddEvent(AutomationElement.MenuOpenedEvent, value); }
            remove { RemoveEvent(value); }
        }

        public event AutomationEventHandler StructureChanged
        {
            add { AddEvent(AutomationElement.StructureChangedEvent, value); }
            remove { RemoveEvent(value); }
        }

        public event AutomationEventHandler ToolTipClosed
        {
            add { AddEvent(AutomationElement.ToolTipClosedEvent, value); }
            remove { RemoveEvent(value); }
        }

        public event AutomationEventHandler ToolTipOpened
        {
            add { AddEvent(AutomationElement.ToolTipOpenedEvent, value); }
            remove { RemoveEvent(value); }
        }

        public AutomationWrapper(AutomationElement automationElement)
        {
            if (automationElement == null)
                throw new ArgumentNullException("automationElement");

            AutomationElement = automationElement;

            if (Debugger.IsAttached)
            {
                _cachedName = Name;
                _cachedControlType = ControlType;
                _cachedAutomationId = AutomationId;
            }
        }

        public Point GetClickablePoint()
        {
            var result = AutomationElement.GetClickablePoint();

            return new Point((int)result.X, (int)result.Y);
        }

        private void AddEvent(AutomationEvent automationEvent, AutomationEventHandler value)
        {
            if (_registeredEvents.ContainsKey(value))
                return;

            var wrapper = new EventWrapper(this, automationEvent, value);

            _registeredEvents.Add(value, wrapper);

            Automation.AddAutomationEventHandler(
                automationEvent,
                AutomationElement,
                TreeScope.Element,
                wrapper.OnEvent
            );
        }

        private void RemoveEvent(AutomationEventHandler value)
        {
            EventWrapper wrapper;

            if (_registeredEvents.TryGetValue(value, out wrapper))
            {
                _registeredEvents.Remove(value);

                Automation.RemoveAutomationEventHandler(
                    wrapper.AutomationEvent,
                    AutomationElement,
                    wrapper.OnEvent
                );
            }
        }

        internal static AutomationWrapper Wrap(AutomationElement automationElement)
        {
            if (automationElement != null)
                return new AutomationWrapper(automationElement);

            return null;
        }

        private T GetPattern<T>(AutomationPattern pattern)
            where T : BasePattern
        {
            return (T)AutomationElement.GetCurrentPattern(pattern);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            var other = obj as AutomationWrapper;

            return other != null && AutomationElement.Equals(other.AutomationElement);
        }

        public override int GetHashCode()
        {
            return AutomationElement.GetHashCode();
        }

        public AutomationWrapperCollection Ancestors()
        {
            return FindAll(TreeScope.Ancestors);
        }

        public AutomationWrapperCollection AncestorsAndSelf()
        {
            return FindAll(TreeScope.Ancestors | TreeScope.Element);
        }

        public AutomationWrapperCollection Children
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return FindAll(TreeScope.Children);
            }
        }

        public AutomationWrapperCollection Descendants()
        {
            return FindAll(TreeScope.Descendants);
        }

        public AutomationWrapperCollection DescendantsAndSelf()
        {
            return FindAll(TreeScope.Descendants | TreeScope.Element);
        }

        private AutomationWrapper FindFirst(TreeScope treeScope)
        {
            return Wrap(AutomationElement.FindFirst(treeScope, Condition.TrueCondition));
        }

        private AutomationWrapperCollection FindAll(TreeScope treeScope)
        {
            return Wrap(AutomationElement.FindAll(treeScope, Condition.TrueCondition));
        }

        private static AutomationWrapper[] Wrap(AutomationElement[] automationElements)
        {
            var result = new AutomationWrapper[automationElements.Length];

            for (int i = 0; i < automationElements.Length; i++)
            {
                result[i] = new AutomationWrapper(automationElements[i]);
            }

            return result;
        }

        private AutomationWrapperCollection Wrap(AutomationElementCollection automationElementCollection)
        {
            return new AutomationWrapperCollection(this, automationElementCollection);
        }

        public int[] GetRuntimeId()
        {
            return AutomationElement.GetRuntimeId();
        }

        public void SetFocus()
        {
            AutomationElement.SetFocus();
        }

        public static bool operator ==(AutomationWrapper left, AutomationWrapper right)
        {
            if (ReferenceEquals(left, right))
                return true;

            return (object)left != null && (object)right != null && left.AutomationElement == right.AutomationElement;
        }

        public static bool operator !=(AutomationWrapper left, AutomationWrapper right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            string name;
            ControlType controlType;
            string automationId;

            if (Debugger.IsAttached)
            {
                name = _cachedName;
                controlType = _cachedControlType;
                automationId = _cachedAutomationId;
            }
            else
            {
                Debugger.NotifyOfCrossThreadDependency();
                name = Name;
                controlType = ControlType;
                automationId = AutomationId;
            }

            return String.Format(
                "Name = {0}, ControlType = {1}, AutomationId = {2}",
                FormatString(name),
                controlType,
                FormatString(automationId)
            );
        }

        private static string FormatString(string value)
        {
            if (value == null)
                return "null";

            return "\"" + value.Replace("\"", "\\\"") + "\"";
        }

        internal static System.Windows.Automation.ControlType GetControlType(ControlType controlType)
        {
            return _controlTypeReverseMap[controlType];
        }

        public AutomationWrapper FindDescendantByAutomationId(string automationId)
        {
            if (automationId == null)
                throw new ArgumentNullException("automationId");

            return Wrap(AutomationElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, automationId)));
        }

        public abstract class PatternWrapper<T>
            where T : BasePattern
        {
            public AutomationWrapper AutomationWrapper { get; private set; }

            public T Pattern { get; private set; }

            protected PatternWrapper(AutomationWrapper automationWrapper, T pattern)
            {
                AutomationWrapper = automationWrapper;
                Pattern = pattern;
            }
        }

        private class EventWrapper
        {
            private readonly AutomationWrapper _automationWrapper;
            private readonly AutomationEventHandler _eventHandler;

            public AutomationEvent AutomationEvent { get; private set; }

            public EventWrapper(AutomationWrapper automationWrapper, AutomationEvent automationEvent, AutomationEventHandler eventHandler)
            {
                _automationWrapper = automationWrapper;
                AutomationEvent = automationEvent;
                _eventHandler = eventHandler;
            }

            public void OnEvent(object sender, AutomationEventArgs e)
            {
                _eventHandler(_automationWrapper, e);
            }
        }
    }
}

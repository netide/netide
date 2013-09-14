using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace UIAutomationWrapper
{
    partial class AutomationWrapper
    {
        public SelectionItemWrapper SelectionItem
        {
            get
            {
                Debugger.NotifyOfCrossThreadDependency();
                return new SelectionItemWrapper(this, GetPattern<SelectionItemPattern>(SelectionItemPattern.Pattern));
            }
        }

        public class SelectionItemWrapper : PatternWrapper<SelectionItemPattern>
        {
            public SelectionItemWrapper(AutomationWrapper automationWrapper, SelectionItemPattern pattern)
                : base(automationWrapper, pattern)
            {
            }

            public void AddToSelection()
            {
                Pattern.AddToSelection();
            }

            public void RemoveFromSelection()
            {
                Pattern.RemoveFromSelection();
            }

            public void Select()
            {
                Pattern.Select();
            }

            public bool IsSelected
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Pattern.Current.IsSelected;
                }
            }

            public AutomationWrapper SelectionContainer
            {
                get
                {
                    Debugger.NotifyOfCrossThreadDependency();
                    return Wrap(Pattern.Current.SelectionContainer);
                }
            }

            public event AutomationEventHandler ElementAddedToSelection
            {
                add { AutomationWrapper.AddEvent(SelectionItemPattern.ElementAddedToSelectionEvent, value); }
                remove { AutomationWrapper.RemoveEvent(value); }
            }

            public event AutomationEventHandler ElementRemovedFromSelection
            {
                add { AutomationWrapper.AddEvent(SelectionItemPattern.ElementRemovedFromSelectionEvent, value); }
                remove { AutomationWrapper.RemoveEvent(value); }
            }

            public event AutomationEventHandler ElementSelected
            {
                add { AutomationWrapper.AddEvent(SelectionItemPattern.ElementSelectedEvent, value); }
                remove { AutomationWrapper.RemoveEvent(value); }
            }
        }
    }
}

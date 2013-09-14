using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using NetIde.Test.Support;
using UIAutomationWrapper;
using ControlType = UIAutomationWrapper.ControlType;

namespace NetIde.Test.FindWindow
{
    internal class FindWindow
    {
        private static Dictionary<string, LookInType> _lookInTypes = (
            from LookInType value in Enum.GetValues(typeof(LookInType))
            let description = value.GetDescription()
            where description != null
            select new { Value = value, Description = description }
        ).ToDictionary(p => p.Description, p => p.Value);

        public AutomationWrapper Window { get; private set; }

        public FindMode FindMode
        {
            get { return ReplaceWithControl == null ? FindMode.Find : FindMode.Replace; }
            set
            {
                Window
                    .Children[ControlType.ToolBar]
                    .Children[value == FindMode.Find ? "Find" : "Find and Replace"]
                    .Invoke.Invoke();
            }
        }

        public AutomationWrapper FindWhatControl
        {
            get { return Window.FindDescendantByAutomationId("_findWhat"); }
        }

        public string FindWhat
        {
            get { return FindWhatControl.Value.Value; }
            set { FindWhatControl.Value.Value = value ?? String.Empty; }
        }

        public AutomationWrapper ReplaceWithControl
        {
            get { return Window.FindDescendantByAutomationId("_replaceWith"); }
        }

        public string ReplaceWith
        {
            get { return ReplaceWithControl.Value.Value; }
            set { ReplaceWithControl.Value.Value = value ?? String.Empty; }
        }

        public AutomationWrapper LookInControl
        {
            get { return Window.FindDescendantByAutomationId("_lookIn"); }
        }

        public string LookIn
        {
            get { return LookInControl.Value.Value; }
            set { LookInControl.Value.Value = value ?? String.Empty; }
        }

        public LookInType LookInType
        {
            get
            {
                LookInType result;
                if (_lookInTypes.TryGetValue(LookIn, out result))
                    return result;
                return LookInType.Unknown;
            }
            set { LookIn = value.GetDescription(); }
        }

        public AutomationWrapper IncludeSubFoldersControl
        {
            get { return Window.FindDescendantByAutomationId("_includeSubFolders"); }
        }

        public bool IncludeSubFolders
        {
            get { return GetCheckboxState(IncludeSubFoldersControl); }
            set { SetCheckboxState(IncludeSubFoldersControl, value); }
        }

        public AutomationWrapper MatchCaseControl
        {
            get { return Window.FindDescendantByAutomationId("_matchCase"); }
        }

        public bool MatchCase
        {
            get { return GetCheckboxState(MatchCaseControl); }
            set { SetCheckboxState(MatchCaseControl, value); }
        }

        public AutomationWrapper MatchWholeWordControl
        {
            get { return Window.FindDescendantByAutomationId("_matchWholeWord"); }
        }

        public bool MatchWholeWord
        {
            get { return GetCheckboxState(MatchWholeWordControl); }
            set { SetCheckboxState(MatchWholeWordControl, value); }
        }

        public AutomationWrapper UseRegularExpressionsControl
        {
            get { return Window.FindDescendantByAutomationId("_useRegularExpressions"); }
        }

        public bool UseRegularExpressions
        {
            get { return GetCheckboxState(UseRegularExpressionsControl); }
            set { SetCheckboxState(UseRegularExpressionsControl, value); }
        }

        public AutomationWrapper LookInFileTypesControl
        {
            get { return Window.FindDescendantByAutomationId("_lookInFileTypes"); }
        }

        public string LookInFileTypes
        {
            get { return LookInFileTypesControl.Value.Value; }
            set { LookInFileTypesControl.Value.Value = value ?? String.Empty; }
        }

        public AutomationWrapper KeepModifiedFilesOpenControl
        {
            get { return Window.FindDescendantByAutomationId("_keepOpen"); }
        }

        public bool KeepModifiedFilesOpen
        {
            get { return GetCheckboxState(KeepModifiedFilesOpenControl); }
            set { SetCheckboxState(KeepModifiedFilesOpenControl, value); }
        }

        public FindWindow(AutomationWrapper window)
        {
            Window = window;
        }

        private bool GetCheckboxState(AutomationWrapper control)
        {
            return control.Toggle.ToggleState == ToggleState.On;
        }

        private void SetCheckboxState(AutomationWrapper control, bool value)
        {
            var togglePattern = control.Toggle;

            if (togglePattern.ToggleState != (value ? ToggleState.On : ToggleState.Off))
                togglePattern.Toggle();
        }

        public void FindPrevious()
        {
            Window.FindDescendantByAutomationId("_findPrevious").Invoke.Invoke();
        }

        public void FindNext()
        {
            Window.FindDescendantByAutomationId("_findNext").Invoke.Invoke();
        }

        public void FindAll()
        {
            Window.FindDescendantByAutomationId("_findAll").Invoke.Invoke();
        }

        public void Replace()
        {
            Window.FindDescendantByAutomationId("_replace").Invoke.Invoke();
        }

        public void ReplaceAll()
        {
            Window.FindDescendantByAutomationId("_replaceAll").Invoke.Invoke();
        }

        public void SkipFile()
        {
            Window.FindDescendantByAutomationId("_skipFile").Invoke.Invoke();
        }
    }
}

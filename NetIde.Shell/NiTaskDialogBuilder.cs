using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiTaskDialogBuilder
    {
        private readonly INiShell _shell;
        private string _mainInstruction;
        private string _content;
        private NiTaskDialogIcon? _mainIcon;
        private NiTaskDialogCommonButtons? _commonButtons;
        private NiTaskDialogFlags _flags = NiTaskDialogFlags.PositionRelativeToWindow;
        private string _expandedControlText;
        private string _expandedInformation;

        internal NiTaskDialogBuilder(INiShell shell)
        {
            if (shell == null)
                throw new ArgumentNullException("shell");

            _shell = shell;
        }

        public NiTaskDialogBuilder MainInstruction(string mainInstruction)
        {
            _mainInstruction = mainInstruction;
            return this;
        }

        public NiTaskDialogBuilder Content(string content)
        {
            _content = content;
            return this;
        }

        public NiTaskDialogBuilder ExpandedControlText(string expandedControlText)
        {
            _expandedControlText = expandedControlText;
            return this;
        }

        public NiTaskDialogBuilder ExpandedInformation(string expandedInformation)
        {
            _expandedInformation = expandedInformation;
            return this;
        }

        public NiTaskDialogBuilder MainIcon(NiTaskDialogIcon mainIcon)
        {
            _mainIcon = mainIcon;
            return this;
        }

        public NiTaskDialogBuilder CommonButtons(NiTaskDialogCommonButtons commonButtons)
        {
            _commonButtons = commonButtons;
            return this;
        }

        public void Alert()
        {
            Alert(null);
        }

        public void Alert(IWin32Window owner)
        {
            if (!_mainIcon.HasValue)
                _mainIcon = NiTaskDialogIcon.Error;
            if (!_commonButtons.HasValue)
                _commonButtons = NiTaskDialogCommonButtons.OK;

            _flags |= NiTaskDialogFlags.AllowDialogCancellation;

            Show(owner);
        }

        public bool Confirm()
        {
            return Confirm(null);
        }

        public bool Confirm(IWin32Window owner)
        {
            if (!_mainIcon.HasValue)
                _mainIcon = NiTaskDialogIcon.Warning;
            if (!_commonButtons.HasValue)
                _commonButtons = NiTaskDialogCommonButtons.YesNo;
            if (_mainInstruction == null)
                _mainInstruction = Labels.AreYouSure;

            return Show(owner) == DialogResult.Yes;
        }

        public DialogResult Show()
        {
            return Show(null);
        }

        public DialogResult Show(IWin32Window owner)
        {
            INiTaskDialog taskDialog;
            ErrorUtil.ThrowOnFailure(_shell.CreateTaskDialog(out taskDialog));

            if (_flags != 0)
                ErrorUtil.ThrowOnFailure(taskDialog.SetFlags(_flags));
            if (_mainInstruction != null)
                ErrorUtil.ThrowOnFailure(taskDialog.SetMainInstruction(_mainInstruction));
            if (_content != null)
                ErrorUtil.ThrowOnFailure(taskDialog.SetContent(_content));
            if (_expandedControlText != null)
                ErrorUtil.ThrowOnFailure(taskDialog.SetExpandedControlText(_expandedControlText));
            if (_expandedInformation != null)
                ErrorUtil.ThrowOnFailure(taskDialog.SetExpandedInformation(_expandedInformation));
            if (_mainIcon.HasValue)
                ErrorUtil.ThrowOnFailure(taskDialog.SetMainIcon(_mainIcon.Value));
            if (_commonButtons.HasValue)
                ErrorUtil.ThrowOnFailure(taskDialog.SetCommonButtons(_commonButtons.Value));

            bool verificationFlagChecked;
            int radioButtonResult;
            int result;
            ErrorUtil.ThrowOnFailure(taskDialog.Show(
                owner != null ? owner.Handle : IntPtr.Zero,
                out verificationFlagChecked,
                out radioButtonResult,
                out result
            ));

            return (DialogResult)result;
        }
    }
}

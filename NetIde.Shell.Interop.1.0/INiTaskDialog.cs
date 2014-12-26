using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiTaskDialog : INiConnectionPoint, IDisposable
    {
        HResult Advise(INiTaskDialogNotify sink, out int cookie);
        HResult GetWindowTitle(out string windowTitle);
        HResult SetWindowTitle(string windowTitle);
        HResult GetMainInstruction(out string mainInstruction);
        HResult SetMainInstruction(string mainInstruction);
        HResult GetContent(out string content);
        HResult SetContent(string content);
        HResult GetVerificationText(out string verificationText);
        HResult SetVerificationText(string verificationText);
        HResult GetExpandedInformation(out string expandedInformation);
        HResult SetExpandedInformation(string expandedInformation);
        HResult GetExpandedControlText(out string expandedControlText);
        HResult SetExpandedControlText(string expandedControlText);
        HResult GetCollapsedControlText(out string collapsedControlText);
        HResult SetCollapsedControlText(string collapsedControlText);
        HResult GetFooter(out string footer);
        HResult SetFooter(string footer);
        HResult GetCommonButtons(out NiTaskDialogCommonButtons commonButtons);
        HResult SetCommonButtons(NiTaskDialogCommonButtons commonButtons);
        HResult GetMainIcon(out NiTaskDialogIcon mainIcon);
        HResult SetMainIcon(NiTaskDialogIcon mainIcon);
        HResult GetMainIcon(out IResource mainIcon);
        HResult SetMainIcon(IResource mainIcon);
        HResult GetFooterIcon(out NiTaskDialogIcon footerIcon);
        HResult SetFooterIcon(NiTaskDialogIcon footerIcon);
        HResult GetFooterIcon(out IResource footerIcon);
        HResult SetFooterIcon(IResource footerIcon);
        HResult GetFlags(out NiTaskDialogFlags flags);
        HResult SetFlags(NiTaskDialogFlags flags);
        HResult GetDefaultButton(out int id);
        HResult SetDefaultButton(int id);
        HResult GetDefaultRadioButton(out int id);
        HResult SetDefaultRadioButton(int id);
        HResult GetWidth(out int width);
        HResult SetWidth(int width);
        HResult AddButton(int id, string text);
        HResult AddRadioButton(int id, string text);
        HResult Show(IntPtr owner, out bool verificationFlagChecked, out int radioButtonResult, out int result);
    }
}

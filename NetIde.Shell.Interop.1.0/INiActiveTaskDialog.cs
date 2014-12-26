using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiActiveTaskDialog
    {
        HResult GetHandle(out IntPtr handle);
        HResult ClickButton(int id);
        HResult ClickRadioButton(int id);
        HResult ClickVerification(bool @checked, bool setFocus);
        HResult SetButtonEnabled(int id, bool enabled);
        HResult SetRadioButtonEnabled(int id, bool enabled);
        HResult SetMarqueeProgressBar(bool marquee);
        HResult SetProgressBarState(NiProgressBarState state);
        HResult SetProgressBarRange(int min, int max);
        HResult SetProgressBarPosition(int position);
        HResult SetProgressBarMarquee(bool startMarquee, int speed);
        HResult SetContent(string content);
        HResult UpdateContent(string content);
        HResult SetExpandedInformation(string expandedInformation);
        HResult UpdateExpandedInformation(string expandedInformation);
        HResult SetFooter(string footer);
        HResult UpdateFooter(string footer);
        HResult SetMainInstruction(string mainInstruction);
        HResult UpdateMainInstruction(string mainInstruction);
        HResult SetButtonElevationRequiredState(int id, bool elevationRequired);
        HResult UpdateMainIcon(NiTaskDialogIcon icon);
        HResult UpdateMainIcon(IResource icon);
        HResult UpdateFooterIcon(NiTaskDialogIcon icon);
        HResult UpdateFooterIcon(IResource icon);
    }
}

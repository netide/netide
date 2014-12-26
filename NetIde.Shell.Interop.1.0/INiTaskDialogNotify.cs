using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiTaskDialogNotify
    {
        void OnButtonClick(INiActiveTaskDialog taskDialog, int id, ref bool close);
        void OnCreated(INiActiveTaskDialog taskDialog);
        void OnDestroyed(INiActiveTaskDialog taskDialog);
        void OnDialogConstructed(INiActiveTaskDialog taskDialog);
        void OnExpandoButtonClicked(INiActiveTaskDialog taskDialog, bool expanded);
        void OnHelp(INiActiveTaskDialog taskDialog);
        void OnHyperlinkClicked(INiActiveTaskDialog taskDialog, string hyperlink);
        void OnRadioButtonClicked(INiActiveTaskDialog taskDialog, int id);
        void OnTimer(INiActiveTaskDialog taskDialog, int elapsed, ref bool resetTimer);
        void OnVerificationClicked(INiActiveTaskDialog taskDialog, bool @checked);
    }
}

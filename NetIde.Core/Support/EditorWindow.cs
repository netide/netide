using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Core.Support
{
    internal abstract class EditorWindow : NiWindowPane
    {
        protected abstract Control CreateClient();

        public override HResult Initialize()
        {
            try
            {
                var hr = base.Initialize();

                if (ErrorUtil.Failure(hr))
                    return hr;

                var control = CreateClient();
                control.Dock = DockStyle.Fill;
                Controls.Add(control);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
    }
}

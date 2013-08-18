using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using NetIde.Services.LocalRegistry;
using NetIde.Shell;
using NetIde.Shell.Interop;
using log4net;

namespace NetIde.Services.ToolsOptions
{
    internal class NiToolsOptions : ServiceBase, INiToolsOptions
    {
        public bool IsToolsOptionsOpen { get; private set; }

        public NiToolsOptions(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public HResult Open()
        {
            try
            {
                if (IsToolsOptionsOpen)
                    return HResult.False;

                IsToolsOptionsOpen = true;

                try
                {
                    using (var form = new ToolsOptionsForm())
                    {
                        form.ShowDialog(this);
                    }
                }
                finally
                {
                    IsToolsOptionsOpen = false;
                }

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
    }
}

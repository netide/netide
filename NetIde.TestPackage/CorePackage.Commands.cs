using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.TestPackage
{
    partial class CorePackage
    {
        private void BuildCommandMapper()
        {
            _commandMapper.Add(
                TpResources.Help_Topic1,
                p => ErrorUtil.ThrowOnFailure(((INiHelp)GetService(typeof(INiHelp))).Navigate("test", "Topic1.html"))
            );
        }
    }
}

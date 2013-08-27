using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Core
{
    internal static class NiShellUtil
    {
        public static bool Confirm(IServiceProvider serviceProvider)
        {
            return Confirm(serviceProvider, Labels.AreYouSure);
        }

        public static bool Confirm(IServiceProvider serviceProvider, string label)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");
            if (label == null)
                throw new ArgumentNullException("label");

            var shell = (INiShell)serviceProvider.GetService(typeof(INiShell));

            DialogResult result;
            ErrorUtil.ThrowOnFailure(shell.ShowMessageBox(
                Labels.AreYouSure,
                null,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                out result
            ));

            return result == DialogResult.Yes;
        }

        public static bool Checked(IServiceProvider owner, Action action)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (action == null)
                throw new ArgumentNullException("action");

            try
            {
                action();

                return true;
            }
            catch (Exception ex)
            {
                ((INiShell)owner.GetService(typeof(INiShell))).ShowMessageBox(
                    ex.Message,
                    null,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return false;
            }
        }
    }
}

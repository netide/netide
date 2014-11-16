using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;

namespace NetIde.Shell
{
    public class NiForm : Form
    {
        private int _cookie;
        public NiCommandMapper CommandMapper { get; private set; }

        public NiForm()
        {
            CommandMapper = new NiCommandMapper();
        }

        protected override void OnShown(EventArgs e)
        {
            var registerPriorityCommandTarget = (INiRegisterPriorityCommandTarget)GetService(typeof(INiRegisterPriorityCommandTarget));

            if (registerPriorityCommandTarget != null)
            {
                ErrorUtil.ThrowOnFailure(
                    registerPriorityCommandTarget.RegisterPriorityCommandTarget(
                        new CommandMapperWrapper(this),
                        out _cookie
                    )
                );
            }

            base.OnShown(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            var registerPriorityCommandTarget = (INiRegisterPriorityCommandTarget)GetService(typeof(INiRegisterPriorityCommandTarget));

            if (registerPriorityCommandTarget != null)
            {
                ErrorUtil.ThrowOnFailure(
                    registerPriorityCommandTarget.UnregisterPriorityCommandTarget(_cookie)
                );
            }

            base.OnClosed(e);
        }

        private class CommandMapperWrapper : ServiceObject, INiCommandTarget
        {
            private readonly NiForm _form;
            private readonly NiCommandMapper _commandMapper;

            public CommandMapperWrapper(NiForm form)
            {
                _form = form;
                _commandMapper = _form.CommandMapper;
            }

            public HResult QueryStatus(Guid command, out NiCommandStatus status)
            {
                status = 0;

                try
                {
                    var hr = _commandMapper.QueryStatus(command, out status);

                    // If we're a modal dialog, we want to prevent other command
                    // handlers from seeing any commands.

                    if (hr == HResult.False && _form.Modal)
                        return HResult.OK;

                    return hr;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult Exec(Guid command, object argument, out object result)
            {
                result = null;

                try
                {
                    var hr = _commandMapper.Exec(command, argument, out result);

                    // If we're a modal dialog, we want to prevent other command
                    // handlers from seeing any commands.

                    if (hr == HResult.False && _form.Modal)
                        return HResult.OK;

                    return hr;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }
        }
    }
}

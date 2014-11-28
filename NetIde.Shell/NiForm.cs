using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NetIde.Shell.Interop;
using Form = NetIde.Util.Forms.Form;

namespace NetIde.Shell
{
    public class NiForm : Form
    {
        private static readonly Regex HelpTopicRe = new Regex("^([^/:]+):(.+)$", RegexOptions.Compiled);

        private int _cookie;
        private string _helpTopic = String.Empty;

        public NiCommandMapper CommandMapper { get; private set; }

        [Category("Behavior")]
        [DefaultValue("")]
        public string HelpTopic
        {
            get { return _helpTopic; }
            set
            {
                if (value == null)
                    value = String.Empty;

                if (!HelpTopicRe.IsMatch(value))
                    throw new ArgumentException("Help topic must be formatted as \"root:path\"");

                if (_helpTopic != value)
                {
                    _helpTopic = value;
                    HelpButton = value.Length > 0;
                }
            }
        }

        public NiForm()
        {
            CommandMapper = new NiCommandMapper();

            CommandMapper.Add(
                NiResources.Help_ViewHelp,
                p => ShowHelp()
            );
        }

        protected override void OnHelpButtonClicked(CancelEventArgs e)
        {
            base.OnHelpButtonClicked(e);

            e.Cancel = true;

            ShowHelp();
        }

        private void ShowHelp()
        {
            if (_helpTopic.Length > 0)
            {
                var match = HelpTopicRe.Match(_helpTopic);

                ErrorUtil.ThrowOnFailure(((INiHelp)GetService(typeof(INiHelp))).Navigate(
                    match.Groups[1].Value,
                    match.Groups[2].Value
                ));
            }
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

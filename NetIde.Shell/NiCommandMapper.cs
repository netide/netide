using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public sealed class NiCommandMapper : INiCommandTarget
    {
        private readonly Dictionary<Guid, CommandRegistration> _registrations = new Dictionary<Guid, CommandRegistration>();

        public void Add(Guid command, NiExecCallback exec)
        {
            Add(command, exec, null);
        }

        public void Add(Guid command, NiExecCallback exec, NiQueryCallback queryStatus)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            if (exec == null)
                throw new ArgumentNullException("exec");

            _registrations[command] = new CommandRegistration(exec, queryStatus);
        }

        public HResult QueryStatus(Guid command, out NiCommandStatus status)
        {
            status = 0;

            try
            {
                CommandRegistration registration;

                if (_registrations.TryGetValue(command, out registration))
                {
                    status = NiCommandStatus.Supported;

                    if (registration.QueryStatus != null)
                    {
                        var e = new NiQueryEventArgs();

                        registration.QueryStatus(e);

                        status |= e.Status;
                    }
                    else
                    {
                        status |= NiCommandStatus.Enabled;
                    }

                    return HResult.OK;
                }

                return HResult.False;
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
                CommandRegistration registration;

                if (_registrations.TryGetValue(command, out registration))
                {
                    var e = new NiExecEventArgs(argument);

                    registration.Exec(e);

                    result = e.Result;

                    return HResult.OK;
                }

                return HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private class CommandRegistration
        {
            public NiExecCallback Exec { get; private set; }
            public NiQueryCallback QueryStatus { get; private set; }

            public CommandRegistration(NiExecCallback exec, NiQueryCallback queryStatus)
            {
                Exec = exec;
                QueryStatus = queryStatus;
            }
        }
    }
}

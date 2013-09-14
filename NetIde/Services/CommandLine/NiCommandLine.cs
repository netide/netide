using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;
using log4net;

namespace NetIde.Services.CommandLine
{
    internal class NiCommandLine : ServiceBase, INiCommandLine
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NiCommandLine));

        private readonly Dictionary<string, string> _arguments = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, bool> _switches = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
        private readonly List<string> _remainingArguments = new List<string>();

        public NiCommandLine(IServiceProvider serviceProvider, string[] args)
            : base(serviceProvider)
        {
            _switches["RuntimeUpdate"] = false;

            RegistryUtil.ForEachPackage(this, "CommandLine", (packageId, key) =>
            {
                foreach (string name in key.GetSubKeyNames())
                {
                    using (var switchKey = key.OpenSubKey(name))
                    {
                        Log.InfoFormat("Loading switch '{0}'", switchKey);

                        try
                        {
                            _switches[name] = RegistryUtil.GetBool(switchKey.GetValue("ExpectArgument"));
                        }
                        catch (Exception ex)
                        {
                            Log.Warn("Could not load switch", ex);
                        }
                    }
                }
            });

            string failedArgument = null;

            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    string arg = args[i];

                    if (arg.StartsWith("/"))
                    {
                        bool expectArgument;
                        if (!_switches.TryGetValue(arg.Substring(1), out expectArgument))
                        {
                            failedArgument = arg;
                            break;
                        }

                        string value = null;

                        if (expectArgument)
                        {
                            if (i >= args.Length - 1)
                            {
                                failedArgument = arg;
                                break;
                            }

                            value = args[++i];
                        }

                        if (!_arguments.ContainsKey(arg))
                            _arguments.Add(arg, value);
                    }
                    else
                    {
                        _remainingArguments.Add(arg);
                    }
                }
            }

            if (failedArgument != null)
            {
                MessageBox.Show(
                    String.Format(Labels.CannotProcessSwitch, failedArgument),
                    Labels.NetIde,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error
                );

                Environment.Exit(0);
            }
        }

        public HResult GetOption(string name, out bool present, out string value)
        {
            present = false;
            value = null;

            try
            {
                if (name == null)
                    throw new ArgumentNullException("name");

                present = _arguments.TryGetValue(name, out value);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetOtherArguments(out string[] arguments)
        {
            arguments = null;

            try
            {
                arguments = _remainingArguments.ToArray();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.CommandManager
{
    internal partial class NiCommandManager : ServiceBase, INiCommandManager, INiRegisterPriorityCommandTarget
    {
        private int _nextCommandTargetCookie = 1;
        private readonly Dictionary<int, INiCommandTarget> _commandTargets = new Dictionary<int, INiCommandTarget>();
        private int _nextPriorityCommandTargetCookie = 1;
        private readonly Dictionary<int, INiCommandTarget> _priorityCommandTargets = new Dictionary<int, INiCommandTarget>();
        private readonly List<INiCommandTarget> _priorityCommandTargetsOrdered = new List<INiCommandTarget>(); 
        private readonly Dictionary<Guid, object> _objects = new Dictionary<Guid, object>();
        private readonly INiEnv _env;

        public NiCommandManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _env = (INiEnv)serviceProvider.GetService(typeof(INiEnv));

            ((IServiceContainer)GetService(typeof(IServiceContainer))).AddService(
                typeof(INiRegisterPriorityCommandTarget),
                this
            );

            Application.AddMessageFilter(new ShortcutMessageFilter(this));
        }

        public void LoadFromResources(INiPackage package, IResource resource)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");

            string niMenu = null;
            var resources = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            using (var stream = new MemoryStream())
            {
                // We copy the stream into a memory stream to limit the number
                // of MBRO calls made.

                using (var source = StreamUtil.ToStream(resource))
                {
                    source.CopyTo(stream);
                }

                stream.Position = 0;

                using (var reader = new ResourceReader(stream))
                {
                    foreach (DictionaryEntry entry in reader)
                    {
                        if (String.Equals((string)entry.Key, "NiMenu", StringComparison.OrdinalIgnoreCase))
                            niMenu = (string)entry.Value;
                        else
                            resources.Add((string)entry.Key, entry.Value);
                    }
                }
            }

            if (niMenu == null)
                throw new NetIdeException(Labels.NiMenuMissing);

            new MenuBuilder(package, niMenu, resources).Build();
        }

        public HResult CreateCommandBar(Guid id, NiCommandBarKind kind, int priority, out INiCommandBar commandBar)
        {
            commandBar = null;

            try
            {
                commandBar = new NiCommandBar(id, kind, priority);

                _objects[id] = commandBar;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult CreateCommandBarPopup(Guid id, int priority, out INiCommandBarPopup popup)
        {
            popup = null;

            try
            {
                popup = new NiCommandBarPopup(id, priority);

                _objects[id] = popup;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult CreateCommandBarWindow(Guid id, out INiWindowPane commandBar)
        {
            commandBar = null;

            try
            {
                commandBar = new CommandBarWindow(id);
                commandBar.SetSite(this);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult CreateCommandBarGroup(Guid id, int priority, out INiCommandBarGroup group)
        {
            group = null;

            try
            {
                group = new NiCommandBarGroup(id, priority);

                _objects[id] = group;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult CreateCommandBarButton(Guid id, int priority, out INiCommandBarButton button)
        {
            return CreateCommandBarButton(id, priority, null, out button);
        }

        public HResult CreateCommandBarButton(Guid id, int priority, string code, out INiCommandBarButton button)
        {
            button = null;

            try
            {
                button = new NiCommandBarButton(id, priority, code);

                _objects[id] = button;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult CreateCommandBarComboBox(Guid id, Guid fillCommand, int priority, out INiCommandBarComboBox comboBox)
        {
            comboBox = null;

            try
            {
                comboBox = new NiCommandBarComboBox(id, fillCommand, priority);

                _objects[id] = comboBox;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult CreateCommandBarTextBox(Guid id, int priority, out INiCommandBarTextBox textBox)
        {
            textBox = null;

            try
            {
                textBox = new NiCommandBarTextBox(id, priority);

                _objects[id] = textBox;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult CreateCommandBarLabel(Guid id, int priority, out INiCommandBarLabel label)
        {
            label = null;

            try
            {
                label = new NiCommandBarLabel(id, priority);

                _objects[id] = label;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult RegisterCommandTarget(INiCommandTarget commandTarget, out int cookie)
        {
            cookie = 0;

            try
            {
                if (commandTarget == null)
                    throw new ArgumentNullException("commandTarget");

                cookie = _nextCommandTargetCookie++;

                _commandTargets.Add(cookie, commandTarget);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult UnregisterCommandTarget(int cookie)
        {
            try
            {
                return _commandTargets.Remove(cookie)
                    ? HResult.OK
                    : HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private IEnumerable<INiCommandTarget> GetCommandTargets()
        {
            // We allow the priority command targets to execute in the reverse
            // order they have been registered. This means that the last one
            // registered has the first try at handling the command.

            for (int i = _priorityCommandTargetsOrdered.Count - 1; i >= 0; i--)
            {
                yield return _priorityCommandTargetsOrdered[i];
            }

            foreach (var commandTarget in _commandTargets.Values)
            {
                yield return commandTarget;
            }

            var activeDocument = _env.ActiveDocument as INiCommandTarget;

            if (activeDocument != null)
                yield return activeDocument;
        }

        public HResult QueryStatus(Guid command, out NiCommandStatus status)
        {
            status = 0;

            try
            {
                foreach (var commandTarget in GetCommandTargets())
                {
                    NiCommandStatus targetStatus;
                    if (ErrorUtil.ThrowOnFailure(commandTarget.QueryStatus(command, out targetStatus)))
                    {
                        status = targetStatus;
                        return HResult.OK;
                    }
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
                foreach (var commandTarget in GetCommandTargets())
                {
                    object thisResult;
                    if (ErrorUtil.ThrowOnFailure(commandTarget.Exec(command, argument, out thisResult)))
                    {
                        result = thisResult;
                        return HResult.OK;
                    }
                }

                return HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult FindCommandBar(Guid id, out INiCommandBar commandBar)
        {
            commandBar = null;

            try
            {
                object obj;

                if (_objects.TryGetValue(id, out obj))
                    commandBar = obj as INiCommandBar;

                return commandBar == null ? HResult.False : HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult FindCommandBarGroup(Guid id, out INiCommandBarGroup group)
        {
            group = null;

            try
            {
                object obj;

                if (_objects.TryGetValue(id, out obj))
                    group = obj as INiCommandBarGroup;

                return group == null ? HResult.False : HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult FindCommandBarControl(Guid id, out INiCommandBarControl command)
        {
            command = null;

            try
            {
                object obj;

                if (_objects.TryGetValue(id, out obj))
                    command = obj as INiCommandBarControl;

                return command == null ? HResult.False : HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult FindCommandBarPopup(Guid id, out INiCommandBarPopup command)
        {
            command = null;

            try
            {
                object obj;

                if (_objects.TryGetValue(id, out obj))
                    command = obj as INiCommandBarPopup;

                return command == null ? HResult.False : HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult RegisterPriorityCommandTarget(INiCommandTarget commandTarget, out int cookie)
        {
            cookie = 0;

            try
            {
                if (commandTarget == null)
                    throw new ArgumentNullException("commandTarget");

                cookie = _nextPriorityCommandTargetCookie++;
                _priorityCommandTargets.Add(cookie, commandTarget);
                _priorityCommandTargetsOrdered.Add(commandTarget);

                // The available command targets have changed; force a requery.

                ErrorUtil.ThrowOnFailure(((INiShell)GetService(typeof(INiShell))).InvalidateRequerySuggested());

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult UnregisterPriorityCommandTarget(int cookie)
        {
            try
            {
                INiCommandTarget commandTarget;
                if (!_priorityCommandTargets.TryGetValue(cookie, out commandTarget))
                    return HResult.False;

                _priorityCommandTargetsOrdered.Remove(commandTarget);

                // The available command targets have changed; force a requery.

                ErrorUtil.ThrowOnFailure(((INiShell)GetService(typeof(INiShell))).InvalidateRequerySuggested());

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private class ShortcutMessageFilter : IMessageFilter
        {
            private const int WM_KEYDOWN = 0x100;
            private const int WM_SYSKEYDOWN = 0x104;

            private readonly NiCommandManager _commandManager;

            public ShortcutMessageFilter(NiCommandManager commandManager)
            {
                _commandManager = commandManager;
            }

            public bool PreFilterMessage(ref Message m)
            {
                switch (m.Msg)
                {
                    case WM_KEYDOWN:
                    case WM_SYSKEYDOWN:
                        return _commandManager._keyboardMappingManager.ProcessMessage(ref m);

                    default:
                        return false;
                }
            }
        }
    }
}

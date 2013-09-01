using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.CommandManager
{
    internal class NiCommandManager : ServiceBase, INiCommandManager
    {
        private int _nextCookie = 1;
        private readonly Dictionary<int, INiCommandTarget> _commandTargets = new Dictionary<int, INiCommandTarget>();
        private readonly Dictionary<Guid, object> _objects = new Dictionary<Guid, object>();
        private INiEnv _env;

        public NiCommandManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _env = (INiEnv)serviceProvider.GetService(typeof(INiEnv));
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
            button = null;

            try
            {
                button = new NiCommandBarButton(id, priority);

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

        public HResult RegisterCommandTarget(INiCommandTarget commandTarget, out int cookie)
        {
            cookie = 0;

            try
            {
                if (commandTarget == null)
                    throw new ArgumentNullException("commandTarget");

                cookie = _nextCookie++;

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
                if (!_commandTargets.ContainsKey(cookie))
                    return HResult.False;

                _commandTargets.Remove(cookie);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult QueryStatus(Guid command, out NiCommandStatus status)
        {
            status = 0;

            try
            {
                foreach (var commandTarget in _commandTargets.Values)
                {
                    NiCommandStatus targetStatus;
                    var hr = commandTarget.QueryStatus(command, out targetStatus);
                    ErrorUtil.ThrowOnFailure(hr);

                    if (hr == HResult.OK)
                    {
                        status = targetStatus;
                        return HResult.OK;
                    }
                }

                var activeDocument = _env.ActiveDocument as INiCommandTarget;

                if (activeDocument != null)
                {
                    NiCommandStatus targetStatus;
                    var hr = activeDocument.QueryStatus(command, out targetStatus);
                    ErrorUtil.ThrowOnFailure(hr);

                    if (hr == HResult.OK)
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
                foreach (var commandTarget in _commandTargets.Values)
                {
                    object thisResult;
                    var hr = commandTarget.Exec(command, argument, out thisResult);
                    ErrorUtil.ThrowOnFailure(hr);

                    if (hr == HResult.OK)
                    {
                        result = thisResult;
                        return HResult.OK;
                    }
                }

                var activeDocument = _env.ActiveDocument as INiCommandTarget;

                if (activeDocument != null)
                {
                    object thisResult;
                    var hr = activeDocument.Exec(command, argument, out thisResult);
                    ErrorUtil.ThrowOnFailure(hr);

                    if (hr == HResult.OK)
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
    }
}

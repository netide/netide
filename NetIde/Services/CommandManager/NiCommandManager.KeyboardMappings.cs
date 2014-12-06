using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using log4net;
using Microsoft.Win32;
using NetIde.Services.Env;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util;

namespace NetIde.Services.CommandManager
{
    partial class NiCommandManager
    {
        private KeyboardMappingManager _keyboardMappingManager;

        public void InitializeKeyboardMappings()
        {
            _keyboardMappingManager = new KeyboardMappingManager(this);
        }

        public HResult LoadKeyboardMappings(out INiKeyboardMappings mappings)
        {
            mappings = null;

            try
            {
                mappings = _keyboardMappingManager.Load();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult SaveKeyboardMappings(INiKeyboardMappings mappings)
        {
            try
            {
                if (mappings == null)
                    throw new ArgumentNullException("mappings");

                _keyboardMappingManager.Save(mappings);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private class KeyboardMappingManager
        {
            private static readonly ILog Log = LogManager.GetLogger(typeof(KeyboardMappingManager));

            private readonly NiCommandManager _commandManager;
            private readonly string _registryKey;
            private readonly Dictionary<Guid, Keys> _initialKeys = new Dictionary<Guid, Keys>();
            private readonly Dictionary<Keys, Guid[]> _keyMap = new Dictionary<Keys, Guid[]>(); 

            public KeyboardMappingManager(NiCommandManager commandManager)
            {
                _commandManager = commandManager;

                _registryKey = ((NiEnv)commandManager.GetService(typeof(INiEnv))).RegistryRoot + "\\KeyboardMappings";

                LoadInitialKeys();
                LoadMappings();
            }

            private void LoadInitialKeys()
            {
                foreach (var obj in _commandManager._objects.Values)
                {
                    var button = obj as INiCommandBarButton;
                    if (button != null)
                        _initialKeys.Add(button.Id, button.ShortcutKeys);
                }
            }

            public INiKeyboardMappings Load()
            {
                return new NiKeyboardMappings(_commandManager);
            }

            public void Save(INiKeyboardMappings mappings)
            {
                if (mappings == null)
                    throw new ArgumentNullException("mappings");

                // Update the registry with the new mappings.

                var newMappings = ((NiKeyboardMappings)mappings).Mappings;

                var seen = new HashSet<Guid>();

                using (var key = OpenRegistryKey(true))
                {
                    foreach (string name in key.GetValueNames())
                    {
                        Guid guid;
                        if (!Guid.TryParse(name, out guid))
                        {
                            key.DeleteValue(name);
                        }
                        else
                        {
                            seen.Add(guid);

                            Keys[] keys;
                            if (newMappings.TryGetValue(guid, out keys))
                                key.SetValue(name, CreateKeysValue(keys));
                            else
                                key.DeleteValue(name);
                        }
                    }

                    foreach (var item in newMappings)
                    {
                        if (seen.Contains(item.Key))
                            continue;

                        key.SetValue(item.Key.ToString("B"), CreateKeysValue(item.Value));
                    }
                }

                // Reload the mappings from the registry.

                LoadMappings();
            }

            private void LoadMappings()
            {
                // We're also rebuilding the keymap in this run.

                _keyMap.Clear();

                var mappings = LoadFromRegistry();

                foreach (var obj in _commandManager._objects.Values)
                {
                    var button = obj as INiCommandBarButton;
                    if (button == null)
                        continue;

                    // Find the effective keys.

                    var id = button.Id;

                    Keys[] keys;
                    if (!mappings.TryGetValue(id, out keys))
                    {
                        Keys key;
                        if (_initialKeys.TryGetValue(id, out key))
                            keys = new[] { key };
                    }

                    // Apply the correct shortcut to the button.

                    if (keys == null || keys.Length == 0)
                        button.ShortcutKeys = 0;
                    else 
                        button.ShortcutKeys = keys[0];

                    if (keys == null)
                        continue;

                    foreach (var key in keys)
                    {
                        Guid[] buttons;
                        if (_keyMap.TryGetValue(key, out buttons))
                        {
                            var newButtons = new Guid[buttons.Length + 1];
                            Array.Copy(buttons, newButtons, buttons.Length);
                            newButtons[buttons.Length] = id;
                            _keyMap[key] = newButtons;
                        }
                        else
                        {
                            _keyMap.Add(key, new[] { id });
                        }
                    }
                }
            }

            private Dictionary<Guid, Keys[]> LoadFromRegistry()
            {
                var mappings = new Dictionary<Guid, Keys[]>();
                var keys = new List<Keys>();

                using (var key = OpenRegistryKey(false))
                {
                    if (key != null)
                    {
                        foreach (string name in key.GetValueNames())
                        {
                            Guid guid;
                            if (Guid.TryParse(name, out guid))
                            {
                                keys.Clear();

                                string value = (string)key.GetValue(name);

                                if (!String.IsNullOrEmpty(value))
                                {
                                    foreach (string item in value.Split('|'))
                                    {
                                        var itemKeys = ShortcutKeysUtil.Parse(item);

                                        foreach (var itemKey in itemKeys)
                                        {
                                            if (!ShortcutKeysUtil.IsValid(itemKey))
                                                Log.WarnFormat("Skipping illegal shortcut key '{0}' for button '{1}'", itemKey, guid);
                                            else
                                                keys.Add(itemKey);
                                        }
                                    }
                                }

                                mappings.Add(guid, keys.ToArray());
                            }
                        }
                    }
                }

                return mappings;
            }

            private string CreateKeysValue(Keys[] keys)
            {
                var sb = new StringBuilder();

                if (keys != null)
                {
                    foreach (var key in keys)
                    {
                        if (sb.Length > 0)
                            sb.Append('|');

                        sb.Append(ShortcutKeysUtil.ToString(key));
                    }
                }

                return sb.ToString();
            }

            private RegistryKey OpenRegistryKey(bool writable)
            {
                if (writable)
                    return Registry.CurrentUser.CreateSubKey(_registryKey);
                else
                    return Registry.CurrentUser.OpenSubKey(_registryKey);
            }

            public bool ProcessMessage(ref Message m)
            {
                var key = (Keys)(int)m.WParam | Control.ModifierKeys;

                Guid[] buttons;
                if (!_keyMap.TryGetValue(key, out buttons))
                    return false;

                foreach (var id in buttons)
                {
                    NiCommandStatus status;
                    ErrorUtil.ThrowOnFailure(_commandManager.QueryStatus(id, out status));

                    bool enabled = (status & NiCommandStatus.Enabled) != 0;
                    bool visible = (status & NiCommandStatus.Invisible) == 0;

                    if (enabled && visible)
                    {
                        object result;
                        if (ErrorUtil.ThrowOnFailure(_commandManager.Exec(id, null, out result)))
                            return true;
                    }
                }

                return false;
            }

            private class NiKeyboardMappings : ServiceObject, INiKeyboardMappings
            {
                private static readonly Keys[] EmptyKeys = new Keys[0];
                private static readonly INiCommandBarButton[] EmptyButtons = new INiCommandBarButton[0];

                private readonly KeyboardMappingManager _keyboardMappingManager;
                private readonly Dictionary<Guid, List<Keys>> _mappingsByButton = new Dictionary<Guid, List<Keys>>();
                private readonly Dictionary<Keys, List<Guid>> _mappingsByKeys = new Dictionary<Keys, List<Guid>>();

                public Dictionary<Guid, Keys[]> Mappings { get; private set; }

                public NiKeyboardMappings(NiCommandManager commandManager)
                {
                    _keyboardMappingManager = commandManager._keyboardMappingManager;

                    Mappings = new Dictionary<Guid, Keys[]>(
                        _keyboardMappingManager.LoadFromRegistry()
                    );

                    // Seed the mappings with mappings from the command manager.

                    foreach (var item in _keyboardMappingManager._initialKeys)
                    {
                        var keys = new List<Keys>();

                        Keys[] loadedKeys;
                        if (Mappings.TryGetValue(item.Key, out loadedKeys))
                            keys.AddRange(loadedKeys);
                        else if (item.Value != 0)
                            keys.Add(item.Value);

                        _mappingsByButton.Add(item.Key, keys);

                        foreach (var key in keys)
                        {
                            List<Guid> buttons;
                            if (!_mappingsByKeys.TryGetValue(key, out buttons))
                            {
                                buttons = new List<Guid>();
                                _mappingsByKeys.Add(key, buttons);
                            }

                            buttons.Add(item.Key);
                        }
                    }
                }

                public HResult GetAllButtons(out INiCommandBarButton[] buttons)
                {
                    buttons = null;

                    try
                    {
                        buttons = _keyboardMappingManager._commandManager._objects.Values.OfType<INiCommandBarButton>().ToArray();

                        return HResult.OK;
                    }
                    catch (Exception ex)
                    {
                        return ErrorUtil.GetHResult(ex);
                    }
                }

                public HResult GetKeys(INiCommandBarButton button, out Keys[] keys)
                {
                    keys = null;

                    try
                    {
                        if (button == null)
                            throw new ArgumentNullException("button");

                        List<Keys> mappedKeys;
                        if (_mappingsByButton.TryGetValue(button.Id, out mappedKeys))
                            keys = mappedKeys.ToArray();
                        else
                            keys = EmptyKeys;

                        return HResult.OK;
                    }
                    catch (Exception ex)
                    {
                        return ErrorUtil.GetHResult(ex);
                    }
                }

                public HResult GetButtons(Keys keys, out INiCommandBarButton[] buttons)
                {
                    buttons = null;

                    try
                    {
                        List<Guid> mappedButtons;
                        if (_mappingsByKeys.TryGetValue(keys, out mappedButtons))
                        {
                            buttons = mappedButtons.Select(
                                p => (INiCommandBarButton)_keyboardMappingManager._commandManager._objects[p]
                            ).ToArray();
                        }
                        else
                        {
                            buttons = EmptyButtons;
                        }

                        return HResult.OK;
                    }
                    catch (Exception ex)
                    {
                        return ErrorUtil.GetHResult(ex);
                    }
                }

                public HResult SetKeys(INiCommandBarButton button, Keys[] keys)
                {
                    try
                    {
                        if (button == null)
                            throw new ArgumentNullException("button");
                        if (keys == null)
                            throw new ArgumentNullException("keys");

                        foreach (var key in keys)
                        {
                            if (!ShortcutKeysUtil.IsValid(key))
                                throw new ArgumentException(String.Format("{0} is not a valid shortcut", key));
                        }

                        var id = button.Id;

                        List<Keys> currentKeys;
                        if (!_mappingsByButton.TryGetValue(id, out currentKeys))
                        {
                            currentKeys = new List<Keys>();
                            _mappingsByButton.Add(id, currentKeys);
                        }

                        // Remove the button from keys that are not set anymore.

                        foreach (var key in currentKeys)
                        {
                            if (!keys.Contains(key))
                            {
                                List<Guid> buttons;
                                if (_mappingsByKeys.TryGetValue(key, out buttons))
                                {
                                    bool removed = buttons.Remove(id);
                                    Debug.Assert(removed);
                                }
                                else
                                {
                                    Debug.Fail("Expected key to appear in mapping");
                                }
                            }
                        }

                        // Add the button to new keys.

                        foreach (var key in keys)
                        {
                            if (!currentKeys.Contains(key))
                            {
                                List<Guid> buttons;
                                if (!_mappingsByKeys.TryGetValue(key, out buttons))
                                {
                                    buttons = new List<Guid>();
                                    _mappingsByKeys.Add(key, buttons);
                                }

                                buttons.Add(id);
                            }
                        }

                        // Save the new key bindings.

                        currentKeys.Clear();
                        currentKeys.AddRange(keys);

                        // Update the new mappings with the new keys.

                        if (AreSameKeys(_keyboardMappingManager._initialKeys[id], keys))
                            Mappings.Remove(id);
                        else
                            Mappings[id] = keys.ToArray();

                        return HResult.OK;
                    }
                    catch (Exception ex)
                    {
                        return ErrorUtil.GetHResult(ex);
                    }
                }

                private bool AreSameKeys(Keys buttonKeys, Keys[] keys)
                {
                    if (buttonKeys == 0)
                        return keys.Length == 0;

                    return keys.Length == 1 && keys[0] == buttonKeys;
                }
            }
        }
    }
}

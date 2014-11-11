using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util;

namespace NetIde.Core.OptionPages.Environment
{
    internal partial class KeyboardOptionControl : OptionPageControl
    {
        private INiCommandManager _commandManager;
        private INiKeyboardMappings _mappings;
        private List<Registration> _buttons;

        public KeyboardOptionControl()
        {
            InitializeComponent();

            UpdateEnabled();
        }

        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;

                _commandManager = (INiCommandManager)GetService(typeof(INiCommandManager));
            }
        }

        private void FontsControl_Load(object sender, EventArgs e)
        {
            ErrorUtil.ThrowOnFailure(_commandManager.LoadKeyboardMappings(out _mappings));

            INiCommandBarButton[] buttons;
            ErrorUtil.ThrowOnFailure(_mappings.GetAllButtons(out buttons));

            _buttons = buttons.Select(p => new Registration(p)).OrderBy(p => p.Code).ToList();

            ReloadList();
        }

        private void _filter_TextChanged(object sender, EventArgs e)
        {
            ReloadList();
        }

        private void ReloadList()
        {
            string filter = _filter.Text.Trim();

            _commands.BeginUpdate();
            _commands.Items.Clear();

            foreach (var button in _buttons)
            {
                if (filter.Length == 0 || button.Code.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) != -1)
                    _commands.Items.Add(button);
            }

            if (_commands.Items.Count > 0)
                _commands.SelectedIndex = 0;

            _commands.EndUpdate();
        }

        private void _commands_SelectedIndexChanged(object sender, EventArgs e)
        {
            var button = (Registration)_commands.SelectedItem;

            Debug.Assert(button != null);

            Keys[] keys;
            ErrorUtil.ThrowOnFailure(_mappings.GetKeys(button.Button, out keys));

            _assignedShortcuts.BeginUpdate();
            _assignedShortcuts.Items.Clear();

            foreach (var key in keys)
            {
                _assignedShortcuts.Items.Add(new Shortcut(key));
            }

            if (_assignedShortcuts.Items.Count > 0)
                _assignedShortcuts.SelectedIndex = 0;

            _assignedShortcuts.EndUpdate();

            UpdateEnabled();
        }

        private void FontsControl_Apply(object sender, EventArgs e)
        {
            ErrorUtil.ThrowOnFailure(_commandManager.SaveKeyboardMappings(_mappings));
        }

        private static string GetButtonCode(INiCommandBarButton button)
        {
            return button.Code.Replace('_', '.');
        }

        private void _assignedShortcuts_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            var haveAssignedShortcut = _assignedShortcuts.SelectedItem != null;

            _assignedShortcuts.Enabled = haveAssignedShortcut;
            _remove.Enabled = haveAssignedShortcut;
            _shortcutsConflicts.Enabled = _shortcutsConflicts.Items.Count > 0;
            _assign.Enabled = _shortcut.Keys != 0;
        }

        private void _remove_Click(object sender, EventArgs e)
        {
            _assignedShortcuts.Items.Remove(_assignedShortcuts.SelectedItem);

            FlushAssignedShortcuts();

            UpdateEnabled();
        }

        private void FlushAssignedShortcuts()
        {
            var button = (Registration)_commands.SelectedItem;
            var keys = _assignedShortcuts.Items.Cast<Shortcut>().Select(p => p.Keys).ToArray();

            ErrorUtil.ThrowOnFailure(_mappings.SetKeys(button.Button, keys));
        }

        private void _shortcut_KeysChanged(object sender, EventArgs e)
        {
            INiCommandBarButton[] buttons;
            ErrorUtil.ThrowOnFailure(_mappings.GetButtons(_shortcut.Keys, out buttons));

            _shortcutsConflicts.BeginUpdate();
            _shortcutsConflicts.Items.Clear();

            foreach (var button in buttons)
            {
                _shortcutsConflicts.Items.Add(String.Format(
                    "{0} ({1})",
                    GetButtonCode(button),
                    ShortcutKeysUtil.ToDisplayString(_shortcut.Keys)
                ));
            }

            if (_shortcutsConflicts.Items.Count > 0)
                _shortcutsConflicts.SelectedIndex = 0;

            _shortcutsConflicts.EndUpdate();

            UpdateEnabled();
        }

        private void _assign_Click(object sender, EventArgs e)
        {
            var shortcut = new Shortcut(_shortcut.Keys);

            if (!_assignedShortcuts.Items.Contains(shortcut))
            {
                _assignedShortcuts.Items.Add(shortcut);
                _assignedShortcuts.SelectedItem = shortcut;

                FlushAssignedShortcuts();
            }

            _shortcut.Keys = 0;

            UpdateEnabled();
        }

        private class Registration
        {
            public INiCommandBarButton Button { get; private set; }
            public string Code { get; private set; }

            public Registration(INiCommandBarButton button)
            {
                Button = button;
                Code = GetButtonCode(button);
            }

            public override string ToString()
            {
                return Code;
            }
        }

        private class Shortcut
        {
            public Keys Keys { get; private set; }

            public Shortcut(Keys keys)
            {
                Keys = keys;
            }

            public override string ToString()
            {
                return ShortcutKeysUtil.ToDisplayString(Keys);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(this, obj))
                    return true;

                var other = obj as Shortcut;

                return other != null && Keys == other.Keys;
            }

            public override int GetHashCode()
            {
                return Keys.GetHashCode();
            }
        }
    }
}

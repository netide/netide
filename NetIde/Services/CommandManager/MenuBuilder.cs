using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Util;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Xml.Resources;
using Button = NetIde.Xml.Resources.Button;
using ComboBox = NetIde.Xml.Resources.ComboBox;
using Menu = NetIde.Xml.Resources.Menu;
using Serialization = NetIde.Xml.Serialization;

namespace NetIde.Services.CommandManager
{
    internal class MenuBuilder
    {
        private static readonly Dictionary<Type, Func<MenuBuilder, UiObjectRef, object>> _builders = new Dictionary<Type, Func<MenuBuilder, UiObjectRef, object>>
        {
            { typeof(Menu), (s, o) => s.OnMenu((Menu)o) },
            { typeof(MenuRef), (s, o) => s.OnMenuRef((MenuRef)o) },
            { typeof(Group), (s, o) => s.OnGroup((Group)o) },
            { typeof(GroupRef), (s, o) => s.OnGroupRef((GroupRef)o) },
            { typeof(Button), (s, o) => s.OnButton((Button)o) },
            { typeof(ButtonRef), (s, o) => s.OnButtonRef((ButtonRef)o) },
            { typeof(ComboBox), (s, o) => s.OnComboBox((ComboBox)o) },
            { typeof(ComboBoxRef), (s, o) => s.OnComboBoxRef((ComboBoxRef)o) },
        };

        private readonly NiCommandManager _commandManager;
        private readonly INiPackage _package;
        private readonly string _niMenu;
        private readonly Dictionary<string, object> _resources;
        private readonly INiMenuManager _menuManager;

        public MenuBuilder(INiPackage package, string niMenu, Dictionary<string, object> resources)
        {
            if (package == null)
                throw new ArgumentNullException("package");
            if (niMenu == null)
                throw new ArgumentNullException("niMenu");
            if (resources == null)
                throw new ArgumentNullException("resources");

            _commandManager = (NiCommandManager)package.GetService(typeof(INiCommandManager));
            _menuManager = (INiMenuManager)package.GetService(typeof(INiMenuManager));
            _package = package;
            _niMenu = niMenu;
            _resources = resources;
        }

        public void Build()
        {
            var resource = Serialization.DeserializeXml<Resources>(_niMenu);

            foreach (var item in resource.Ui.Items)
            {
                var command = _builders[item.GetType()](this, item);

                if (
                    item is Menu &&
                    ((Menu)item).Kind != MenuKind.Popup
                )
                    _menuManager.RegisterCommandBar((NiCommandBar)command);
            }
        }

        private IEnumerable Build(IEnumerable<UiObjectRef> collection)
        {
            return collection.Select(p => _builders[p.GetType()](this, p));
        }

        private object OnMenu(Menu menu)
        {
            if (menu.Kind == MenuKind.Popup)
            {
                INiCommandBarPopup popup;
                ErrorUtil.ThrowOnFailure(_commandManager.CreateCommandBarPopup(
                    menu.Guid != Guid.Empty ? menu.Guid : Guid.NewGuid(),
                    menu.Priority,
                    out popup
                ));

                popup.Text = _package.ResolveStringResource(menu.Text);
                popup.DisplayStyle = Enum<NiCommandDisplayStyle>.Parse(menu.Style.ToString());
                ((NiCommandBarPopup)popup).Bitmap = ResolveBitmapResource(menu.Image);

                foreach (INiCommandBarGroup group in Build(menu.Items))
                {
                    ErrorUtil.ThrowOnFailure(popup.Controls.Add(group));
                }

                return popup;
            }
            else
            {
                INiCommandBar commandBar;
                ErrorUtil.ThrowOnFailure(_commandManager.CreateCommandBar(
                    menu.Guid != Guid.Empty ? menu.Guid : Guid.NewGuid(),
                    Enum<NiCommandBarKind>.Parse(menu.Kind.ToString()),
                    menu.Priority,
                    out commandBar
                ));

                commandBar.Text = _package.ResolveStringResource(menu.Text);
                commandBar.DisplayStyle = Enum<NiCommandDisplayStyle>.Parse(menu.Style.ToString());
                ((NiCommandBar)commandBar).Bitmap = ResolveBitmapResource(menu.Image);

                foreach (INiCommandBarGroup group in Build(menu.Items))
                {
                    ErrorUtil.ThrowOnFailure(commandBar.Controls.Add(group));
                }

                return commandBar;
            }
        }

        private object OnMenuRef(MenuRef menuRef)
        {
            INiCommandBar commandBar;
            ErrorUtil.ThrowOnFailure(_commandManager.FindCommandBar(
                menuRef.Guid,
                out commandBar
            ));

            if (commandBar == null)
                throw new NetIdeException(String.Format(Labels.CannotFindCommandBar, menuRef.Guid));

            foreach (INiCommandBarGroup group in Build(menuRef.Items))
            {
                ErrorUtil.ThrowOnFailure(commandBar.Controls.Add(group));
            }

            return commandBar;
        }

        private object OnGroup(Group group)
        {
            INiCommandBarGroup commandGroup;
            ErrorUtil.ThrowOnFailure(_commandManager.CreateCommandBarGroup(
                group.Guid != Guid.Empty ? group.Guid : Guid.NewGuid(),
                group.Priority,
                out commandGroup
            ));

            foreach (INiCommandBarControl control in Build(group.Items))
            {
                ErrorUtil.ThrowOnFailure(commandGroup.Controls.Add(control));
            }

            return commandGroup;
        }

        private object OnGroupRef(GroupRef groupRef)
        {
            INiCommandBarGroup commandGroup;
            ErrorUtil.ThrowOnFailure(_commandManager.FindCommandBarGroup(
                groupRef.Guid,
                out commandGroup
            ));

            if (commandGroup == null)
                throw new NetIdeException(String.Format(Labels.CannotFindCommandGroup, groupRef.Guid));

            foreach (INiCommandBarControl control in Build(groupRef.Items))
            {
                ErrorUtil.ThrowOnFailure(commandGroup.Controls.Add(control));
            }

            return commandGroup;
        }

        private object OnButton(Button button)
        {
            INiCommandBarButton command;
            ErrorUtil.ThrowOnFailure(_commandManager.CreateCommandBarButton(
                button.Guid != Guid.Empty ? button.Guid : Guid.NewGuid(),
                button.Priority,
                out command
            ));

            command.Text = _package.ResolveStringResource(button.Text);
            command.ToolTip = _package.ResolveStringResource(button.ToolTip);
            command.DisplayStyle = Enum<NiCommandDisplayStyle>.Parse(button.Style.ToString());
            command.ShortcutKeys = ParseShortcutKeys(button.Key);
            ((NiCommandBarButton)command).Bitmap = ResolveBitmapResource(button.Image);

            return command;
        }

        private object OnButtonRef(ButtonRef buttonRef)
        {
            INiCommandBarControl control;
            ErrorUtil.ThrowOnFailure(_commandManager.FindCommandBarControl(
                buttonRef.Guid,
                out control
            ));

            if (control == null)
                throw new NetIdeException(String.Format(Labels.CannotFindCommand, buttonRef.Guid));

            return control;
        }

        private object OnComboBox(ComboBox comboBox)
        {
            INiCommandBarComboBox command;
            ErrorUtil.ThrowOnFailure(_commandManager.CreateCommandBarComboBox(
                comboBox.Guid != Guid.Empty ? comboBox.Guid : Guid.NewGuid(),
                comboBox.FillCommandGuid,
                comboBox.Priority,
                out command
            ));

            command.Text = _package.ResolveStringResource(comboBox.Text);
            command.ToolTip = _package.ResolveStringResource(comboBox.ToolTip);
            command.Style = Enum<NiCommandComboBoxStyle>.Parse(comboBox.Style.ToString());

            return command;
        }

        private object OnComboBoxRef(ComboBoxRef comboBoxRef)
        {
            INiCommandBarControl control;
            ErrorUtil.ThrowOnFailure(_commandManager.FindCommandBarControl(
                comboBoxRef.Guid,
                out control
            ));

            if (control == null)
                throw new NetIdeException(String.Format(Labels.CannotFindCommand, comboBoxRef.Guid));

            return control;
        }

        private Image ResolveBitmapResource(string key)
        {
            if (key == null)
                return null;

            if (key.StartsWith("@"))
            {
                object resource;
                if (
                    _resources.TryGetValue(key.Substring(1), out resource) &&
                    resource is Image
                )
                    return (Image)resource;
            }

            throw new NetIdeException(String.Format(Labels.InvalidBitmapResource, key));
        }

        private Keys ParseShortcutKeys(string keys)
        {
            if (String.IsNullOrEmpty(keys))
                return Keys.None;

            Keys result = 0;

            foreach (string part in keys.Split('+'))
            {
                Keys partKey;

                if (!Enum<Keys>.TryParse(part, out partKey, true))
                    throw new NetIdeException(String.Format(Labels.CouldNotParseKeys, keys));

                result |= partKey;
            }

            return result;
        }
    }
}

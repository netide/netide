using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Util;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.CommandManager.Controls
{
    internal class ComboBoxControl : ControlControl<ToolStripComboBox>
    {
        private static readonly string[] EmptyValues = new string[0];

        private bool _suppressPublish;
        private readonly IServiceProvider _serviceProvider;
        private string _lastPublished;

        public new NiCommandBarComboBox NiCommand
        {
            get { return (NiCommandBarComboBox)base.NiCommand; }
        }

        public ComboBoxControl(IServiceProvider serviceProvider, NiCommandBarControl control)
            : base(control, ToolStripItemDisplayStyle.None)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            _serviceProvider = serviceProvider;

            Item.DropDown += (s, e) => RequestValues();
            Item.DropDownClosed += (s, e) => Publish();
            Item.Leave += (s, e) => Publish();
            Item.SelectedIndexChanged += (s, e) => Publish();
        }

        private void Publish()
        {
            if (_suppressPublish || Item.DroppedDown)
                return;

            // Don't publish if we got a selected index from our fake list.

            if (
                NiCommand.Style == NiCommandComboBoxStyle.DropDownList &&
                Array.IndexOf(NiCommand.Values, Item.Text) == -1
            )
                return;

            _suppressPublish = true;

            try
            {
                NiCommand.SelectedValue = Item.Text;

                if (NiCommand.SelectedValue != _lastPublished)
                {
                    _lastPublished = NiCommand.SelectedValue;

                    var commandManager = (INiCommandManager)_serviceProvider.GetService(typeof(INiCommandManager));

                    object unused;
                    ErrorUtil.ThrowOnFailure(commandManager.Exec(NiCommand.Id, Item.Text, out unused));
                }
            }
            finally
            {
                _suppressPublish = false;
            }
        }

        private void RequestValues()
        {
            UpdateValues();

            if (CollectionEquals(NiCommand.Values, Item.Items))
                return;

            _suppressPublish = true;
            Item.BeginUpdate();

            try
            {
                Item.Items.Clear();
                Item.Items.AddRange(NiCommand.Values);
            }
            finally
            {
                Item.EndUpdate();
                _suppressPublish = false;
            }
        }

        private void UpdateValues()
        {
            var commandManager = (INiCommandManager)_serviceProvider.GetService(typeof(INiCommandManager));

            object result;
            ErrorUtil.ThrowOnFailure(commandManager.Exec(NiCommand.FillCommand, null, out result));

            var values = result as ICollection<string>;

            if (values != null)
                NiCommand.Values = values.ToArray();
            else
                NiCommand.Values = EmptyValues;
        }

        protected override void UpdateItem()
        {
            base.UpdateItem();

            var style = Enum<ComboBoxStyle>.Parse(NiCommand.Style.ToString());

            if (Item.DropDownStyle != style)
                Item.DropDownStyle = style;

            string text = NiCommand.SelectedValue ?? String.Empty;

            if (Item.Text != text)
            {
                _suppressPublish = true;
                bool beganUpdate = false;

                try
                {
                    UpdateValues();

                    int index = Array.IndexOf(NiCommand.Values, text);

                    if (NiCommand.Style == NiCommandComboBoxStyle.DropDownList)
                    {
                        // We specifically support the text of the combo box to not be in
                        // the list. However, when the style is set to DropDownList, the
                        // combo box does not support this. We solve this by simply not
                        // having the items in the combo box. When the combo box is opened,
                        // we populate the items just in time (making the selected item
                        // disappear when it wasn't in the list) and fix-up on the
                        // re-query.

                        if (index == -1)
                        {
                            beganUpdate = true;

                            Item.BeginUpdate();

                            // If the item doesn't appear in the list, create a
                            // fake list with only the single item. This will disable
                            // functionality like changing the selected item using
                            // arrows, but is the only way to solve this.

                            Item.Items.Clear();
                            Item.Items.Add(text);

                            index = 0;
                        }
                        else if (!CollectionEquals(NiCommand.Values, Item.Items))
                        {
                            beganUpdate = true;

                            Item.BeginUpdate();

                            // When the item does appear in the list, we can use
                            // the actual list.

                            Item.Items.Clear();
                            Item.Items.AddRange(NiCommand.Values);
                        }

                        Item.SelectedIndex = index;
                    }
                    else
                    {
                        if (index == -1)
                            Item.SelectedIndex = index;
                        else
                            Item.Text = text;
                    }
                }
                finally
                {
                    if (beganUpdate)
                        Item.EndUpdate();

                    _suppressPublish = false;
                }
            }
        }

        private bool CollectionEquals(IList a, IList b)
        {
            if (a.Count != b.Count)
                return false;

            for (int i = 0; i < a.Count; i++)
            {
                if (!Equals(a[i], b[i]))
                    return false;
            }

            return true;
        }
    }
}

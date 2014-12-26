using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    internal class NiTaskDialogCommonButtonsEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override bool IsDropDownResizable
        {
            get { return false; }
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service == null)
                return value;

            var enumValue = (value as NiTaskDialogCommonButtons?).GetValueOrDefault();
            var container = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            foreach (var button in new[]
            {
                NiTaskDialogCommonButtons.OK,
                NiTaskDialogCommonButtons.Yes,
                NiTaskDialogCommonButtons.No,
                NiTaskDialogCommonButtons.Cancel,
                NiTaskDialogCommonButtons.Retry,
                NiTaskDialogCommonButtons.Close
            })
            {
                var checkBox = new CheckBox
                {
                    AutoSize = true,
                    FlatStyle = FlatStyle.System,
                    Text = button.ToString(),
                    UseVisualStyleBackColor = true,
                    Tag = button,
                    Margin = new Padding(3, 0, 3, 0),
                    Checked = enumValue.HasFlag(button)
                };

                checkBox.CheckedChanged += (s, e) =>
                {
                    enumValue = 0;

                    foreach (CheckBox item in container.Controls)
                    {
                        if (item.Checked)
                            enumValue |= (NiTaskDialogCommonButtons)item.Tag;
                    }
                };

                checkBox.SizeChanged += (s, e) => FixHeight(container);

                container.Controls.Add(checkBox);
            }

            FixHeight(container);

            service.DropDownControl(container);

            return enumValue;
        }

        private static void FixHeight(FlowLayoutPanel container)
        {
            container.Height =
                (container.Height - container.ClientSize.Height) +
                container.Controls[container.Controls.Count - 1].Bottom;
        }
    }
}

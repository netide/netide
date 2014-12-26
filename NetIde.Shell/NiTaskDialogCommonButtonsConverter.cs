using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    internal class NiTaskDialogCommonButtonsConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string))
                return base.ConvertTo(context, culture, value, destinationType);

            if (value == null)
                return "None";

            var enumValue = (value as NiTaskDialogCommonButtons?).GetValueOrDefault();
            var sb = new StringBuilder();

            Append(sb, enumValue, NiTaskDialogCommonButtons.OK);
            Append(sb, enumValue, NiTaskDialogCommonButtons.Yes);
            Append(sb, enumValue, NiTaskDialogCommonButtons.No);
            Append(sb, enumValue, NiTaskDialogCommonButtons.Cancel);
            Append(sb, enumValue, NiTaskDialogCommonButtons.Retry);
            Append(sb, enumValue, NiTaskDialogCommonButtons.Close);

            return sb.ToString();
        }

        private void Append(StringBuilder sb, NiTaskDialogCommonButtons value, NiTaskDialogCommonButtons button)
        {
            if (value.HasFlag(button))
            {
                if (sb.Length > 0)
                    sb.Append(", ");

                sb.Append(button);
            }
        }
    }
}

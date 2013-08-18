using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public static class NiSettingsExtensions
    {
        public static string GetString(this INiSettings self, string key)
        {
            string value;
            ErrorUtil.ThrowOnFailure(self.GetSetting(key, out value));

            return value;
        }

        public static int? GetInt32(this INiSettings self, string key)
        {
            string value = GetString(self, key);

            int result;
            if (
                value == null ||
                !int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result)
            )
                return null;

            return result;
        }

        public static decimal? GetDecimal(this INiSettings self, string key)
        {
            string value = GetString(self, key);

            decimal result;
            if (
                value == null ||
                !decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result)
            )
                return null;

            return result;
        }

        public static Guid? GetGuid(this INiSettings self, string key)
        {
            string value = GetString(self, key);

            Guid result;
            if (
                value == null ||
                !Guid.TryParse(value, out result)
            )
                return null;

            return result;
        }

        public static bool? GetBool(this INiSettings self, string key)
        {
            var value = GetInt32(self, key);

            if (value.HasValue)
                return value.Value != 0;

            return null;
        }

        public static Font GetFont(this INiSettings self, string key)
        {
            return DeserializeFont(self.GetString(key));
        }

        public static Color? GetColor(this INiSettings self, string key)
        {
            return DeserializeColor(self.GetString(key));
        }
        
        public static void SetValue(this INiSettings self, string key, string value)
        {
            ErrorUtil.ThrowOnFailure(self.SetSetting(key, value));
        }

        public static void SetValue(this INiSettings self, string key, int value)
        {
            SetValue(self, key, value.ToString(CultureInfo.InvariantCulture));
        }

        public static void SetValue(this INiSettings self, string key, decimal value)
        {
            SetValue(self, key, value.ToString(CultureInfo.InvariantCulture));
        }

        public static void SetValue(this INiSettings self, string key, Guid value)
        {
            SetValue(self, key, value.ToString("B").ToUpperInvariant());
        }

        public static void SetValue(this INiSettings self, string key, bool value)
        {
            SetValue(self, key, value ? 1 : 0);
        }

        public static void SetValue(this INiSettings self, string key, Font value)
        {
            SetValue(self, key, SerializeFont(value));
        }

        public static void SetValue(this INiSettings self, string key, Color value)
        {
            SetValue(self, key, SerializeColor(value));
        }

        public static void DeleteValue(this INiSettings self, string key)
        {
            SetValue(self, key, (string)null);
        }

        public static bool HasValue(this INiSettings self, string key)
        {
            return GetString(self, key) != null;
        }

        private static string SerializeFont(Font font)
        {
            return
                font.FontFamily.Name + ", " +
                font.Size.ToString(CultureInfo.InvariantCulture) + ", " +
                ((int)font.Style).ToString(CultureInfo.InvariantCulture);
        }

        private static string SerializeColor(Color color)
        {
            return
                color.A.ToString(CultureInfo.InvariantCulture) + ", " +
                color.R.ToString(CultureInfo.InvariantCulture) + ", " +
                color.G.ToString(CultureInfo.InvariantCulture) + ", " +
                color.B.ToString(CultureInfo.InvariantCulture);
        }

        private static Font DeserializeFont(string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;

            // We parse the string instead of using split because
            // the font family may contain a comma.

            int index = value.LastIndexOf(',');
            int fontStyle;

            if (
                index != -1 &&
                int.TryParse(value.Substring(index + 1).Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out fontStyle)
            )
            {
                value = value.Substring(0, index);

                index = value.LastIndexOf(',');

                float fontSize;

                if (
                    index != -1 &&
                    float.TryParse(value.Substring(index + 1).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out fontSize)
                )
                {
                    string fontFamily = value.Substring(0, index).Trim();

                    return new Font(fontFamily, fontSize, (FontStyle)fontStyle);
                }
            }

            return null;
        }

        private static Color? DeserializeColor(string value)
        {
            if (value == null)
                return null;

            string[] parts = value.Split(',');

            int a;
            int r;
            int g;
            int b;

            if (
                parts.Length == 4 &&
                int.TryParse(parts[0].Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out a) &&
                int.TryParse(parts[1].Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out r) &&
                int.TryParse(parts[2].Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out g) &&
                int.TryParse(parts[3].Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out b)
            )
                return Color.FromArgb(a, r, g, b);

            return null;
        }
    }
}

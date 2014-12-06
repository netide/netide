using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Util
{
    public static class ShortcutKeysUtil
    {
        private const string KeysDelete = "Delete";
        private const string KeysEnd = "End";
        private const string KeysInsert = "Insert";
        private const string KeysHome = "Home";
        private const string KeysBackslash = "Backslash";
        private const string KeysPause = "Pause";
        private const string KeysSpace = "Space";
        private const string KeysEnter = "Enter";
        private const string KeysTab = "Tab";
        private const string KeysPeriod = "Period";
        private const string KeysTilde = "Tilde";
        private const string KeysComma = "Comma";
        private const string KeysQuestion = "Question";
        private const string KeysPlus = "Plus";
        private const string KeysMinus = "Minus";

        public static Keys[] Parse(string keys)
        {
            if (String.IsNullOrEmpty(keys))
                return ArrayUtil.GetEmptyArray<Keys>();

            string[] keysPart = keys.Split(',');
            var result = new Keys[keysPart.Length];

            for (int i = 0; i < keysPart.Length; i++)
            {
                result[i] = ParseKeys(keysPart[i]);
            }

            return result;
        }

        private static Keys ParseKeys(string keys)
        {
            Keys result = 0;

            foreach (string part in keys.Split('+'))
            {
                switch (part)
                {
                    case "Control":
                    case "Alt":
                    case "Shift":
                    case "F1":
                    case "F2":
                    case "F3":
                    case "F4":
                    case "F5":
                    case "F6":
                    case "F7":
                    case "F8":
                    case "F9":
                    case "F10":
                    case "F11":
                    case "F12":
                    case "F13":
                    case "F14":
                    case "F15":
                    case "F16":
                    case "F17":
                    case "F18":
                    case "F19":
                    case "F20":
                    case "F21":
                    case "F22":
                    case "F23":
                    case "F24":
                    case "A":
                    case "B":
                    case "C":
                    case "D":
                    case "E":
                    case "F":
                    case "G":
                    case "H":
                    case "I":
                    case "J":
                    case "K":
                    case "L":
                    case "M":
                    case "N":
                    case "O":
                    case "P":
                    case "Q":
                    case "R":
                    case "S":
                    case "T":
                    case "U":
                    case "V":
                    case "W":
                    case "X":
                    case "Y":
                    case "Z":
                        result |= Enum<Keys>.Parse(part);
                        break;

                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        result |= Enum<Keys>.Parse("D" + part);
                        break;

                    case KeysDelete:
                        result |= Keys.Delete;
                        break;

                    case KeysEnd:
                        result |= Keys.End;
                        break;

                    case KeysInsert:
                        result |= Keys.Insert;
                        break;

                    case KeysHome:
                        result |= Keys.Home;
                        break;

                    case KeysBackslash:
                        result |= Keys.OemBackslash;
                        break;

                    case KeysPause:
                        result |= Keys.Pause;
                        break;

                    case KeysSpace:
                        result |= Keys.Space;
                        break;

                    case KeysEnter:
                        result |= Keys.Enter;
                        break;

                    case KeysTab:
                        result |= Keys.Tab;
                        break;

                    case KeysPeriod:
                        result |= Keys.OemPeriod;
                        break;

                    case KeysTilde:
                        result |= Keys.Oemtilde;
                        break;

                    case KeysComma:
                        result |= Keys.Oemcomma;
                        break;

                    case KeysQuestion:
                        result |= Keys.OemQuestion;
                        break;

                    case KeysPlus:
                        result |= Keys.Oemplus;
                        break;

                    case KeysMinus:
                        result |= Keys.OemMinus;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("keys");
                }
            }

            return result;
        }

        public static string ToString(Keys keys)
        {
            var sb = new StringBuilder();

            if ((keys & Keys.Control) != 0)
            {
                keys &= ~Keys.Control;
                sb.Append("Control");
            }
            if ((keys & Keys.Alt) != 0)
            {
                keys &= ~Keys.Alt;
                if (sb.Length > 0)
                    sb.Append("+");
                sb.Append("Alt");
            }
            Keys shiftKey = Keys.Shift;
            if ((keys & shiftKey) != 0)
            {
                keys &= ~shiftKey;
                if (sb.Length > 0)
                    sb.Append("+");
                sb.Append("Shift");
            }

            Debug.Assert((keys & Keys.Modifiers) == 0);

            switch (keys & Keys.KeyCode)
            {
                case Keys.F1:
                case Keys.F2:
                case Keys.F3:
                case Keys.F4:
                case Keys.F5:
                case Keys.F6:
                case Keys.F7:
                case Keys.F8:
                case Keys.F9:
                case Keys.F10:
                case Keys.F11:
                case Keys.F12:
                case Keys.F13:
                case Keys.F14:
                case Keys.F15:
                case Keys.F16:
                case Keys.F17:
                case Keys.F18:
                case Keys.F19:
                case Keys.F20:
                case Keys.F21:
                case Keys.F22:
                case Keys.F23:
                case Keys.F24:
                case Keys.A:
                case Keys.B:
                case Keys.C:
                case Keys.D:
                case Keys.E:
                case Keys.F:
                case Keys.G:
                case Keys.H:
                case Keys.I:
                case Keys.J:
                case Keys.K:
                case Keys.L:
                case Keys.M:
                case Keys.N:
                case Keys.O:
                case Keys.P:
                case Keys.Q:
                case Keys.R:
                case Keys.S:
                case Keys.T:
                case Keys.U:
                case Keys.V:
                case Keys.W:
                case Keys.X:
                case Keys.Y:
                case Keys.Z:
                    sb.Append(keys);
                    break;

                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    sb.Append(keys.ToString().Substring(1));
                    break;

                case Keys.Delete:
                    sb.Append(KeysDelete);
                    break;

                case Keys.End:
                    sb.Append(KeysEnd);
                    break;

                case Keys.Insert:
                    sb.Append(KeysInsert);
                    break;

                case Keys.Home:
                    sb.Append(KeysHome);
                    break;

                case Keys.OemBackslash:
                    sb.Append(KeysBackslash);
                    break;

                case Keys.Pause:
                    sb.Append(KeysPause);
                    break;

                case Keys.Space:
                    sb.Append(KeysSpace);
                    break;

                case Keys.Enter:
                    sb.Append(KeysEnter);
                    break;

                case Keys.Tab:
                    sb.Append(KeysTab);
                    break;

                case Keys.OemPeriod:
                    sb.Append(KeysPeriod);
                    break;

                case Keys.Oemtilde:
                    sb.Append(KeysTilde);
                    break;

                case Keys.Oemcomma:
                    sb.Append(KeysComma);
                    break;

                case Keys.OemQuestion:
                    sb.Append(KeysQuestion);
                    break;

                case Keys.Oemplus:
                    sb.Append(KeysPlus);
                    break;

                case Keys.OemMinus:
                    sb.Append(KeysMinus);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("keys");
            }

            return sb.ToString();
        }

        public static string ToDisplayString(Keys keys)
        {
            if (keys == 0)
                return null;

            var sb = new StringBuilder();

            if ((keys & Keys.Control) != 0)
            {
                keys &= ~Keys.Control;
                sb.Append(Labels.KeysControl).Append(Labels.KeysSeparator);
            }
            if ((keys & Keys.Alt) != 0)
            {
                keys &= ~Keys.Alt;
                sb.Append(Labels.KeysAlt).Append(Labels.KeysSeparator);
            }
            Keys shiftKey = Keys.Shift;
            if ((keys & shiftKey) != 0)
            {
                keys &= ~shiftKey;
                sb.Append(Labels.KeysShift).Append(Labels.KeysSeparator);
            }

            Debug.Assert((keys & Keys.Modifiers) == 0);

            switch (keys & Keys.KeyCode)
            {
                case Keys.F1:
                case Keys.F2:
                case Keys.F3:
                case Keys.F4:
                case Keys.F5:
                case Keys.F6:
                case Keys.F7:
                case Keys.F8:
                case Keys.F9:
                case Keys.F10:
                case Keys.F11:
                case Keys.F12:
                case Keys.F13:
                case Keys.F14:
                case Keys.F15:
                case Keys.F16:
                case Keys.F17:
                case Keys.F18:
                case Keys.F19:
                case Keys.F20:
                case Keys.F21:
                case Keys.F22:
                case Keys.F23:
                case Keys.F24:
                case Keys.A:
                case Keys.B:
                case Keys.C:
                case Keys.D:
                case Keys.E:
                case Keys.F:
                case Keys.G:
                case Keys.H:
                case Keys.I:
                case Keys.J:
                case Keys.K:
                case Keys.L:
                case Keys.M:
                case Keys.N:
                case Keys.O:
                case Keys.P:
                case Keys.Q:
                case Keys.R:
                case Keys.S:
                case Keys.T:
                case Keys.U:
                case Keys.V:
                case Keys.W:
                case Keys.X:
                case Keys.Y:
                case Keys.Z:
                    sb.Append(keys);
                    break;

                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    sb.Append(keys.ToString().Substring(1));
                    break;

                case Keys.Delete:
                    sb.Append(Labels.KeysDelete);
                    break;

                case Keys.End:
                    sb.Append(Labels.KeysEnd);
                    break;

                case Keys.Insert:
                    sb.Append(Labels.KeysInsert);
                    break;

                case Keys.Home:
                    sb.Append(Labels.KeysHome);
                    break;

                case Keys.OemBackslash:
                    sb.Append(Labels.KeysBackslash);
                    break;

                case Keys.Pause:
                    sb.Append(Labels.KeysPause);
                    break;

                case Keys.Space:
                    sb.Append(Labels.KeysSpace);
                    break;

                case Keys.Enter:
                    sb.Append(Labels.KeysEnter);
                    break;

                case Keys.Tab:
                    sb.Append(Labels.KeysTab);
                    break;

                case Keys.OemPeriod:
                    sb.Append(Labels.KeysPeriod);
                    break;

                case Keys.Oemtilde:
                    sb.Append(Labels.KeysTilde);
                    break;

                case Keys.Oemcomma:
                    sb.Append(Labels.KeysComma);
                    break;

                case Keys.OemQuestion:
                    sb.Append(Labels.KeysQuestion);
                    break;

                case Keys.Oemplus:
                    sb.Append(Labels.KeysPlus);
                    break;

                case Keys.OemMinus:
                    sb.Append(Labels.KeysMinus);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("keys");
            }

            return sb.ToString();
        }

        public static bool IsValid(Keys keys)
        {
            switch (keys & Keys.KeyCode)
            {
                case Keys.F1:
                case Keys.F2:
                case Keys.F3:
                case Keys.F4:
                case Keys.F5:
                case Keys.F6:
                case Keys.F7:
                case Keys.F8:
                case Keys.F9:
                case Keys.F10:
                case Keys.F11:
                case Keys.F12:
                case Keys.F13:
                case Keys.F14:
                case Keys.F15:
                case Keys.F16:
                case Keys.F17:
                case Keys.F18:
                case Keys.F19:
                case Keys.F20:
                case Keys.F21:
                case Keys.F22:
                case Keys.F23:
                case Keys.F24:
                    return true;

                case Keys.A:
                case Keys.B:
                case Keys.C:
                case Keys.D:
                case Keys.E:
                case Keys.F:
                case Keys.G:
                case Keys.H:
                case Keys.I:
                case Keys.J:
                case Keys.K:
                case Keys.L:
                case Keys.M:
                case Keys.N:
                case Keys.O:
                case Keys.P:
                case Keys.Q:
                case Keys.R:
                case Keys.S:
                case Keys.T:
                case Keys.U:
                case Keys.V:
                case Keys.W:
                case Keys.X:
                case Keys.Y:
                case Keys.Z:
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                case Keys.Delete:
                case Keys.End:
                case Keys.Insert:
                case Keys.Home:
                case Keys.OemBackslash:
                case Keys.Pause:
                case Keys.Space:
                case Keys.Enter:
                case Keys.Tab:
                case Keys.OemPeriod:
                case Keys.Oemtilde:
                case Keys.Oemcomma:
                case Keys.OemQuestion:
                case Keys.Oemplus:
                case Keys.OemMinus:
                    return (keys & (Keys.Control | Keys.Alt)) != 0;

                default:
                    return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Shell
{
    internal static class ControlStubs
    {
        public const int STATE2_INPUTKEY = 0x00000080;
        public const int STATE2_INPUTCHAR = 0x00000100;

        private delegate void ControlOnPreviewKeyDownDelegate(Control target, PreviewKeyDownEventArgs e);
        private delegate bool ControlGetState2Delegate(Control target, int flag);
        private delegate void ControlSetState2Delegate(Control target, int flag, bool state);
        private delegate void ControlSelectDelegate(Control target, bool directed, bool forward);

        private static readonly ControlOnPreviewKeyDownDelegate _controlOnPreviewKeyDownDelegate = CreateControlOnPreviewKeyDownDelegate();
        private static readonly ControlGetState2Delegate _controlGetState2 = CreateControlGetState2();
        private static readonly ControlSetState2Delegate _controlSetState2 = CreateControlSetState2();
        private static readonly ControlSelectDelegate _controlSelect = CreateControlSelect();

        private static ControlOnPreviewKeyDownDelegate CreateControlOnPreviewKeyDownDelegate()
        {
            var method = new DynamicMethod(
                "ControlOnPreviewKeyDown",
                typeof(void),
                new[] { typeof(Control), typeof(PreviewKeyDownEventArgs) },
                typeof(ControlStubs).Module,
                true
            );

            var il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, typeof(Control).GetMethod(
                "OnPreviewKeyDown",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null,
                new[] { typeof(PreviewKeyDownEventArgs) },
                null
            ));
            il.Emit(OpCodes.Ret);

            return (ControlOnPreviewKeyDownDelegate)method.CreateDelegate(typeof(ControlOnPreviewKeyDownDelegate));
        }

        private static ControlGetState2Delegate CreateControlGetState2()
        {
            var method = new DynamicMethod(
                "ControlGetState2",
                typeof(bool),
                new[] { typeof(Control), typeof(int) },
                typeof(ControlStubs).Module,
                true
            );

            var il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, typeof(Control).GetMethod(
                "GetState2",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null,
                new[] { typeof(int) },
                null
            ));
            il.Emit(OpCodes.Ret);

            return (ControlGetState2Delegate)method.CreateDelegate(typeof(ControlGetState2Delegate));
        }

        private static ControlSetState2Delegate CreateControlSetState2()
        {
            var method = new DynamicMethod(
                "ControlSetState2",
                typeof(void),
                new[] { typeof(Control), typeof(int), typeof(bool) },
                typeof(ControlStubs).Module,
                true
            );

            var il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Callvirt, typeof(Control).GetMethod(
                "SetState2",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null,
                new[] { typeof(int), typeof(bool) },
                null
            ));
            il.Emit(OpCodes.Ret);

            return (ControlSetState2Delegate)method.CreateDelegate(typeof(ControlSetState2Delegate));
        }

        private static ControlSelectDelegate CreateControlSelect()
        {
            var method = new DynamicMethod(
                "ControlSelect",
                typeof(void),
                new[] { typeof(Control), typeof(bool), typeof(bool) },
                typeof(ControlStubs).Module,
                true
            );

            var il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Callvirt, typeof(Control).GetMethod(
                "Select",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null,
                new[] { typeof(bool), typeof(bool) },
                null
            ));
            il.Emit(OpCodes.Ret);

            return (ControlSelectDelegate)method.CreateDelegate(typeof(ControlSelectDelegate));
        }

        public static void ControlOnPreviewKeyDown(Control target, PreviewKeyDownEventArgs e)
        {
            _controlOnPreviewKeyDownDelegate(target, e);
        }

        public static bool ControlGetState2(Control target, int flag)
        {
            return _controlGetState2(target, flag);
        }

        public static void ControlSetState2(Control target, int flag, bool value)
        {
            _controlSetState2(target, flag, value);
        }

        public static void ControlSelect(Control target, bool directed, bool forward)
        {
            _controlSelect(target, directed, forward);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public static class MessageFilterUtil
    {
        private static readonly Type MsgType = typeof(Application).Assembly.GetType(
            "System.Windows.Forms.NativeMethods+MSG"
        );

        private static readonly FieldInfo MsgHwnd = MsgType.GetField("hwnd");
        private static readonly FieldInfo MsgMessage = MsgType.GetField("message");
        private static readonly FieldInfo MsgWParam = MsgType.GetField("wParam");
        private static readonly FieldInfo MsgLParam = MsgType.GetField("lParam");

        private static readonly Type NiMessageType = typeof(NiMessage);

        private static readonly PropertyInfo NiMessageHwnd = NiMessageType.GetProperty("HWnd");
        private static readonly PropertyInfo NiMessageMsg = NiMessageType.GetProperty("Msg");
        private static readonly PropertyInfo NiMessageWParam = NiMessageType.GetProperty("WParam");
        private static readonly PropertyInfo NiMessageLParam = NiMessageType.GetProperty("LParam");

        private static readonly Type ThreadContextType = typeof(Application).Assembly.GetType(
            "System.Windows.Forms.Application+ThreadContext"
        );

        private static readonly MethodInfo FromCurrentMethod = ThreadContextType.GetMethod(
            "FromCurrent",
            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
            null,
            new Type[0],
            null
        );

        private static readonly MethodInfo ProcessFiltersMethod = ThreadContextType.GetMethod(
            "ProcessFilters",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            null,
            new[] { MsgType.MakeByRefType(), typeof(bool).MakeByRefType() },
            null
        );

        private static readonly InvokeMessageFilterDelegate _invokeMessageFilterDelegate;

        private delegate bool InvokeMessageFilterDelegate(ref NiMessage message);

        static MessageFilterUtil()
        {
            var invokeMessageFilterMethod = new DynamicMethod(
                "InvokeMessageFilter",
                typeof(bool),
                new[] { typeof(NiMessage).MakeByRefType() },
                typeof(MessageFilterUtil).Module,
                true
            );

            ILGenerator il = invokeMessageFilterMethod.GetILGenerator();

            // MSG msg;
            // bool modified;
            // bool processed;

            var msg = il.DeclareLocal(MsgType);
            var modified = il.DeclareLocal(typeof(bool));
            var processed = il.DeclareLocal(typeof(bool));

            // msg = new MSG();

            il.Emit(OpCodes.Ldloca_S, msg);
            il.Emit(OpCodes.Initobj, MsgType);

            // msg.hwnd = message.HWnd;

            il.Emit(OpCodes.Ldloca_S, msg);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, NiMessageHwnd.GetGetMethod());
            il.Emit(OpCodes.Stfld, MsgHwnd);

            // msg.message = message.Msg;

            il.Emit(OpCodes.Ldloca_S, msg);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, NiMessageMsg.GetGetMethod());
            il.Emit(OpCodes.Stfld, MsgMessage);

            // msg.wParam = message.WParam;

            il.Emit(OpCodes.Ldloca_S, msg);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, NiMessageWParam.GetGetMethod());
            il.Emit(OpCodes.Stfld, MsgWParam);

            // msg.lParam = message.LParam;

            il.Emit(OpCodes.Ldloca_S, msg);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, NiMessageLParam.GetGetMethod());
            il.Emit(OpCodes.Stfld, MsgLParam);

            // processed = ThreadContext.FromCurrent().ProcessFilters(ref msg, ref modified);

            il.Emit(OpCodes.Call, FromCurrentMethod);
            il.Emit(OpCodes.Ldloca_S, msg);
            il.Emit(OpCodes.Ldloca_S, modified);
            il.Emit(OpCodes.Callvirt, ProcessFiltersMethod);
            il.Emit(OpCodes.Stloc_2);

            // if (modified) {

            il.Emit(OpCodes.Ldloc_1);
            var label = il.DefineLabel();
            il.Emit(OpCodes.Brfalse_S, label);

            // message.HWnd = msg.hwnd;

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldloca_S, msg);
            il.Emit(OpCodes.Ldfld, MsgHwnd);
            il.Emit(OpCodes.Call, NiMessageHwnd.GetSetMethod());

            // message.Msg = msg.message;

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldloca_S, msg);
            il.Emit(OpCodes.Ldfld, MsgMessage);
            il.Emit(OpCodes.Call, NiMessageMsg.GetSetMethod());

            // message.WParam = msg.wParam;

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldloca_S, msg);
            il.Emit(OpCodes.Ldfld, MsgWParam);
            il.Emit(OpCodes.Call, NiMessageWParam.GetSetMethod());

            // message.LParam = msg.lParam;

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldloca_S, msg);
            il.Emit(OpCodes.Ldfld, MsgLParam);
            il.Emit(OpCodes.Call, NiMessageLParam.GetSetMethod());

            // }
            // return processed;

            il.MarkLabel(label);
            il.Emit(OpCodes.Ldloc_2);
            il.Emit(OpCodes.Ret);

            _invokeMessageFilterDelegate = (InvokeMessageFilterDelegate)invokeMessageFilterMethod.CreateDelegate(typeof(InvokeMessageFilterDelegate));
        }

        public static bool InvokeMessageFilter(ref NiMessage message)
        {
            // This methods invokes System.Windows.Forms.Application+ThreadContext.ProcessFilters.
            // The purpose of this is to broadcast pre-filter messages over
            // AppDomains. The idea is that every AppDomain has a mesage filter.
            // When this message filter receives a message, it is passed
            // to INiShell.BroadcastPreFilterMessage. That method invokes
            // this method on AppDomain 1 and passes it on to all INiPackages
            // that implement INiPreMessageFilter. That implementation in turn
            // also invokes this method so that the message is broadcasted to
            // the rest of the AppDomains. Care must be taken to make sure that
            // this does not recurse, so all calls to INiShell.BroadcastPreMessageFilter
            // and MessageFilterUtil.InvokeMessageFilter must be protected with
            // recursion prevention.

            return _invokeMessageFilterDelegate(ref message);
        }
    }
}

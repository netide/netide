using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public abstract class NiWindow : ServiceObject, IWin32Window, INiMessageFilter
    {
        private static readonly MethodInfo IsInputKeyMethod = typeof(Control).GetMethod(
            "IsInputKey",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
            null,
            new[] { typeof(Keys) },
            null
        );

        private static readonly MethodInfo IsInputCharMethod = typeof(Control).GetMethod(
            "IsInputChar",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
            null,
            new[] { typeof(char) },
            null
        );

        private delegate bool IsInputKeyDelegate(Control control, Keys key);
        private delegate bool IsInputCharDelegate(Control control, char charData);

        private static readonly IsInputKeyDelegate _isInputKeyDelegate;
        private static readonly IsInputCharDelegate _isInputCharDelegate;

        static NiWindow()
        {
            _isInputKeyDelegate = BuildIsInputKeyDelegate();
            _isInputCharDelegate = BuildIsInputCharDelegate();
        }

        private static IsInputKeyDelegate BuildIsInputKeyDelegate()
        {
            var isInputKeyMethod = new DynamicMethod(
                "IsInputKey",
                typeof(bool),
                new[] { typeof(Control), typeof(Keys) },
                typeof(NiWindow).Module,
                true
            );

            var il = isInputKeyMethod.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, IsInputKeyMethod);
            il.Emit(OpCodes.Ret);

            return (IsInputKeyDelegate)isInputKeyMethod.CreateDelegate(typeof(IsInputKeyDelegate));
        }

        private static IsInputCharDelegate BuildIsInputCharDelegate()
        {
            var isInputCharMethod = new DynamicMethod(
                "IsInputChar",
                typeof(bool),
                new[] { typeof(Control), typeof(char) },
                typeof(NiWindow).Module,
                true
            );

            var il = isInputCharMethod.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, IsInputCharMethod);
            il.Emit(OpCodes.Ret);

            return (IsInputCharDelegate)isInputCharMethod.CreateDelegate(typeof(IsInputCharDelegate));
        }

        private bool _hwndCreated;
        private IntPtr _hwnd;
        private bool _disposed;

        public IWin32Window Window { get; set; }

        IntPtr IWin32Window.Handle
        {
            get
            {
                if (!_hwndCreated)
                    CreateHandle();

                return _hwnd;
            }
        }

        private void CreateHandle()
        {
            _hwndCreated = true;

            if (Window != null)
                _hwnd = Window.Handle;
        }

        public HResult PreFilterMessage(ref NiMessage message, out bool processed)
        {
            processed = false;

            try
            {
                if (Window == null)
                    return HResult.OK;

                var control = FindTarget(message.HWnd);

                if (control == null)
                    return HResult.OK;

                var msg = (Message)message;

                processed = control.PreProcessMessage(ref msg);

                message = msg;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult IsInputKey(Keys key, out bool result)
        {
            result = false;

            try
            {
                var focused = FindTarget(GetFocus());
                if (focused != null)
                    result = _isInputKeyDelegate(focused, key);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult IsInputChar(char charCode, out bool result)
        {
            result = false;

            try
            {
                var focused = FindTarget(GetFocus());
                if (focused != null)
                    result = _isInputCharDelegate.Invoke(focused, charCode);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private Control FindTarget(IntPtr hWnd)
        {
            // The messages we get here may not be messages of a control that
            // is in our AppDomain. Because of this, we find a control that is
            // in our AppDomain and request that control to handle it. If
            // that control is a NativeWindowHost, that itself will redirect
            // the message to the correct AppDomain.

            while (hWnd != IntPtr.Zero && hWnd != Window.Handle)
            {
                var control = Control.FromHandle(hWnd);

                if (control != null)
                    return control;

                hWnd = GetParent(hWnd);
            }

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                var disposable = Window as IDisposable;

                if (disposable != null)
                    disposable.Dispose();

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetParent(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetFocus();
    }
}

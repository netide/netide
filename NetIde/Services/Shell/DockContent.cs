using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Collections.Generic;
using System;
using System.Windows.Forms;
using NetIde.Services.Env;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.Shell
{
    internal class DockContent : Support.DockContent
    {
        private Proxy _proxy;
        private WindowHost _host;
        private bool _disposed;
        private bool _suppressClosing;

        public INiWindowPane WindowPane { get; private set; }
        public NiDockStyle DockStyle { get; private set; }
        public NiToolWindowOrientation Orientation { get; private set; }

        public DockContent(INiWindowPane windowPane, NiDockStyle dockStyle, NiToolWindowOrientation orientation)
        {
            if (windowPane == null)
                throw new ArgumentNullException("windowPane");

            WindowPane = windowPane;
            DockStyle = dockStyle;
            Orientation = orientation;
            ShowIcon = false;

            switch (dockStyle)
            {
                case NiDockStyle.Document:
                    DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
                    break;

                case NiDockStyle.Float:
                    DockAreas =
                        WeifenLuo.WinFormsUI.Docking.DockAreas.Document |
                        WeifenLuo.WinFormsUI.Docking.DockAreas.Float;
                    break;

                case NiDockStyle.AlwaysFloat:
                    DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Float;
                    break;

                default:
                    DockAreas =
                        WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom |
                        WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft |
                        WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight |
                        WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop;
                    break;
            }

            _host = new WindowHost(this)
            {
                Dock = System.Windows.Forms.DockStyle.Fill
            };

            Controls.Add(_host);
        }

        public INiWindowFrame GetProxy()
        {
            return _proxy ?? (_proxy = new Proxy(this));
        }

        public void RaiseShow(NiWindowShow show)
        {
            if (!_disposed)
                _host.RaiseShow(show);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!_suppressClosing)
            {
                bool cancel = false;
                RaiseClose(NiFrameCloseMode.PromptSave, ref cancel);

                if (cancel)
                    e.Cancel = true;
            }

            base.OnFormClosing(e);
        }

        public void RaiseClose(NiFrameCloseMode closeMode, ref bool cancel)
        {
            if (!_disposed)
                _host.RaiseClose(closeMode, ref cancel);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_host != null)
                {
                    _host.Dispose();
                    _host = null;
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        private class WindowHost : NiWindowHost
        {
            private static readonly MethodInfo ProcessCmdKeyMethod = typeof(Control).GetMethod(
                "ProcessCmdKey",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null,
                new[] { typeof(Message).MakeByRefType(), typeof(Keys) },
                null
            );

            private static readonly MethodInfo ProcessDialogCharMethod = typeof(Control).GetMethod(
                "ProcessDialogChar",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null,
                new[] { typeof(char) },
                null
            );

            private static readonly MethodInfo IsInputCharMethod = typeof(Control).GetMethod(
                "IsInputChar",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null,
                new[] { typeof(char) },
                null
            );

            private delegate bool ProcessCmdKeyDelegate(Control control, ref Message msg, Keys key);
            private delegate bool ProcessDialogCharDelegate(Control control, char charData);
            private delegate bool IsInputCharDelegate(Control control, char charData);

            private static readonly ProcessCmdKeyDelegate _processCmdKeyDelegate;
            private static readonly ProcessDialogCharDelegate _processDialogCharDelegate;
            private static readonly IsInputCharDelegate _isInputCharDelegate;

            static WindowHost()
            {
                var processCmdKeyMethod = new DynamicMethod(
                    "ProcessCmdKey",
                    typeof(bool),
                    new[] { typeof(Control), typeof(Message).MakeByRefType(), typeof(Keys) },
                    typeof(DockContent).Module,
                    true
                );

                var il = processCmdKeyMethod.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Callvirt, ProcessCmdKeyMethod);
                il.Emit(OpCodes.Ret);

                _processCmdKeyDelegate = (ProcessCmdKeyDelegate)processCmdKeyMethod.CreateDelegate(typeof(ProcessCmdKeyDelegate));

                var isInputCharMethod = new DynamicMethod(
                    "IsInputChar",
                    typeof(bool),
                    new[] { typeof(Control), typeof(char) },
                    typeof(NiWindow).Module,
                    true
                );

                il = isInputCharMethod.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Callvirt, IsInputCharMethod);
                il.Emit(OpCodes.Ret);

                _isInputCharDelegate = (IsInputCharDelegate)isInputCharMethod.CreateDelegate(typeof(IsInputCharDelegate));

                var processDialogCharMethod = new DynamicMethod(
                    "ProcessDialogChar",
                    typeof(bool),
                    new[] { typeof(Control), typeof(char) },
                    typeof(NiWindow).Module,
                    true
                );

                il = processDialogCharMethod.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Callvirt, ProcessDialogCharMethod);
                il.Emit(OpCodes.Ret);

                _processDialogCharDelegate = (ProcessDialogCharDelegate)processDialogCharMethod.CreateDelegate(typeof(ProcessDialogCharDelegate));
            }

            private readonly DockContent _dockContent;

            public WindowHost(DockContent dockContent)
            {
                _dockContent = dockContent;
            }

            protected override INiWindowPane CreateWindow()
            {
                return _dockContent.WindowPane;
            }

            public override bool PreProcessMessage(ref Message msg)
            {
                // We give the form a chance to handle the message first. This is
                // to allow main menu mnemonics to work correctly.

                var form = _dockContent.FindForm();
                if (form != null)
                {
                    if (msg.Msg == WM_KEYDOWN || msg.Msg == WM_SYSKEYDOWN)
                    {
                        var keyData = (Keys)(unchecked((int)(long)msg.WParam) | (int)ModifierKeys);
                        if (_processCmdKeyDelegate(form, ref msg, keyData))
                            return true;
                    }
                    else if (msg.Msg == WM_CHAR || msg.Msg == WM_SYSCHAR)
                    {
                        if (msg.Msg != WM_CHAR || !_isInputCharDelegate(form, (char)msg.WParam))
                        {
                            if (_processDialogCharDelegate(form, (char)msg.WParam))
                                return true;
                        }
                    }
                }

                return base.PreProcessMessage(ref msg);
            }

            private const int WM_KEYDOWN = 0x100;
            private const int WM_SYSKEYDOWN = 0x104;
            private const int WM_CHAR = 0x102;
            private const int WM_SYSCHAR = 0x106;
        }

        private class Proxy : ServiceObject, INiWindowFrame
        {
            private readonly DockContent _owner;
            private IResource _icon;
            private readonly NiEnv _env;
            private bool _shown;
            private readonly Dictionary<int, object> _properties = new Dictionary<int, object>();

            public Proxy(DockContent owner)
            {
                _owner = owner;
                _env = (NiEnv)_owner.GetService(typeof(INiEnv));

                if ((owner.DockAreas & WeifenLuo.WinFormsUI.Docking.DockAreas.Document) != 0)
                    _properties[(int)NiFrameProperty.Type] = NiFrameType.Document;
                else
                    _properties[(int)NiFrameProperty.Type] = NiFrameType.Tool;
            }

            public bool IsVisible
            {
                get { return _owner.Visible; }
            }

            public string Caption
            {
                get { return _owner.TabText; }
                set { _owner.Text = _owner.TabText = value; }
            }

            public IResource Icon
            {
                get { return _icon; }
                set
                {
                    if (_icon != value)
                    {
                        _icon = value;

                        _owner.Icon =
                            value == null
                            ? null
                            : _env.ResourceManager.GetIcon(value);
                        _owner.ShowIcon = _owner.Icon != null;
                    }
                }
            }

            public HResult Show()
            {
                try
                {
                    if (_shown)
                    {
                        _owner.IsHidden = false;
                        _owner.Show();
                    }
                    else
                    {
                        _shown = true;

                        ((NiEnv)_owner.GetService(typeof(INiEnv))).MainForm.ShowContent(_owner);
                    }

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult Hide()
            {
                try
                {
                    _owner.IsHidden = true;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult Close(NiFrameCloseMode closeMode)
            {
                try
                {
                    _owner._suppressClosing = true;

                    try
                    {
                        bool cancel = false;
                        _owner.RaiseClose(closeMode, ref cancel);

                        if (cancel)
                            return HResult.False;

                        _owner.Close();
                    }
                    finally
                    {
                        _owner._suppressClosing = false;
                    }

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult Advise(object sink, out int cookie)
            {
                return _owner._host.Advise(sink, out cookie);
            }

            public HResult Advise(INiWindowFrameNotify sink, out int cookie)
            {
                return Advise((object)sink, out cookie);
            }

            public HResult Unadvise(int cookie)
            {
                return _owner._host.Unadvise(cookie);
            }

            public HResult GetProperty(int property, out object value)
            {
                value = null;

                try
                {
                    if (!_properties.TryGetValue(property, out value))
                        return HResult.False;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetProperty(int property, object value)
            {
                try
                {
                    if (value == null)
                        _properties.Remove(property);
                    else
                        _properties[property] = value;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Shell.Interop
{
    [Serializable]
    public struct NiMessage
    {
        public IntPtr HWnd { get; set; }
        public int Msg { get; set; }
        public IntPtr WParam { get; set; }
        public IntPtr LParam { get; set; }
        public IntPtr Result { get; set; }

        public static implicit operator Message(NiMessage message)
        {
            return new Message
            {
                HWnd = message.HWnd,
                Msg = message.Msg,
                WParam = message.WParam,
                LParam = message.LParam,
                Result = message.Result
            };
        }

        public static implicit operator NiMessage(Message message)
        {
            return new NiMessage
            {
                HWnd = message.HWnd,
                Msg = message.Msg,
                WParam = message.WParam,
                LParam = message.LParam,
                Result = message.Result
            };
        }
    }
}

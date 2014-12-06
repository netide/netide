using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Core.ToolWindows.TextEditor
{
    partial class TextEditorWindow
    {
        public HResult GetTextBuffer(out INiTextBuffer textBuffer)
        {
            textBuffer = _textLines;
            return HResult.OK;
        }

        public HResult Find(string search, NiFindOptions options, bool resetStartPoint, INiFindHelper helper, out NiFindResult result)
        {
            return Control.FindTarget.Find(search, options, resetStartPoint, helper, out result);
        }

        public HResult Replace(string search, string replace, NiFindOptions options, bool resetStartPoint, INiFindHelper helper, out NiFindResult result)
        {
            return Control.FindTarget.Replace(search, replace, options, resetStartPoint, helper, out result);
        }

        public HResult GetCurrentSpan(out NiTextSpan span)
        {
            return Control.FindTarget.GetCurrentSpan(out span);
        }

        public HResult GetFindState(out object state)
        {
            return Control.FindTarget.GetFindState(out state);
        }

        public HResult SetFindState(object state)
        {
            return Control.FindTarget.SetFindState(state);
        }

        public HResult MarkSpan(NiTextSpan span)
        {
            return Control.FindTarget.MarkSpan(span);
        }

        public HResult NavigateTo(NiTextSpan span)
        {
            return Control.FindTarget.NavigateTo(span);
        }

        public HResult NotifyFindTarget(NiFindTargetNotify notify)
        {
            return Control.FindTarget.NotifyFindTarget(notify);
        }

        public HResult GetCapabilities(out NiFindCapabilities options)
        {
            return Control.FindTarget.GetCapabilities(out options);
        }

        public HResult GetInitialPattern(out string pattern)
        {
            return Control.FindTarget.GetInitialPattern(out pattern);
        }
    }
}

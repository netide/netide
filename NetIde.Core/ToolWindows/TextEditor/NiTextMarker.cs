using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.TextEditor.Document;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util;

namespace NetIde.Core.ToolWindows.TextEditor
{
    internal class NiTextMarker : ServiceObject, INiTextMarker
    {
        private readonly NiTextLines _owner;
        private TextMarker _marker;
        private bool _disposed;

        public NiTextMarker(NiTextLines owner, TextMarker marker)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (marker == null)
                throw new ArgumentNullException("marker");

            _owner = owner;
            _marker = marker;
        }

        public HResult GetType(out NiTextMarkerType type)
        {
            switch (_marker.TextMarkerType & ~TextMarkerType.ExtendToBorder)
            {
                case TextMarkerType.Invisible: type = NiTextMarkerType.Invisible; break;
                case TextMarkerType.SolidBlock: type = NiTextMarkerType.SolidBlock; break;
                case TextMarkerType.Underlined: type = NiTextMarkerType.Underline; break;
                case TextMarkerType.WaveLine: type = NiTextMarkerType.WaveLine; break;
                default: type = 0; break;
            }

            return HResult.OK;
        }

        public HResult GetColor(out int color)
        {
            color = _marker.Color.ToArgb();
            return HResult.OK;
        }

        public HResult GetForeColor(out int foreColor)
        {
            foreColor = _marker.OverrideForeColor ? _marker.ForeColor.ToArgb() : 0;
            return HResult.OK;
        }

        public HResult GetHatchStyle(out NiTextMarkerHatchStyle hatchStyle)
        {
            hatchStyle = 0;

            try
            {
                hatchStyle = _marker.HaveHatchStyle
                    ? Enum<NiTextMarkerHatchStyle>.Parse(_marker.HatchStyle.ToString())
                    : NiTextMarkerHatchStyle.Default;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetExtendToBorder(out bool extendToBorder)
        {
            extendToBorder = (_marker.TextMarkerType & TextMarkerType.ExtendToBorder) != 0;
            return HResult.OK;
        }

        public HResult GetIsReadOnly(out bool isReadOnly)
        {
            isReadOnly = _marker.IsReadOnly;
            return HResult.OK;
        }

        public HResult SetIsReadOnly(bool isReadOnly)
        {
            try
            {
                _marker.IsReadOnly = isReadOnly;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetToolTip(out string toolTip)
        {
            toolTip = _marker.ToolTip;
            return HResult.OK;
        }

        public HResult SetToolTip(string toolTip)
        {
            try
            {
                _marker.ToolTip = toolTip;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetOffset(out int startLine, out int startIndex, out int endLine, out int endIndex)
        {
            startLine = startIndex = endLine = endIndex = -1;

            try
            {
                var segment = _owner.Document.GetLineSegmentForOffset(_marker.Offset);

                startLine = segment.LineNumber;
                startIndex = segment.Offset - _marker.Offset;

                segment = _owner.Document.GetLineSegmentForOffset(_marker.EndOffset);

                endLine = segment.LineNumber;
                endIndex = segment.Offset - _marker.EndOffset;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_marker != null)
                {
                    _owner.Document.MarkerStrategy.RemoveMarker(_marker);
                    _marker = null;
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}

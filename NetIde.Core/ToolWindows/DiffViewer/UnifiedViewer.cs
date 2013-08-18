using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NGit.Diff;
using NetIde.Core.Settings;
using NetIde.Shell.Interop;
using NetIde.Shell.Settings;
using NetIde.Util;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal partial class UnifiedViewer : NetIde.Util.Forms.UserControl, ITextViewer
    {
        private const string NoNewLine = "\\ No newline at end of file\n";

        private StringBuilder _out;

        public int Context { get; set; }

        public UnifiedViewer()
        {
            InitializeComponent();

            _editor.IsReadOnly = true;
            _editor.SetHighlighting("Patch");

            Context = 3;
        }

        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;

                var fontSettings = SettingsBuilder.GetSettings<IFontSettings>(Site);

                _editor.Font = fontSettings.CodeFont ?? Constants.DefaultCodeFont;
            }
        }

        public void SelectDetails(IStream leftStream, FileType leftFileType, IStream rightStream, FileType rightFileType)
        {
            _leftDetails.SelectDetails(leftStream, leftFileType);
            _rightDetails.SelectDetails(rightStream, rightFileType);
        }

        public void LoadDiff(Text leftText, Text rightText, EditList editList)
        {
            _out = new StringBuilder();

            Format(editList, leftText, rightText);

            _editor.Text = _out.ToString();
            _editor.Refresh();

            _out = null;
        }

        private void Format(EditList edits, Text a, Text b)
        {
            for (int curIdx = 0; curIdx < edits.Count; )
            {
                Edit curEdit = edits[curIdx];
                int endIdx = FindCombinedEnd(edits, curIdx);
                Edit endEdit = edits[endIdx];
                int aCur = Math.Max(0, curEdit.GetBeginA() - Context);
                int bCur = Math.Max(0, curEdit.GetBeginB() - Context);
                int aEnd = Math.Min(a.Size(), endEdit.GetEndA() + Context);
                int bEnd = Math.Min(b.Size(), endEdit.GetEndB() + Context);
                WriteHunkHeader(aCur, aEnd, bCur, bEnd);
                while (aCur < aEnd || bCur < bEnd)
                {
                    if (aCur < curEdit.GetBeginA() || endIdx + 1 < curIdx)
                    {
                        WriteContextLine(a, aCur);
                        if (IsEndOfLineMissing(a, aCur))
                        {
                            _out.Append(NoNewLine);
                        }
                        aCur++;
                        bCur++;
                    }
                    else
                    {
                        if (aCur < curEdit.GetEndA())
                        {
                            WriteRemovedLine(a, aCur);
                            if (IsEndOfLineMissing(a, aCur))
                            {
                                _out.Append(NoNewLine);
                            }
                            aCur++;
                        }
                        else
                        {
                            if (bCur < curEdit.GetEndB())
                            {
                                WriteAddedLine(b, bCur);
                                if (IsEndOfLineMissing(b, bCur))
                                {
                                    _out.Append(NoNewLine);
                                }
                                bCur++;
                            }
                        }
                    }
                    if (End(curEdit, aCur, bCur) && ++curIdx < edits.Count)
                    {
                        curEdit = edits[curIdx];
                    }
                }
            }
        }

        private void WriteContextLine(Text text, int line)
        {
            WriteLine(' ', text, line);
        }

        private bool IsEndOfLineMissing(Text text, int line)
        {
            return line + 1 == text.Size() && text.IsMissingNewlineAtEnd();
        }

        private void WriteAddedLine(Text text, int line)
        {
            WriteLine('+', text, line);
        }

        private void WriteRemovedLine(Text text, int line)
        {
            WriteLine('-', text, line);
        }

        private void WriteHunkHeader(int aStartLine, int aEndLine, int bStartLine, int bEndLine)
        {
            _out.Append('@');
            _out.Append('@');
            WriteRange('-', aStartLine + 1, aEndLine - aStartLine);
            WriteRange('+', bStartLine + 1, bEndLine - bStartLine);
            _out.Append(' ');
            _out.Append('@');
            _out.Append('@');
            _out.Append('\n');
        }

        private void WriteRange(char prefix, int begin, int cnt)
        {
            _out.Append(' ');
            _out.Append(prefix);
            switch (cnt)
            {
                case 0:
                    {
                        // If the range is empty, its beginning number must be the
                        // line just before the range, or 0 if the range is at the
                        // start of the file stream. Here, begin is always 1 based,
                        // so an empty file would produce "0,0".
                        //
                        _out.Append((begin - 1).ToString(CultureInfo.InvariantCulture));
                        _out.Append(',');
                        _out.Append('0');
                        break;
                    }

                case 1:
                    {
                        // If the range is exactly one line, produce only the number.
                        //
                        _out.Append(begin.ToString(CultureInfo.InvariantCulture));
                        break;
                    }

                default:
                    {
                        _out.Append(begin.ToString(CultureInfo.InvariantCulture));
                        _out.Append(',');
                        _out.Append(cnt.ToString(CultureInfo.InvariantCulture));
                        break;
                    }
            }
        }

        private void WriteLine(char prefix, Text text, int cur)
        {
            _out.Append(prefix);
            text.WriteLine(_out, cur);
            _out.Append('\n');
        }

        private int FindCombinedEnd(IList<Edit> edits, int i)
        {
            int end = i + 1;
            while (end < edits.Count && (CombineA(edits, end) || CombineB(edits, end)))
            {
                end++;
            }
            return end - 1;
        }

        private bool CombineA(IList<Edit> e, int i)
        {
            return e[i].GetBeginA() - e[i - 1].GetEndA() <= 2 * Context;
        }

        private bool CombineB(IList<Edit> e, int i)
        {
            return e[i].GetBeginB() - e[i - 1].GetEndB() <= 2 * Context;
        }

        private static bool End(Edit edit, int a, int b)
        {
            return edit.GetEndA() <= a && edit.GetEndB() <= b;
        }

        public void Reset()
        {
            _editor.Text = null;
            _editor.Refresh();
        }
    }
}

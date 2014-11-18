using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Automation;
using System.Windows.Automation.Provider;
using System.Windows.Automation.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using UIAutomationWrapper;
using Point = System.Windows.Point;

namespace NetIde.Core
{
    internal class TextEditorControl : ICSharpCode.TextEditor.TextEditorControl
    {
        public TextEditorControl()
        {
            ElementProvider.Install(new TextEditorElementProvider(this));
            ActiveTextAreaControl.TextArea.DoProcessDialogKey += TextArea_DoProcessDialogKey;
        }

        void TextArea_DoProcessDialogKey(Keys keyData, ref DialogKeyProcessorResult result)
        {
            if (Document.ReadOnly)
            {
                switch (keyData)
                {
                    case Keys.Tab:
                    case Keys.Tab | Keys.Shift:
                    case Keys.Enter:
                    case Keys.Escape:
                        result = DialogKeyProcessorResult.NotProcessed;
                        break;
                }
            }
        }

        [ComVisible(true)]
        private class TextEditorElementProvider : ElementProvider, IValueProvider, ITextProvider
        {
            public new TextEditorControl Control
            {
                get { return (TextEditorControl)base.Control; }
            }

            public override System.Windows.Automation.ControlType ControlType
            {
                get { return System.Windows.Automation.ControlType.Document; }
            }

            public TextEditorElementProvider(TextEditorControl control)
                : base(control)
            {
            }

            bool IValueProvider.IsReadOnly
            {
                get { return Control.Document.ReadOnly; }
            }

            void IValueProvider.SetValue(string value)
            {
                Control.Document.TextContent = value;
            }

            string IValueProvider.Value
            {
                get { return Control.Document.TextContent; }
            }

            ITextRangeProvider ITextProvider.DocumentRange
            {
                get
                {
                    var document = Control.Document;

                    return new TextRangeProvider(
                        Control,
                        new DefaultSelection(
                            document,
                            document.OffsetToPosition(0),
                            document.OffsetToPosition(document.TextLength)
                        )
                    );
                }
            }

            ITextRangeProvider[] ITextProvider.GetSelection()
            {
                var selectionManager = Control.ActiveTextAreaControl.SelectionManager;

                var result = new ITextRangeProvider[selectionManager.SelectionCollection.Count];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = new TextRangeProvider(Control, selectionManager.SelectionCollection[0]);
                }

                return result;
            }

            ITextRangeProvider[] ITextProvider.GetVisibleRanges()
            {
                var document = Control.Document;
                var textView = Control.ActiveTextAreaControl.TextArea.TextView;

                var result = new ITextRangeProvider[textView.VisibleLineCount];

                for (int line = 0; line < textView.VisibleLineCount; line++)
                {
                    result[line] = RangeFromLine(document, textView.FirstVisibleLine + line);
                }

                return result;
            }

            private TextRangeProvider RangeFromLine(IDocument document, int line)
            {
                var lineSegment = document.GetLineSegment(line);

                return new TextRangeProvider(
                    Control,
                    new DefaultSelection(
                        document,
                        document.OffsetToPosition(lineSegment.Offset),
                        document.OffsetToPosition(lineSegment.Offset + lineSegment.TotalLength)
                    )
                );
            }

            ITextRangeProvider ITextProvider.RangeFromChild(IRawElementProviderSimple childElement)
            {
                return null;
            }

            ITextRangeProvider ITextProvider.RangeFromPoint(Point screenLocation)
            {
                var document = Control.Document;
                var textArea = Control.ActiveTextAreaControl.TextArea;
                var point = textArea.PointToScreen(new System.Drawing.Point((int)screenLocation.X, (int)screenLocation.Y));
                var location = textArea.TextView.GetLogicalPosition(point);

                return RangeFromLine(document, location.Y);
            }

            SupportedTextSelection ITextProvider.SupportedTextSelection
            {
                get { return SupportedTextSelection.Multiple; }
            }

            [ComVisible(true)]
            private class TextRangeProvider : ITextRangeProvider
            {
                private static readonly Dictionary<AutomationTextAttribute, Func<TextRangeProvider, object>> _attributeGetter = new Dictionary<AutomationTextAttribute, Func<TextRangeProvider, object>>
                {
                    { TextPattern.BackgroundColorAttribute, p => p.GetColor(false) },
                    { TextPattern.FontNameAttribute, p => p.Font.Name },
                    { TextPattern.FontSizeAttribute, p => (double)p.Font.Size },
                    { TextPattern.FontWeightAttribute, p => p.Font.Bold ? 700 /* bold */ : 400 /* normal */ },
                    { TextPattern.ForegroundColorAttribute, p => p.GetColor(true) },
                    { TextPattern.HorizontalTextAlignmentAttribute, p => HorizontalTextAlignment.Left},
                    { TextPattern.IsHiddenAttribute, p => false },
                    { TextPattern.IsItalicAttribute, p => p.Font.Italic },
                    { TextPattern.IsReadOnlyAttribute, p => p._control.Document.ReadOnly },
                    { TextPattern.IsSubscriptAttribute, p => false },
                    { TextPattern.IsSuperscriptAttribute, p => false },
                    { TextPattern.TextFlowDirectionsAttribute, p => FlowDirections.Default }
                };

                private readonly TextEditorControl _control;
                private ISelection _selection;

                private Font Font
                {
                    get { return _control.ActiveTextAreaControl.TextArea.Font; }
                }

                public TextRangeProvider(TextEditorControl control, ISelection selection)
                {
                    _control = control;
                    _selection = selection;
                }

                public void AddToSelection()
                {
                    var selectionManager = _control.ActiveTextAreaControl.SelectionManager;

                    selectionManager.SelectionCollection.Add(_selection);
                    selectionManager.FireSelectionChanged();
                }

                public ITextRangeProvider Clone()
                {
                    return new TextRangeProvider(_control, _selection);
                }

                public bool Compare(ITextRangeProvider range)
                {
                    if (range == null)
                        throw new ArgumentNullException("range");

                    var textRange = range as TextRangeProvider;

                    return
                        textRange != null &&
                        _selection.StartPosition == textRange._selection.StartPosition &&
                        _selection.EndOffset == textRange._selection.EndOffset;
                }

                public int CompareEndpoints(TextPatternRangeEndpoint endpoint, ITextRangeProvider targetRange, TextPatternRangeEndpoint targetEndpoint)
                {
                    var range = targetRange as TextRangeProvider;

                    if (range == null)
                        return -1;

                    var a = endpoint == TextPatternRangeEndpoint.Start ? range._selection.StartPosition : range._selection.EndPosition;
                    var b = targetEndpoint == TextPatternRangeEndpoint.Start ? range._selection.StartPosition : range._selection.EndPosition;

                    var document = _control.Document;

                    return document.PositionToOffset(a).CompareTo(document.PositionToOffset(b));
                }

                public void ExpandToEnclosingUnit(TextUnit unit)
                {
                    var document = _control.Document;
                    int endOffset = _selection.EndOffset;

                    switch (unit)
                    {
                        case TextUnit.Character:
                        case TextUnit.Line:
                        case TextUnit.Paragraph:
                        case TextUnit.Word:
                            endOffset = FindOffset(endOffset, unit, true);

                            _selection = new DefaultSelection(
                                document,
                                _selection.StartPosition,
                                document.OffsetToPosition(endOffset)
                            );
                            break;

                        case TextUnit.Document:
                            _selection = new DefaultSelection(
                                document,
                                document.OffsetToPosition(0),
                                document.OffsetToPosition(document.TextLength)
                            );
                            break;
                    }
                }

                private int FindOffset(int offset, TextUnit unit, bool forward)
                {
                    throw new NotImplementedException();
                }

                public ITextRangeProvider FindAttribute(int attribute, object value, bool backward)
                {
                    // Finding a range by an attribute value doesn't make sense
                    // for this type of editor.

                    return null;
                }

                public ITextRangeProvider FindText(string text, bool backward, bool ignoreCase)
                {
                    throw new NotImplementedException();
                }

                public object GetAttributeValue(int attribute)
                {
                    return GetAttributeValue(AutomationTextAttribute.LookupById(attribute));
                }

                private object GetAttributeValue(AutomationTextAttribute attribute)
                {
                    Func<TextRangeProvider, object> func;
                    if (_attributeGetter.TryGetValue(attribute, out func))
                        return func(this);

                    return null;
                }

                public double[] GetBoundingRectangles()
                {
                    throw new NotImplementedException();
                }

                public IRawElementProviderSimple[] GetChildren()
                {
                    return new IRawElementProviderSimple[0];
                }

                public IRawElementProviderSimple GetEnclosingElement()
                {
                    return null;
                }

                public string GetText(int maxLength)
                {
                    string text = _control.Document.GetText(
                        _selection.Offset,
                        _selection.Length
                    );

                    if (text.Length > maxLength)
                        text = text.Substring(0, maxLength);

                    return text;
                }

                public int Move(TextUnit unit, int count)
                {
                    throw new NotImplementedException();
                }

                public void MoveEndpointByRange(TextPatternRangeEndpoint endpoint, ITextRangeProvider targetRange, TextPatternRangeEndpoint targetEndpoint)
                {
                    throw new NotImplementedException();
                }

                public int MoveEndpointByUnit(TextPatternRangeEndpoint endpoint, TextUnit unit, int count)
                {
                    throw new NotImplementedException();
                }

                public void RemoveFromSelection()
                {
                    var selectionManager = _control.ActiveTextAreaControl.SelectionManager;

                    for (int i = 0; i < selectionManager.SelectionCollection.Count; i++)
                    {
                        var selection = selectionManager.SelectionCollection[i];

                        if (
                            selection.StartPosition == _selection.StartPosition &&
                            selection.EndPosition == _selection.EndPosition
                        )
                        {
                            selectionManager.SelectionCollection.RemoveAt(i);
                            selectionManager.FireSelectionChanged();
                            break;
                        }
                    }
                }

                public void ScrollIntoView(bool alignToTop)
                {
                    _control.ActiveTextAreaControl.ScrollTo(_selection.StartPosition.Line);
                }

                public void Select()
                {
                    _control.ActiveTextAreaControl.SelectionManager.SetSelection(_selection);
                }

                private int GetColor(bool foreground)
                {
                    var color = _control.Document.GetLineSegment(_selection.StartPosition.Line).GetColorForPosition(_selection.StartPosition.X);

                    if (foreground)
                    {
                        if (color.HasForeground)
                            return color.Color.ToArgb();

                        return _control.ActiveTextAreaControl.TextArea.ForeColor.ToArgb();
                    }

                    if (color.HasBackground)
                        return color.BackgroundColor.ToArgb();

                    return _control.ActiveTextAreaControl.TextArea.BackColor.ToArgb();
                }
            }
        }
    }
}

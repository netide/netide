using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GdiPresentation;
using NetIde.Core.Support;
using Color = GdiPresentation.Color;
using ComboBox = System.Windows.Forms.ComboBox;
using Orientation = GdiPresentation.Orientation;
using SolidBrush = GdiPresentation.SolidBrush;

namespace NetIde.Core.TextEditor
{
    partial class FindControl
    {
        private static readonly Bitmap CaseSensitiveImage = NeutralResources.CaseSensitive;
        private static readonly Bitmap CloseFindImage = NeutralResources.CloseFind;
        private static readonly Bitmap RegularExpressionImage = NeutralResources.RegularExpression;
        private static readonly Bitmap ReplaceAllImage = NeutralResources.ReplaceAll;
        private static readonly Bitmap ReplaceNextImage = NeutralResources.ReplaceNext;
        private static readonly Bitmap ToggleDownImage = NeutralResources.ToggleDown;
        private static readonly Bitmap ToggleUpImage = NeutralResources.ToggleUp;
        private static readonly Bitmap WholeWordImage = NeutralResources.WholeWord;
        private static readonly Bitmap FindNextImage = NeutralResources.FindNext;

        private Element BuildContent()
        {
            _findWhat = new ComboBoxInput
            {
                MinWidth = 200,
                Margin = new Thickness(2)
            };
            _replaceWith = new ComboBoxInput
            {
                MinWidth = 200,
                Margin = new Thickness(2)
            };
            _toggleReplace = new GdiButton
            {
                Bitmap = ToggleDownImage,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Padding = new Thickness(3, 5),
                Margin = new Thickness(2)
            };
            _toggleReplace.Click += _toggleReplace_Click;
            _findNext = new GdiButton
            {
                Bitmap = FindNextImage,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle,
                Margin = new Thickness(2)
            };
            _findNext.Click += _findNext_Click;
            _close = new GdiButton
            {
                Bitmap = CloseFindImage,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle,
                Margin = new Thickness(2)
            };
            _close.Click += _close_Click;
            _replaceNext = new GdiButton
            {
                Bitmap = ReplaceNextImage,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle,
                Margin = new Thickness(2)
            };
            _replaceNext.Click += _replaceNext_Click;
            _replaceAll = new GdiButton
            {
                Bitmap = ReplaceAllImage,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle,
                Margin = new Thickness(2)
            };
            _replaceAll.Click += _replaceAll_Click;
            _matchCase = new GdiButton
            {
                IsToggle = true,
                Bitmap = CaseSensitiveImage,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle,
                Margin = new Thickness(2)
            };
            _matchCase.IsCheckedChanged += _matchCase_IsCheckedChanged;
            _matchWholeWord = new GdiButton
            {
                IsToggle = true,
                Bitmap = WholeWordImage,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle,
                Margin = new Thickness(2)
            };
            _matchWholeWord.IsCheckedChanged += _matchWholeWord_IsCheckedChanged;
            _useRegularExpressions = new GdiButton
            {
                IsToggle = true,
                Bitmap = RegularExpressionImage,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle,
                Margin = new Thickness(2)
            };
            _useRegularExpressions.IsCheckedChanged += _useRegularExpressions_IsCheckedChanged;

            _findRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children =
                {
                    _findWhat,
                    _findNext,
                    _close
                }
            };
            _replaceRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children =
                {
                    _replaceWith,
                    _replaceNext,
                    _replaceAll
                }
            };
            _optionsRow = new StackPanel
            {
                Children =
                {
                    _matchCase,
                    _matchWholeWord,
                    _useRegularExpressions
                }
            };

            return new Border
            {
                Content = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Children =
                    {
                        _toggleReplace,
                        new StackPanel
                        {
                            Orientation = Orientation.Vertical,
                            Children =
                            {
                                _findRow,
                                _replaceRow,
                                _optionsRow
                            }
                        }
                    }
                },
                Padding = new Thickness(4)
            };
        }

        private class ComboBoxInput : ControlHost<ComboBox>
        {
            public ComboBoxInput()
                : base(new ComboBox())
            {
            }
        }
    }
}

/*
 * Copyright © 2007 Hedley Muscroft
 * 
 * Design Notes:-
 * --------------
 * References:
 * - http://www.codeproject.com/KB/vista/Vista_TaskDialog_Wrapper.aspx
 * 
 * Revision Control:-
 * ------------------
 * Created On: 2007 November 26
 * 
 * $Revision:$
 * $LastChangedDate:$
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Util.Forms
{
    [DesignTimeVisible(false)]
    internal partial class EmulateCommandLink : Button
    {
        private const TextFormatFlags LargeTextFlags = TextFormatFlags.SingleLine;
        private const TextFormatFlags SmallTextFlags = 0;

        enum ButtonState { Normal, MouseOver, Down }

        bool m_autoHeight = true;
        ButtonState buttonState = ButtonState.Normal;
        Image imgArrowNormal = null;
        Image imgArrowHovered = null;

        // Make sure the control is invalidated(repainted) when the text is changed.
        public override string Text { get { return base.Text; } set { base.Text = value; this.Invalidate ( ); } }

        public bool AutoHeight { get { return m_autoHeight; } set { m_autoHeight = value; if ( m_autoHeight ) this.Invalidate ( ); } }
        public Font SmallFont { get; set; }

        //--------------------------------------------------------------------------------
        public EmulateCommandLink ( )
        {
            InitializeComponent ( );
            base.Font = EmulateTaskDialog.LargeThemedFont;
            SmallFont = EmulateTaskDialog.ThemedFont;
        }

        //--------------------------------------------------------------------------------
        protected override void OnCreateControl ( )
        {
            base.OnCreateControl ( );
            imgArrowNormal = NeutralResources.TaskDialog_ArrowNormal;
            imgArrowHovered = NeutralResources.TaskDialog_ArrowHovered;
        }

        //--------------------------------------------------------------------------------
        const int LEFT_MARGIN = 10;
        const int TOP_MARGIN = 10;
        const int ARROW_WIDTH = 19;

        string GetLargeText ( )
        {
            string[] lines = this.Text.Split ( new char[] { '\n' } );
            return lines[0];
        }

        string GetSmallText ( )
        {
            if ( this.Text.IndexOf ( '\n' ) < 0 )
                return "";

            string s = this.Text;
            string[] lines = s.Split ( new char[] { '\n' } );
            s = "";
            for ( int i = 1; i < lines.Length; i++ )
                s += lines[i] + "\n";
            return s.Trim ( new char[] { '\n' } );
        }

        Size GetLargeTextSizeF ( )
        {
            int x = LEFT_MARGIN + ARROW_WIDTH + 5;
            Size mzSize = new Size ( this.Width - x - LEFT_MARGIN, 5000 );  // presume RIGHT_MARGIN = LEFT_MARGIN
            return TextRenderer.MeasureText(
                GetLargeText(),
                base.Font,
                mzSize,
                LargeTextFlags
            );
        }

        Size GetSmallTextSizeF ( )
        {
            string s = GetSmallText ( );
            if ( s == "" ) return new Size ( 0, 0 );
            int x = LEFT_MARGIN + ARROW_WIDTH + 8; // <- indent small text slightly more
            Size mzSize = new Size ( this.Width - x - LEFT_MARGIN, 5000 );  // presume RIGHT_MARGIN = LEFT_MARGIN
            return TextRenderer.MeasureText(
                s,
                SmallFont,
                mzSize,
                SmallTextFlags
            );
        }

        public int GetBestHeight()
        {
            //return 40;
            return (TOP_MARGIN * 2) + GetSmallTextSizeF().Height + GetLargeTextSizeF().Height;
        }

        //--------------------------------------------------------------------------------
        protected override void OnPaint ( PaintEventArgs e )
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            LinearGradientMode mode = LinearGradientMode.Vertical;

            Rectangle newRect = new Rectangle ( ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1 );
            Color textColor = SystemColors.WindowText;

            Image img = imgArrowNormal;

            if ( Enabled )
            {
                switch ( buttonState )
                {
                    case ButtonState.Normal:
                        e.Graphics.FillRectangle ( Brushes.White, newRect );
                        if ( base.Focused )
                            e.Graphics.DrawRectangle ( Pens.SkyBlue, newRect );
                        else
                            e.Graphics.DrawRectangle ( Pens.White, newRect );
                        textColor = Color.DarkBlue;
                        break;

                    case ButtonState.MouseOver:
                        using (var brush = new LinearGradientBrush(newRect, Color.White, Color.WhiteSmoke, mode))
                        {
                            e.Graphics.FillRectangle(brush, newRect);
                        }
                        e.Graphics.DrawRectangle ( Pens.Silver, newRect );
                        img = imgArrowHovered;
                        textColor = Color.Blue;
                        break;

                    case ButtonState.Down:
                        using (var brush = new LinearGradientBrush(newRect, Color.WhiteSmoke, Color.White, mode))
                        {
                            e.Graphics.FillRectangle(brush, newRect);
                        }
                        e.Graphics.DrawRectangle ( Pens.DarkGray, newRect );
                        textColor = Color.DarkBlue;
                        break;
                }
            }
            else
            {
                using (var brush = new LinearGradientBrush(newRect, Color.WhiteSmoke, Color.Gainsboro, mode))
                {
                    e.Graphics.FillRectangle(brush, newRect);
                }
                e.Graphics.DrawRectangle ( Pens.DarkGray, newRect );
                textColor = Color.DarkBlue;
            }


            string largetext = this.GetLargeText ( );
            string smalltext = this.GetSmallText ( );

            Size szL = GetLargeTextSizeF ( );

            TextRenderer.DrawText(
                e.Graphics,
                largetext,
                base.Font,
                new Rectangle(
                    LEFT_MARGIN + imgArrowNormal.Width + 5,
                    TOP_MARGIN,
                    szL.Width,
                    szL.Height
                ),
                textColor,
                Color.Transparent,
                LargeTextFlags
            );

            if (smalltext != "")
            {
                Size szS = GetSmallTextSizeF();

                TextRenderer.DrawText(
                    e.Graphics,
                    smalltext,
                    SmallFont,
                    new Rectangle(
                        LEFT_MARGIN + imgArrowNormal.Width + 8,
                        TOP_MARGIN + szL.Height,
                        szS.Width,
                        szS.Height
                    ),
                    textColor,
                    Color.Transparent,
                    SmallTextFlags
                );
            }

            e.Graphics.DrawImage ( img, new Point ( LEFT_MARGIN, TOP_MARGIN + (int)( szL.Height / 2 ) - (int)( img.Height / 2 ) ) );
        }

        //--------------------------------------------------------------------------------
        protected override void OnMouseLeave ( System.EventArgs e )
        {
            buttonState = ButtonState.Normal;
            this.Invalidate ( );
            base.OnMouseLeave ( e );
        }

        //--------------------------------------------------------------------------------
        protected override void OnMouseEnter ( System.EventArgs e )
        {
            buttonState = ButtonState.MouseOver;
            this.Invalidate ( );
            base.OnMouseEnter ( e );
        }

        //--------------------------------------------------------------------------------
        protected override void OnMouseUp ( System.Windows.Forms.MouseEventArgs e )
        {
            buttonState = ButtonState.MouseOver;
            this.Invalidate ( );
            base.OnMouseUp ( e );
        }

        //--------------------------------------------------------------------------------
        protected override void OnMouseDown ( System.Windows.Forms.MouseEventArgs e )
        {
            buttonState = ButtonState.Down;
            this.Invalidate ( );
            base.OnMouseDown ( e );
        }

        //--------------------------------------------------------------------------------
        protected override void OnSizeChanged ( EventArgs e )
        {
            if ( m_autoHeight )
            {
                int h = GetBestHeight ( );
                if ( this.Height != h )
                {
                    this.Height = h;
                    return;
                }
            }
            base.OnSizeChanged ( e );
        }

        //--------------------------------------------------------------------------------
    }
}

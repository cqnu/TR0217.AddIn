using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace AddIn.Gui
{
    [ToolboxBitmap(typeof(TextBox))]
    public class WatermarkTextBox:TextBox
    {
        private string _emptyTextTip;
        private Color _emptyTextTipColor = Color.LightGray;

        private const int WM_PAINT = 0xF;

        [Browsable(true)]
        [Category("apparent")]
        [Description("The Text show in the editbox when the ComboBox is not focused and its DropDownStyle is not DropDownList and its Text is empty.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string EmptyTextTip
        {
            get { return _emptyTextTip; }
            set
            {
                _emptyTextTip = value;
                base.Invalidate();
            }
        }

        [Browsable(true)]
        [Category("apparent")]
        [Description("The color of the EmptyTextTip.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(typeof(Color), "LightGray")]
        public Color EmptyTextTipColor
        {
            get { return _emptyTextTipColor; }
            set
            {
                _emptyTextTipColor = value;
                base.Invalidate();
            }
        }


        protected override void WndProc(ref Message m)
        {

            base.WndProc(ref m);

            if (m.Msg == WM_PAINT)
            {
                WmPaint(ref m);
            }

        }



        private void WmPaint(ref Message m)
        {
            using (Graphics graphics = Graphics.FromHwnd(base.Handle))
            {
                if (Text.Length == 0
                    && !string.IsNullOrEmpty(_emptyTextTip)
                    && !Focused)
                {
                    TextFormatFlags format =
                        TextFormatFlags.EndEllipsis |
                        TextFormatFlags.VerticalCenter;

                    if (RightToLeft == RightToLeft.Yes)
                    {
                        format |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
                    }

                    TextRenderer.DrawText(
                        graphics,
                        _emptyTextTip,
                        Font,
                        base.ClientRectangle,
                        _emptyTextTipColor,
                        format);
                }
            }
        }

    }
}

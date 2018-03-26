using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AddIn.Gui
{
    [ToolboxBitmap(typeof(ComboBox))]
    public class WatermarkComboBox : ComboBox
    {
        private string _emptyTextTip;
        private Color _emptyTextTipColor = Color.DarkGray;
        private IntPtr _editHandle;
        private EditNativeWindow _editNativeWindow;
        private bool _textchanged = false;

        public WatermarkComboBox()
            : base()
        {
        }
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

        protected override void OnTextChanged(EventArgs e)
        {
            _textchanged = true;
            base.OnTextChanged(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (!_textchanged)
                base.OnSelectedIndexChanged(e);
            else
                _textchanged = false;
        }

        internal IntPtr EditHandle
        {
            get { return _editHandle; }
        }

        internal Rectangle EditRect
        {
            get
            {
                if (DropDownStyle != ComboBoxStyle.DropDownList)
                {
                    if (IsHandleCreated && EditHandle != IntPtr.Zero)
                    {
                        RECT rcClient = new RECT();
                        GetWindowRect(EditHandle, ref rcClient);
                        return RectangleToClient(rcClient.Rect);
                    }
                }
                return Rectangle.Empty;
            }
        }

        [DllImport("user32.dll")]
        private static extern bool GetComboBoxInfo(
            IntPtr hwndCombo, ref ComboBoxInfo info);

        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, ref RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(Rectangle rect)
            {
                Left = rect.Left;
                Top = rect.Top;
                Right = rect.Right;
                Bottom = rect.Bottom;
            }

            public Rectangle Rect
            {
                get
                {
                    return new Rectangle(
                        Left,
                        Top,
                        Right - Left,
                        Bottom - Top);
                }
            }

            public Size Size
            {
                get
                {
                    return new Size(Right - Left, Bottom - Top);
                }
            }

            public static RECT FromXYWH(int x, int y, int width, int height)
            {
                return new RECT(x,
                                y,
                                x + width,
                                y + height);
            }

            public static RECT FromRectangle(Rectangle rect)
            {
                return new RECT(rect.Left,
                                 rect.Top,
                                 rect.Right,
                                 rect.Bottom);
            }
        }

        private struct ComboBoxInfo
        {
            public int cbSize;
            public RECT rcItem;
            public RECT rcButton;
            public ComboBoxButtonState stateButton;
            public IntPtr hwndCombo;
            public IntPtr hwndEdit;
            public IntPtr hwndList;
        }

        private enum ComboBoxButtonState
        {
            STATE_SYSTEM_NONE = 0,
            STATE_SYSTEM_INVISIBLE = 0x00008000,
            STATE_SYSTEM_PRESSED = 0x00000008
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ComboBoxInfo cbi = new ComboBoxInfo();
            cbi.cbSize = Marshal.SizeOf(cbi);
            GetComboBoxInfo(base.Handle, ref cbi);
            _editHandle = cbi.hwndEdit;
            if (DropDownStyle != ComboBoxStyle.DropDownList)
            {
                _editNativeWindow = new EditNativeWindow(this);
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            if (_editNativeWindow != null)
            {
                _editNativeWindow.Dispose();
                _editNativeWindow = null;
            }
        }

        private class EditNativeWindow : NativeWindow, IDisposable
        {
            private WatermarkComboBox _owner;
            private const int WM_PAINT = 0xF;

            public EditNativeWindow(WatermarkComboBox owner)
                : base()
            {
                _owner = owner;
                AssignHandle(_owner.EditHandle);
            }

            [DllImport("user32.dll")]
            private static extern IntPtr GetDC(IntPtr ptr);

            [DllImport("user32.dll")]
            private static extern int ReleaseDC(IntPtr hwnd, IntPtr hDC);

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if (m.Msg == WM_PAINT)
                {
                    IntPtr handle = m.HWnd;
                    IntPtr hdc = GetDC(handle);
                    if (hdc == IntPtr.Zero)
                    {
                        return;
                    }
                    try
                    {
                        using (Graphics graphics = Graphics.FromHdc(hdc))
                        {
                            if (_owner.Text.Length == 0
                                && !_owner.Focused
                                && !string.IsNullOrEmpty(_owner.EmptyTextTip))
                            {
                                TextFormatFlags format =
                                    TextFormatFlags.EndEllipsis |
                                    TextFormatFlags.VerticalCenter;

                                if (_owner.RightToLeft == RightToLeft.Yes)
                                {
                                    format |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
                                }

                                TextRenderer.DrawText(
                                    graphics,
                                    _owner.EmptyTextTip,
                                    _owner.Font,
                                    new Rectangle(0, 0, _owner.EditRect.Width, _owner.EditRect.Height),
                                    _owner.EmptyTextTipColor,
                                    format);
                            }
                        }
                    }
                    finally
                    {
                        ReleaseDC(handle, hdc);
                    }
                }
            }

            #region IDisposable 成员

            public void Dispose()
            {
                ReleaseHandle();
                _owner = null;
            }

            #endregion
        }
    }
}

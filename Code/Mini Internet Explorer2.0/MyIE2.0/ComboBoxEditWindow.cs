// Copyright (c) 2010 2011 2012 Ã∆»Ò(also tr0217)
// mailto:tr0217@163.com
// The earliest release time: 2011-01-09
// Last modification time: 
// Accompanying files of necessity:
//        *
//        *
//
// This file and the accompanying files of this project may be freely used provided the following
// conditions are met:
//        * This copyright statement is not removed or modified.
//        * The code is not sold in uncompiled form.  (Release as a compiled binary which is part
//          of an application is fine)
//        * The design, code, or compiled binaries are not "Re-branded".
//        
// Optional:
//        * Redistributions in binary form must reproduce the above copyright notice.
//        * I receive a fully licensed copy of the product (regardless of whether the product is
//          is free, shrinkwrap, or commercial).  This is optional, though if you release products
//          which use code I've contributed to, I would appreciate a fully licensed copy.
//
// In addition, you may not:
//        * Publicly release modified versions of the code or publicly release works derived from
//          the code without express written authorization.
//
// NO WARRANTY:
//        THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
//        ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
//        WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
//        IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
//        INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
//        NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
//        OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//        WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
//        ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
//        OF SUCH DAMAGE.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace TR0217.ControlEx
{
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

        public static RECT Create(int x, int y, int width, int height)
        {
            return new RECT(x,
                            y,
                            x + width,
                            y + height);
        }

        public static RECT Create(Rectangle rect)
        {
            return new RECT(rect.Left,
                             rect.Top,
                             rect.Right,
                             rect.Bottom);
        }
    }

    internal struct ComboBoxInfo
    {
        public int cbSize;
        public RECT rcItem;
        public RECT rcButton;
        public ComboBoxButtonState stateButton;
        public IntPtr hwndCombo;
        public IntPtr hwndEdit;
        public IntPtr hwndList;
    }

    internal enum ComboBoxButtonState
    {
        STATE_SYSTEM_NONE = 0,
        STATE_SYSTEM_INVISIBLE = 0x00008000,
        STATE_SYSTEM_PRESSED = 0x00000008
    }

    internal class ComboBoxEditWindow : NativeWindow, IDisposable
    {
        private ComboBox _owner;
        private ComboBoxInfo _cbi;
        private Image _image;
        private Color _emptyTextTipColor = Color.DarkGray;
        private string _emptyTextTip;

        public string EmptyTextTip
        {
            get { return _emptyTextTip; }
            set
            {
                _emptyTextTip = value;
                _owner.Invalidate();
            }
        }


        public Color EmptyTextTipColor
        {
            get { return _emptyTextTipColor; }
            set
            {
                _emptyTextTipColor = value;
                _owner.Invalidate();
            }
        }

        private bool _rightToLeft = false;

        public bool RightToLeft
        {
            get { return _rightToLeft; }
            set { _rightToLeft = value; }
        }

        private const int EC_LEFTMARGIN = 0x1;
        private const int EC_RIGHTMARGIN = 0x2;
        private const int WM_PAINT = 0xF;
        private const int WM_SETCURSOR = 0x20;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_CHAR = 0x102;
        private const int WM_GETTEXTLENGTH = 0xe;
        private const int WM_GETTEXT = 0xd;
        private const int EM_SETMARGINS = 0xD3;
        private const int WM_MOUSEMOVE = 0x200;   

        public Image Image
        {
            get { return _image; }
            set 
            { 
                _image = value;
                if (_image != null)
                    this.SetMargin(_image.Width+2);
                else
                    this.SetMargin(1);
            }
        }

        public ComboBoxEditWindow(ComboBox owner)
            : base()
        {
            _owner = owner;
            _cbi = new ComboBoxInfo();
            _cbi.cbSize = Marshal.SizeOf(_cbi);
            GetComboBoxInfo(_owner.Handle, ref _cbi);

            if (!this.Handle.Equals(IntPtr.Zero))
                this.ReleaseHandle();
            this.AssignHandle(_cbi.hwndEdit);
        }

        [DllImport("user32.dll")]
        private static extern bool GetComboBoxInfo(IntPtr hwndCombo, ref ComboBoxInfo info);

        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hwnd, IntPtr hDC);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void SetMargin(int margin)
        {
            if (_owner == null)
                return;

            //if (_owner.RightToLeft == RightToLeft.Inherit)
            //{
            //    if (_owner.Parent.RightToLeft == RightToLeft.Yes)
            //        _rightToLeft = true;
            //}
            //else if (_owner.RightToLeft == RightToLeft.Yes)
            //    _rightToLeft = true;
            //else
            //    _rightToLeft = false;

            _margin = margin;
            // If combobox is RightToLeft Aligned, margin is set to the right of the combobox.
            if (_rightToLeft)
            {
                // To set the right margin, the lparam's hiword is taken as the margin.
                // hence,to shift the margin to hiword, we multiply the it with 65536.
                SendMessage(this.Handle, EM_SETMARGINS, EC_RIGHTMARGIN, margin * 65536);
                SendMessage(this.Handle, EM_SETMARGINS, EC_LEFTMARGIN, 0);
            }// If combobox is LeftToRight Aligned, margin is set to the left of the combobox.
            else
            {
                // To set the left margin, the lparam's loword is taken as the margin.
                SendMessage(this.Handle, EM_SETMARGINS, EC_LEFTMARGIN, margin);
                SendMessage(this.Handle, EM_SETMARGINS, EC_RIGHTMARGIN, 0);
            }
        }

        /// <summary>
        /// Whenever the textbox is repainted, we have to draw the image.
        /// </summary>
        private void DrawImage()
        {
            if (_image != null)
            {
                // Gets a GDI drawing surface from the textbox.
                using (Graphics graphics = Graphics.FromHwnd(this.Handle))
                {
                    if (_rightToLeft)
                        graphics.DrawImage(_image, graphics.VisibleClipBounds.Width - _image.Width, 0);
                    else
                        graphics.DrawImage(_image, 0, 0);

                    graphics.Flush();
                    graphics.Dispose();
                }
            }
        }

        int _margin = 0;
        private void DrawEmptyTipText()
        {
            if (_owner.DropDownStyle != ComboBoxStyle.DropDownList)
            {
                if (_owner.Text.Length == 0
                    && !_owner.Focused
                    && !string.IsNullOrEmpty(_emptyTextTip))
                {
                    StringFormat format = new StringFormat();
                    //format.FormatFlags = StringFormatFlags.NoWrap;
                    format.FormatFlags = StringFormatFlags.NoClip;
                    if (_rightToLeft)
                        format.Alignment = StringAlignment.Far;

                    using (Graphics graphics = Graphics.FromHwnd(this.Handle))
                    {
                        Brush brush =  new SolidBrush(_emptyTextTipColor);
                        Rectangle rect = new Rectangle(
                            (int)graphics.VisibleClipBounds.X + _margin,
                            (int)graphics.VisibleClipBounds.Y,
                            (int)graphics.VisibleClipBounds.Width - _margin - _margin,
                            (int)graphics.VisibleClipBounds.Height);

                        graphics.DrawString(_emptyTextTip, _owner.Font, brush, rect, format);
                        brush.Dispose();

                        graphics.Flush();
                        graphics.Dispose();
                    }
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_PAINT:
                    this.DrawEmptyTipText();
                    this.DrawImage();
                    break;
                case WM_LBUTTONDOWN:
                case WM_KEYDOWN:
                case WM_KEYUP:
                case WM_CHAR:
                case WM_GETTEXTLENGTH:
                case WM_GETTEXT:
                case WM_MOUSEMOVE:
                    this.DrawImage();
                    break;
            }
        }

        #region IDisposable ≥…‘±

        public void Dispose()
        {
            ReleaseHandle();
            _owner = null;
        }

        #endregion
    }
}

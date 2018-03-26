// Copyright (c) 2010 2011 2012 ÌÆÈñ(also tr0217)
// mailto:tr0217@163.com
// The earliest release time: 2010-12-08
// Last modification time: 2011-01-09
// Accompanying files of necessity:
//        * ComboBoxImageItem.cs
//        * ListItem.cs
//        * ComboBoxEditWindow.cs
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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Drawing.Design;

namespace TR0217.ControlEx
{
    [ToolboxBitmap(typeof(ComboBoxImage), "ComboBoxImage.bmp")]
    public class ComboBoxImage : ComboBox
    {
        #region field
        private Image _defaultImage = null;
        private ImageList _imageList = null;
        private object nextSelItem = null;
        private DashStyle _separatorStyle;
        private Color _separatorColor;
        private int _separatorWidth;
        private int _separatorMargin;
        private ComboBoxEditWindow _eidtWin;
        private int _levelAlign = 8;
        private string _emptyTextTip;
        private Color _emptyTextTipColor = Color.DarkGray;
        private bool _rightToLeft = false;

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
        [DefaultValue(typeof(Color), "DarkGray")]
        public Color EmptyTextTipColor
        {
            get { return _emptyTextTipColor; }
            set
            {
                _emptyTextTipColor = value;
                base.Invalidate();
            }
        }

        [Browsable(true)]
        [Category("Image")]
        [Description("The associated iamgelist, to provide image show in droplist an Combobox.")]
        public ImageList ImageList
        {
            get
            {
                return _imageList;
            }
            set
            {
                _imageList = value;
                //if (_imageList != null)
                //    this.Font = new Font(this.Font.FontFamily,_imageList.ImageSize.Height-4,GraphicsUnit.Pixel);
            }
        }


        [Browsable(true)]
        [Category("Image")]
        [Description("The default image show in droplist an Combobox.")]
        public Image DefaultImage
        {
            get
            {
                return _defaultImage;
            }
            set
            {
                _defaultImage = value;
            }
        }
 
        [Browsable(true)]
        [Category("Separator")]
        [Description("Gets or sets the Separator Style")]
        public DashStyle SeparatorStyle
        {
            get 
            { 
                return _separatorStyle;
            }
            set 
            { 
                _separatorStyle = value; 
            }
        }

        [Browsable(true)]
        [Category("Separator")]
        [Description("Gets or sets the Separator Color")]
        public Color SeparatorColor
        {
            get 
            { 
                return _separatorColor; 
            }
            set 
            { 
                _separatorColor = value;
            }
        }

        [Browsable(true)]
        [Category("Separator")]
        [Description("Gets or sets the Separator Width")]
        public int SeparatorWidth
        {
            get 
            { 
                return _separatorWidth; 
            }
            set 
            {
                _separatorWidth = value; 
            }
        }
        [Category("Separator")]
        [Description("Gets or sets the Separator Margin")]
        public int SeparatorMargin
        {
            get 
            {
                return _separatorMargin;
            }
            set 
            { 
                _separatorMargin = value;
            }
        }

        [Browsable(false)]
        public new DrawMode DrawMode
        {
            get { return base.DrawMode; }
            set { ;}
        }

        [Browsable(true)]
        [Category("apparent")]
        [Description("the level alignment of a level in pixel.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int LevelAlign
        {
            get { return _levelAlign; }
            set { _levelAlign = value; }
        }

        [Category("Behavior")]
        [Description("The collection of items in the ImageComboBox.")]
        [Localizable(true)]
        [MergableProperty(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new ComboBox.ObjectCollection Items
        {
            get { return base.Items; }
        }

        #endregion

        public ComboBoxImage():base()
        {
            base.DrawMode = DrawMode.OwnerDrawVariable;
            _imageList = new ImageList();
            _separatorStyle = DashStyle.Dash;
            _separatorColor = Color.Black;
            _separatorWidth = 1;
            _separatorMargin = 2;
            _levelAlign = this.Font.Height / 2 + 1;
        }

        /// <summary>
        /// When the RightToLeft property is changed the margin of the TextBox also should be changed accordingly.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);

            this.CalcRightToLeft();
            if (_eidtWin != null)
            {
                _eidtWin.RightToLeft = _rightToLeft;
            }
        }

        private void CalcRightToLeft()
        {
            if (base.RightToLeft == RightToLeft.Inherit)
            {
                if (base.Parent.RightToLeft == RightToLeft.Yes)
                    _rightToLeft = true;
            }
            else if (base.RightToLeft == RightToLeft.Yes)
                _rightToLeft = true;
            else
                _rightToLeft = false;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (base.DropDownStyle != ComboBoxStyle.DropDownList)
            {
                _eidtWin = new ComboBoxEditWindow(this);
                _eidtWin.EmptyTextTip = _emptyTextTip;
                _eidtWin.EmptyTextTipColor = _emptyTextTipColor;
                _eidtWin.RightToLeft = _rightToLeft;
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            if (_eidtWin != null)
            {
                _eidtWin.Dispose();
                _eidtWin = null;
            }
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            base.OnMeasureItem(e);
            //e.ItemWidth = this.Width;
            if (e.Index >= 0)
            {
                if (this.Items[e.Index].GetType() == typeof(ComboBoxImageItem))
                {
                    //e.ItemHeight = _imageList.ImageSize.Height;
                    //get item to draw
                    ComboBoxImageItem item = (ComboBoxImageItem)this.Items[e.Index];
                    if (item.IsSeparator)
                    {
                        e.ItemHeight = 3 + _separatorWidth;
                        return;
                    }
                }
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (this.SelectedIndex < 0)
                return;
            ComboBoxImageItem ci = this.Items[this.SelectedIndex] as ComboBoxImageItem;
            if (ci != null)
            {
                if (ci.IsSeparator)
                    this.SelectedItem = nextSelItem;
                else
                    base.OnSelectedIndexChanged(e);
                if (_eidtWin != null)
                    _eidtWin.Image = this.PrepareImage(ci);
            }
            else
            {
                if (_eidtWin != null)
                    _eidtWin.Image = null;
                nextSelItem = this.Items[this.SelectedIndex];
                base.OnSelectedIndexChanged(e);
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            ComboBoxImageItem ci;
            int nextSelIndex;
            int wheelOrientation;

            if (e.Delta > 0)
            {
                nextSelIndex = this.SelectedIndex - 1;
                wheelOrientation = -1;
            }
            else
            {
                nextSelIndex = this.SelectedIndex + 1;
                wheelOrientation = 1;
            }

            if ((nextSelIndex > -1) && (nextSelIndex < this.Items.Count))
            {
                ci = this.Items[nextSelIndex] as ComboBoxImageItem;
                while ((ci != null) && (ci.IsSeparator))
                {
                    nextSelIndex += wheelOrientation;
                    if ((nextSelIndex > -1) && (nextSelIndex < this.Items.Count))
                    {
                        ci = this.Items[nextSelIndex] as ComboBoxImageItem;
                    }
                    else
                    {
                        return;
                    }
                }
                nextSelItem = this.Items[nextSelIndex];
            }
        }

        // customized drawing process
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            int strOffset = e.Bounds.Height - e.Font.Height;
            StringFormat format = new StringFormat();
            //format.FormatFlags = StringFormatFlags.NoWrap;
            format.FormatFlags = StringFormatFlags.NoClip;
            if (_rightToLeft)
                format.Alignment = StringAlignment.Far;

            RectangleF itemRect = new RectangleF(e.Bounds.X, e.Bounds.Top + strOffset, e.Bounds.Width, e.Bounds.Height);

            // check if it is an item from the Items collection
            if (e.Index < 0)
            {
                // draw background & focus rect
                e.DrawBackground();
                e.DrawFocusRectangle();
                // not an item, draw the text (indented)
                e.Graphics.DrawString(this.Text, e.Font, new SolidBrush(e.ForeColor), itemRect, format);
            }
            else
            {
                // check if item is an ImageComboItem
                if (this.Items[e.Index].GetType() == typeof(ComboBoxImageItem))
                {
                    // get item to draw
                    ComboBoxImageItem item = (ComboBoxImageItem)base.Items[e.Index];

                    if (item.IsSeparator)
                    {
                        Pen pen = new Pen(_separatorColor, _separatorWidth);
                        pen.DashStyle = _separatorStyle;

                        e.Graphics.DrawLine(pen,e.Bounds.Left + _separatorMargin, e.Bounds.Top + 2, e.Bounds.Right - _separatorMargin,e.Bounds.Top + 2);
                        return;
                    }
                    // draw background & focus rect
                    e.DrawBackground();
                    e.DrawFocusRectangle();
                    // get forecolor & font
                    Color forecolor = (item.TextColor != Color.FromKnownColor(KnownColor.Transparent)) ? item.TextColor : e.ForeColor;
                    Font font = item.TextFont == null ? e.Font : item.TextFont;
                    Image image = this.PrepareImage(item);

                    //  no image
                    if (!item.BaseStyle && image != null)
                    {
                        int offset = 0;
                        if ((e.State & DrawItemState.ComboBoxEdit) != DrawItemState.ComboBoxEdit)
                            offset = item.Level* _levelAlign;

                        Rectangle imageRect;
                        int y = e.Bounds.Top + strOffset;
                        if (_rightToLeft)
                        {
                            int x = e.Bounds.Right - this.Font.Height - 2 - offset;

                            itemRect = new RectangleF(e.Bounds.X, y, x - e.Bounds.X, e.Bounds.Height);
                            imageRect = new Rectangle(x, e.Bounds.Top, e.Bounds.Height, e.Bounds.Height);
                        }
                        else
                        {
                            int x = e.Bounds.Left + this.Font.Height + 2 + offset;
                            itemRect = new RectangleF(x, y, e.Bounds.Width - x, e.Bounds.Height - y);
                            imageRect = new Rectangle(e.Bounds.Left + offset, e.Bounds.Top, e.Bounds.Height, e.Bounds.Height);
                        }

                        // draw image, then draw text next to it
                        e.Graphics.DrawImage(image, imageRect);
                        e.Graphics.DrawString(item.Text, font, new SolidBrush(e.ForeColor), itemRect, format);
                    }
                    else
                        // draw text (indented)
                        e.Graphics.DrawString(item.Text, font, new SolidBrush(e.ForeColor), itemRect, format);
                }
                else
                {
                    // draw background & focus rect
                    e.DrawBackground();
                    e.DrawFocusRectangle();
                    // it is not an ImageComboItem, draw it
                    e.Graphics.DrawString(base.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), itemRect, format);
                }
            }
        }

        private Image PrepareImage(ComboBoxImageItem item)
        {
            Image image = null;

            if (item.Image == null)
            {
                if ((item.ImageIndex >= 0) && (item.ImageIndex < _imageList.Images.Count))
                {
                    image = _imageList.Images[item.ImageIndex];
                }
                else if (!string.IsNullOrEmpty(item.ImageKey))
                {
                    image = _imageList.Images[item.ImageKey];
                }
                else
                {
                    image = _defaultImage;
                }
            }
            else
            {
                image = item.Image;
            }

            return image;
        }//end of last method
    }//end of class
}//end of namespace


// Copyright (c) 2010 2011 2012 ÌÆÈñ(also tr0217)
// mailto:tr0217@163.com
// The earliest release time: 2010-12-08
// Last modification time: 2011-01-09
// Accompanying files of necessity:
// 
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
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace TR0217.ControlEx
{
    public class ListItem
    {
        #region field
        private int _level = 0;
        private int _imageIndex = -1;
        private string _imageKey;
        private string _text = null;
        private Color _textColor = Color.FromKnownColor(KnownColor.Transparent);
        private Font _textFont = null;
        private object _tag = null;
        private Image _image;


        [Browsable(true)]
        [Category("apparent")]
        [Description("the display level of this item.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }

        [Browsable(true)]
        [Category("apparent")]
        [Description("the iamge index of this item in imagelist. If this property be correctly set, ImageIndex will cut no ice.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
            }
        }

        [Browsable(true)]
        [Category("apparent")]
        [Description("the iamge index of this item in imagelist ")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public int ImageIndex
        {
            get
            {
                return _imageIndex;
            }
            set
            {
                _imageIndex = value;
            }
        }

        [Browsable(true)]
        [Category("apparent")]
        [Description("the iamge key of this item in imagelist ")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string ImageKey
        {
            get { return _imageKey; }
            set { _imageKey = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        [Browsable(true)]
        [Category("apparent")]
        [Description("the text color")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color TextColor
        {
            get
            {
                return _textColor;
            }
            set
            {
                _textColor = value;
            }
        }

        [Browsable(true)]
        [Category("apparent")]
        [Description("the text font")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Font TextFont
        {
            get
            {
                return _textFont;
            }
            set
            {
                _textFont = value;
            }
        }

        [Browsable(false)]
        public object Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }

        #endregion

        #region method

        public ListItem()
        {
            _image = null;
        }

        public ListItem(string text)
        {
            _text = text;
            _image = null;
        }

        public ListItem(string text, int imageIndex)
        {
            _text = text;
            _imageIndex = imageIndex;
            _image = null;
        }

        public ListItem(string text, int imageIndex, Color textColor)
        {
            _text = text;
            _imageIndex = imageIndex;
            _image = null;
            _textColor = textColor;
        }

        public ListItem(string text, int imageIndex, Color textColor, Font textFont)
        {
            _text = text;
            _imageIndex = imageIndex;
            _image = null;
            _textColor = textColor;
            _textFont = textFont;
        }

        public override string ToString()
        {
            if (_text == null)
                return base.ToString();
            else
                return _text;
        }

        #endregion
    }
}

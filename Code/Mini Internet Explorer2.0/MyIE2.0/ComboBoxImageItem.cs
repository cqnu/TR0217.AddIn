// Copyright (c) 2010 2011 2012 Ã∆»Ò(also tr0217)
// mailto:tr0217@163.com
// The earliest release time: 2010-12-08
// Last modification time: 2011-01-09
// Accompanying files of necessity:
//        * ListItem.cs
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
using System.Runtime.Serialization;

namespace TR0217.ControlEx
{
    [ToolboxItem(false)]
    [DesignTimeVisible(false)]
    [Serializable()]
    public class ComboBoxImageItem : ListItem ,ISerializable
    {
        #region field
        private bool _isSeparator = false;
        private bool _baseStyle = false;



        [Browsable(true)]
        [Category("apparent")]
        [Description("is this item shown as separator.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool IsSeparator
        {
            get
            {
                return _isSeparator;
            }
            set
            {
                _isSeparator = value;
            }
        }

        [Browsable(true)]
        [Category("apparent")]
        [Description("do not display image and take an original outlook")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool BaseStyle
        {
            get
            {
                return _baseStyle;
            }
            set
            {
                _baseStyle = value;
            }
        }

        #endregion

        #region method

        public ComboBoxImageItem()
        {
        }

        public ComboBoxImageItem(string text)
            : base(text)
        {
        }

        public ComboBoxImageItem(string text, int imageIndex)
            :base(text,imageIndex)
        {
        }

        public ComboBoxImageItem(string text, int imageIndex, Color textColor)
            :base(text,imageIndex,textColor)
        {
        }

        public ComboBoxImageItem(string text, int imageIndex, Color textColor, Font textFont)
            :base(text,imageIndex,textColor,textFont)
        {
        }

        public ComboBoxImageItem(string text, bool baseStyle, int imageIndex)
            :base(text,imageIndex)
        {
            _baseStyle = baseStyle;
        }

        public ComboBoxImageItem(string text, bool baseStyle, int imageIndex, Color textColor)
            :base(text,imageIndex,textColor)
        {
            _baseStyle = baseStyle;
        }

        public ComboBoxImageItem(string text, bool baseStyle, int imageIndex, Color textColor, Font textFont)
            :base(text,imageIndex,textColor,textFont)
        {
            _baseStyle = baseStyle;
        }

        #endregion

        #region ISerializable ≥…‘±

        public ComboBoxImageItem(SerializationInfo info, StreamingContext context) 
		{
            this.Text = (string)info.GetValue("Text", typeof(string));
            this.TextColor = (Color)info.GetValue("TextColor", typeof(Color));
            this.TextFont = (Font)info.GetValue("TextFont", typeof(Font));
            this.Tag = info.GetValue("Tag", typeof(Object));
            this.Image = (Image)info.GetValue("Image", typeof(Image));
            this.ImageIndex = (int)info.GetValue("ImageIndex", typeof(int));
            this.ImageKey = (string)info.GetValue("ImageKey", typeof(string));
            this.IsSeparator = (bool)info.GetValue("IsSeparator", typeof(bool));
            this.ImageIndex = (int)info.GetValue("BaseStyle", typeof(int));
            this.Level = (int)info.GetValue("Level", typeof(int));
		}

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Text", this.Text);
            info.AddValue("TextColor", this.TextColor);
            info.AddValue("TextFont", this.TextFont);
            info.AddValue("Tag", this.Tag);
            info.AddValue("Image", this.Image);
            info.AddValue("ImageIndex", this.ImageIndex);
            info.AddValue("ImageKey", this.ImageKey);
            info.AddValue("IsSeparator", this.IsSeparator);
            info.AddValue("BaseStyle", this.ImageIndex);
            info.AddValue("Level", this.Level);
        }

        #endregion
    }
}

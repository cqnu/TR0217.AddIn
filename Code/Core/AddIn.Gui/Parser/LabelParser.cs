using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using AddIn.Core;
using AddIn.Gui.Loader;

namespace AddIn.Gui.Parser
{
    class LabelParser:CmdParser
    {
        static private int _num = -1;
        private string _image;
        private ContentAlignment _textAlign;
        private ContentAlignment _imageAlign;


        public LabelParser(UiLoader uiLoader)
            : base(uiLoader)
        {
            _num++;
            _uiElemType = UiElemType.Label;
            Name = "tsl" + _num.ToString();
            _text = "Label";
            _paramProvider = Name;
            _image = string.Empty;
            _textAlign = ContentAlignment.MiddleLeft;
            _imageAlign = ContentAlignment.MiddleLeft;
        }

        //[CategoryAttribute("基本属性")]
        //[ DescriptionAttribute("文本对齐方式。")]
        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("The alignment of the text that will be displayed on the item.")]
        public ContentAlignment TextAlign
        {
            get { return _textAlign; }
            set 
            { 
                _textAlign = value;
                (this.UiElem as ToolStripLabel).TextAlign = _textAlign;
            }
        }

        //[CategoryAttribute("基本属性")]
        //[ DescriptionAttribute("图片对齐方式。")]
        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("The alignment of the image that will be displayed on the item.")] 
        public ContentAlignment ImageAlign
        {
            get { return _imageAlign; }
            set 
            { 
                _imageAlign = value;
                (this.UiElem as ToolStripLabel).ImageAlign = _imageAlign;
            }
        }

        //[CategoryAttribute("基本属性")]
        //[ DescriptionAttribute("显示在控件上的图片文件的路径。")]
        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("Path of image file shown on the control.")]
        [Editor(typeof(ImagePathEditor), typeof(System.Drawing.Design.UITypeEditor))] 
        public string Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                Image img = null;
                if (_image != string.Empty)
                {
                    string imgPath = _image;
                    if (_image.StartsWith("."))
                        imgPath = Application.StartupPath + _image.Substring(1);

                    try
                    {
                        Bitmap tempBmp = new Bitmap(imgPath);
                        img = new Bitmap(tempBmp);
                        tempBmp.Dispose();
                    }
                    catch
                    {
                        MessageBox.Show("图像路径不合法", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                (this.UiElem as ToolStripLabel).Image = img;
            }
        }

        public override object Clone()
        {
            LabelParser uep = new LabelParser(UiLoader);
            uep.TextAlign = _textAlign;
            uep.ImageAlign = _imageAlign;
            uep.Image = _image;
            uep.Text = _text;
            uep.ToolTipText = _toolTipText;
            uep.Enabled = _enabled;
            uep.Visible = _visible;
            uep.UpdateEvent = _updateEvent;
            uep.Service = _service;
            uep.Function = _function;
            uep.Parameter = _parameter;
            uep.ParamProvider = _paramProvider;
            uep.Alignment = _alignment;
            uep.AutoSize = _autoSize;

            uep.UiElemParserList = new List<UiElemParser>(this.UiElemParserList);

            return uep;
        }

        public override void FromXmlNode(XmlNode node)
        {
            XmlElement elem = node as XmlElement;
            try
            {
                base.FromXmlNode(node);
                _textAlign = (ContentAlignment)Enum.Parse(typeof(ContentAlignment), elem.GetAttribute("textAlign"));
                _imageAlign = (ContentAlignment)Enum.Parse(typeof(ContentAlignment), elem.GetAttribute("imageAlign"));

                XmlNode n1 = UiElemParser.FindChildXmlNode(node, "image");
                _image = n1.InnerText;
            }
            catch { }

            try
            {
                int num = int.Parse(Name.Substring(4));
                if (num > _num)
                    _num = num;
            }
            catch { }

            _uiElem = this.CreateUiElem();
        }

        public override XmlElement ToXmlNode(XmlDocument doc, string name = null)
        {
            XmlElement elem = base.ToXmlNode(doc, "label");

            elem.SetAttribute("textAlign", _textAlign.ToString());
            elem.SetAttribute("imageAlign", _imageAlign.ToString());

            XmlElement elemService = doc.CreateElement("image");
            elemService.InnerText = _image;
            elem.AppendChild(elemService);

            return elem;
        }

        protected override object CreateUiElem()
        {
            Image img = null;
            if (_image != string.Empty)
            {
                string imgPath = _image;
                if (_image.StartsWith("."))
                    imgPath = Application.StartupPath + _image.Substring(1);
                try
                {
                    Bitmap tempBmp = new Bitmap(imgPath);
                    img = new Bitmap(tempBmp);
                    tempBmp.Dispose();
                }
                catch (Exception e)
                {
                    AppFrame.FrameLogger.Error("载入图像失败！请确认配置界面时指定了正确的图像路径，或者图像是否存在。" + "界面元素文本：" + _text, e);
                }
            }

            ToolStripLabel tsl = new ToolStripLabel();
            tsl.ToolTipText = _toolTipText;
            tsl.Enabled = _enabled;
            tsl.Visible = _visible;
            tsl.Image = img;
            tsl.Name = Name;
            tsl.Text = _text;
            tsl.TextAlign = _textAlign;
            tsl.ImageAlign = _imageAlign;
            tsl.Alignment = _alignment;
            tsl.AutoSize = _autoSize;

            return tsl;
        }
    }
}

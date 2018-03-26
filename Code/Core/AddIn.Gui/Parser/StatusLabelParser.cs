using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using System.ComponentModel;
using AddIn.Core;
using AddIn.Gui.Loader;

namespace AddIn.Gui.Parser
{
    class StatusLabelParser:CmdParser
    {
        static private int _num;
        private bool _spring;
        private string _image;
        private ContentAlignment _textAlign;
        private ContentAlignment _imageAlign;


        public StatusLabelParser(UiLoader uiLoader)
            : base(uiLoader)
        {
            _num++;
            _uiElemType = UiElemType.StatusLabel;
            Name = "tssl" + _num.ToString();
            _text = "StatusLabel";
            _paramProvider = Name;
            _textAlign = ContentAlignment.MiddleLeft;
            _imageAlign = ContentAlignment.MiddleLeft;
            _image = string.Empty;
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
               (this.UiElem as ToolStripStatusLabel).TextAlign = _textAlign;
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
               (this.UiElem as ToolStripStatusLabel).ImageAlign = _imageAlign;
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
                (this.UiElem as ToolStripStatusLabel).Image = img;
            }
        }

        //[CategoryAttribute("基本属性")]
        //[ DescriptionAttribute("是否填充父容器的剩余空间")]
        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("Whether fill the container control's remian space.")]
        public bool Spring
        {
            get { return _spring; }
            set 
            { 
                _spring = value;
                (this.UiElem as ToolStripStatusLabel).Spring = _spring;
            }
        }

        public override object Clone()
        {
            StatusLabelParser uep = new StatusLabelParser(UiLoader);
            uep.Spring = _spring;
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
                _spring = bool.Parse((node as XmlElement).GetAttribute("spring"));
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
            XmlElement elem = base.ToXmlNode(doc, "statusLabel");

            elem.SetAttribute("spring", _spring.ToString());
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

            ToolStripStatusLabel tssl = new ToolStripStatusLabel();
            tssl.Spring = _spring;
            tssl.ToolTipText = _toolTipText;
            tssl.Enabled = _enabled;
            tssl.Visible = _visible;
            tssl.Image = img;
            tssl.Name = Name;
            tssl.Text = _text;
            tssl.TextAlign = _textAlign;
            tssl.ImageAlign = _imageAlign;
            tssl.Alignment = _alignment;
            tssl.AutoSize = _autoSize;

            return tssl;
        }
    }
}

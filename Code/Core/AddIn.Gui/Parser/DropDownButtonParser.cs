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
    class DropDownButtonParser:CmdParser
    {
        static private int _num = -1;
        private ToolStripItemDisplayStyle _displayStyle = ToolStripItemDisplayStyle.Image;
        private string _image;

        public DropDownButtonParser(UiLoader uiLoader)
            : base(uiLoader)
        {
            _num++;
            _uiElemType = UiElemType.DropDownButton;
            Name = "tsddb" + _num.ToString();
            _text = "DropDownButton";
            _paramProvider = Name;
            _image = string.Empty;
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
                (this.UiElem as ToolStripDropDownButton).Image = img;
            }
        }

        //[CategoryAttribute("基本属性")]
        //[ DescriptionAttribute("按钮的显示样式。")]
        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("Display style of the ToolStripButton.")]
        public ToolStripItemDisplayStyle DisplayStyle
        {
            get
            {
                return _displayStyle;
            }
            set
            {
                _displayStyle = value;
                (this.UiElem as ToolStripDropDownButton).DisplayStyle = _displayStyle;
            }
        }

        public override object Clone()
        {
            DropDownButtonParser uep = new DropDownButtonParser(UiLoader);
            uep.DisplayStyle = _displayStyle;
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
                _displayStyle = (ToolStripItemDisplayStyle)Enum.Parse(typeof(ToolStripItemDisplayStyle), elem.GetAttribute("displayStyle"));
               
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

            XmlNode n = UiElemParser.FindChildXmlNode(node, "subItems");
            base.ParseSubItems((this.UiElem as ToolStripDropDownButton).DropDownItems, n,_text);
        }

        public override XmlNode ToXmlNode(XmlDocument doc)
        {
            XmlElement elem = base.ToXml(doc, "dropDownButton");
            elem.SetAttribute("displayStyle", _displayStyle.ToString());

            XmlElement elemService = doc.CreateElement("image");
            elemService.InnerText = _image;
            elem.AppendChild(elemService);
            
            XmlElement elemSubElem = doc.CreateElement("subItems");
            foreach (UiElemParser up in base.UiElemParserList)
            {
                elemSubElem.AppendChild(up.ToXmlNode(doc));
            }
            elem.AppendChild(elemSubElem);

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

            ToolStripDropDownButton tsddb = new ToolStripDropDownButton();
            tsddb.ToolTipText = _toolTipText;
            tsddb.Text = _text;
            tsddb.Image = img;
            tsddb.Enabled = _enabled;
            tsddb.Visible = _visible;
            tsddb.Name = Name;
            tsddb.DisplayStyle = _displayStyle;
            tsddb.Alignment = _alignment;
            tsddb.AutoSize = _autoSize;

            return tsddb;
        }
    }
}

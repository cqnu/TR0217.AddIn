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
    class ButtonParser:CmdParser
    {
        static private int _num = -1;
        private ToolStripItemDisplayStyle _displayStyle = ToolStripItemDisplayStyle.Image;
        private bool _checked;
        private bool _checkOnClick;
        private string _image;

        public ButtonParser(UiLoader uiLoader)
            : base(uiLoader)
        {
            _num++;
            _uiElemType = UiElemType.Button;
            Name = "tsb" + _num.ToString();
            _text = "Button";
            _paramProvider = Name;
            _checked = false;
            _checkOnClick = false;
            _image = string.Empty;
        }

        //[CategoryAttribute("基本属性")]
        //[ DescriptionAttribute("表示是否启用该控件。")]
        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("Indicating whether the control is is in state of selected.")]
        public bool Checked
        {
            get
            {
                return _checked;
            }
            set
            {
                _checked = value;
                (this.UiElem as ToolStripButton).Checked = _checked;
            }
        }

        //[CategoryAttribute("基本属性")]
        //[ DescriptionAttribute("控件的选中状态是否在点击后改变。")]
        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("Whether the control's CheckState change after clicking.")]
        public bool CheckOnClick
        {
            get
            {
                return _checkOnClick;
            }
            set
            {
                _checkOnClick = value;
                (this.UiElem as ToolStripButton).CheckOnClick = _checkOnClick;
            }
        }

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
                    if(_image.StartsWith("."))
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
                (this.UiElem as ToolStripButton).Image = img;
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
                (this.UiElem as ToolStripButton).DisplayStyle = _displayStyle;
            }
        }

        public override void FromXmlNode(XmlNode node)
        {
            XmlElement elem = node as XmlElement;
            try
            {
                base.FromXmlNode(node);
                _displayStyle = (ToolStripItemDisplayStyle)Enum.Parse(typeof(ToolStripItemDisplayStyle), elem.GetAttribute("displayStyle"));
                _checked = bool.Parse(elem.GetAttribute("checked"));
                _checkOnClick = bool.Parse(elem.GetAttribute("checkOnClick"));

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
            XmlElement elem = base.ToXmlNode(doc, "button");

            elem.SetAttribute("displayStyle", _displayStyle.ToString());
            elem.SetAttribute("checked", _checked.ToString());
            elem.SetAttribute("checkOnClick", _checkOnClick.ToString());

            XmlElement elemService = doc.CreateElement("image");
            elemService.InnerText = _image;
            elem.AppendChild(elemService);

            return elem;
        }

        public override object Clone()
        {
            ButtonParser uep = new ButtonParser(UiLoader);
            
            uep.DisplayStyle = _displayStyle;
            uep.Image = _image;
            uep.Text = _text;
            uep.ToolTipText = _toolTipText;
            uep.Checked = _checked;
            uep.CheckOnClick = _checkOnClick;
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
                catch(Exception e)
                {
                    AppFrame.FrameLogger.Error("载入图像失败！请确认配置界面时指定了正确的图像路径，或者图像是否存在。"+"界面元素文本："+_text,e);
                }
            }

            ToolStripButton tsb = new ToolStripButton();
            tsb.Text = _text;
            tsb.ToolTipText = _toolTipText;
            tsb.Checked = _checked;
            tsb.CheckOnClick = _checkOnClick;
            tsb.Enabled = _enabled;
            tsb.Image = img;
            tsb.Name = Name;
            tsb.DisplayStyle = _displayStyle;
            tsb.Alignment = _alignment;
            tsb.AutoSize = _autoSize;

            return tsb;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.ComponentModel;
using AddIn.Core;
using AddIn.Gui.Loader;

namespace AddIn.Gui.Parser
{
    class MenuItemParser:CmdParser
    {
        static private int _num = -1;
        private Keys _shortcutKeys = Keys.None;
        private string _image;
        private bool _checked;
        private bool _checkOnClick;

        public MenuItemParser(UiLoader uiLoader)
            : base(uiLoader)
        {
            _num++;
            _uiElemType = UiElemType.MenuItem;
            Name = "tsmi" + _num.ToString();
            _text = "MenuItem";
            _paramProvider = Name;
            _image = string.Empty;
        }

        //[CategoryAttribute("基本属性")]
        //[ DescriptionAttribute("菜单项的显示样式。")]
        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("Display style of the ToolStripMenuItem.")]
        public Keys ShortcutKeys
        {
            get 
            { 
                return _shortcutKeys; 
            }
            set 
            { 
                _shortcutKeys = value;
                (this.UiElem as ToolStripMenuItem).ShortcutKeys = _shortcutKeys;
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
                (this.UiElem as ToolStripMenuItem).Image = img;
            }
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
                (this.UiElem as ToolStripMenuItem).Checked = _checked;
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
                (this.UiElem as ToolStripMenuItem).CheckOnClick = _checkOnClick;
            }
        }

        public override object Clone()
        {
            MenuItemParser uep = new MenuItemParser(UiLoader);
            uep.ShortcutKeys = _shortcutKeys;
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

        public override void FromXmlNode(XmlNode node)
        {
            XmlElement elem = node as XmlElement;
            try
            {
                base.FromXmlNode(node);
                _shortcutKeys = (Keys)Enum.Parse(typeof(Keys), elem.GetAttribute("shortcut"));
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

            XmlNode n = UiElemParser.FindChildXmlNode(node, "subItems");
            base.ParseSubItems((this.UiElem as ToolStripMenuItem).DropDownItems, n,_text);
        }

        public override XmlNode ToXmlNode(XmlDocument doc)
        {
            XmlElement elem = base.ToXml(doc, "menuItem");
            elem.SetAttribute("shortcut", _shortcutKeys.ToString());
            elem.SetAttribute("checked", _checked.ToString());
            elem.SetAttribute("checkOnClick", _checkOnClick.ToString());


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

            ToolStripMenuItem tsmi = new ToolStripMenuItem();
            tsmi.Text = _text;
            tsmi.ToolTipText = _toolTipText;
            tsmi.Checked = _checked;
            tsmi.CheckOnClick = _checkOnClick;
            tsmi.Enabled = _enabled;
            tsmi.Visible = _visible;
            tsmi.Image = img;
            tsmi.ShortcutKeys = _shortcutKeys;
            tsmi.Name = Name;
            tsmi.Alignment = _alignment;
            tsmi.AutoSize = _autoSize;

            return tsmi;
        }

    }
}

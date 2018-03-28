using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml;
using System.Windows.Forms;
using System.Collections;
using AddIn.Gui.Loader;
using System.Drawing;

namespace AddIn.Gui.Parser
{
    internal abstract class UiElemParser : ICloneable
    {
        private static List<string> _uiElemNames = new List<string>();

        protected object _uiElem;
        private string _name;
        protected string _text;
        protected string _service = string.Empty;
        protected bool _visible = true;
        protected bool _enabled = true;

        protected string _updateEvent = string.Empty;

        protected UiElemType _uiElemType;


        private List<UiElemParser> _uiElemParserList = new List<UiElemParser>();

        private UiLoader _uiLoader;

        public UiElemParser(UiLoader uiLoader)
        {
            _uiLoader = uiLoader;
        }

        [BrowsableAttribute(false)]
        public UiLoader UiLoader
        {
            get { return _uiLoader; }
        }

        [BrowsableAttribute(false)]
        public UiElemType UiElemType
        {
            get { return _uiElemType; }
        }

        [BrowsableAttribute(false)]
        public List<UiElemParser> UiElemParserList
        {
            get { return _uiElemParserList; }
            set { _uiElemParserList = value; }
        }

        //[CategoryAttribute("基本属性")]
        //[ DescriptionAttribute("控件实例的名称，不同控件的名称各异。")]
        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("Control instance's name, the name of the different controls varies.")]
        public virtual string Name
        {
            get { return _name; }
            set
            {
                if (!_uiElemNames.Contains(value))
                {
                    _uiElemNames.Remove(_name);
                    _name = value;
                    _uiElemNames.Add(_name);
                }               
            }
        }

        //[CategoryAttribute("行为关联")]
        //[ DescriptionAttribute("插件类。为该控件的各种行为提供响应方法，并提供更新该控件状态的事件与方法。")]
        [CategoryAttribute("Action related")]
        [DescriptionAttribute("Class that provide method that response to the control's action and provide methods and events to update the control's state.")]
        [TypeConverter(typeof(ServiceConverter))]
        public virtual string Service
        {
            get { return _service; }
            set { _service = value; }
        }


        //[CategoryAttribute("行为关联")]
        //[ DescriptionAttribute("插件类中用于更新控件可用状态的事件。系统初始化时会为该事件生成并注册一个用于更新控件可用状态的方法。")]
        [CategoryAttribute("Action related")]
        [DescriptionAttribute("Event that addin-class provide to update control's enabled or checked state and selected index or value. On system initialization, it will generate a method and and register it to this event to update the control's state.")]
        [TypeConverter(typeof(EventConverter))]
        public virtual string UpdateEvent
        {
            get { return _updateEvent; }
            set { _updateEvent = value; }
        }

        protected string _injector = string.Empty;

        //[CategoryAttribute("行为关联")]
        //[ DescriptionAttribute("插件类提供的以该UI元素为参数的方法，通常用于在插件加载完毕后用于将该UI元素注入插件服务。")]
        [CategoryAttribute("Action related")]
        [DescriptionAttribute("Method that takes this uiElem as parameter, it usually used to inject the uiElem into an AddIn service.")]
        [TypeConverter(typeof(InjectorConverter))]
        public virtual string Injector
        {
            get { return _injector; }
            set { _injector = value; }
        }

        //[CategoryAttribute("基本属性")]
        //[DescriptionAttribute("显示在控件上的文本。在界面配置页面里，作为TreeNode的标签。")]
        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("The text displayed on the control. In the user interface configuration pages (current page), used as the TreeNode label.")]
        public virtual string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        //[CategoryAttribute("基本属性")]
        //[ DescriptionAttribute("表示是否启用该控件。")]
        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("Indicating whether the control is visible.")]
        public virtual bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        //[CategoryAttribute("基本属性")]
        //[ DescriptionAttribute("表示是否启用该控件。")]
        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("Indicating whether the control is enabled.")]
        public virtual bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }


        [BrowsableAttribute(false)]
        public virtual object UiElem
        {
            get
            {
                if (_uiElem == null)
                    _uiElem = this.CreateUiElem();
                return _uiElem;
            }
        }

        virtual public void FromXmlNode(XmlNode node)
        {
            try
            {
                XmlElement elem = node as XmlElement;
                Name = elem.GetAttribute("name");
                _text = elem.GetAttribute("text");
                _enabled = bool.Parse(elem.GetAttribute("enabled"));
                _visible = bool.Parse(elem.GetAttribute("visible"));

                XmlNode n1 = UiElemParser.FindChildXmlNode(node, "service");
                _service = n1.InnerText;

                XmlNode n2 = UiElemParser.FindChildXmlNode(node, "updateEvent");
                _updateEvent = n2.InnerText;

                XmlNode n3 = UiElemParser.FindChildXmlNode(node, "injector");
                _injector = n3.InnerText;
            }
            catch { }
        }

        virtual public XmlElement ToXmlNode(System.Xml.XmlDocument doc, string name = null)
        {
            if(name == null)
                return null;

            XmlElement elem = doc.CreateElement(name);
            elem.SetAttribute("name", Name);
            elem.SetAttribute("text", _text);
            elem.SetAttribute("visible", _visible.ToString());
            elem.SetAttribute("enabled", _enabled.ToString());

            XmlElement elemService = doc.CreateElement("service");
            elemService.InnerText = _service;
            elem.AppendChild(elemService);

            XmlElement elemUpdateEvent = doc.CreateElement("updateEvent");
            elemUpdateEvent.InnerText = _updateEvent;
            elem.AppendChild(elemUpdateEvent);

            XmlElement elemInjector = doc.CreateElement("injector");
            elemInjector.InnerText = _injector;
            elem.AppendChild(elemInjector);

            return elem;
        }

        virtual protected object CreateUiElem()
        {
            return null;
        }

        public static Image GetImageFormPath(string path)
        {
            Image img = null;
            if (!string.IsNullOrEmpty(path))
            {
                string imgPath = path;
                if (path.StartsWith("."))
                    imgPath = Application.StartupPath + path.Substring(1);

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
            return img;
        }
        #region ICloneable 成员

        virtual public object Clone()
        {
            return null;
        }

        #endregion


        private static string _path = string.Empty;
        protected void ParseSubItems(IList tsci, XmlNode node, string text)
        {
            string _tempPath = new string(UiElemParser._path.ToCharArray());
            UiElemParser._path += (text + "/");
            UiElemParser uep = null;
            foreach (XmlNode n in node.ChildNodes)
            {
                switch (n.Name)
                {
                    case "menuItem":
                        MenuItemParser mip = new MenuItemParser(UiLoader);
                        uep = mip;
                        mip.FromXmlNode(n);
                        this.UiElemParserList.Add(mip);
                        tsci.Add(mip.UiElem);
                        break;
                    case "statusStrip":
                        StatusStripParser ssp = new StatusStripParser(UiLoader);
                        ssp.FromXmlNode(n);
                        this.UiElemParserList.Add(ssp);
                        tsci.Add(ssp.UiElem);
                        break;
                    case "toolStrip":
                        ToolStripParser tsp = new ToolStripParser(UiLoader);
                        tsp.FromXmlNode(n);
                        this.UiElemParserList.Add(tsp);
                        ToolStripWrapper tsw = tsp.UiElem as ToolStripWrapper;
                        tsci.Add(tsp.UiElem);
                        UiLoader.ToolStrips.Add(tsp.Name, tsw as ToolStrip);
                        break;
                    case "ContextMenuStrip":
                        ContextMenuStripParser cmp = new ContextMenuStripParser(UiLoader);
                        cmp.FromXmlNode(n);
                        this.UiElemParserList.Add(cmp);
                        UiLoader.ContextMenuStrips.Add(cmp.Name, cmp.UiElem as ContextMenuStrip);
                         break;
                    case "textBox":
                        TextBoxParser tbp = new TextBoxParser(UiLoader);
                        uep = tbp;
                        tbp.FromXmlNode(n);
                        this.UiElemParserList.Add(tbp);
                        tsci.Add(tbp.UiElem);
                        break;
                    case "comboBox":
                        ComboBoxParser cbp = new ComboBoxParser(UiLoader);
                        uep = cbp;
                        cbp.FromXmlNode(n);
                        this.UiElemParserList.Add(cbp);
                        tsci.Add(cbp.UiElem);
                        break;
                    case "button":
                        ButtonParser bp = new ButtonParser(UiLoader);
                        uep = bp;
                        bp.FromXmlNode(n);
                        this.UiElemParserList.Add(bp);
                        tsci.Add(bp.UiElem);
                        break;
                    case "splitButton":
                        SplitButtonParser sbp = new SplitButtonParser(UiLoader);
                        uep = sbp;
                        sbp.FromXmlNode(n);
                        this.UiElemParserList.Add(sbp);
                        tsci.Add(sbp.UiElem);
                        break;
                    case "dropDownButton":
                        DropDownButtonParser dbbp = new DropDownButtonParser(UiLoader);
                        uep = dbbp;
                        dbbp.FromXmlNode(n);
                        this.UiElemParserList.Add(dbbp);
                        tsci.Add(dbbp.UiElem);
                        break;
                    case "statusLabel":
                        StatusLabelParser slp = new StatusLabelParser(UiLoader);
                        uep = slp;
                        slp.FromXmlNode(n);
                        this.UiElemParserList.Add(slp);
                        tsci.Add(slp.UiElem);
                        break;
                    case "label":
                        LabelParser lp = new LabelParser(UiLoader);
                        uep = lp;
                        lp.FromXmlNode(n);
                        this.UiElemParserList.Add(lp);
                        tsci.Add(lp.UiElem);
                        break;
                    case "separator":
                        SeparatorParser sp = new SeparatorParser(UiLoader);
                        sp.FromXmlNode(n);
                        this.UiElemParserList.Add(sp);
                        tsci.Add(sp.UiElem);
                        break;
                    case "progressBar":
                        ProgressBarParser pbp = new ProgressBarParser(UiLoader);
                        uep = pbp;
                        pbp.FromXmlNode(n);
                        this.UiElemParserList.Add(pbp);
                        tsci.Add(pbp.UiElem);
                        break;
                    default:
                        uep = null;
                        break;
                }
                if (uep != null)
                {
                    try
                    {
                        UiLoader.ToolStripitems.Add(UiElemParser._path + uep.Text + "/", uep.UiElem as ToolStripItem);
                    }
                    catch
                    {
 
                    }
                }
            }
            UiElemParser._path = _tempPath;
        }

        public static XmlNode FindChildXmlNode(XmlNode node, string name)
        {
            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.Name == name)
                    return n;
            }

            return null;
        }
    }
}

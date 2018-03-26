using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.ComponentModel;
using AddIn.Gui.Loader;

namespace AddIn.Gui.Parser
{
    class MenuStripParser:UiElemParser
    {
        static private int _num = -1;

        public void SetUiElem(MenuStrip ms)
        {
            _uiElem = ms;
        }

        public MenuStripParser(UiLoader uiLoader)
            : base(uiLoader)
        {
            _num++;
            _uiElemType = UiElemType.MenuStrip;
            Name = "tsms" + _num.ToString();
            _text = "MenuStrip";
            _visible = false;
        }

        public override object Clone()
        {
            MenuStripParser uep = new MenuStripParser(UiLoader);
            uep.Text = _text;
            uep.Service = _service;
            uep.UpdateEvent = _updateEvent;
            uep.Text = _text;
            uep.Visible = _visible;
            uep.Enabled = _enabled;

            uep.UiElemParserList = new List<UiElemParser>(this.UiElemParserList);
            return uep;
        }

        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                ;
            }
        }

        public override void FromXmlNode(XmlNode node)
        {
            XmlElement elem = node as XmlElement;
            try
            {
                base.FromXmlNode(node);

                XmlNode n1 = UiElemParser.FindChildXmlNode(node, "service");
                _service = n1.InnerText;

                XmlNode n2 = UiElemParser.FindChildXmlNode(node, "updateEvent");
                _updateEvent = n2.InnerText;
            }
            catch { }

            try
            {
                int num = int.Parse(Name.Substring(4));
                if (num > _num)
                    _num = num;
            }
            catch { }

            if (_uiElem == null)
                _uiElem = this.CreateUiElem();
            else
            {
                MenuStrip tsmi = _uiElem as MenuStrip;
                tsmi.Text = _text;
                tsmi.Name = Name;
                tsmi.Visible = _visible;
                tsmi.Enabled = _enabled;
            }

            XmlNode n = UiElemParser.FindChildXmlNode(node, "subItems");
            MenuStrip mse = this.UiElem as MenuStrip;
            mse.SuspendLayout();
            base.ParseSubItems(mse.Items, n,_text);
            mse.ResumeLayout(false);
        }

        public override XmlNode ToXmlNode(XmlDocument doc)
        {
            XmlElement elem = doc.CreateElement("menuStrip");
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
            MenuStrip tsmi = new MenuStrip();
            tsmi.Text = _text;
            tsmi.Name = Name;
            tsmi.Visible = _visible;
            tsmi.Enabled = _enabled;

            return tsmi;
        }
    }
}

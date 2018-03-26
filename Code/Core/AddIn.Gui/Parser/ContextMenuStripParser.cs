using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.ComponentModel;
using AddIn.Core;
using AddIn.Gui.Loader;

namespace AddIn.Gui.Parser
{
    class ContextMenuStripParser:UiElemParser
    {
        static private int _num = -1;

        public ContextMenuStripParser(UiLoader uiLoader)
            : base(uiLoader)
        {
            _num++;
            _uiElemType = UiElemType.ContextMenuStrip;
            Name = "cms" + _num.ToString();
            _text = "ContextMenuStrip";
        }

        public override object Clone()
        {
            ContextMenuStripParser uep = new ContextMenuStripParser(UiLoader);
            uep.Text = _text;
            uep.Service = _service;
            uep.UpdateEvent = _updateEvent;
            uep.UiElemParserList = new List<UiElemParser>(this.UiElemParserList);
 
            return uep;
        }

        public override void FromXmlNode(XmlNode node)
        {
            base.FromXmlNode(node);
            XmlElement elem = node as XmlElement;
            try
            {
                int num = int.Parse(Name.Substring(4));
                if (num > _num)
                    _num = num;
            }
            catch { }

            XmlNode n1 = UiElemParser.FindChildXmlNode(node, "service");
            _service = n1.InnerText;
            XmlNode n2 = UiElemParser.FindChildXmlNode(node, "updateEvent");
            _updateEvent = n2.InnerText;

            _uiElem = this.CreateUiElem();
            XmlNode n = UiElemParser.FindChildXmlNode(node, "subItems");
            ContextMenuStrip cms = this.UiElem as ContextMenuStrip;
            //cms.SuspendLayout();
            base.ParseSubItems(cms.Items, n,_text);
            //cms.ResumeLayout();
        }

        public override XmlNode ToXmlNode(XmlDocument doc)
        {
            XmlElement elem = doc.CreateElement("contextMenuStrip");
            elem.SetAttribute("name", Name);
            elem.SetAttribute("text", _text);
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
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Text = _text;
            cms.Name = Name;

            return cms;
        }
    }
}

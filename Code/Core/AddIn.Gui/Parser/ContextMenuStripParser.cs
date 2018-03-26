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

            _uiElem = this.CreateUiElem();
            XmlNode n = UiElemParser.FindChildXmlNode(node, "subItems");
            ContextMenuStrip cms = this.UiElem as ContextMenuStrip;
            //cms.SuspendLayout();
            base.ParseSubItems(cms.Items, n,_text);
            //cms.ResumeLayout();
        }

        public override XmlElement ToXmlNode(XmlDocument doc, string name = null)
        {
            XmlElement elem = base.ToXmlNode(doc, "contextMenuStrip");

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

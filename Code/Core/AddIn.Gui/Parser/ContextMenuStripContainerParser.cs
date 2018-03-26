using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using AddIn.Gui.Loader;
using System.Windows.Forms;

namespace AddIn.Gui.Parser
{
    class ContextMenuStripContainerParser : UiElemParser
    {
        static private int _num = -1;

        public ContextMenuStripContainerParser(UiLoader uiLoader)
            : base(uiLoader)
        {
            _num++;
            _uiElemType = UiElemType.ContextMenuStrips;
            Name = "cmsc" + _num.ToString();
            _text = "contextMenus";
        }

        public override object Clone()
        {
            ContextMenuStripContainerParser uep = new ContextMenuStripContainerParser(UiLoader);

            uep.UiElemParserList = new List<UiElemParser>(this.UiElemParserList);

            return uep;
        }

        public override void FromXmlNode(XmlNode node)
        {
            XmlNode n1 = UiElemParser.FindChildXmlNode(node, "subItems");
            foreach (XmlNode n2 in n1.ChildNodes)
            {
                ContextMenuStripParser cmp = new ContextMenuStripParser(UiLoader);
                cmp.FromXmlNode(n2);
                UiElemParserList.Add(cmp);
                UiLoader.ContextMenuStrips.Add(cmp.Name, cmp.UiElem as ContextMenuStrip);
            }
        }

        public override XmlNode ToXmlNode(XmlDocument doc)
        {
            XmlElement elem = doc.CreateElement("contextMenus");

            XmlElement elemSubElem = doc.CreateElement("subItems");
            foreach (UiElemParser up in base.UiElemParserList)
            {
                elemSubElem.AppendChild(up.ToXmlNode(doc));
            }
            elem.AppendChild(elemSubElem);

            return elem;
        }
    }
}

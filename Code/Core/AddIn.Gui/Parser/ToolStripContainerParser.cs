using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.ComponentModel;
using AddIn.Gui.Loader;

namespace AddIn.Gui.Parser
{
    class ToolStripContainerParser:UiElemParser
    {
        static private int _num = -1;

        public void SetUiElem(MyToolStripContainer tsc)
        {
            _uiElem = tsc;
        }

        public ToolStripContainerParser(UiLoader uiLoader)
            : base(uiLoader)
        {
            _num++;
            _uiElemType = UiElemType.ToolStripContainer;
            Name = "tsc" + _num.ToString();
            _text = "ToolStripContainer";
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

        public override object Clone()
        {
            ToolStripContainerParser uep = new ToolStripContainerParser(UiLoader);

            uep.Service = _service;
            uep.UpdateEvent = _updateEvent;
            uep.Text = _text;
            uep.Visible = _visible;
            uep.Enabled = _enabled;
            uep.UiElemParserList = new List<UiElemParser>(this.UiElemParserList);

            return uep;
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

            XmlNode n = UiElemParser.FindChildXmlNode(node, "subItems");
            MyToolStripContainer tsp = this.UiElem as MyToolStripContainer;
            tsp.SuspendLayout();
            base.ParseSubItems(tsp.ToolStrips, n, _text);
            tsp.ResumeLayout(false);
            tsp.PerformLayout();
        }

        public override XmlElement ToXmlNode(XmlDocument doc, string name = null)
        {
            XmlElement elem = base.ToXmlNode(doc, "toolStripContainer");

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

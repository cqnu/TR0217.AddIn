using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.ComponentModel;
using AddIn.Gui.Loader;

namespace AddIn.Gui.Parser
{
    class StatusStripParser:UiElemParser
    {
        static private int _num = -1;

        public void SetUiElem(StatusStrip ss)
        {
            _uiElem = ss;
        }

        public StatusStripParser(UiLoader uiLoader)
            : base(uiLoader)
        {
            _num++;
            _uiElemType = UiElemType.StatusStrip;
            Name = "tsss" + _num.ToString();
            _text = "StatusStrip";
        }

        public override object Clone()
        {
            StatusStripParser uep = new StatusStripParser(UiLoader);
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

            if (_uiElem == null)
                _uiElem = this.CreateUiElem();
            else
            {
                StatusStrip uie = _uiElem as StatusStrip;
                uie.Text = _text;
                uie.Name = Name;
                uie.Visible = _visible;
                uie.Enabled = _enabled;
            }

            XmlNode n = UiElemParser.FindChildXmlNode(node, "subItems");
            StatusStrip ss = this.UiElem as StatusStrip;
            ss.SuspendLayout();
            base.ParseSubItems(ss.Items, n,_text);
            ss.ResumeLayout(false);
        }

        public override XmlElement ToXmlNode(XmlDocument doc, string name = null)
        {
            XmlElement elem = base.ToXmlNode(doc, "statusStrip");

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
            StatusStrip ss = new StatusStrip();
            ss.Text = _text;
            ss.Name = Name;
            ss.Visible = _visible;
            ss.Enabled = _enabled;

            return ss;
        }
    }
}

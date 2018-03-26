using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.ComponentModel;
using AddIn.Gui.Loader;

namespace AddIn.Gui.Parser
{
    enum ToolStripLocation
    {
        Top = 1,
        Bottom = 2,
        Left = 3,
        Right = 4
    }

    class ToolStripParser:UiElemParser
    {
        static private int _num = -1;

        private ToolStripLocation _location;

        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("Defines where the ToolStrip to be positioned.")]
        public ToolStripLocation Location
        {
            get { return _location; }
            set { _location = value; }
        }

        private bool _joined;

        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("Whether the ToolStrip join the row or col of the previous ToolStrip.")]
        public bool Joined
        {
            get { return _joined; }
            set { _joined = value; }
        }

        public ToolStripParser(UiLoader uiLoader)
            : base(uiLoader)
        {
            _num++;
            _uiElemType = UiElemType.ToolStrip;
            Name = "ts" + _num.ToString();
            _text = "ToolStrip";
            _location = ToolStripLocation.Top;
            _joined = false;
        }

        public override object Clone()
        {
            ToolStripParser uep = new ToolStripParser(UiLoader);

            uep.Service = _service;
            uep.UpdateEvent = _updateEvent;
            uep.Text = _text;
            uep.Visible = _visible;
            uep.Enabled = _enabled;
            uep.Location = _location;
            uep.Joined = _joined;

            uep.UiElemParserList = new List<UiElemParser>(this.UiElemParserList);

            return uep;
        }

        public override void FromXmlNode(XmlNode node)
        {
            XmlElement elem = node as XmlElement;
            try
            {
                base.FromXmlNode(node);               
                _location = (ToolStripLocation)Enum.Parse(typeof(ToolStripLocation), elem.GetAttribute("location"));
                _joined = bool.Parse(elem.GetAttribute("joined"));

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

            _uiElem = this.CreateUiElem();

            XmlNode n = UiElemParser.FindChildXmlNode(node, "subItems");

            ToolStripWrapper tsw = this.UiElem as ToolStripWrapper;
            tsw.ToolStrip.SuspendLayout();
            base.ParseSubItems(tsw.ToolStrip.Items, n, _text);
            tsw.ToolStrip.ResumeLayout(false);
        }

        public override XmlNode ToXmlNode(XmlDocument doc)
        {
            XmlElement elem = doc.CreateElement("toolStrip");
            elem.SetAttribute("name", Name);
            elem.SetAttribute("text", _text);
            elem.SetAttribute("visible", _visible.ToString());
            elem.SetAttribute("enabled", _enabled.ToString());
            elem.SetAttribute("joined", _joined.ToString());
            elem.SetAttribute("location", _location.ToString());

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
            ToolStrip tsmi = new ToolStrip();
            ToolStripWrapper tsw = new ToolStripWrapper();
            tsw.ToolStrip = tsmi;
            tsw.Location = _location;
            tsw.Joined = _joined;
            tsmi.Text = _text;
            tsmi.Name = Name;
            tsmi.Visible = _visible;
            tsmi.Enabled = _enabled;

            return tsw;
        }
    }
}

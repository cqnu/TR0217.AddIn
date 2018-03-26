using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using AddIn.Gui.Loader;

namespace AddIn.Gui.Parser
{
    class SeparatorParser:UiElemParser
    {
        static private int _num = -1;

        public SeparatorParser(UiLoader uiLoader)
            :base(uiLoader)
        {
            _num++;
            _uiElemType = UiElemType.Separator;
            Name = "tss" + _num.ToString();
            _text = "Separator";
        }

        public override object Clone()
        {
            SeparatorParser uep = new SeparatorParser(UiLoader);
            uep.Text = _text;

            return uep;
        }

        public override void FromXmlNode(XmlNode node)
        {
            base.FromXmlNode(node);
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
            return base.ToXmlNode(doc, "separator");
        }

        protected override object CreateUiElem()
        {
            ToolStripSeparator tss = new ToolStripSeparator();
            tss.Visible = _visible;

            return tss;
        }

        
    }
}

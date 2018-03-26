using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using AddIn.Core;
using AddIn.Gui.Loader;

namespace AddIn.Gui.Parser
{
    class ProgressBarParser:CmdParser
    {
        static private int _num = -1;
        private int _minimun = 0;
        private int _maxmum = 100;
        private int _width = 100;

        public ProgressBarParser(UiLoader uiLoader)
            : base(uiLoader)
        {
            _num++;
            _uiElemType = UiElemType.ProgressBar;
            Name = "tspb" + _num.ToString();
            _text = "ProgressBar";
        }

        public int Minimun
        {
            get 
            { 
                return _minimun; 
            }
            set
            {
                _minimun = value;
                (this.UiElem as ToolStripProgressBar).Minimum = _minimun;
            }
        }

        public int Maxmum
        {
            get 
            { 
                return _maxmum; 
            }
            set 
            { 
                _maxmum = value;
                (this.UiElem as ToolStripProgressBar).Maximum = _maxmum;
            }
        }


        public int Width
        {
            get 
            { 
                return _width; 
            }
            set 
            {
                _width = value;
                (this.UiElem as ToolStripProgressBar).Width = _width;
            }
        }

        public override bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;
                (this.UiElem as ToolStripProgressBar).Enabled = _enabled;
            }
        }

        public override object Clone()
        {
            ProgressBarParser uep = new ProgressBarParser(UiLoader);
            uep.Minimun = _minimun;
            uep.Maxmum = _maxmum;
            uep.Width = _width;
            uep.Text = _text;
            uep.ToolTipText = _toolTipText;
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
            base.FromXmlNode(node);
            XmlElement elem = node as XmlElement;
            _width = int.Parse(elem.GetAttribute("width"));
            _maxmum = int.Parse(elem.GetAttribute("maxmum"));
            _minimun = int.Parse(elem.GetAttribute("minimum"));

            try
            {
                int num = int.Parse(Name.Substring(4));
                if (num > _num)
                    _num = num;
            }
            catch { }
            _uiElem = this.CreateUiElem();
        }

        public override XmlNode ToXmlNode(XmlDocument doc)
        {
            XmlElement elem = base.ToXml(doc, "progressBar");
            elem.SetAttribute("width", _width.ToString());
            elem.SetAttribute("maxmum", _maxmum.ToString());
            elem.SetAttribute("minimum", _minimun.ToString());

            return elem;
        }

        protected override object CreateUiElem()
        {
            ToolStripProgressBar tspb = new ToolStripProgressBar();
            tspb.Enabled = _enabled;
            tspb.Visible = _visible;
            tspb.ToolTipText = _toolTipText;
            tspb.Width = _width;
            tspb.Maximum = _maxmum;
            tspb.Minimum = _minimun;
            tspb.Name = Name;
            tspb.Alignment = _alignment;
            tspb.AutoSize = _autoSize;
            return tspb;
        }
    }
}

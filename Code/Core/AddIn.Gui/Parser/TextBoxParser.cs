using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.ComponentModel;
using System.Drawing;
using AddIn.Core;
using AddIn.Gui.Loader;

namespace AddIn.Gui.Parser
{
    class TextBoxParser:CmdParser
    {
        static private int _num = -1;
        private int _width = 100;
        private bool _readOnly = false;
        private string _emptyTextTip;
        private Color _emptyTextTipColor = Color.LightGray;
        private BorderStyle _borderStyle;

        public TextBoxParser(UiLoader uiLoader)
            : base(uiLoader)
        {
            _num++;
            _uiElemType = UiElemType.TextBox;
            Name = "tstb" + _num.ToString();
            _text = "TextBox";
            _paramProvider = Name;
            _emptyTextTip = string.Empty;
            _autoSize = false;
            _borderStyle = BorderStyle.Fixed3D;
        }


        [CategoryAttribute("Basic properties")]
        public int Width
        {
            get 
            { 
                return _width; 
            }
            set 
            { 
                _width = value;
                ToolStripWatermarkTextBox tstb = this.UiElem as ToolStripWatermarkTextBox;
                tstb.Size = new System.Drawing.Size(_width, tstb.Height);
            }
        }

        [CategoryAttribute("Basic properties")]
        [Description("The border style of the TextBox.")]
        public BorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set { _borderStyle = value; }
        }

        [CategoryAttribute("Basic properties")]
        [Description("Controls whether the text in the edit control can be changed or not.")]
        public bool ReadOnly
        {
            get { return _readOnly; }
            set 
            { 
                _readOnly = value;
                (this.UiElem as ToolStripWatermarkTextBox).WatermarkTextBox.ReadOnly = value;
            }
        }

        [CategoryAttribute("Basic properties")]
        [Description("The Text show in the editbox when the TextBox is not focused and its Text is empty.")]
        public string EmptyTextTip
        {
            get { return _emptyTextTip; }
            set 
            { 
                _emptyTextTip = value;
                (this.UiElem as ToolStripWatermarkTextBox).EmptyTextTip = _emptyTextTip;
            }
        }

        [CategoryAttribute("Basic properties")]
        [Description("The color of the EmptyTextTip.")]
        public Color EmptyTextTipColor
        {
            get { return _emptyTextTipColor; }
            set
            {
                _emptyTextTipColor = value;
                (this.UiElem as ToolStripWatermarkTextBox).EmptyTextTipColor = value;
            }
        }

        public override object Clone()
        {
            TextBoxParser uep = new TextBoxParser(UiLoader);
            uep.Width = _width;
            uep.Text = _text;
            uep.EmptyTextTip = _emptyTextTip;
            uep.EmptyTextTipColor = _emptyTextTipColor;
            uep.ToolTipText = _toolTipText;
            uep.Enabled = _enabled;
            uep.Visible = _visible;
            uep.BorderStyle = _borderStyle;
            uep.UpdateEvent = _updateEvent;
            uep.Service = _service;
            uep.Function = _function;
            uep.Parameter = _parameter;
            uep.ParamProvider = _paramProvider;
            uep.Alignment = _alignment;
            uep.AutoSize = _autoSize;
            uep.ReadOnly = _readOnly;

            uep.UiElemParserList = new List<UiElemParser>(this.UiElemParserList);

            return uep;
        }

        public override void FromXmlNode(XmlNode node)
        {
            base.FromXmlNode(node);
            XmlElement elem = node as XmlElement;
            _width = int.Parse(elem.GetAttribute("width"));
            _borderStyle = (BorderStyle)Enum.Parse(typeof(BorderStyle), elem.GetAttribute("borderStyle"));
            _readOnly = bool.Parse(elem.GetAttribute("readOnly"));
            _emptyTextTip = elem.GetAttribute("emptyTextTip");
            _emptyTextTipColor = Color.FromName(elem.GetAttribute("emptyTextTipColor"));

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
            XmlElement elem = base.ToXml(doc, "textBox");
            elem.SetAttribute("borderStyle", _borderStyle.ToString());
            elem.SetAttribute("width",_width.ToString());
            elem.SetAttribute("emptyTextTip", _emptyTextTip);
            elem.SetAttribute("emptyTextTipColor", _emptyTextTipColor.Name);
            elem.SetAttribute("readOnly", _readOnly.ToString());

            return elem;
        }

        protected override object CreateUiElem()
        {
            ToolStripWatermarkTextBox tstb = new ToolStripWatermarkTextBox();
            tstb.Text = _text;
            tstb.EmptyTextTip = _emptyTextTip;
            tstb.EmptyTextTipColor = _emptyTextTipColor;
            tstb.Enabled = _enabled;
            tstb.Visible = _visible;
            tstb.Size = new System.Drawing.Size(_width, tstb.Height);
            tstb.Name = Name;
            tstb.ToolTipText = _toolTipText;
            tstb.Alignment = _alignment;
            tstb.AutoSize = _autoSize;
            tstb.WatermarkTextBox.BorderStyle = _borderStyle;
            tstb.WatermarkTextBox.ReadOnly = _readOnly;
            return tstb;
        }

    }
}

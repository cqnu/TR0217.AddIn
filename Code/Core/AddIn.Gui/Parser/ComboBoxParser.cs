using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using AddIn.Core;
using AddIn.Gui.Loader;

namespace AddIn.Gui.Parser
{
    class ComboBoxItemParser:UiElemParser
    {
        static private int _num = -1;

        public ComboBoxItemParser(UiLoader uiLoader)
            : base(uiLoader)
        {
            _uiElemType = UiElemType.ComboBoxItem;
            Name = "cbi" + _num.ToString();
            _text = "ComboBoxItem";
        }

        public override object UiElem
        {
            get
            {
                return _text;
            }
        }

        public override object Clone()
        {
            ComboBoxItemParser uep = new ComboBoxItemParser(UiLoader);
            uep._text = _text;
            return uep;
        }

        public override XmlElement ToXmlNode(XmlDocument doc, string name = null)
        {
            return base.ToXmlNode(doc,"comboBoxItem");
        }
    }

    class ComboBoxParser : CmdParser
    {
        static private int _num = -1;
        private int _width = 121;
        private int _dropDownWidth = 121;
        private int _dropDownHeight = 106;
        private int _maxDropDownItems = 10;
        private ComboBoxStyle _dropDownStyle = ComboBoxStyle.DropDown;
        private AutoCompleteMode _autoCompleteMode = AutoCompleteMode.None;
        private AutoCompleteSource _autoCompleteSource = AutoCompleteSource.None;
        private string _emptyTextTip;
        private Color _emptyTextTipColor = Color.LightGray;

        public ComboBoxParser(UiLoader uiLoader)
            : base(uiLoader)
        {
            _num++;
            _uiElemType = UiElemType.ComboBox;
            Name = "tscb" + _num.ToString();
            _text = "ComboBox";
            _paramProvider = Name;
            _autoSize = false;
        }

        [CategoryAttribute("Basic properties")]
        [Description("The Text show in the editbox when the ComboBox is not focused and its DropDownStyle is not DropDownList and its Text is empty.")]
        public string EmptyTextTip
        {
            get { return _emptyTextTip; }
            set
            {
                _emptyTextTip = value;
                (this.UiElem as ToolStripWatermarkComboBox).EmptyTextTip = _emptyTextTip;
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
                (this.UiElem as ToolStripWatermarkComboBox).EmptyTextTipColor = value;
            }
        }

        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("The source of complete strings used for automatic completion.")]
        public AutoCompleteSource AutoCompleteSource
        {
            get 
            {
                return _autoCompleteSource; 
            }
            set 
            {
                _autoCompleteSource = value;
                (this.UiElem as ToolStripWatermarkComboBox).AutoCompleteSource = _autoCompleteSource;
            }
        }

        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("Indicates the text completion behavior of the combo box.")]
        public AutoCompleteMode AutoCompleteMode
        {
            get 
            { 
                return _autoCompleteMode; 
            }
            set 
            { 
                _autoCompleteMode = value;
                (this.UiElem as ToolStripWatermarkComboBox).AutoCompleteMode = _autoCompleteMode;
            }
        }

        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("Controls the appearance and functionality of the combo box.")]
        public ComboBoxStyle DropDownStyle
        {
            get 
            { 
                return _dropDownStyle; 
            }
            set 
            { 
                _dropDownStyle = value;
                (this.UiElem as ToolStripWatermarkComboBox).DropDownStyle = _dropDownStyle;
            }
        }

        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("The width, in pixel, of the drop-down box in a combo box.")]
        public int DropDownWidth
        {
            get 
            { 
                return _dropDownWidth;
            }
            set 
            { 
                _dropDownWidth = value;
                (this.UiElem as ToolStripWatermarkComboBox).DropDownWidth = _dropDownWidth;
            }
        }

        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("The height, in pixel, of the drop-down box in a combo box.")]
        public int DropDownHeight
        {
            get 
            { 
                return _dropDownHeight; 
            }
            set 
            { 
                _dropDownHeight = value;
                (this.UiElem as ToolStripWatermarkComboBox).DropDownHeight = _dropDownHeight;
            }
        }

        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("The maximum number of entries to display in the drop-down list.")]
        public int MaxDropDownItems
        {
            get 
            { 
                return _maxDropDownItems; 
            }
            set 
            { 
                _maxDropDownItems = value;
                (this.UiElem as ToolStripWatermarkComboBox).MaxDropDownItems = _maxDropDownItems;
            }
        }

        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("The width of the control in pixel.")]
        public int Width
        {
            get 
            { 
                return _width; 
            }
            set 
            { 
                _width = value;
                (this.UiElem as ToolStripWatermarkComboBox).Size = new System.Drawing.Size(_width, 25);
            }
        }

        public override object Clone()
        {
            ComboBoxParser uep = new ComboBoxParser(UiLoader);
            uep.Width = _width;
            uep.DropDownHeight = _dropDownHeight;
            uep.DropDownWidth = _dropDownWidth;
            uep.EmptyTextTip = _emptyTextTip;
            uep.EmptyTextTipColor = _emptyTextTipColor;
            uep.DropDownStyle = _dropDownStyle;
            uep.AutoCompleteMode = _autoCompleteMode;
            uep.AutoCompleteSource = _autoCompleteSource;
            uep.MaxDropDownItems = _maxDropDownItems;
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
            _dropDownWidth = int.Parse(elem.GetAttribute("dropDownWidth"));
            _dropDownHeight = int.Parse(elem.GetAttribute("dropDownHeight"));
            _emptyTextTip = elem.GetAttribute("emptyTextTip");
            _emptyTextTipColor = Color.FromName(elem.GetAttribute("emptyTextTipColor"));
            _dropDownStyle = (ComboBoxStyle)Enum.Parse(typeof(ComboBoxStyle), elem.GetAttribute("dropDownStyle"));
            _maxDropDownItems = int.Parse(elem.GetAttribute("maxDropDownItems"));
            _autoCompleteSource = (AutoCompleteSource)Enum.Parse(typeof(AutoCompleteSource), elem.GetAttribute("autoCompleteSource"));
            _autoCompleteMode = (AutoCompleteMode)Enum.Parse(typeof(AutoCompleteMode), elem.GetAttribute("autoCompleteMode"));

            try
            {
                int num = int.Parse(Name.Substring(4));
                if (num > _num)
                    _num = num;
            }
            catch { }
            _uiElem = this.CreateUiElem();

            XmlNode subItems = CmdParser.FindChildXmlNode(node, "subItems");
            ComboBox.ObjectCollection oc = (this.UiElem as ToolStripWatermarkComboBox).Items;
            foreach (XmlNode n in subItems.ChildNodes)
            {
                ComboBoxItemParser cbp = new ComboBoxItemParser(UiLoader);
                cbp.FromXmlNode(n);
                this.UiElemParserList.Add(cbp);
                oc.Add(cbp.UiElem);
            }

        }

        public override XmlElement ToXmlNode(XmlDocument doc, string name = null)
        {
            XmlElement elem = base.ToXmlNode(doc, "comboBox");

            elem.SetAttribute("width", _width.ToString());
            elem.SetAttribute("dropDownWidth", _dropDownWidth.ToString());
            elem.SetAttribute("dropDownHeight", _dropDownHeight.ToString());
            elem.SetAttribute("emptyTextTip", _emptyTextTip);
            elem.SetAttribute("emptyTextTipColor", _emptyTextTipColor.Name);
            elem.SetAttribute("dropDownStyle", _dropDownStyle.ToString());
            elem.SetAttribute("autoCompleteMode", _autoCompleteMode.ToString());
            elem.SetAttribute("autoCompleteSource", _autoCompleteSource.ToString());
            elem.SetAttribute("maxDropDownItems",_maxDropDownItems.ToString());

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
            ToolStripWatermarkComboBox tscb = new ToolStripWatermarkComboBox();
            tscb.Text = _text;
            tscb.ToolTipText = _toolTipText;
            tscb.EmptyTextTip = _emptyTextTip;
            tscb.EmptyTextTipColor = _emptyTextTipColor;
            tscb.Enabled = _enabled;
            tscb.Visible = _visible;
            tscb.DropDownWidth = _dropDownWidth;
            tscb.DropDownHeight = _dropDownHeight;
            tscb.DropDownStyle = _dropDownStyle;
            tscb.AutoCompleteMode = _autoCompleteMode;
            tscb.AutoCompleteSource = _autoCompleteSource;
            tscb.MaxDropDownItems = _maxDropDownItems;
            tscb.Name = Name;
            tscb.Size = new System.Drawing.Size(_width, 25);
            tscb.Alignment = _alignment;
            tscb.AutoSize = _autoSize;

            return tscb;
        }

    }
}

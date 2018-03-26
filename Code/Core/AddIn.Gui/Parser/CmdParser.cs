using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.ComponentModel;
using AddIn.Gui.Loader;

namespace AddIn.Gui.Parser
{
   internal abstract class CmdParser : UiElemParser
    {
        protected ToolStripItemAlignment _alignment = ToolStripItemAlignment.Left;
        protected bool _autoSize = true;
        protected string _function = string.Empty;
        protected string _paramProvider = string.Empty;
        protected string _parameter = string.Empty;
        protected string _toolTipText = string.Empty;

        public CmdParser(UiLoader uiLoader)
            :base(uiLoader)
        { 
        }


        public override void FromXmlNode(XmlNode node)
        {
            XmlElement elem = node as XmlElement;
            try
            {
                base.FromXmlNode(node);
                _autoSize = bool.Parse(elem.GetAttribute("_autoSize"));
                _alignment = (ToolStripItemAlignment)Enum.Parse(typeof(ToolStripItemAlignment), elem.GetAttribute("alignment"));
            }
            catch { }

            foreach (XmlNode n in node.ChildNodes)
            {
                switch (n.Name)
                {
                    case "service":
                        _service = n.InnerText;
                        break;
                    case "function":
                        _function = n.InnerText;
                        break;
                    case "paramProvider":
                        _paramProvider = n.InnerText;
                        break;
                    case "parameter":
                        _parameter = n.InnerText;
                        break;
                    case "updateEvent":
                        _updateEvent = n.InnerText;
                        break;
                    case "toolTipText":
                        _toolTipText = n.InnerText;
                        break;
                    default: break;
                }
            }
        }

        public override XmlElement ToXmlNode(XmlDocument doc, string name = null)
        {
            XmlElement elem = base.ToXmlNode(doc, name);

            elem.SetAttribute("alignment", _alignment.ToString());
            elem.SetAttribute("autoSize", _autoSize.ToString());

            XmlElement elemFunction = doc.CreateElement("function");
            elemFunction.InnerText = _function;
            XmlElement elemParamProvider = doc.CreateElement("paramProvider");
            elemParamProvider.InnerText = _paramProvider;
            XmlElement elemParameter = doc.CreateElement("parameter");
            elemParameter.InnerText = _parameter;
            XmlElement elemToolTipText = doc.CreateElement("toolTipText");
            elemToolTipText.InnerText = _toolTipText;

            elem.AppendChild(elemFunction);
            elem.AppendChild(elemParamProvider);
            elem.AppendChild(elemParameter);
            elem.AppendChild(elemToolTipText);

            return elem;
        }


        //[CategoryAttribute("基本属性")]
        //[ DescriptionAttribute("显示在ToolTip上的文本。")]
        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("Text displayed on the ToolTip.")]
        public virtual string ToolTipText
        {
            get { return _toolTipText; }
            set 
            { 
                _toolTipText = value;
                (this.UiElem as ToolStripItem).ToolTipText = _toolTipText;
            }
        }

        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("Whether auto change its size to fit the father Control.")]
        public virtual bool AutoSize
        {
            get { return _autoSize; }
            set 
            {
                _autoSize = value;
                (this.UiElem as ToolStripItem).AutoSize = value;
            }
        }

        //[CategoryAttribute("基本属性")]
        //[ DescriptionAttribute("表示是否启用该控件。")]
        [CategoryAttribute("Basic properties")]
        [DescriptionAttribute("Indicating whether the ToolStripItem's is Left-aligned or Right-aligned")]
        public virtual ToolStripItemAlignment Alignment
        {
            get 
            { 
                return _alignment; 
            }
            set 
            { 
                _alignment = value;
                (this.UiElem as ToolStripItem).Alignment = _alignment;
            }
        }

        //[CategoryAttribute("行为关联")]
        //[ DescriptionAttribute("插件类提供的完成某一功能的方法，注册到该控件的Click或者SelectedIndexChange事件上。")]
        [CategoryAttribute("Action related")]
        [DescriptionAttribute("Method that addin-class provided to complete a function, registed to the control's Click or SelectedIndexChange event.")]
        [TypeConverter(typeof(MethodConverter))]
        public virtual  string Function
        {
            get { return _function; }
            set { _function = value; }
        }

        //[CategoryAttribute("行为关联")]
        //[ DescriptionAttribute("为完成功能的方法提供参数的控件的名称。"
        //+"提供参数的控件和该控件处于同一个层次。"
        //+"提供参数的控件一般是TextBox、ComboBox、Button。提供的参数是它们的Text、SelectedValue、Checked属性。")]
        [CategoryAttribute("Action related")]
        [DescriptionAttribute("Name of the Control that provide parameter for the mentod to complete a function. ")]
        [TypeConverter(typeof(TSIConverter))]
        public virtual string ParamProvider
        {
            get { return _paramProvider; }
            set { _paramProvider = value; }
        }

        //[CategoryAttribute("行为关联")]
        //[ DescriptionAttribute("完成功能的方法的参数。系统优先使用ParamProvider提供的参数。"
        //+"如果这个字段为空或者完成方法的参数多于一个，系统才会使用这里的参数。"
        //+"如果这个字段不空，会作为完成功能的方法的第一个参数")]
        [CategoryAttribute("Action related")]
        [DescriptionAttribute("Parameter for the method."
        +"System use the parameter provided by ParamProvider in priority."
        +"If ParamProvider is empty or the menthod have more than one parameter, this parameter will in use."
        +"If this field is not empty, it will be used as the first parameter for the method.")]
        public virtual string Parameter
        {
            get { return _parameter; }
            set { _parameter = value; }
        }

       public override bool Enabled
       {
           get
           {
               return _enabled;
           }
           set
           {
               _enabled = value;
               (this.UiElem as ToolStripItem).Enabled = _enabled;
           }
       }

       public override bool Visible
       {
           get
           {
               return _visible;
           }
           set
           {
               _visible = value;
               (this.UiElem as ToolStripItem).Visible = _visible;
           }
       }


       public override string Text
       {
           get
           {
               return _text;
           }
           set
           {
               _text = value;
               (this.UiElem as ToolStripItem).Text = _text;
           }
       }
    }
}

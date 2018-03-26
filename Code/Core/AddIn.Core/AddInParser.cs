using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Windows.Forms;

namespace AddIn.Core
{
    public class AddInParser
    {
        string _name;
        string _author;
        string _version;
        string _copyright;
        string _url;
        bool   _lazyload;
        string _path;
        string _description;
        ServiceBase _service;
        Assembly _assembly;
        bool _valid;
        bool _isBaseService;

        public bool IsBaseService
        {
            get { return _isBaseService; }
            set { _isBaseService = value; }
        }

        public bool Valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

        public Assembly Assembly
        {
            get { return _assembly; }
            set { _assembly = value; }
        }

        public ServiceBase Service
        {
            get { return _service; }
        }

        public AddInParser()
        {
            _name = _author = _copyright = _url = _path = _description = string.Empty;
            _lazyload = false;
            _service = null;
            _assembly = null;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public string Copyright
        {
            get { return _copyright; }
            set { _copyright = value; }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public bool Lazyload
        {
            get { return _lazyload; }
            set { _lazyload = value; }
        }

        internal void FromXmlNode(XmlNode node)
        {
            XmlElement elem = node as XmlElement;
            _name = elem.GetAttribute("name");
            _author = elem.GetAttribute("author");
            _version = elem.GetAttribute("version");
            _copyright = elem.GetAttribute("copyright");
            _url = elem.GetAttribute("url");
            _lazyload = bool.Parse(elem.GetAttribute("lazyload"));
            _path = node.FirstChild.InnerText;
            _description = node.LastChild.InnerText;
        }

        internal XmlNode ToXmlNode(XmlDocument doc)
        {
            XmlElement elem = doc.CreateElement("AddIn");

            elem.SetAttribute("name", _name);
            elem.SetAttribute("author", _author);
            elem.SetAttribute("version", _version);
            elem.SetAttribute("copyright", _copyright);
            elem.SetAttribute("url", _url);
            elem.SetAttribute("lazyload", _lazyload.ToString());
            XmlElement path = doc.CreateElement("path");
            path.InnerText = _path;
            XmlElement description = doc.CreateElement("description");
            description.InnerText = _description;
            elem.AppendChild(path);
            elem.AppendChild(description);

            return elem;
        }


        public ServiceBase GetService()
        {
            string path = Application.StartupPath + _path.Substring(1);
            if (_service == null && _path != string.Empty)
            {
                try
                {
                    _valid = true;
                    _assembly = Assembly.LoadFile(path);
                    Type type = _assembly.GetType(_name, true);
                    ConstructorInfo constructorInfo = type.GetConstructor(System.Type.EmptyTypes);
                    _service = constructorInfo.Invoke(null) as ServiceBase;
                    _service._addInParser = this;
                }
                catch(Exception e)
                {
                    _valid = false;
                    //记录日志
                    AppFrame.FrameLogger.Error("加载插件" + _name + "失败！", e);
                }
            }

            return _service;
        }
    }
}

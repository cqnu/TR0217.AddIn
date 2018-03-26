using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using AddIn.Core.Properties;

namespace AddIn.Core
{
    internal class ServiceCollection : IServiceCollection
    {
        public event LoadAddInHandler AfterLoadOneAddIn;
        public event LoadAddInHandler BeforLoadeOneAddIn;

        private Dictionary<string, ServiceBase> _services;

        public Dictionary<string, ServiceBase> Services
        {
            get { return _services; }
        }
        
        private XmlDocument _addInConfigFile;
        private string _addInConfigPath;
        private List<AddInParser> _addInParserList;
        private XmlDocument _baseServiceConfigFile;
        private string _baseServiceConfigPath;
        private List<AddInParser> _baseServiceParserList;

        public List<AddInParser> BaseServiceParserList
        {
            get { return _baseServiceParserList; }
        }

        public List<AddInParser> AddInParserList
        {
            get { return _addInParserList; }
        }

        public ServiceCollection()
        {
            _services = new Dictionary<string, ServiceBase>();
            _addInParserList = new List<AddInParser>();
            _baseServiceParserList = new List<AddInParser>();

            _addInConfigPath = Application.StartupPath + "\\Config\\AddIns.xml";
            _baseServiceConfigPath = Application.StartupPath + "\\Config\\BaseServices.xml";
            _addInConfigFile = new XmlDocument();
            _baseServiceConfigFile = new XmlDocument();
        }

        internal void LoadBaseServices()
        {
            if (!File.Exists(_baseServiceConfigPath) || !ReadConfigFile(_baseServiceConfigFile, _baseServiceConfigPath))
            {
                InitialBaseServiceConfig();
            }

            foreach (XmlNode n in _baseServiceConfigFile.DocumentElement.ChildNodes)
            {
                AddInParser ap = new AddInParser();
                ap.FromXmlNode(n);
                ap.IsBaseService = true;

                if (this.BeforLoadeOneAddIn != null)
                    this.BeforLoadeOneAddIn(new LoadAddInEventArgs(ap, this));

                _baseServiceParserList.Add(ap);

                ServiceBase service = ap.GetService();
                if (service != null)
                    _services.Add(ap.Name, service);

                if (this.AfterLoadOneAddIn != null)
                    this.AfterLoadOneAddIn(new LoadAddInEventArgs(ap, this));
            }
        }

        internal void Load()
        {
            if (!File.Exists(_addInConfigPath) || !ReadConfigFile(_addInConfigFile,_addInConfigPath))
            {
                InitialAddInConfig();
            }

            foreach (XmlNode n in _addInConfigFile.DocumentElement.ChildNodes)
            {
                AddInParser ap = new AddInParser();
                ap.FromXmlNode(n);
                ap.IsBaseService = false;

                if (this.BeforLoadeOneAddIn != null)
                    this.BeforLoadeOneAddIn(new LoadAddInEventArgs(ap, this));

                if (!_services.ContainsKey(ap.Name))
                {
                    _addInParserList.Add(ap);
                    ServiceBase service = ap.GetService();
                    if (service != null)
                        _services.Add(ap.Name, service);

                    if (this.AfterLoadOneAddIn != null)
                        this.AfterLoadOneAddIn(new LoadAddInEventArgs(ap, this));
                }
            }
        }

        private void InitialBaseServiceConfig()
        {
            _baseServiceConfigFile.LoadXml(Resources.BaseServices);
            SaveConfigFile(_baseServiceConfigFile, _baseServiceConfigPath);
        }

        private void InitialAddInConfig()
        {
            _addInConfigFile.LoadXml(Resources.AddIns);
            SaveConfigFile(_addInConfigFile, _addInConfigPath);
        }

        public void SaveAddInConfig()
        {
            SaveAddInConfig(_addInConfigPath);
        }
        public void SaveBaseServiceConfig()
        {
            SaveBaseServiceConfig(_baseServiceConfigPath);
        }

        public void SaveAddInConfig(String configPath)
        {
            _addInConfigFile.RemoveChild(_addInConfigFile.DocumentElement);

            XmlElement rootElem = _addInConfigFile.CreateElement("AddIns");

            foreach (AddInParser ap in _addInParserList)
            {
                rootElem.AppendChild(ap.ToXmlNode(_addInConfigFile));
            }

            _addInConfigFile.AppendChild(rootElem);

            SaveConfigFile(_addInConfigFile, configPath);
        }

        public void SaveBaseServiceConfig(String bsConfigPath)
        {
            _baseServiceConfigFile.RemoveChild(_baseServiceConfigFile.DocumentElement);

            XmlElement rootElem = _baseServiceConfigFile.CreateElement("BaseServices");

            foreach (AddInParser ap in _baseServiceParserList)
            {
                rootElem.AppendChild(ap.ToXmlNode(_baseServiceConfigFile));
            }

            _baseServiceConfigFile.AppendChild(rootElem);

            SaveConfigFile(_baseServiceConfigFile, bsConfigPath);
        }

        private bool ReadConfigFile(XmlDocument doc, string xmlFile)
        {
            try
            {
                doc.Load(xmlFile);
            }
            catch (XmlException xe)
            {
                AppFrame.FrameLogger.Fatal("读取插件列表失败！", xe);
                MessageBox.Show("读取插件列表失败！", "注意！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            catch (Exception e)
            {
                AppFrame.FrameLogger.Fatal("读取插件列表失败！", e);
                MessageBox.Show("读取插件列表失败！", "注意！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void SaveConfigFile(XmlDocument doc, string path)
        {
            try
            {
                String dir = Path.GetDirectoryName(path);

                if (Directory.Exists(dir) && File.Exists(path))
                {
                    File.Delete(path);
                }
                else
                {
                    Directory.CreateDirectory(dir);
                }

                StreamWriter sw = new StreamWriter(path, false);
                doc.Save(sw);
                sw.Close();
                sw = null;
            }
            catch (Exception e)
            {
                AppFrame.FrameLogger.Fatal("保存插件列表失败！", e);
                MessageBox.Show("保存插件列表失败！", "注意！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public ServiceBase GetService(string name)
        {
            ServiceBase service = null;

            try
            {
                service = _services[name];
            }
            catch
            {
                AppFrame.FrameLogger.Info("获取服务" + name + "失败！请确认注册插件时填入了正确的服务名称。");
            }

            return service;
        }

        public T GetService<T>()
            where T: class
        {
            string str = typeof(T).ToString();
            if (_services.ContainsKey(str))
                return _services[str] as T;
            foreach(KeyValuePair<string,ServiceBase> ser in _services)
            {
                if(ser.Value is T)
                    return (ser.Value as T);
            }

            return null;
        }
    }
}

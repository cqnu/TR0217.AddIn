using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using AddIn.Gui.Parser;
using System.IO;
using AddIn.Core;
using System.ComponentModel;
using System.Reflection;
using System.Collections;
using AddIn.Gui.Properties;


namespace AddIn.Gui.Loader
{
    internal class UiLoader
    {
        private Form _form;
        private XmlDocument _configFile;
        [BrowsableAttribute(false)]
        public System.Xml.XmlDocument ConfigFile
        {
            get { return _configFile; }
            set { _configFile = value; }
        }
        private string _image;
        private string _caption;

        private MenuStripParser _menuStripParser;
        private ToolStripContainerParser _toolStripContainerParser;
        private StatusStripParser _statusStripParser;
        private ContextMenuStripContainerParser _contextMenuContainerParser;

        private Dictionary<string, ContextMenuStrip> _contextMenuStrips;
        private Dictionary<string, ToolStrip> _toolStrips;
        private Dictionary<string, ToolStripItem> _toolStripitems;

        private Dictionary<string, EventHandler> _eventHandler;

        //use for reload ui, after edit the config file.
        private List<UiElemParser> _modified;
        private List<UiElemParser> _added;
        private List<UiElemParser> _removed;

        [BrowsableAttribute(false)]
        internal MenuStripParser MenuStripParser
        {
            get { return _menuStripParser; }
        }

        [BrowsableAttribute(false)]
        internal ToolStripContainerParser ToolStripContainerParser
        {
            get { return _toolStripContainerParser; }
        }

        [BrowsableAttribute(false)]
        internal StatusStripParser StatusStripParser
        {
            get { return _statusStripParser; }
        }

        [BrowsableAttribute(false)]
        internal ContextMenuStripContainerParser ContextMenuContainerParser
        {
            get { return _contextMenuContainerParser; }
        }

        [BrowsableAttribute(false)]
        internal Dictionary<string, ContextMenuStrip> ContextMenuStrips
        {
            get { return _contextMenuStrips; }
        }

        [BrowsableAttribute(false)]
        internal Dictionary<string, ToolStripItem> ToolStripitems
        {
            get { return _toolStripitems; }
        }

        [BrowsableAttribute(false)]
        internal Dictionary<string, ToolStrip> ToolStrips
        {
            get { return _toolStrips; }
        }

        [BrowsableAttribute(false)]
        internal Form Form
        {
            get { return _form; }
            set { _form = value; }
        }

        [Editor(typeof(ImagePathEditor), typeof(System.Drawing.Design.UITypeEditor))] 
        public string Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        public UiLoader()
        {
            _configFile = null;
            _image = null;
            _menuStripParser = new MenuStripParser(this);
            _toolStripContainerParser = new ToolStripContainerParser(this);
            _statusStripParser = new StatusStripParser(this);
            _contextMenuContainerParser = new ContextMenuStripContainerParser(this);

            _contextMenuStrips = new Dictionary<string, ContextMenuStrip>();
            _toolStrips = new Dictionary<string, ToolStrip>();
            _toolStripitems = new Dictionary<string, ToolStripItem>();

            _eventHandler = new Dictionary<string, EventHandler>();

            _modified = new List<UiElemParser>();
            _added = new List<UiElemParser>();
            _removed = new List<UiElemParser>();
        }

        public bool ReadConfig(string xmlFile)
        {
            try
            {
                _configFile = new XmlDocument();
                _configFile.Load(xmlFile);
                XmlElement elem = _configFile.DocumentElement;
                _image = elem.GetAttribute("image");
                _caption = elem.GetAttribute("caption");

                return true;
            }
            catch (XmlException xe)
            {
                AppFrame.FrameLogger.Fatal("��ȡ��������ʧ�ܣ�", xe);
                MessageBox.Show(xe.Message, "ע�⣡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            catch (Exception e)
            {
                AppFrame.FrameLogger.Fatal("��ȡ��������ʧ�ܣ�", e);
                MessageBox.Show(e.Message, "ע�⣡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        public void SaveConfig(String configPath)
        {
            try
            {
                _configFile = new XmlDocument();
                XmlElement rootElem = _configFile.CreateElement("UiConfig");
                rootElem.SetAttribute("image", _image);
                rootElem.SetAttribute("caption", _caption);
                rootElem.AppendChild(_menuStripParser.ToXmlNode(_configFile));
                rootElem.AppendChild(_toolStripContainerParser.ToXmlNode(_configFile));
                rootElem.AppendChild(_statusStripParser.ToXmlNode(_configFile));
                rootElem.AppendChild(_contextMenuContainerParser.ToXmlNode(_configFile));

                _configFile.AppendChild(rootElem);

                StreamWriter sw = new StreamWriter(configPath, false);
                _configFile.Save(sw);
                sw.Close();
                sw = null;
            }
            catch (XmlException xe)
            {
                MessageBox.Show(xe.Message, "ע�⣡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ע�⣡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void InitialConfig(string configPath)
        {
            try
            {
                _configFile = new XmlDocument();
                _configFile.LoadXml(Resources.UI);

                String dir = Path.GetDirectoryName(configPath);

                if (Directory.Exists(dir) && File.Exists(configPath))
                {
                    File.Delete(configPath);
                }
                else
                {
                    Directory.CreateDirectory(dir);
                }

                StreamWriter sw = new StreamWriter(configPath, false);
                _configFile.Save(sw);
                sw.Close();
                sw = null;
            }
            catch (Exception e)
            {
                AppFrame.FrameLogger.Fatal("��ʼ����������ʧ�ܣ�", e);
                MessageBox.Show(e.Message, "ע�⣡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
       
        /// <summary>
        /// Ϊ����װ�ؽ���Ԫ�ء�
        /// </summary>
        /// <param name="b">�Ƿ���װ��������,true��ʾװ��������</param>
        /// <returns>װ����ɵĵĴ���</returns>
        public Form Load(bool b)
        {
            //��������½���װ��
            _menuStripParser.UiElemParserList.Clear();
            _toolStripContainerParser.UiElemParserList.Clear();           
            _statusStripParser.UiElemParserList.Clear();
            _contextMenuContainerParser.UiElemParserList.Clear();

            _toolStrips.Clear();
            _contextMenuStrips.Clear();
            _toolStripitems.Clear();

            if (_form == null)
                _form = new Form();

            _form.SuspendLayout();

            if (b)
            {
                Icon icon = null;
                if (_image != string.Empty)
                {
                    string imgPath = _image;
                    if (_image.StartsWith("."))
                        imgPath = Application.StartupPath + _image.Substring(1);

                    try
                    {
                        Bitmap tempBmp = new Bitmap(imgPath);
                        icon = Icon.FromHandle(tempBmp.GetHicon());
                        tempBmp.Dispose();
                    }
                    catch(Exception e)
                    {
                        AppFrame.FrameLogger.Error("��ȡ����ͼ��ʧ�ܣ���ȷ�����ý���ʱΪ����ͼ��ָ������ȷ��·�������ߴ���ͼ���ļ��Ƿ���ڡ�", e);
                    }
                }

                _form.Icon = icon;
                _form.Text = _caption;
            }

            //�����ô����ܵ���ģ�������Ҫע���¼�
            XmlNodeList nl = _configFile.DocumentElement.ChildNodes;
            foreach (XmlNode n in nl)
            {
                switch (n.Name)
                {
                    case "menuStrip":
                        _menuStripParser.FromXmlNode(n);
                        break;
                    case "toolStripContainer":
                        _toolStripContainerParser.FromXmlNode(n);
                        break;
                    case "statusStrip":
                        _statusStripParser.FromXmlNode(n);
                        break;
                    case "contextMenus":
                        _contextMenuContainerParser.FromXmlNode(n);
                        break;
                    default: break;
                }
            }

            _form.ResumeLayout();

            return _form;
        }

        /// <summary>
        /// ΪUIԪ��ע���¼����˷���������Load����֮�����
        /// </summary>
        internal void RegistEvent(IServiceCollection sc)
        {
            RegistEvent(sc, _menuStripParser.UiElemParserList);
            RegistEvent(sc, _toolStripContainerParser.UiElemParserList);
            
            RegistEvent(sc, _statusStripParser.UiElemParserList);
            RegistEvent(sc, _contextMenuContainerParser.UiElemParserList);
        }

        private void RegistEvent(IServiceCollection sc, List<UiElemParser> uiElemParserList)
        {
            foreach (UiElemParser uip in uiElemParserList)
            {
                //get service object that complete function.
                if (!string.IsNullOrEmpty(uip.Service))
                {
                    ServiceBase service = sc.GetService(uip.Service);
                    if (service != null)
                    {
                        RegistUiStateHandler(uip, service);

                        InjectUiElem(uip, service);

                        //if the uiElem can raise event.
                        if (uip is CmdParser)
                            this.RegistUiFunctionEvent(uip as CmdParser, service, uiElemParserList);
                    }
                    else
                    {
                        AppFrame.FrameLogger.Error("��ʾ����Ϊ ��" + uip.Text + "�� �Ľ���Ԫ�������õķ��� " + uip.Service + " �����ڣ�");
                    }
                }

                RegistEvent(sc, uip.UiElemParserList);
            }
        }

        private void InjectUiElem(UiElemParser uip, ServiceBase service)
        {
            if (string.IsNullOrEmpty(uip.Injector))
                return;

            Type type = service.GetType();
            MethodInfo[] mis = type.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            ParameterInfo[] pis = null;
            MethodInfo mi = null;
            foreach (MethodInfo m in mis)
            {
                if (m.ToString() == uip.Injector)
                {
                    pis = m.GetParameters();
                    mi = m;
                    break;
                }
            }

            if (mi == null)
            {
                AppFrame.FrameLogger.Error("��ʾ����Ϊ ��" + uip.Text + "�� �Ľ���Ԫ���������ķ��� " + uip.Service + " �в����ڷ��� " + uip.Injector);
                return;
            }

            if (pis.Length != 1)
            {
                AppFrame.FrameLogger.Error("���� " + uip.Service + " �ĳ�Ա���� " + uip.Injector + "δ��Ϊע�����Ԫ��" + uip.Text + " �ṩ��������Ĳ�����");
                return;
            }

            object[] paras = new object[pis.Length];
            try
            {                
                paras[0] = Convert.ChangeType(uip.UiElem, pis[0].ParameterType);
            }
            catch (Exception e)
            {
                AppFrame.FrameLogger.Error("���� " + uip.Service + " �ĳ�Ա���� " + uip.Injector + "δ��Ϊע�����Ԫ��" + uip.Text + " �ṩ��������Ĳ�����", e);
            }

            try
            {
                mi.Invoke(service, paras);
            }
            catch (System.Exception ex)
            {
                AppFrame.FrameLogger.Error(ex.Message, ex);
            }
        }

        private void RegistUiFunctionEvent(CmdParser cp, ServiceBase service, List<UiElemParser> uiElemParserList)
        {
            if (string.IsNullOrEmpty(cp.Function))
                return;

            //get info to prepare parameters and to create eventhandler.
            Type type = service.GetType();
            MethodInfo[] mis = type.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            ParameterInfo[] pis = null;
            MethodInfo mi = null;
            foreach (MethodInfo m in mis)
            {
                if (m.ToString() == cp.Function)
                {
                    pis = m.GetParameters();
                    mi = m;
                    break;
                }
            }

            if (mi == null)
            {
                AppFrame.FrameLogger.Error("��ʾ����Ϊ ��" + cp.Text + "�� �Ľ���Ԫ�������õķ��� " + cp.Service + " �в����ڷ��� " + cp.Function);
                return;
            }

            ArrayList parameters = null;

            if (cp.Parameter != string.Empty)
            {
                parameters = PrepareParams(pis, cp);
            }

            EventHandler handler = null;
            string key = cp.Function + cp.Parameter + cp.ParamProvider;
            if (_eventHandler.ContainsKey(key))
            {
                handler = _eventHandler[key];
            }
            else
            {
                handler = CreateFuncEventHandler(service, mi, parameters, pis, cp, uiElemParserList);
                _eventHandler.Add(key, handler);
            }

            switch (cp.UiElemType)
            {
                case UiElemType.ComboBox:
                    ToolStripWatermarkComboBox tscb = cp.UiElem as ToolStripWatermarkComboBox;
                    if (handler != null)
                    {
                        tscb.SelectedIndexChanged += handler;
                        tscb.KeyDown += delegate(object sender, KeyEventArgs e)
                        {
                            if (e.KeyCode == Keys.Enter)
                                handler(tscb, new EventArgs());
                        };
                    }
                    break;
                case UiElemType.TextBox:
                    ToolStripWatermarkTextBox tstb = cp.UiElem as ToolStripWatermarkTextBox;
                    if (handler != null)
                        tstb.KeyDown += delegate(object sender, KeyEventArgs e)
                        {
                            if (e.KeyCode == Keys.Enter)
                                handler(tstb, new EventArgs());
                        };
                    break;
                case UiElemType.ProgressBar:
                    break;
                case UiElemType.SplitButton:
                    ToolStripSplitButton tssb = cp.UiElem as ToolStripSplitButton;
                    if (handler != null)
                        tssb.ButtonClick += handler;
                    break;
                default:
                    ToolStripItem tsi = cp.UiElem as ToolStripItem;
                    if (handler != null)
                        tsi.Click += handler;
                    break;
            }
        }

        //regist eventhandler to update ui logic.
        private void RegistUiStateHandler(UiElemParser uip, ServiceBase service)
        {
            if (string.IsNullOrEmpty(uip.UpdateEvent))
                return;

            Type type = service.GetType();
            EventInfo eventInfo = type.GetEvent(uip.UpdateEvent, BindingFlags.Instance | BindingFlags.Public);
            UpdateUiElemHandler handler = null;

            if (eventInfo == null)
            {
                //�ڴ�Ӧ�ü�¼��־
                AppFrame.FrameLogger.Error("��ʾ����Ϊ ��" + uip.Text + "�� �Ľ���Ԫ�������õķ��� " + uip.Service + " �в������¼� " + uip.UpdateEvent);
                return;
            }

            if (uip.UiElem is ToolStripItem)
            {
                ToolStripItem tsi = uip.UiElem as ToolStripItem;

                if (uip.UiElemType == UiElemType.Button)
                {
                    ToolStripButton tsb = uip.UiElem as ToolStripButton;
                    handler = delegate(object sender, UpdateUiElemEventArgs e)
                    {
                        tsb.Checked = e.Checked;
                        tsb.Enabled = e.Enabled;
                        tsb.Visible = e.Visible;
                        if (e.Text != string.Empty)
                            tsb.Text = e.Text;
                    };
                }
                else if (uip.UiElemType == UiElemType.MenuItem)
                {
                    ToolStripMenuItem tsmi = uip.UiElem as ToolStripMenuItem;
                    handler = delegate(object sender, UpdateUiElemEventArgs e)
                    {
                        tsmi.Checked = e.Checked;
                        tsmi.Enabled = e.Enabled;
                        tsmi.Visible = e.Visible;
                        if (e.Text != string.Empty)
                            tsmi.Text = e.Text;
                    };
                }
                else if (uip.UiElemType == UiElemType.ProgressBar)
                {
                    ToolStripProgressBar tspb = uip.UiElem as ToolStripProgressBar;
                    handler = delegate(object sender, UpdateUiElemEventArgs e)
                    {
                        try
                        {
                            tspb.Enabled = e.Enabled;
                            tspb.Visible = e.Visible;
                            int value = e.Count;
                            if (e.Maximum > 0)
                            {
                                value = value * tspb.Maximum / e.Maximum;
                            }
                            if (value > -1)
                                tspb.Value = value;
                        }
                        catch
                        {
                        }
                    };
                }
                else if (uip.UiElemType == UiElemType.ComboBox)
                {
                    ToolStripWatermarkComboBox tscb = uip.UiElem as ToolStripWatermarkComboBox;
                    handler = delegate(object sender, UpdateUiElemEventArgs e)
                    {
                        tscb.Enabled = e.Enabled;
                        tscb.Visible = e.Visible;
                        if (e.Text != string.Empty)
                        {
                            tscb.Text = e.Text;
                            if (e.Value != null)
                            {
                                tscb.Items.Insert(0, e.Value);
                                if (tscb.Items.Count > e.Maximum)
                                {
                                    tscb.Items.RemoveAt(tscb.Items.Count - 1);
                                }
                            }
                        }
                        else
                        {
                            if (e.Count > -1 && e.Count < tscb.Items.Count)
                                tscb.SelectedIndex = e.Count;
                            else if (e.Value != null)
                                tscb.SelectedItem = e.Value;
                        }
                    };
                }
                //else if (cp.UiElemType == UiElemType.TextBox)
                //{
                //    ToolStripTextBox tstb = cp.UiElem as ToolStripTextBox;
                //    handler = delegate(object sender, UpdateUiElemEventArgs e)
                //    {
                //        tstb.Enabled = e.Enabled;
                //        tstb.Text = e.Text;
                //    };
                //}
                else if (uip.UiElemType == UiElemType.ComboBoxImage)
                {
                    throw new NotImplementedException("not implemented!");
                }
                else
                {
                    handler = delegate(object sender, UpdateUiElemEventArgs e)
                    {
                        tsi.Enabled = e.Enabled;
                        tsi.Visible = e.Visible;
                        if (e.Text != string.Empty)
                            tsi.Text = e.Text;
                    };
                }
            }
            else if (uip.UiElem is ToolStrip)
            {
                ToolStrip ts = uip.UiElem as ToolStrip;
                handler = delegate(object sender, UpdateUiElemEventArgs e)
                {
                    ts.Enabled = e.Enabled;
                    ts.Visible = e.Visible;
                };
            }

            if(handler != null)
                eventInfo.AddEventHandler(service, handler);
        }

        private EventHandler CreateFuncEventHandler(ServiceBase service, MethodInfo mi, ArrayList parameters, ParameterInfo[] pis, CmdParser cp, List<UiElemParser> uiElemParserList)
        {

            if (pis.Length == 0)
            {
                return delegate(object sender, EventArgs e)
                {
                    try
                    {
                        mi.Invoke(service, null);
                    }
                    catch (System.Exception ex)
                    {
                        AppFrame.FrameLogger.Error(ex.Message, ex);
                    }
                };
            }
            else
            {
                int index = uiElemParserList.IndexOf(cp);
                int providerIndex = -1;
                bool b = true;
                if (cp.ParamProvider != string.Empty)
                {
                    //һ������ʹ��ĳ��Ԫ��ǰ��Ԫ�صò���
                    for (int i = index; i > -1; i--)
                    {
                        if (uiElemParserList[i].Name == cp.ParamProvider)
                        {
                            providerIndex = i;
                            b = false;
                            break;
                        }
                    }

                    //������ʹ�÷�Χ��û���ҵ�
                    if (b)//if did not find in the first traverse.
                    {
                        for (int i = index + 1; i < uiElemParserList.Count; i++)
                        {
                            if (uiElemParserList[i].Name == cp.ParamProvider)
                            {
                                providerIndex = i;
                                break;
                            }
                        }
                    }
                }

                object[] paras = new object[pis.Length];

                if (providerIndex == -1)//û���ṩ��̬�����ý���Ԫ��
                {
                    if (parameters == null || parameters.Count < pis.Length)
                    {
                        AppFrame.FrameLogger.Error("�ڽ���Ԫ�� ��"+ cp.Text + "�� ��δ��Ϊ���� " + cp.Service + " �ĳ�Ա���� " + cp.Function + " �ṩ�㹻�Ĳ�����");
                        return null;//��¼��־��������
                    }
                    if (parameters.Count >= pis.Length)
                    {
                        parameters.CopyTo(0, paras, 0, pis.Length);
                        return delegate(object sender, EventArgs e)
                        {
                            try
                            {
                                mi.Invoke(service, paras);
                            }
                            catch (System.Exception ex)
                            {
                                AppFrame.FrameLogger.Error(ex.Message, ex);
                            }
                        };
                    }
                }
                else //���ṩ��̬�����ý���Ԫ��
                {
                    if (pis.Length > 1)
                    {
                        if (parameters == null)
                        {
                            AppFrame.FrameLogger.Error("�ڽ���Ԫ�� ��" + cp.Text + "�� ��δ��Ϊ���� " + cp.Service + " �ĳ�Ա���� " + cp.Function + " �ṩ�㹻�Ĳ�����");
                            return null;//��¼��־��������
                        }
                        int count = pis.Length - 1;
                        if (parameters.Count < count)
                        {
                            AppFrame.FrameLogger.Error("�ڽ���Ԫ�� ��" + cp.Text + "�� ��δ��Ϊ���� " + cp.Service + " �ĳ�Ա���� " + cp.Function + " �ṩ�㹻�Ĳ�����");
                            return null;//��¼��־��������
                        }
                        parameters.CopyTo(0, paras, 1, count);
                    }

                    UiElemParser uip1 = uiElemParserList[providerIndex];
                    ToolStripItem tsi1 = uip1.UiElem as ToolStripItem;

                    if (pis[0].ParameterType == typeof(bool))
                    {
                        if (uip1.UiElemType == UiElemType.MenuItem)
                        {
                            ToolStripMenuItem tsmi = uip1.UiElem as ToolStripMenuItem;
                            return delegate(object sender, EventArgs e)
                            {
                                paras[0] = tsmi.Checked;
                                try
                                {
                                    mi.Invoke(service, paras);
                                }
                                catch (System.Exception ex)
                                {
                                    AppFrame.FrameLogger.Error(ex.Message, ex);
                                }
                            };
                        }
                        else if (uip1.UiElemType == UiElemType.Button)
                        {
                            ToolStripButton tsb = uip1.UiElem as ToolStripButton;
                            return delegate(object sender, EventArgs e)
                            {
                                paras[0] = tsb.Checked;
                                try
                                {
                                    mi.Invoke(service, paras);
                                }
                                catch (System.Exception ex)
                                {
                                    AppFrame.FrameLogger.Error(ex.Message, ex);
                                }                                
                            };
                        }
                    }
                    else
                    {
                        return delegate(object sender, EventArgs e)
                        {
                            try
                            {
                                paras[0] = Convert.ChangeType(tsi1.Text, pis[0].ParameterType);
                                mi.Invoke(service, paras);
                            }
                            catch (Exception ex)
                            {
                                AppFrame.FrameLogger.Error(ex.Message, ex);
                            }
                        };
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pis"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private ArrayList PrepareParams(ParameterInfo[] pis, CmdParser cp)
        {
            ArrayList paras = new ArrayList();
            
            try
            {
                string[] parameters = cp.Parameter.Split(',');
                int lenPara = parameters.Length;
                int lenPis = pis.Length;
                int k = 0;
                if (cp.ParamProvider != string.Empty)
                {
                    k = 1;
                }

                int length = lenPara < lenPis ? lenPara : lenPis-k;
                
                for (int i = 0; i < length; i++)
                {
                    if (pis[k].ParameterType == typeof(string))
                    {
                        paras.Add(parameters[i]);
                    }
                    else
                    {
                        paras.Add(Convert.ChangeType(parameters[i], pis[k].ParameterType));
                    }
                    k++;
                }
            }
            catch (Exception e)
            {
                AppFrame.FrameLogger.Error("�ڽ���Ԫ��" + cp.Text + "��Ϊ����" + cp.Service + "�ĳ�Ա����" + cp.Function + "�ṩ�Ĳ����޷�������", e);
                MessageBox.Show("�ڽ���Ԫ��" + cp.Text + "��Ϊ����" + cp.Service + "�ĳ�Ա����" + cp.Function + "�ṩ�Ĳ����޷�������");
            }


            if (paras.Count > 0)
                return paras;
            else
                return null;
        }
    }
}

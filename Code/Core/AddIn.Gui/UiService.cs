using System;
using System.Collections.Generic;
using System.Text;
using AddIn.Core;
using AddIn.Gui.Loader;
using System.Windows.Forms;
using AddIn.Gui.Parser;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using AddIn.UiInterface;
using System.Xml;
using AddIn.Gui.Properties;

namespace AddIn.Gui
{
    public class UiService : ServiceBase, IUiExService
    {
        private UiLoader _uiLoader;
        private AddInModifyForm _addinModifyForm = null;
        private MainForm _mainForm = null;

        public Form MainForm
        {
            get { return _mainForm; }
        }

        private string _configPath;

        public UiService()
        {
            _configPath = Application.StartupPath + "\\Config\\UI.xml";
            _uiLoader = new UiLoader();
            if (!File.Exists(_configPath) || !_uiLoader.ReadConfig(_configPath))
            {
                _uiLoader.InitialConfig(_configPath);
            }
        }

        private bool _addinModifyFormLoaded = false;

        public void ModifyAddIns()
        {
            if (!_addinModifyFormLoaded)
            {
                try
                {
                    if (_addinModifyForm == null)
                    {
                        _addinModifyForm = new AddInModifyForm();
                        UiLoader uiLoader = new UiLoader();
                        uiLoader.Caption = _uiLoader.Caption;
                        uiLoader.Image = _uiLoader.Image;
                        uiLoader.ConfigFile = _uiLoader.ConfigFile;
                        uiLoader.Form = _addinModifyForm;
                        uiLoader.MenuStripParser.SetUiElem(_addinModifyForm.MenuStrip);
                        uiLoader.ToolStripContainerParser.SetUiElem(_addinModifyForm.ToolStripContainer);
                        uiLoader.StatusStripParser.SetUiElem(_addinModifyForm.StatusStrip);
                        uiLoader.Load(false);
                        _addinModifyForm.UiLoader = uiLoader;
                        _addinModifyForm.ServiceCollection = AppFrame.ServiceCollection;
                    }                    
                }
                catch(Exception e)
                {
                    MessageBox.Show("加载界面元素失败！请检查日志获取详细信息。");
                    AppFrame.FrameLogger.Fatal("加载界面元素失败！", e);
                }

                _addinModifyFormLoaded = true;
            }
            _addinModifyForm.ShowDialog();
        }

        public void InitialUiServiceInfo(AddInParser ap)
        {
            ap.Lazyload = false;
            ap.IsBaseService = false;
            ap.Version = "1.5.0.0";
            ap.Url = "http://tr0217.blog.163.com/";
            ap.Author = "tr0217";
            ap.Copyright = "CopyLeft";
            ap.Description = "a lot thanks will come to you for your advice.";
        }

        public Form LoadMainForm()
        {

            if(_mainForm == null)
                _mainForm = new MainForm();
            _uiLoader.Form = _mainForm;
            _uiLoader.MenuStripParser.SetUiElem(_mainForm.MenuStrip);
            _uiLoader.ToolStripContainerParser.SetUiElem(_mainForm.ToolStripContainer);
            _uiLoader.StatusStripParser.SetUiElem(_mainForm.StatusStrip);
            try
            {
                _uiLoader.Load(true);
                _uiLoader.RegistEvent(AppFrame.ServiceCollection);
            }
            catch (Exception e)
            {
                //MessageBox.Show("Some problems happened when load mian form." );
                MessageBox.Show("加载系统主窗体出错！");
                AppFrame.FrameLogger.Fatal("加载系统主窗体出错！", e);
            }
            
            return _mainForm;
        }

        public void ShowDocForm(Form docForm)
        {
            DockContent dc = docForm as DockContent;
            dc.Show(_mainForm.DockPanel);
        }

        public void ShowToolWin(Form toolWin, DockStyle dockStyle)
        {
            DockContent dc = toolWin as DockContent;
            dc.Show(_mainForm.DockPanel);
            dc.DockTo(_mainForm.DockPanel, dockStyle);
        }

        /// <summary>
        /// Hide or show ToolStrip 
        /// </summary>
        /// <param name="visible">true to show,false to hide</param>
        /// <param name="name">ToolStrip's name</param>
        public void SetToolStripVisible(bool visible, string name)
        {
            if (string.IsNullOrEmpty(name))
                return;
            _uiLoader.ToolStrips[name].Visible = visible;
        }

        /// <summary>
        /// Hide or show StatusStrip
        /// </summary>
        /// <param name="visible">true to show,false to hide</param>
        public void SetStatusStripVisible(bool visible)
        {
            _mainForm.StatusStrip.Visible = visible;
        }

        public ToolStrip GetToolStrip(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            try
            {
                return _uiLoader.ToolStrips[name];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// get ToolStripItem
        /// </summary>
        /// <param name="path">make up by Text and "/"</param>
        public ToolStripItem GetToolStripItem(string path)
        {
            try
            {
                return _uiLoader.ToolStripitems[path];
            }
            catch
            {
                return null;
            }
        }

        public ContextMenuStrip GetContextMenuStrip(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            try
            {
                return _uiLoader.ContextMenuStrips[name];
            }
            catch
            {
                return null;
            }
        }

        public void ShowInfoMsg(string msg)
        {
            MessageBox.Show(_mainForm, msg, _mainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ShowInfoMsg(string msg, string caption)
        {
            MessageBox.Show(_mainForm, msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ShowWarningMsg(string msg)
        {
            MessageBox.Show(_mainForm, msg, _mainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void ShowWarningMsg(string msg, string caption)
        {
            MessageBox.Show(_mainForm, msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void ShowErrorMsg(string msg)
        {
            MessageBox.Show(_mainForm, msg, _mainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowErrorMsg(string msg, string caption)
        {
            MessageBox.Show(_mainForm, msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public DialogResult PromptYesNo(string msg)
        {
            return MessageBox.Show(_mainForm, msg, _mainForm.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public DialogResult PromptYesNo(string msg, string caption)
        {
            return MessageBox.Show(_mainForm, msg, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public DialogResult PromptYesNoCancel(string msg)
        {
            return MessageBox.Show(_mainForm, msg, _mainForm.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        public DialogResult PromptYesNoCancel(string msg, string caption)
        {
            return MessageBox.Show(_mainForm, msg, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        public DialogResult PromptOKCancel(string msg)
        {
            return MessageBox.Show(_mainForm, msg, _mainForm.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }

        public DialogResult PromptOKCancel(string msg, string caption)
        {
            return MessageBox.Show(_mainForm, msg, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }

        public DialogResult PromptRetryCancel(string msg)
        {
            return MessageBox.Show(_mainForm, msg, _mainForm.Text, MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
        }

        public DialogResult PromptRetryCancel(string msg, string caption)
        {
            return MessageBox.Show(_mainForm, msg, caption, MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
        }

        public DialogResult PromptAbortRetryIgnore(string msg)
        {
            return MessageBox.Show(_mainForm, msg, _mainForm.Text, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Question);
        }

        public DialogResult PromptAbortRetryIgnore(string msg, string caption)
        {
            return MessageBox.Show(_mainForm, msg, caption, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Question);
        }

        public void Exit()
        {
            Application.Exit();
        }

        public void Execute(string exe,string parameter)
        {
            System.Diagnostics.Process.Start(exe, parameter);
        }

        public void Exexute(string exe)
        {
            System.Diagnostics.Process.Start(exe);
        }

        ConfigForm _configForm = null;
        public override void Config()
        {
            if (_configForm == null || _configForm.IsDisposed)
                _configForm = new ConfigForm(AppFrame.ServiceCollection);

            _configForm.ShowDialog();
        }
    }
}

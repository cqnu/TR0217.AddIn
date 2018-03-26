using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Drawing;

namespace AddIn.Core
{
    public class AppFrame
    {
        private static AppFrame _appFrame = null;
        
        private static log4net.ILog _frameLogger = log4net.LogManager.GetLogger("AddIn.Core");
        private static ServiceCollection _addInCollection = new ServiceCollection();

        private ILoginDialog _loginDialog;
        private ISplashScreen _splashScreen;
        private Form _mainForm;
        private string[] _args;
        
        private Thread _splashScreenThread;
        private UInt16 _splashInterval = 3000;

        private bool _finishInitial = false;
        private volatile bool _tick = false;

        private System.Threading.Timer _timer;

        public static bool FireEvent = true;
        public static event StartUpHandler   StartUp;
        public static event LoadAddInHandler AfterLoadOneAddIn;
        public static event LoadAddInHandler BeforeLoadOneAddIn;
        public static event LoadAddInHandler FinishLoadAddIn;
        public static event LoadMainFormHandler BeforeLoadMainForm;
        public static event LoadMainFormHandler AfterLoadMainForm;

        public static AppFrame Instance
        {
            get 
            {
                if (_appFrame == null)
                    new AppFrame();
                return _appFrame;  
            }
        }

        public static AppFrame GetInstance()
        {
            if (_appFrame == null)
                new AppFrame();
            return _appFrame;
        }

        public AppFrame()
        {
            if (_appFrame == null)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                _timer = new System.Threading.Timer(CloseSplash, null, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                _addInCollection.BeforLoadeOneAddIn += new LoadAddInHandler(_addInCollection_BeforLoadeOneAddIn);
                _addInCollection.AfterLoadOneAddIn += new LoadAddInHandler(_addInCollection_AfterLoadOneAddIn);
                _appFrame = this;
            }
            else
            {
                throw new ApplicationException(" Only one instance of AppFrame is allowed!");
            }
        }

        public static IServiceCollection GetServiceCollection()
        {
            return _addInCollection;
        }

        public static IServiceCollection ServiceCollection
        {
            get { return _addInCollection; }
        }

        public static log4net.ILog FrameLogger
        {
            get { return _frameLogger; }
        }

        /// <summary>
        /// SplashScreen,if don't need, set to null.
        /// </summary>
        public ISplashScreen SplashScreen
        {
            get { return _splashScreen; }
            set { _splashScreen = value; }
        }

        public  Form MainForm
        {
            get { return _mainForm; }
        }

        public string[] Args
        {
            get { return _args; }
            set { _args = value; }
        }

        /// <summary>
        /// how long the splash screen will last in millisecond.
        /// </summary>
        public UInt16 SplashInterval
        {
            get 
            { 
                return _splashInterval; 
            }
            set
            {
                if (value > 0)
                {
                    _splashInterval = value;
                }
            }
        }

        private void _addInCollection_AfterLoadOneAddIn(LoadAddInEventArgs e)
        {
            if (FireEvent && AfterLoadOneAddIn != null)
                AfterLoadOneAddIn(e);
        }

        private void _addInCollection_BeforLoadeOneAddIn(LoadAddInEventArgs e)
        {
            if (FireEvent && BeforeLoadOneAddIn != null)
                BeforeLoadOneAddIn(e);
        }

        private void CloseSplash(object state)
        {
            _tick = true;
            //�����ʱ������ǻ�û�м�����ɾͲ��ر�
            //���ҹر�����������Ͳ��ɼ�ʱ����ִ����
            if (_finishInitial)
            {
                _timer.Dispose();
                _timer = null;
                _splashScreen.CloseSplash();
                _splashScreen = null;
            }
            else
            {
                _timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            }
        }

        private void Splash()
        {
            if (_loginDialog != null)
            {
                if (!_loginDialog.ShowDialog() || !_loginDialog.Valid)
                    Environment.Exit(0);
            }

            if (_splashScreen != null)
            {
                _timer.Change(_splashInterval, System.Threading.Timeout.Infinite);
                _splashScreen.ShowSplash();
            }
        }

        /// <summary>
        /// ���ز��������ʵ�����еĳ�����봫��true
        /// </summary>
        /// <param name="splash">�Ƿ���ʾ��¼���ڻ�������</param>
        /// <returns>����Ƿ���سɹ�</returns>
        private bool LoadServices(bool splash = true)
        {
            _addInCollection.LoadBaseServices();
            if (FireEvent && StartUp != null)
            {
                StartUpEventArgs arg = new StartUpEventArgs(_addInCollection);
                StartUp(arg);
                if (arg.Cancel)
                {
                    return false;
                }
                _loginDialog = arg.LoginDialog;
                if (arg.SplashScreen != null)
                {
                    _splashScreen = arg.SplashScreen;
                }
            }

            if (splash)
            {
                try
                {
                    // run splashscreen 
                    _splashScreenThread = new Thread(new ThreadStart(Splash));
                    _splashScreenThread.Start();
                }
                catch (System.Exception ex)
                {
                    FrameLogger.Fatal("����SplashScreen�߳�ʧ�ܣ�", ex);
                    MessageBox.Show(null, "����SplashScreen�߳�ʧ�ܣ�", "���棡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            try
            {
                _addInCollection.Load();

                try
                {
                    if (FireEvent && FinishLoadAddIn != null)
                        FinishLoadAddIn(new LoadAddInEventArgs(null, _addInCollection));
                }
                catch (System.Exception e)
                {
                    FrameLogger.Fatal("�ڴ���FinishLoadAddIn�¼�ʱ�׳��쳣��", e);
                    MessageBox.Show(null, "�����в���������֮�����쳣�����", "���棡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (System.Exception ex)
            {
                //�����������ʧ��ҲӦ�ùر�����
                //flag ,true to indicate timer can close splashscreen
                _finishInitial = true;

                FrameLogger.Fatal("���ز�������г����������⣡", ex);
                MessageBox.Show(null, "���������г����������⣡", "���棡", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (splash)
                {
                    //�������������ɣ����ܳɹ���ʧ�ܣ���
                    //�����Ѿ���������С����ʱ�䣨_tick=true��
                    //����Ҫ�ֶ��ر�����
                    if (_tick && _splashScreen != null)
                    {
                        _timer.Dispose();
                        _timer = null;
                        _splashScreen.CloseSplash();
                        _splashScreen = null;
                    }
                    _splashScreenThread.Join();
                }

                return false;
            }

            return true;
        }

        private void InnerRun(IUiService uiService)
        {
            try
            {
                if (FireEvent && BeforeLoadMainForm != null)
                    BeforeLoadMainForm(new LoadMainFormEventArgs(null, uiService, _args));
            }
            catch (Exception e)
            {
                FrameLogger.Fatal("�ڴ���BeforeLoadMainForm�¼�ʱ�׳��쳣��", e);
                MessageBox.Show(null, "�ڼ���ס����֮ǰ�����쳣�����", "���棡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            try
            {
                if (uiService != null)
                    _mainForm = uiService.LoadMainForm();

                try
                {
                    if (FireEvent && AfterLoadMainForm != null)
                        AfterLoadMainForm(new LoadMainFormEventArgs(_mainForm, uiService, _args));
                }
                catch (System.Exception e)
                {
                    FrameLogger.Fatal("�ڴ���AfterLoadMainForm�¼�ʱ�׳��쳣��", e);
                    MessageBox.Show(null, "�ڼ���ס����֮�����쳣�����", "���棡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch (System.Exception ex)
            {
                FrameLogger.Fatal("���������ʧ�ܣ�δ����������", ex);
                MessageBox.Show(null, "���������ʧ�ܣ�δ����������", "���棡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //flag ,true to indicate timer can close splashscreen
            _finishInitial = true;

            //�������������ɣ����ܳɹ���ʧ�ܣ���
            //�����Ѿ���������С����ʱ�䣨_tick=true��
            //����Ҫ�ֶ��ر�����
            if (_tick && _splashScreen != null)
            {
                _timer.Dispose();
                _timer = null;
                _splashScreen.CloseSplash();
                _splashScreen = null;
            }
            _splashScreenThread.Join();

            //run system
            if (_mainForm != null) Application.Run(_mainForm);
        }

        public void Run()
        {
            if (LoadServices(true))
            {
                //load mainform
                IUiService ui = null;
                try
                {
                    ui = _addInCollection.GetService<IUiService>();

                }
                catch (Exception e)
                {
                    FrameLogger.Fatal("��ȡUI����ʧ�ܣ�δ����������", e);
                    MessageBox.Show("��ȡUI����ʧ�ܣ�δ����������", "���棡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //MessageBox.Show("Fail to get AddIn.Gui.UiService, please make sure AddIn.Gui.UiService is correctly configured.", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                
                InnerRun(ui);
            }
        }

        public void Run(string uiService)
        {
            if (LoadServices(true))
            {
                //load mainform
                IUiService ui = null;
                try
                {
                    ui = (IUiService)_addInCollection.GetService(uiService);

                }
                catch (Exception e)
                {
                    FrameLogger.Fatal("��ȡUI����ʧ�ܣ�δ����������", e);
                    MessageBox.Show("��ȡUI����ʧ�ܣ�δ����������", "���棡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //MessageBox.Show("Fail to get AddIn.Gui.UiService, please make sure AddIn.Gui.UiService is correctly configured.", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                InnerRun(ui);                    
            }
        }

        public void RunConfig()
        {
            if (LoadServices(false))
            {
                //load mainform
                IUiService ui = null;
                try
                {
                    ui = _addInCollection.GetService<IUiService>();
                }
                catch (Exception e)
                {
                    FrameLogger.Fatal("��ȡUI����ʧ�ܣ�δ����������", e);
                    MessageBox.Show("��ȡUI����ʧ�ܣ�δ����������", "���棡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //MessageBox.Show("Fail to get AddIn.Gui.UiService, please make sure AddIn.Gui.UiService is correctly configured.", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (ui != null)
                    ui.ModifyAddIns();
            }
        }


        public void RunConfig(string uiService)
        {
            if (LoadServices(false))
            {
                //load mainform
                IUiService ui = null;
                try
                {
                    ui = (IUiService)_addInCollection.GetService(uiService);

                }
                catch (Exception e)
                {
                    FrameLogger.Fatal("��ȡUI����ʧ�ܣ�δ����������", e);
                    MessageBox.Show("��ȡUI����ʧ�ܣ�δ����������", "���棡", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //MessageBox.Show("Fail to get AddIn.Gui.UiService, please make sure AddIn.Gui.UiService is correctly configured.", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (ui != null)
                    ui.ModifyAddIns();
            }
        }
    }
}

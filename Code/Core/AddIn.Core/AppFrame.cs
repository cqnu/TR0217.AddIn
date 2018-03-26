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
            //如果计时到达，但是还没有加载完成就不关闭
            //并且关闭闪屏的任务就不由计时器来执行了
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
        /// 加载插件，对于实际运行的程序必须传入true
        /// </summary>
        /// <param name="splash">是否显示登录窗口或者闪屏</param>
        /// <returns>插件是否加载成功</returns>
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
                    FrameLogger.Fatal("启动SplashScreen线程失败！", ex);
                    MessageBox.Show(null, "启动SplashScreen线程失败！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    FrameLogger.Fatal("在处理FinishLoadAddIn事件时抛出异常。", e);
                    MessageBox.Show(null, "在所有插件加载完成之后发生异常情况。", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (System.Exception ex)
            {
                //如果加载任务失败也应该关闭闪屏
                //flag ,true to indicate timer can close splashscreen
                _finishInitial = true;

                FrameLogger.Fatal("加载插件过程中出现严重问题！", ex);
                MessageBox.Show(null, "启动过程中出现严重问题！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (splash)
                {
                    //如果加载任务完成（不管成功或失败），
                    //但是已经超过了最小闪屏时间（_tick=true）
                    //就需要手动关闭闪屏
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
                FrameLogger.Fatal("在处理BeforeLoadMainForm事件时抛出异常。", e);
                MessageBox.Show(null, "在加载住窗体之前发生异常情况。", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    FrameLogger.Fatal("在处理AfterLoadMainForm事件时抛出异常。", e);
                    MessageBox.Show(null, "在加载住窗体之后发生异常情况。", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch (System.Exception ex)
            {
                FrameLogger.Fatal("主窗体加载失败！未能启动程序。", ex);
                MessageBox.Show(null, "主窗体加载失败！未能启动程序。", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //flag ,true to indicate timer can close splashscreen
            _finishInitial = true;

            //如果加载任务完成（不管成功或失败），
            //但是已经超过了最小闪屏时间（_tick=true）
            //就需要手动关闭闪屏
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
                    FrameLogger.Fatal("获取UI服务失败！未能启动程序。", e);
                    MessageBox.Show("获取UI服务失败！未能启动程序。", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    FrameLogger.Fatal("获取UI服务失败！未能启动程序。", e);
                    MessageBox.Show("获取UI服务失败！未能启动程序。", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    FrameLogger.Fatal("获取UI服务失败！未能启动程序。", e);
                    MessageBox.Show("获取UI服务失败！未能启动程序。", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    FrameLogger.Fatal("获取UI服务失败！未能启动程序。", e);
                    MessageBox.Show("获取UI服务失败！未能启动程序。", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //MessageBox.Show("Fail to get AddIn.Gui.UiService, please make sure AddIn.Gui.UiService is correctly configured.", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (ui != null)
                    ui.ModifyAddIns();
            }
        }
    }
}

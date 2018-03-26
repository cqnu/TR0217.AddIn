using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AddIn.Core;

namespace hello_cs
{
    static class Program
    {
        private static AppFrame app;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            app = new AppFrame();
            app.LoginDialog = new LoginDialog();
            app.SplashScreen = new SplashWin();
            app.SplashInterval = 8000;
            AppFrame.BeforeLoadOneAddIn += new LoadAddInHandler(AppFrame_BeforeLoadOneAddIn);
            AppFrame.FinishLoadAddIn += new LoadAddInHandler(AppFrame_FinishLoadAddIn);
            AppFrame.BeforeLoadMainForm += new LoadMainFormHandler(AppFrame_BeforeLoadMainForm);
            AppFrame.AfterLoadMainForm += new LoadMainFormHandler(AppFrame_AfterLoadMainForm);
            app.Run();
        }

        static void AppFrame_FinishLoadAddIn(LoadAddInEventArgs e)
        {
            string info = "插件加载完毕。";
            app.SplashScreen.SetInfo(info);
        }

        static void AppFrame_AfterLoadMainForm(LoadMainFormEventArgs e)
        {
            string info = "主窗体加载完成。";
            app.SplashScreen.SetInfo(info);
        }

        static void AppFrame_BeforeLoadMainForm(LoadMainFormEventArgs e)
        {
            string info = "正在加载主窗体......";
            app.SplashScreen.SetInfo(info);
        }


        static void AppFrame_BeforeLoadOneAddIn(LoadAddInEventArgs e)
        {
            string info = "正在加载" + e.AddInParser.Name
                + System.Environment.NewLine
                + "作者：" + e.AddInParser.Author
                + "Copyright:" + e.AddInParser.Copyright;
            app.SplashScreen.SetInfo(info);
        }
    }
}
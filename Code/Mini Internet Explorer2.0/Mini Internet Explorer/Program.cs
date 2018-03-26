using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AddIn.Core;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Mini_Internet_Explorer
{
    static class Program
    {
        //private static Mutex mutex = new Mutex(false, "ThisShouldOnlyRunOnce");
        //private static MessageQueue queue = null;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Process instance = RunningInstance(); if (instance != null)
            //{
            //    MessageBox.Show("程序已经运行！");
            //    return;
            //}

            //queue = new MessageQueue(".\\Private$\\Mini_Internet_Explorer");
            //bool Running = !mutex.WaitOne(0, false);
            //if (Running)
            //{
            //    MessageBox.Show("程序已经运行！");
            //    //if (args.Length > 0)
            //    //    queue.Send(args[0]);
            //    return;
            //}
            //else
            //{
            //    queue = MessageQueue.Create(".\\Private$\\Mini_Internet_Explorer",false);
            //}
            AppFrame app = AppFrame.GetInstance();
            app.SplashInterval = 3500;
            SplashWin splash = new SplashWin();
            app.SplashScreen = splash;
            app.Args = args;
            AppFrame.BeforeLoadOneAddIn += new LoadAddInHandler(AppFrame_BeforeLoadOneAddIn);
            AppFrame.FinishLoadAddIn += new LoadAddInHandler(AppFrame_FinishLoadAddIn);
            AppFrame.BeforeLoadMainForm += new LoadMainFormHandler(AppFrame_BeforeLoadMainForm);
            AppFrame.AfterLoadMainForm += new LoadMainFormHandler(AppFrame_AfterLoadMainForm);
            app.Run();
        }

        static void AppFrame_FinishLoadAddIn(LoadAddInEventArgs e)
        {
            string info = "插件加载完毕。";
            AppFrame.Instance.SplashScreen.SetInfo(info);
        }

        static void AppFrame_AfterLoadMainForm(LoadMainFormEventArgs e)
        {
            string info = "主窗体加载完成。";
            AppFrame.GetInstance().SplashScreen.SetInfo(info);
        }

        static void AppFrame_BeforeLoadMainForm(LoadMainFormEventArgs e)
        {
            string info = "正在加载主窗体......";
            AppFrame.GetInstance().SplashScreen.SetInfo(info);
        }


        static void AppFrame_BeforeLoadOneAddIn(LoadAddInEventArgs e)
        {
            string info = "正在加载" + e.AddInParser.Name
                + System.Environment.NewLine
                + "作者：" + e.AddInParser.Author
                + "Copyright:" + e.AddInParser.Copyright;
            AppFrame.GetInstance().SplashScreen.SetInfo(info);
        }


        private static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);

            //遍历正在有相同名字运行的例程
            foreach (Process process in processes)
            {
                //忽略现有的例程
                if (process.Id != current.Id)
                {
                    //确保例程从EXE文件运行
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        //返回另一个例程实例
                        return process;
                    }
                }
            }

            //没有其它的例程，返回Null
            return null;
        }
    }
}
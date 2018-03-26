using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AddIn.Core;

namespace StartUp
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppFrame app =  new AppFrame();
            //app.LoginDialog = new LoginDialog();
            app.Run();
        }
    }
}
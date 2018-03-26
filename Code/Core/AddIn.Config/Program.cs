using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AddIn.Core;
using System.Reflection;
using System.Configuration;

namespace AddInConfig
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            AppFrame app = AppFrame.GetInstance();
            AppFrame.StartUp += new StartUpHandler(AppFrame_StartUp);
            app.RunConfig();
        }

        static void AppFrame_StartUp(StartUpEventArgs e)
        {
            IUiService ui = e.ServiceCollection.GetService<IUiService>();
            if (ui == null)
            {
                AddInParser ap = new AddInParser();
                ap.Name = ConfigurationManager.AppSettings["service"];
                ap.Path = ConfigurationManager.AppSettings["addin"];
                IUiService uiService = ap.GetService() as IUiService;
                uiService.InitialUiServiceInfo(ap);
                AppFrame.ServiceCollection.BaseServiceParserList.Add(ap);
                AppFrame.ServiceCollection.Services.Add(ap.Name, ap.GetService());
            }
        }
    }
}
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
            AddInParser ap = new AddInParser();
            ap.Name = ConfigurationManager.AppSettings["service"];
            ap.Path = ConfigurationManager.AppSettings["addin"];
            IUiService uiService = ap.GetService() as IUiService;
            uiService.InitialUiServiceInfo(ap);
            AppFrame app = AppFrame.GetInstance(false);
            AppFrame.ServiceCollection.AddInParserList.Add(ap);
            AppFrame.ServiceCollection.Services.Add(ap.Name, ap.GetService());
            app.RunConfig();
        }
    }
}
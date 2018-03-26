using System;
using System.Collections.Generic;
using System.Text;
using AddIn.Core;

namespace helloDock
{
    public class HelloDock : ServiceBase,IHelloDock
    {
        private IUiService _uiService;

        public HelloDock()
        {
            AppFrame.FinishLoadAddIn += new LoadAddInHandler(AppFrame_FinishLoadAddIn);
        }

        void AppFrame_FinishLoadAddIn(LoadAddInEventArgs e)
        {
            _uiService = AppFrame.GetServiceCollection().GetService<IUiService>();
        }

        public void SayHello()
        {
            HelloForm frm = new HelloForm();
            _uiService.ShowDocForm(frm);
        }

        #region IHelloDock 成员

        public void SayHello(string str)
        {
            HelloForm frm = new HelloForm();
            frm.Hello = str;
            _uiService.ShowDocForm(frm);
        }

        #endregion

        private string[] greetings = new string[] {"Hello, ","Hi, ", "Hey, " };
        Random random = new Random(3);

        public void Greet(string str)
        {
            int i = random.Next(3);
            HelloForm frm = new HelloForm();
            frm.Hello = greetings[i] + str;
            _uiService.ShowDocForm(frm);
        }
    }
}

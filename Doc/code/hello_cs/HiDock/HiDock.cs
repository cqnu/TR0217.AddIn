using System;
using System.Collections.Generic;
using System.Text;
using AddIn.Core;
using System.Windows.Forms;

namespace HiDock
{
    public class HiDock:ServiceBase
    {
        Type _helloType;//HeloDock的类型说明
        ServiceBase _helloService;//指向HelloDock实例的引用

        public HiDock()
        {
            AppFrame.FinishLoadAddIn += new LoadAddInHandler(AppFrame_FinishLoadAddIn);
        }

        void AppFrame_FinishLoadAddIn(LoadAddInEventArgs e)
        {
            try
            {
                //根据名称获取服务对象
                _helloService = e.ServiceCollection.GetService("helloDock.HelloDock");
                _helloType = _helloService.GetType();
            }
            catch { }
        }

        public void SayHi()
        {
            //通过实例的类型和实例调用成员，即调用SayHello方法，传入的参数为"Hi, beauty!"
            if (_helloType != null)
                _helloType.InvokeMember("SayHello", System.Reflection.BindingFlags.InvokeMethod, 
                    null, _helloService, new object[] { "Hi, beauty!" });
            else MessageBox.Show("未能获取helloDock.HelloDock服务，调用目标失败！");
        }
    }
}

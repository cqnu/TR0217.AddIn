using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AddIn.Core;
using System.Threading;

namespace StartUp
{
    public partial class LoginDialog : Form,ILoginDialog
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        bool _valid = true;
        Thread _LoginThread;

        #region ILoginDialog 成员

        public new bool ShowDialog()
        {
            bool ret = (DialogResult.OK == base.ShowDialog());
            return ret;
        }

        public bool Valid
        {
            get { return _valid; }
        }

        #endregion

        private void btnOK_Click(object sender, EventArgs e)
        {
            btnOK.Enabled = false;
            //仅仅演示，并没有必要在另外线程里登录
            _LoginThread = new Thread(new ThreadStart(Login));
            _LoginThread.Start();
        }

        private void Login()
        {
            Thread.Sleep(5000);//验证用户过程,并将验证结果赋值给_valid
            this.Invoke(new MethodInvoker(delegate() { this.DialogResult = DialogResult.OK;}));
        }
    }
}
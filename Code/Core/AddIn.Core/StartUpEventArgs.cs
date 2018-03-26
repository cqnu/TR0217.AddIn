using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace AddIn.Core
{
    public class StartUpEventArgs : CancelEventArgs
    {
        ILoginDialog _loginDialog = null;

        public AddIn.Core.ILoginDialog LoginDialog
        {
            get { return _loginDialog; }
            set { _loginDialog = value; }
        }

        ISplashScreen _splashScreen = null;

        public AddIn.Core.ISplashScreen SplashScreen
        {
            get { return _splashScreen; }
            set { _splashScreen = value; }
        }


        private IServiceCollection _serviceCollection;

        public IServiceCollection ServiceCollection
        {
            get { return _serviceCollection; }
        }

        public StartUpEventArgs(IServiceCollection sc)
        {
            _serviceCollection = sc;
        }
    }
}

using System;

namespace AddIn.Core
{
    public interface ISplashScreen
    {
        void SetInfo(string info);
        void CloseSplash();
        void ShowSplash();
    }
}

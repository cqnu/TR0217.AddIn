using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AddIn.Core;
using System.Threading;

namespace Mini_Internet_Explorer
{
    public partial class SplashWin :Form,ISplashScreen
    {
        public SplashWin()
        {
            InitializeComponent();
        }

        //private bool _showing = true;
        //private void fadeTimer_Tick(object sender, EventArgs e)
        //{
        //    if (_showing)
        //    {
        //        double d = 1000.0 / fadeTimer.Interval / 100.0;
        //        if (base.Opacity + d >= 1.0)
        //        {
        //            base.Opacity = 1.0;
        //            _fadeFinish = true;
        //            fadeTimer.Stop();
        //        }
        //        else
        //        {
        //            base.Opacity += d;
        //        }
        //    }
        //    else
        //    {
        //        double d = 1000.0 / fadeTimer.Interval / 100.0;
        //        if (base.Opacity - d <= 0.0)
        //        {
        //            base.Opacity = 0.0;
        //            fadeTimer.Stop();
        //            _fadeFinish = true;
        //        }
        //        else
        //        {
        //            base.Opacity -= d;
        //        }
        //    }
        //}


        public void SetInfo(string info)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate() { this.label1.Text = info; }));
            }
            catch
            { }
        }

        public void CloseSplash()
        {
            this.Invoke(new MethodInvoker(this.Close));
        }

        public void ShowSplash()
        {
            this.ShowDialog();
        }
    }
}
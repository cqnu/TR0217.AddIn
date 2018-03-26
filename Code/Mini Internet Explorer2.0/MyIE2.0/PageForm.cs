using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AddIn.Core;
using WeifenLuo.WinFormsUI.Docking;

namespace MyIE
{
    public partial class PageForm : DockContent
    {
        private int _progress = 0;
        private bool _complete = false;
        private string _url = "about:blank";
        //private string _title = "about:blank";
        public bool _annal = true;

        public string Url
        {
            get { return _url; }
        }

        public bool Complete
        {
            get { return _complete; }
        }

        public int Progress
        {
            get { return _progress; }
        }


        public WebBrowserEx WebBrowser
        {
            get { return webBrowser; }
        }

        public event WebBrowserProgressChangedEventHandler ProgressChanged;
        public event WebBrowserDocumentCompletedEventHandler DocumentCompleted;
        public event EventHandler CanGoForwardChanged;
        public event EventHandler CanGoBackChanged;
        public event EventHandler StatusTextChanged;
        public event WebBrowserNavigatedEventHandler Navigated;

        public PageForm()
        {
            InitializeComponent();
        }

        public void Navigate(string url)
        {
            _url = url;
            this.webBrowser.Navigate(url);
        }

        private void webBrowser_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            if (e.CurrentProgress >= 0)
            {
                if (e.MaximumProgress != 0)
                    _progress = (int)((100 * e.CurrentProgress) / e.MaximumProgress);
                else
                    _progress = 100;
            }

            if (ProgressChanged != null)
                ProgressChanged(this, e);
        }

        void webBrowser_DocumentTitleChanged(object sender, System.EventArgs e)
        {          
            if (!string.IsNullOrEmpty(this.webBrowser.DocumentTitle))
            {
                this.Text = this.webBrowser.DocumentTitle;
            }
            else
            {
                this.Text = this.Url;
            }
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            _complete = true;
            _progress = 100;
            if (DocumentCompleted != null)
                DocumentCompleted(this, e);
        }


        void webBrowser_StatusTextChanged(object sender, System.EventArgs e)
        {
            if (StatusTextChanged != null)
                StatusTextChanged(this, e);
        }

        void webBrowser_CanGoForwardChanged(object sender, System.EventArgs e)
        {
            if (CanGoForwardChanged != null)
                CanGoForwardChanged(this, e);
        }

        void webBrowser_CanGoBackChanged(object sender, System.EventArgs e)
        {
            if (CanGoBackChanged != null)
                CanGoBackChanged(this, e);
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (webBrowser.Url != null)
                _url = webBrowser.Url.AbsoluteUri;
            if (Navigated != null)
                Navigated(this, e);
        }

        private void webBrowser_Quit(object sender, EventArgs e)
        {
            this.Close();       
        }
    }
}
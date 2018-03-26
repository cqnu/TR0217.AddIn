using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Forms;
using mshtml;

namespace MyIE
{
    public delegate void NewWindowExEventHandler(object sender, NewWindowExEventArgs e);

    public class NewWindowExEventArgs : CancelEventArgs
    {
        Uri url;

        public Uri Url
        {
            get
            {
                return url;
            }
        }

        public NewWindowExEventArgs(Uri url)
        {
            this.url = url;
        }
    }

    public delegate void TextSelectEventHandler(object sender, TextSelectEventArgs e);

    public class TextSelectEventArgs : EventArgs
    {
        string _text;
        public string Text
        {
            get { return _text; }
        }

        public TextSelectEventArgs(string text)
        {
            _text = text;
        }
    }

    /// <summary>
    /// Microsoft didn't include the URL being surfed to in the NewWindow event args,
    /// but here is the workaround:
    /// </summary>
    public class WebBrowserEx : WebBrowser
    {
        class WebBrowserExEvents : System.Runtime.InteropServices.StandardOleMarshalObject, DWebBrowserEvents2
        {
            private WebBrowserEx browser;

            public WebBrowserExEvents(WebBrowserEx browser)
            {
                this.browser = browser;
            }

            public void NewWindow3(object pDisp, ref bool cancel, ref object flags, ref string urlContext, ref string url)
            {
                NewWindowExEventArgs e = new NewWindowExEventArgs(new Uri(url));
                browser.OnNewWindowEx(e);
                cancel = e.Cancel;
            }
        }


        [ComImport()]
        [Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D")]
        [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
        [TypeLibType(TypeLibTypeFlags.FHidden)]
        interface DWebBrowserEvents2
        {
            [DispId(273)]
            void NewWindow3([InAttribute(), MarshalAs(UnmanagedType.IDispatch)] object pDisp,
                            [InAttribute(), OutAttribute()]                 ref bool cancel,
                            [InAttribute()]                                 ref object flags,
                            [InAttribute(), MarshalAs(UnmanagedType.BStr)]  ref string urlContext,
                            [InAttribute(), MarshalAs(UnmanagedType.BStr)]  ref string url);
        }

        public event NewWindowExEventHandler NewWindowEx;
        public event EventHandler Quit;
        public event TextSelectEventHandler TextSelected;

        private AxHost.ConnectionPointCookie cookie;
        private WebBrowserExEvents wevents;

        protected override void OnDocumentCompleted(WebBrowserDocumentCompletedEventArgs e)
        {
            base.OnDocumentCompleted(e);
            this.Document.MouseUp += new HtmlElementEventHandler(Document_MouseUp);
        }


        public event EventHandler SelectedTextChanged;
        string _selectedText = string.Empty;
        public string SelectedText
        {
            get { return _selectedText; }
            set 
            {
                if (_selectedText != value)
                {
                    _selectedText = value;
                    this.OnSelectedTextChanged();
                }                
            }
        }

        protected virtual void OnSelectedTextChanged()
        {
            if (SelectedTextChanged != null)
                SelectedTextChanged(this, EventArgs.Empty);
        }

        void Document_MouseUp(object sender, HtmlElementEventArgs e)
        {
            try
            {
                IHTMLDocument2 document = (IHTMLDocument2)this.Document.DomDocument;
                if (document.selection != null)
                {
                    IHTMLTxtRange htmlElem = (IHTMLTxtRange)document.selection.createRange();
                    SelectedText = htmlElem.text;
                    if (!string.IsNullOrEmpty(htmlElem.text))
                        this.OnTextSelected(new TextSelectEventArgs(htmlElem.text));
                }
            }
            catch { }
        }

        protected virtual void OnTextSelected(TextSelectEventArgs arg)
        {
            if(TextSelected != null)
                TextSelected(this,arg);
        }

        protected override void CreateSink()
        {
            base.CreateSink();
            wevents = new WebBrowserExEvents(this);
            cookie = new AxHost.ConnectionPointCookie(this.ActiveXInstance, wevents, typeof(DWebBrowserEvents2));
        }

        protected override void DetachSink()
        {
            if (cookie != null)
            {
                cookie.Disconnect();
                cookie = null;
            }
            base.DetachSink();
        }

        protected virtual void OnNewWindowEx(NewWindowExEventArgs e)
        {
            if (NewWindowEx != null)
            {
                NewWindowEx(this, e);
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == (int)WindowsMessages.WM_PARENTNOTIFY)
            {
                //int lp = m.LParam.ToInt32();   
                int wp = m.WParam.ToInt32();

                int X = wp & 0xFFFF;
                //int Y = (wp >> 16) & 0xFFFF;   
                if (X == (int)WindowsMessages.WM_DESTROY)
                {
                    this.OnQuit();
                }
            }            
        }

        /// <summary>   
        /// A list of all the available window messages   
        /// </summary>   
        enum WindowsMessages
        {
            WM_COPY = 0x0301,
            WM_DESTROY = 0x2,
            WM_PARENTNOTIFY = 0x210,
        }

        protected virtual void OnQuit()
        {
            EventHandler h = Quit;
            if (null != h)
                h(this, EventArgs.Empty);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using mshtml;
using System.Runtime.InteropServices;

namespace MyIE
{
    internal partial class FindForm : Form
    {
        private WebBrowser _webBrowser;
        private IHTMLTxtRange _searchRange;
        private string _text;

        public WebBrowser WebBrowser
        {
            get { return _webBrowser; }
            set { _webBrowser = value; }
        }

        private void GetSearchRange()
        {
            IHTMLDocument2 document = (IHTMLDocument2)_webBrowser.Document.DomDocument;
            if (document.selection.type.ToLower() != "none")
            {
                _searchRange = (IHTMLTxtRange)document.selection.createRange();
                //_searchRange.collapse(true);
                _searchRange.moveStart("character", 1);
            }
            else
            {
                IHTMLBodyElement body = (IHTMLBodyElement)document.body;
                _searchRange = (IHTMLTxtRange)body.createTextRange();
            }
        }



        private void Find(int a)
        {
            if (_searchRange == null)
                return;
            try
            {
                if (_searchRange.findText(_text, a, 0))
                {
                    _searchRange.select();
                    _searchRange.scrollIntoView(a<0);

                    //if (a < 0)
                    //    _searchRange.moveEnd("character", _text.Length * a);
                    //else
                    //    _searchRange.moveStart("character", _text.Length);
                }
            }
            catch
            {
                Marshal.FinalReleaseComObject(_searchRange);
                _searchRange = null;
            }
        }

        public FindForm()
        {
            InitializeComponent();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.Find(999999);
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            this.Find(-999999);
        }

        private void FindForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            textBox1.Text = string.Empty;
            btnPre.Enabled = false;
            btnNext.Enabled = false;
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
            if (_searchRange != null)
            {
                Marshal.FinalReleaseComObject(_searchRange);
                _searchRange = null;
            }
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            this.InitialSearch();
        }

        private void InitialSearch()
        {
            _text = textBox1.Text;
            if (_searchRange != null)
            {
                Marshal.FinalReleaseComObject(_searchRange);
                _searchRange = null;
            }
            if (string.IsNullOrEmpty(_text))
            {
                btnPre.Enabled = false;
                btnNext.Enabled = false;
            }
            else
            {
                if (_webBrowser != null)
                {
                    this.GetSearchRange();
                    btnPre.Enabled = true;
                    btnNext.Enabled = true;
                }
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.InitialSearch();
        }

    }
}
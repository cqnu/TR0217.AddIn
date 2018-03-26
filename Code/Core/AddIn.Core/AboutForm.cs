using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AddIn.Core
{
    internal partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        public string Author
        {
            get { return lbAuthor.Text; }
            set { lbAuthor.Text = value; }
        }

        public string Version
        {
            get { return lbVersion.Text; }
            set { lbVersion.Text = value; }
        }

        public string Copyright
        {
            get { return lbCopyright.Text; }
            set { lbCopyright.Text = value; }
        }

        public string Url
        {
            get { return llbUrl.Text; }
            set 
            { 
                if(!string.IsNullOrEmpty(value))
                    llbUrl.Text = value; 
            }
        }

        public string Description
        {
            get { return txtDescription.Text; }
            set { txtDescription.Text = value; }
        }

        private void llbUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(llbUrl.Text))
                System.Diagnostics.Process.Start(llbUrl.Text);   
        }
    }
}
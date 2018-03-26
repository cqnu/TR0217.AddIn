using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AddIn.Core;
using WeifenLuo.WinFormsUI.Docking;

namespace helloDock
{
    public partial class HelloForm : DockContent
    {
        public HelloForm()
        {
            InitializeComponent();
        }

        public string Hello
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }
    }
}
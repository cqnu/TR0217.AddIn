using System;
using System.Collections.Generic;
using System.Text;

using AddIn.Core;
using System.Windows.Forms;

namespace hello
{
    public class Hello : ServiceBase
    {
        public void SayHello()
        {
            MessageBox.Show("Hello, world!");
        }
    }
}
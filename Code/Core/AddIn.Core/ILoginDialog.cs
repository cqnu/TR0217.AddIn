using System;
using System.Collections.Generic;
using System.Text;

namespace AddIn.Core
{
    public interface ILoginDialog
    {
        bool Valid
        {
            get;
        }

        bool ShowDialog();
    }
}

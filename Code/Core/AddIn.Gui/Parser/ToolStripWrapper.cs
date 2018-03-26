using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AddIn.Gui.Parser
{
    class ToolStripWrapper
    {
        ToolStrip _toolStrip;
        public ToolStrip ToolStrip
        {
            get { return _toolStrip; }
            set { _toolStrip = value; }
        }

        ToolStripLocation _location;
        public ToolStripLocation Location
        {
            get { return _location; }
            set { _location = value; }
        }

        private bool _joined;

        public bool Joined
        {
            get { return _joined; }
            set { _joined = value; }
        }
    }
}

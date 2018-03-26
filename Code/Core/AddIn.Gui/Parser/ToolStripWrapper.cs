using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AddIn.Gui.Parser
{
    class ToolStripWrapper : ToolStrip
    {
        ToolStripLocation _location;
        public new ToolStripLocation Location
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

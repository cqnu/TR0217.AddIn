using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AddIn.Gui.Parser
{
    class MyToolStripContainer
    {
        public MyToolStripContainer(            
            ToolStripPanel leftToolStripPanel,
            ToolStripPanel topToolStripPanel,
            ToolStripPanel rightToolStripPanel,
            ToolStripPanel bottomToolStripPanel
            )
        {
            _leftToolStripPanel = leftToolStripPanel;
            _topToolStripPanel = topToolStripPanel;
            _rightToolStripPanel = rightToolStripPanel;           
            _bottomToolStripPanel = bottomToolStripPanel;
        }

        ToolStripPanel _bottomToolStripPanel;
        ToolStripPanel _leftToolStripPanel;
        ToolStripPanel _rightToolStripPanel;
        ToolStripPanel _topToolStripPanel;

        public ToolStripPanel BottomToolStripPanel
        {
            get { return _bottomToolStripPanel; }
        }

        public ToolStripPanel LeftToolStripPanel
        {
            get { return _leftToolStripPanel; }
        }

        public ToolStripPanel RightToolStripPanel
        {
            get { return _rightToolStripPanel; }
        }
        
        public ToolStripPanel TopToolStripPanel
        {
            get { return _topToolStripPanel; }
        }

        public void SuspendLayout()
        {
            //_bottomToolStripPanel.SuspendLayout();
            //_topToolStripPanel.SuspendLayout();
            //_leftToolStripPanel.SuspendLayout();
            //_rightToolStripPanel.SuspendLayout();
        }

        public void ResumeLayout(bool performLayout)
        {
            //_topToolStripPanel.ResumeLayout(performLayout);
            //_bottomToolStripPanel.ResumeLayout(performLayout);
            //_leftToolStripPanel.ResumeLayout(performLayout);
            //_rightToolStripPanel.ResumeLayout(performLayout);
        }

        public void PerformLayout()
        {
            //_topToolStripPanel.PerformLayout();
            //_bottomToolStripPanel.PerformLayout();
            //_leftToolStripPanel.PerformLayout();
            //_rightToolStripPanel.PerformLayout();
        }

        ToolStripDispatchList _toolStrips;
        internal ToolStripDispatchList ToolStrips
        {
            get
            {
                if (_toolStrips == null)
                    _toolStrips = new ToolStripDispatchList(this);
                return _toolStrips;
            }
        }
    }
}

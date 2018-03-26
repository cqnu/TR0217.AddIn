using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using AddIn.Gui.Parser;

namespace AddIn.Gui
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        internal DockPanel DockPanel
        {
            get { return dockPanel; }
        }

        internal StatusStrip StatusStrip
        {
            get { return statusStrip1; }
        }

        internal MenuStrip MenuStrip
        {
            get { return menuStrip1; }
        }

        MyToolStripContainer _toolStripContainer;

        internal MyToolStripContainer ToolStripContainer
        {
            get
            {
                if (_toolStripContainer == null)
                    _toolStripContainer = new MyToolStripContainer(
                        toolStripContainer1.LeftToolStripPanel,
                        toolStripContainer1.TopToolStripPanel,
                        toolStripContainer1.RightToolStripPanel,
                        toolStripContainer1.BottomToolStripPanel);
                return _toolStripContainer;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Activate();
            this.BringToFront();
        }

        private void dockPanel_ActiveContentChanged(object sender, EventArgs e)
        {
            IDockContent c = dockPanel.ActiveContent;
            if (c != null && c is Form)
                c.OnActivated(e);
        }

        private void menuStrip1_ItemAdded(object sender, ToolStripItemEventArgs e)
        {
            menuStrip1.Visible = true;
        }

        private void menuStrip1_ItemRemoved(object sender, ToolStripItemEventArgs e)
        {
            if (menuStrip1.Items.Count == 0)
                menuStrip1.Visible = false;
        }
    }
}
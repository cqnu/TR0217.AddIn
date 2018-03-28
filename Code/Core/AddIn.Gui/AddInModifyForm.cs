using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AddIn.Gui.Loader;
using AddIn.Gui.Parser;
using System.Collections;
using AddIn.Core;
using System.IO;
using System.Reflection;

namespace AddIn.Gui
{
    internal partial class AddInModifyForm : Form
    {
        private UiLoader _uiLoader = null;
        private string _uiConfigPath = string.Empty;

        private IServiceCollection _serviceCollection = null;
        private bool _servicesModified = false;
        private bool _uiLoaderModified = false;


        public static List<string> _serviceList = new List<string>();
        public static HashSet<string> _functionList = new HashSet<string>();
        public static HashSet<string> _injectorList = new HashSet<string>();
        public static HashSet<string> _eventList = new HashSet<string>();
        public static List<string> _tsiList = new List<string>();

        private string _lastService = string.Empty;
        private string _lastParent = string.Empty;

        /// <summary>
        /// 当前复制或者剪切的界面元素
        /// </summary>
        private UiElemParser _uieParser = null;

        public string UiConfigPath
        {
            get { return _uiConfigPath; }
            set { _uiConfigPath = value; }
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

        public UiLoader UiLoader
        {
            get { return _uiLoader; }
            set 
            { 
                _uiLoader = value;
                if (_uiLoader != null)
                {
                    SetUiElemTree();
                }
            }
        }

        public IServiceCollection ServiceCollection
        {
            get { return _serviceCollection; }
            set
            {
                _serviceCollection = value;
                if (_serviceCollection != null)
                {
                    SetAddInListView();
                    _serviceList.Add("");
                    foreach (AddInParser aip in _serviceCollection.AddInParserList)
                    {
                        _serviceList.Add(aip.Name);
                    }

                    foreach (AddInParser aip in _serviceCollection.BaseServiceParserList)
                    {
                        _serviceList.Add(aip.Name);
                    }
                }
            }
        }

        public AddInModifyForm()
        {
            InitializeComponent();
            tsmiCreateButton.Image = imageList.Images["Button"];
            tsmiCreateComboBox.Image = imageList.Images["ComboBox"];
            tsmiCreateComboBoxImage.Image = imageList.Images["ComboBoxImage"];
            tsmiCreateComboBoxImageItem.Image = imageList.Images["ComboBoxImageItem"];
            tsmiCreateComboBoxItem.Image = imageList.Images["ComboBoxItem"];
            tsmiCreateContextMenuStrip.Image = imageList.Images["ContextMenuStrip"];
            tsmiCreateDropDownButton.Image = imageList.Images["MenuItem"];
            tsmiCreateLabel.Image = imageList.Images["Label"];
            tsmiCreateMenuItem.Image = imageList.Images["MenuItem"];
            tsmiCreateProgressBar.Image = imageList.Images["ProgressBar"];
            tsmiCreateSeparator.Image = imageList.Images["Separator"];
            tsmiCreateSplitButton.Image = imageList.Images["SplitButton"];
            tsmiCreateStatusLabel.Image = imageList.Images["Label"];
            tsmiCreateTextBox.Image = imageList.Images["TextBox"];
            tsmiCreateToolStrip.Image = imageList.Images["ToolStrip"];
            tsmiSingleCopy.Image = imageList.Images["copy"];
            tsmiSingleCut.Image = imageList.Images["cut"];
            tsmiSingleDelete.Image = imageList.Images["delete"];
            tsmiSinglePaste.Image = imageList.Images["paste"];
            tsmiCreateCopy.Image = imageList.Images["copy"];
            tsmiCreateCut.Image = imageList.Images["cut"];
            tsmiCreateDelete.Image = imageList.Images["delete"];
            tsmiCreatePaste.Image = imageList.Images["paste"];
            this.Size = new System.Drawing.Size(800, 600);

            _uiConfigPath = Application.StartupPath + "\\Config\\UI.xml";
        }

        private void AddInModifyForm_Resize(object sender, EventArgs e)
        {
            this.tabControl1.Height = (this.Height - 84 - tabControl1.Location.Y);
        }

        private void tabControl1_LocationChanged(object sender, EventArgs e)
        {
            this.tabControl1.Height = (this.Height - 84 - tabControl1.Location.Y);
        }


        private void SetAddInListView()
        {
            lvAddIn.Items.Clear();

            foreach (AddInParser aip in _serviceCollection.BaseServiceParserList)
            {
                ListViewItem lvi = CreateListViewItem(aip);
                lvAddIn.Items.Add(lvi);
            }

            foreach (AddInParser aip in _serviceCollection.AddInParserList)
            {
                ListViewItem lvi = CreateListViewItem(aip);
                lvAddIn.Items.Add(lvi);
            }
        }

        private ListViewItem CreateListViewItem(AddInParser aip)
        {
            ListViewItem lvi = new ListViewItem(aip.Name);
            lvi.Tag = aip;
            lvi.SubItems.Add(aip.Author);
            lvi.SubItems.Add(aip.Version);
            lvi.SubItems.Add(aip.Copyright);
            lvi.SubItems.Add(aip.Url);
            lvi.SubItems.Add(aip.Path);
            lvi.SubItems.Add(aip.IsBaseService.ToString());
            lvi.SubItems.Add(aip.Lazyload.ToString());
            lvi.SubItems.Add(aip.Description);
            

            lvi.ImageKey = aip.Valid? "valid":"invalid";

            return lvi;
        }

        private TreeNode CreateNode(string nodeText, string key)
        {
            TreeNode node = new TreeNode(nodeText);
            node.ImageKey = key;
            node.SelectedImageKey = key;
            node.Name = key;

            return node;
        }

        private TreeNode CreateNode(UiElemParser cp)
        {
            TreeNode node;
            switch (cp.UiElemType)
            {
                case UiElemType.StatusStrip:
                    node = CreateNode(cp.Text, "StatusStrip");
                    break;
                case UiElemType.MenuStrip:
                    node = CreateNode(cp.Text, "MenuStrip");
                    break;
                case UiElemType.ToolStrip:
                    node = CreateNode(cp.Text, "ToolStrip");
                    break;
                case UiElemType.ContextMenuStrips:
                    node = CreateNode("ContextMenuStrips", "ContextMenuStrips");
                    break;
                case UiElemType.TextBox:
                    node = CreateNode(cp.Text, "TextBox");
                    break;
                case UiElemType.ComboBox:
                    node = CreateNode(cp.Text, "ComboBox");
                    break;
                case UiElemType.ComboBoxItem:
                    node = CreateNode(cp.Text, "ComboBoxItem");
                    break;
                case UiElemType.MenuItem:
                    node = CreateNode(cp.Text, "MenuItem");
                    break;
                case UiElemType.Button:
                    node = CreateNode(cp.Text, "Button");
                    break;
                case UiElemType.SplitButton:
                    node = CreateNode(cp.Text, "SplitButton");
                    break;
                case UiElemType.DropDownButton:
                    node = CreateNode(cp.Text, "DropDownButton");
                    break;
                case UiElemType.StatusLabel:
                    node = CreateNode(cp.Text, "Label");
                    break;
                case UiElemType.Label:
                    node = CreateNode(cp.Text, "Label");
                    break;
                case UiElemType.ProgressBar:
                    node = CreateNode(cp.Text, "ProgressBar");
                    break;
                case UiElemType.Separator:
                    node = CreateNode(cp.Text, "Separator");
                    break;
                case UiElemType.ContextMenuStrip:
                    node = CreateNode(cp.Text, "ContextMenuStrip");
                    break;
                case UiElemType.ToolStripContainer:
                    node = CreateNode(cp.Text, "ToolStripContainer");
                    break;
                case UiElemType.ComboBoxImage:
                    node = CreateNode(cp.Text, "ComboBoxImage");
                    break;
                case UiElemType.ComboBoxImageItem:
                    node = CreateNode(cp.Text, "ComboBoxImageItem");
                    break;
                default:
                    node = null;
                    break;
            }
            node.Tag = cp;

            return node;
        }

        private TreeNode CreateNodeRecursive(UiElemParser cp)
        {
            TreeNode node = CreateNode(cp);
            foreach (UiElemParser up in cp.UiElemParserList)
            {
                TreeNode subnode = CreateNodeRecursive(up);
                node.Nodes.Add(subnode);
            }
            return node;
        }

        private void SetUiElemTree()
        {
            treeView.Nodes.Clear();
            TreeNode root = CreateNode("System UI", "System");
            root.Tag = _uiLoader;

            TreeNode menustrip = CreateNodeRecursive(_uiLoader.MenuStripParser);
            TreeNode toolstrippanel =  CreateNodeRecursive(_uiLoader.ToolStripContainerParser);
            TreeNode statusstrip = CreateNodeRecursive(_uiLoader.StatusStripParser);
            TreeNode contextmenus = CreateNodeRecursive(_uiLoader.ContextMenuContainerParser);

            root.Nodes.AddRange(new TreeNode[] { menustrip, toolstrippanel, statusstrip,contextmenus });
            treeView.Nodes.Add(root);
            menustrip.Expand();
            toolstrippanel.Expand();
            statusstrip.Expand();
            contextmenus.Expand();
            root.Expand();
        }

        private void treeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            tsmiCreateButton.Visible = false;
            tsmiCreateContextMenuStrip.Visible = false;
            tsmiCreateDropDownButton.Visible = false;
            tsmiCreateLabel.Visible = false;
            tsmiCreateMenuItem.Visible = false;
            tsmiCreateSplitButton.Visible = false;
            tsmiCreateStatusLabel.Visible = false;
            tsmiCreateToolStrip.Visible = false;
            tsmiCreateComboBoxItem.Visible = false;
            tsmiCreateComboBoxImageItem.Visible = false;
            tsmiCreateProgressBar.Visible = false;

            tsmiCreateSeparator.Visible = true;
            tsmiCreateCopy.Visible = true;
            tsmiCreateCut.Visible = true;
            tsmiCreateDelete.Visible = true;
            tsmiCreatePaste.Visible = true;

            tsmiSingleCopy.Visible = true;
            tsmiSingleCut.Visible = true;
            tsmiSingleDelete.Visible = true;
            tsmiSinglePaste.Visible = true;
            tssSingle.Visible = true;
            tssCreate.Visible = true;
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //data binding
            propertyGrid.SelectedObject = treeView.SelectedNode.Tag;
            UiElemParser cp = treeView.SelectedNode.Tag as UiElemParser;
            if (cp != null)
            {
                UpdateEditorServiceResource(cp.Service);
                if(cp is CmdParser)
                    UpdateEditorParaResource(treeView.SelectedNode.Parent.Name);
            }
            if (_uieParser != null)
            {
                tsmiSinglePaste.Enabled = true;
                tsmiCreatePaste.Enabled = true;
            }
            else
            {
                tsmiSinglePaste.Enabled = false;
                tsmiCreatePaste.Enabled = false;
            }

            switch (treeView.SelectedNode.Name)
            {
                case "ContextMenuStrips":
                    treeView.ContextMenuStrip = cmsSingleItem;
                    tsmiCreateContextMenuStrip.Visible = true;
                    tsmiSingleCopy.Visible = false;
                    tsmiSingleCut.Visible = false;
                    tsmiSingleDelete.Visible = false;
                    if (tsmiSinglePaste.Enabled)
                    {
                        if (_uieParser.UiElemType != UiElemType.ContextMenuStrip)
                            tsmiSinglePaste.Enabled = false;
                    }
                    break;
                case "ToolStripContainer":
                    treeView.ContextMenuStrip = cmsSingleItem;
                    tsmiCreateToolStrip.Visible = true;
                    tsmiSingleCopy.Visible = false;
                    tsmiSingleCut.Visible = false;
                    tsmiSingleDelete.Visible = false;
                    if (tsmiSinglePaste.Enabled)
                    {
                        if (_uieParser.UiElemType != UiElemType.ToolStrip)
                            tsmiSinglePaste.Enabled = false;
                    }
                    break;
                case "ComboBox":
                    treeView.ContextMenuStrip = cmsSingleItem;
                    tsmiCreateComboBoxItem.Visible = true;
                    if (tsmiCreatePaste.Enabled)
                    {
                        if (_uieParser.UiElemType != UiElemType.ComboBoxItem)
                            tsmiCreatePaste.Enabled = false;
                    }
                    break;
                case "ComboBoxImage":
                    treeView.ContextMenuStrip = cmsSingleItem;
                    tsmiCreateComboBoxImageItem.Visible = true;
                    if (tsmiCreatePaste.Enabled)
                    {
                        if (_uieParser.UiElemType != UiElemType.ComboBoxImageItem)
                            tsmiCreatePaste.Enabled = false;
                    }
                    break;
                case "MenuStrip":
                    treeView.ContextMenuStrip = cmsCreateCommand;
                    tsmiCreateMenuItem.Visible = true;
                    //tsmiCreateComboBox.Visible = true;
                    //tsmiCreateComboBoxImage.Visible = true;
                    //tsmiCreateTextBox.Visible = true;
                    tsmiCreateSeparator.Visible = false;
                    tsmiCreateCopy.Visible = false;
                    tsmiCreateCut.Visible = false;
                    tsmiCreateDelete.Visible = false;
                    if (tsmiCreatePaste.Enabled)
                    {
                        if (_uieParser.UiElemType == UiElemType.MenuItem
                            || _uieParser.UiElemType == UiElemType.ComboBox
                            || _uieParser.UiElemType == UiElemType.TextBox
                            || _uieParser.UiElemType == UiElemType.ComboBoxImage)
                            tsmiCreatePaste.Enabled = true;
                        else
                            tsmiCreatePaste.Enabled = false;
                    }
                    break;
                case "MenuItem":
                    treeView.ContextMenuStrip = cmsCreateCommand;
                    tsmiCreateMenuItem.Visible = true;
                    //tsmiCreateComboBox.Visible = true;
                    //tsmiCreateComboBoxImage.Visible = true;
                    //tsmiCreateTextBox.Visible = true;
                    //tsmiCreateSeparartor.Visible = true;
                    if (tsmiCreatePaste.Enabled)
                    {
                        if (_uieParser.UiElemType == UiElemType.MenuItem
                            || _uieParser.UiElemType == UiElemType.ComboBox
                            || _uieParser.UiElemType == UiElemType.TextBox
                            || _uieParser.UiElemType == UiElemType.ComboBoxImage
                            || _uieParser.UiElemType == UiElemType.Separator)
                            tsmiCreatePaste.Enabled = true;
                        else
                            tsmiCreatePaste.Enabled = false;
                    }
                    break;
                case "DropDownButton":
                    treeView.ContextMenuStrip = cmsCreateCommand;
                    tsmiCreateMenuItem.Visible = true;
                    //tsmiCreateComboBox.Visible = true;
                    //tsmiCreateComboBoxImage.Visible = true;
                    //tsmiCreateTextBox.Visible = true;
                    //tsmiCreateSeparartor.Visible = true;
                    if (tsmiCreatePaste.Enabled)
                    {
                        if (_uieParser.UiElemType == UiElemType.MenuItem
                            || _uieParser.UiElemType == UiElemType.ComboBox
                            || _uieParser.UiElemType == UiElemType.TextBox
                            || _uieParser.UiElemType == UiElemType.ComboBoxImage
                            || _uieParser.UiElemType == UiElemType.Separator)
                            tsmiCreatePaste.Enabled = true;
                        else
                            tsmiCreatePaste.Enabled = false;
                    }
                    break;
                case "SplitButton":
                    treeView.ContextMenuStrip = cmsCreateCommand;
                    tsmiCreateMenuItem.Visible = true;
                    //tsmiCreateComboBox.Visible = true;
                    //tsmiCreateComboBoxImage.Visible = true;
                    //tsmiCreateTextBox.Visible = true;
                    //tsmiCreateSeparartor.Visible = true;
                    if (tsmiCreatePaste.Enabled)
                    {
                        if (_uieParser.UiElemType == UiElemType.MenuItem
                            || _uieParser.UiElemType == UiElemType.ComboBox
                            || _uieParser.UiElemType == UiElemType.TextBox
                            || _uieParser.UiElemType == UiElemType.ComboBoxImage
                            || _uieParser.UiElemType == UiElemType.Separator)
                            tsmiCreatePaste.Enabled = true;
                        else
                            tsmiCreatePaste.Enabled = false;
                    }
                    break;
                case "StatusStrip":
                    treeView.ContextMenuStrip = cmsCreateCommand;
                    tsmiCreateStatusLabel.Visible = true;
                    tsmiCreateButton.Visible = true;
                    tsmiCreateSplitButton.Visible = true;
                    tsmiCreateDropDownButton.Visible = true;
                    tsmiCreateProgressBar.Visible = true;
                    //tsmiCreateTextBox.Visible = true;
                    //tsmiCreateComboBox.Visible = true;
                    //tsmiCreateComboBoxImage.Visible = true;
                    //tsmiCreateSeparartor.Visible = true;
                    tsmiCreateCopy.Visible = false;
                    tsmiCreateCut.Visible = false;
                    tsmiCreateDelete.Visible = false;
                    if (tsmiCreatePaste.Enabled)
                    {
                        if (_uieParser.UiElemType == UiElemType.ComboBoxImageItem
                            || _uieParser.UiElemType == UiElemType.ComboBoxItem
                            || _uieParser.UiElemType == UiElemType.ContextMenuStrip
                            || _uieParser.UiElemType == UiElemType.MenuStrip
                            || _uieParser.UiElemType == UiElemType.ToolStripContainer
                            || _uieParser.UiElemType == UiElemType.ToolStrip
                            || _uieParser.UiElemType == UiElemType.StatusStrip)
                            tsmiCreatePaste.Enabled = false;
                    }
                    break;
                case "ContextMenuStrip":
                    treeView.ContextMenuStrip = cmsCreateCommand;
                    tsmiCreateMenuItem.Visible = true;
                    if (tsmiCreatePaste.Enabled)
                    {
                        if (_uieParser.UiElemType == UiElemType.MenuItem
                            || _uieParser.UiElemType == UiElemType.ComboBox
                            || _uieParser.UiElemType == UiElemType.TextBox
                            || _uieParser.UiElemType == UiElemType.ComboBoxImage
                            || _uieParser.UiElemType == UiElemType.Separator)
                            tsmiCreatePaste.Enabled = true;
                        else
                            tsmiCreatePaste.Enabled = false;
                    }
                    break;
                case "ToolStrip":
                    treeView.ContextMenuStrip = cmsCreateCommand;
                    tsmiCreateLabel.Visible = true;
                    tsmiCreateButton.Visible = true;
                    tsmiCreateSplitButton.Visible = true;
                    tsmiCreateDropDownButton.Visible = true;
                    tsmiCreateProgressBar.Visible = true;
                    //tsmiCreateTextBox.Visible = true;
                    //tsmiCreateComboBox.Visible = true;
                    //tsmiCreateComboBoxImage.Visible = true;
                    //tsmiCreateSeparartor.Visible = true;
                    if (tsmiCreatePaste.Enabled)
                    {
                        if (_uieParser.UiElemType == UiElemType.ComboBoxImageItem
                            || _uieParser.UiElemType == UiElemType.ComboBoxItem
                            || _uieParser.UiElemType == UiElemType.ContextMenuStrip
                            || _uieParser.UiElemType == UiElemType.MenuStrip
                            || _uieParser.UiElemType == UiElemType.ToolStripContainer
                            || _uieParser.UiElemType == UiElemType.StatusStrip)
                            tsmiCreatePaste.Enabled = false;
                    }
                    break;
                case "System":
                    treeView.ContextMenuStrip = null;
                    break;
                default:
                    treeView.ContextMenuStrip = cmsSingleItem;
                    tssSingle.Visible = false;
                    break;
            }
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                treeView.SelectedNode = e.Node;
        }


        private void tsmiCreate_Click(object sender, EventArgs e)
        {
            UiElemParser uep = null;
            
            switch ((sender as ToolStripMenuItem).Name)
            {
                case "tsmiCreateToolStrip":
                    uep = new ToolStripParser(UiLoader);
                    break;
                case "tsmiCreateComboBoxItem":
                    uep = new ComboBoxItemParser(UiLoader);
                    break;
                case "tsmiCreateComboBoxImageItem":
                    //uep = new ComboBoxImageItemParser();
                    break;
                case "tsmiCreateContextMenuStrip":
                    uep = new ContextMenuStripParser(UiLoader);
                    _uiLoader.ContextMenuContainerParser.UiElemParserList.Add(uep);
                    TreeNode node2 = CreateNode(uep);
                    treeView.SelectedNode.Nodes.Add(node2);
                    btnSave.Enabled = true;
                    _uiLoaderModified = true;
                    return;
                case "tsmiCreateMenuItem":
                    uep = new MenuItemParser(UiLoader);
                    break;
                case "tsmiCreateButton":
                    uep = new ButtonParser(UiLoader);
                    break;
                case "tsmiCreateDropDownButton":
                    uep = new DropDownButtonParser(UiLoader);
                    break;
                case "tsmiCreateSplitButton":
                    uep = new SplitButtonParser(UiLoader);
                    break;
                case "tsmiCreateComboBox":
                    uep = new ComboBoxParser(UiLoader);
                    break;
                case "tsmiCreateComboBoxImage":
                    //uep = new ComboBoxImageParser();
                    break;
                case "tsmiCreateTextBox":
                    uep = new TextBoxParser(UiLoader);
                    break;
                case "tsmiCreateLabel":
                    uep = new LabelParser(UiLoader);
                    break;
                case "tsmiCreateStatusLabel":
                    uep = new StatusLabelParser(UiLoader);
                    break;
                case "tsmiCreateSeparator":
                    uep = new SeparatorParser(UiLoader);
                    break;
                case "tsmiCreateProgressBar":
                    uep = new ProgressBarParser(UiLoader);
                    break;
                default: break;

            }

            if (uep != null)
            {
                UiElemParser up = treeView.SelectedNode.Tag as UiElemParser;
                up.UiElemParserList.Add(uep);
                TreeNode node = CreateNode(uep);
                treeView.SelectedNode.Nodes.Add(node);
                IList list = GetCtrlList(treeView.SelectedNode);
                list.Add(uep.UiElem);
            }

            btnSave.Enabled = true;
            _uiLoaderModified = true;
        }

        private IList GetCtrlList(TreeNode treeNode)
        {
            UiElemParser uep = treeNode.Tag as UiElemParser;
            switch (treeNode.Name)
            {
                case "MenuStrip":
                    return (uep.UiElem as MenuStrip).Items;
                case "MenuItem":
                    return (uep.UiElem as ToolStripMenuItem).DropDownItems;
                case "ToolStripContainer":
                    return (uep.UiElem as MyToolStripContainer).ToolStrips;
                case "ToolStrip":
                    return (uep.UiElem as ToolStrip).Items;
                case "DropDownButton":
                    return (uep.UiElem as ToolStripDropDownButton).DropDownItems;
                case "SplitButton":
                    return (uep.UiElem as ToolStripSplitButton).DropDownItems;
                case "StatusStrip":
                    return (uep.UiElem as StatusStrip).Items;
                case "ContextMenuStrip":
                    return (uep.UiElem as ContextMenuStrip).Items;
                case "ComboBox":
                    return (uep.UiElem as ToolStripComboBox).Items;
            }

            return null;
        }


        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            UiElemParser up = treeView.SelectedNode.Tag as UiElemParser;
            if (up != null)
            {
                _uieParser = up.Clone() as UiElemParser;
            }
        }

        private void tsmiCut_Click(object sender, EventArgs e)
        {
            UiElemParser up = treeView.SelectedNode.Tag as UiElemParser;
            if (up != null)
            {
                _uieParser = up;

                UiElemParser uep = treeView.SelectedNode.Parent.Tag as UiElemParser;
                uep.UiElemParserList.Remove(up);
                IList ctrlList = GetCtrlList(treeView.SelectedNode.Parent);
                if (ctrlList != null)
                    ctrlList.Remove(up.UiElem);

                treeView.SelectedNode.Remove();
                btnSave.Enabled = true;
                _uiLoaderModified = true;
            }
        }

        private void tsmiPaste_Click(object sender, EventArgs e)
        {
            if (_uieParser != null)
            {
                UiElemParser uie = _uieParser.Clone() as UiElemParser;
                TreeNode node = CreateNodeRecursive(uie);
                node.Tag = uie;

                UiElemParser uep = treeView.SelectedNode.Tag as UiElemParser;
                uep.UiElemParserList.Add(uie);
                IList ctrlList = GetCtrlList(treeView.SelectedNode);
                if (ctrlList != null)
                    ctrlList.Add(uie.UiElem);

                treeView.SelectedNode.Nodes.Add(node);
                treeView.SelectedNode = node;
                _uiLoaderModified = true;
                btnSave.Enabled = true;
            }
        }

        TreeNode _dragNode;
        UiElemParser _dragUiElemParser;
        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode node = e.Item as TreeNode;
            UiElemParser uie = node.Tag as UiElemParser;
            if (uie != null
                && uie.UiElemType != UiElemType.ToolStripContainer
                && uie.UiElemType != UiElemType.StatusStrip
                && uie.UiElemType != UiElemType.MenuStrip
                && uie.UiElemType != UiElemType.ContextMenuStrips)
            {
                _dragUiElemParser = uie;
                _dragNode = node;
                DoDragDrop(_dragNode, DragDropEffects.Scroll | DragDropEffects.Move);
            }
        }

        int _insertPos = -1;

        private void EraseInsertIndicator()
        {
            if (_insertPos != -1)
            {
                using (Graphics g = treeView.CreateGraphics())
                {
                    using (Pen pen = new Pen(treeView.BackColor))
                    {
                        g.DrawLine(pen, treeView.Bounds.Left, _insertPos, treeView.Bounds.Right, _insertPos);
                    }
                }

                _insertPos = -1;
            }
        }

        private void PaintInsertIndicator()
        {
            using (Graphics g = treeView.CreateGraphics())
            {
                using (Pen pen = new Pen(Color.Red))
                {
                    g.DrawLine(pen, treeView.Bounds.Left, _insertPos, treeView.Bounds.Right, _insertPos);
                }
            }
        }

        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            EraseInsertIndicator();
            Point pt = treeView.PointToClient(new Point(e.X, e.Y));
            if (treeView.ClientRectangle.Contains(pt))
            {
                TreeNode destNode = treeView.GetNodeAt(pt);
                if (destNode != null
                    && destNode != _dragNode
                    && destNode.Parent == _dragNode.Parent)
                {
                    Rectangle rectTop = new Rectangle(destNode.Bounds.Left, 
                        destNode.Bounds.Top,
                        destNode.Bounds.Width / 2,
                        destNode.Bounds.Height / 2);

                    if (rectTop.Contains(pt)) //插入到上方
                        _insertPos = rectTop.Top;
                    else //插入到下方
                        _insertPos = destNode.Bounds.Bottom;

                    PaintInsertIndicator();
                    e.Effect = DragDropEffects.Move;
                    return;
                }
            }
            else
            {
                e.Effect = DragDropEffects.Move;
                return;
            }
            e.Effect = DragDropEffects.None;
        }

        private void treeView_DragDrop(object sender, DragEventArgs e)
        {
            EraseInsertIndicator();
            Point pt = treeView.PointToClient(new Point(e.X, e.Y));
            TreeNode destNode = treeView.GetNodeAt(pt);
            if (destNode != null
                && destNode != _dragNode
                && destNode.Parent == _dragNode.Parent)
            {
                Rectangle rectTop = new Rectangle(destNode.Bounds.Left,
                    destNode.Bounds.Top,
                    destNode.Bounds.Width / 2,
                    destNode.Bounds.Height / 2);

                int insertIndex = -1;
                if (rectTop.Contains(pt))
                    insertIndex = destNode.Index;
                else
                    insertIndex = destNode.Index+1;

                if (insertIndex != -1 && insertIndex != _dragNode.Index)
                {
                    UiElemParser uep = destNode.Parent.Tag as UiElemParser;
                    IList ctrlList = GetCtrlList(destNode.Parent);
                    if (_dragNode.Index > destNode.Index)
                    {
                        _dragNode.Remove();
                        destNode.Parent.Nodes.Insert(insertIndex, _dragNode);

                        uep.UiElemParserList.Remove(_dragUiElemParser);                        
                        uep.UiElemParserList.Insert(insertIndex, _dragUiElemParser);
                        if (ctrlList != null)
                        {
                            ctrlList.Remove(_dragUiElemParser.UiElem);
                            ctrlList.Insert(insertIndex, _dragUiElemParser.UiElem);
                        }                    
                    }
                    else
                    {
                        _dragNode.Remove();
                        destNode.Parent.Nodes.Insert(insertIndex-1, _dragNode);

                        uep.UiElemParserList.Remove(_dragUiElemParser);
                        uep.UiElemParserList.Insert(insertIndex-1, _dragUiElemParser);
                        if (ctrlList != null)
                        {
                            ctrlList.Remove(_dragUiElemParser.UiElem);
                            ctrlList.Insert(insertIndex-1, _dragUiElemParser.UiElem);
                        } 
                    }

                    _uiLoaderModified = true;
                    btnSave.Enabled = true;
                }
            }
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            UiElemParser up = treeView.SelectedNode.Tag as UiElemParser;
            if (up != null)
            {
                UiElemParser uep = treeView.SelectedNode.Parent.Tag as UiElemParser;
                uep.UiElemParserList.Remove(up);
                IList ctrlList = GetCtrlList(treeView.SelectedNode.Parent);
                if (ctrlList != null)
                    ctrlList.Remove(up.UiElem);
                   
                treeView.SelectedNode.Remove();
                btnSave.Enabled = true;
                _uiLoaderModified = true;
            }
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "Service")
            {
                string value = e.ChangedItem.Value.ToString();
                UpdateEditorServiceResource(value);
            }
            else if (e.ChangedItem.Label == "Text")
            {
                treeView.SelectedNode.Text = e.ChangedItem.Value.ToString();
            }
            btnSave.Enabled = true;
            _uiLoaderModified = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_uiLoaderModified)
            {
                _uiLoader.SaveConfig(_uiConfigPath);
            }
            if (_servicesModified)
            {
                _serviceCollection.SaveAddInConfig();
                _serviceCollection.SaveBaseServiceConfig();
            }
            btnSave.Enabled = false;
            _servicesModified = false;
            _uiLoaderModified = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_uiLoaderModified)
            {
                _uiLoader.SaveConfig(_uiConfigPath);
            }
            if (_servicesModified)
            {
                _serviceCollection.SaveAddInConfig();
                _serviceCollection.SaveBaseServiceConfig();
            }
            btnSave.Enabled = false;
            _uiLoaderModified = false;
            _servicesModified = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddInParser addInParser = new AddInParser();
            AddAddInDialog dlgAddAddin = new AddAddInDialog(addInParser);
            dlgAddAddin.Text = "Add AddIn";
            if (dlgAddAddin.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _serviceList.Add(addInParser.Name);
                    lvAddIn.Items.Add(CreateListViewItem(addInParser));
                    _servicesModified = true;
                    btnSave.Enabled = true;
                    if (addInParser.IsBaseService)
                    {
                        _serviceCollection.BaseServiceParserList.Add(addInParser);
                    }
                    else
                    {
                        _serviceCollection.AddInParserList.Add(addInParser);
                    }
                    
                    _serviceCollection.Services.Add(addInParser.Name, addInParser.GetService());
                }
                catch (ArgumentNullException ex2)
                {
                    MessageBox.Show("Filed in creating add-in class instance."+ex2.Message);
                }
                catch (ArgumentException ex1)
                {
                    MessageBox.Show("This add-in have already added." + ex1.Message);
                }
            }
        }

        private void lvAddIn_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ModifySelected();
        }

        private void lvAddIn_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                if (lvAddIn.SelectedItems.Count == 1)
                {
                    btnModifySelected.Enabled = true;
                }
                btnRemoveSelected.Enabled = !((e.Item.Tag as AddInParser).Service is IUiService);
            }
            else
            {
                btnRemoveSelected.Enabled = false;
                btnModifySelected.Enabled = false;
            }
        }

        private void btnModifySelected_Click(object sender, EventArgs e)
        {
            this.ModifySelected();
        }

        private void ModifySelected()
        {
            ListViewItem lvi = lvAddIn.SelectedItems[0];
            AddInParser aip = lvi.Tag as AddInParser;
            AddAddInDialog dlgAddAddin = new AddAddInDialog(aip);
            dlgAddAddin.IsModify = true;
            if (dlgAddAddin.ShowDialog() == DialogResult.OK)
            {
                lvi.SubItems[0].Text = aip.Name;
                lvi.SubItems[1].Text = aip.Author;
                lvi.SubItems[2].Text = aip.Version;
                lvi.SubItems[3].Text = aip.Copyright;
                lvi.SubItems[4].Text = aip.Url;
                lvi.SubItems[5].Text = aip.Path;
                lvi.SubItems[7].Text = aip.Lazyload.ToString();
                lvi.SubItems[8].Text = aip.Description;
                lvi.ImageKey = aip.Valid ? "valid" : "invalid";
                _servicesModified = true;
                btnSave.Enabled = true;
            }
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            AddInParser aip = _serviceCollection.AddInParserList[0];
            lvAddIn.Items.Clear();
            _serviceCollection.BaseServiceParserList.Clear();
            _serviceCollection.AddInParserList.Clear();
            _serviceCollection.Services.Clear();

            _serviceList.Clear();
            if (_lastService != aip.Name)
            {
                _eventList.Clear();
                _functionList.Clear();
                _lastService = string.Empty;
            }

            _serviceList.Add("");
            _serviceList.Add(aip.Name);
            _serviceCollection.Services.Add(aip.Name, aip.Service);
            _serviceCollection.AddInParserList.Add(aip);
            lvAddIn.Items.Add(CreateListViewItem(aip));
            _servicesModified = true;
            btnSave.Enabled = true;
        }

        private void btnDeflate_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lvAddIn.Items)
            {
                if (lvi.ImageKey == "invalid")
                {
                    AddInParser aip = lvi.Tag as AddInParser;
                    if (aip.IsBaseService)
                    {
                        _serviceCollection.BaseServiceParserList.Remove(aip);
                    }
                    else
                    {
                        _serviceCollection.AddInParserList.Remove(aip);
                    }
                    
                    lvAddIn.Items.Remove(lvi);
                    _servicesModified = true;
                    btnSave.Enabled = true;
                }
            }
        }

        private void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            AddInParser aip = lvAddIn.SelectedItems[0].Tag as AddInParser;
            _serviceCollection.AddInParserList.Remove(aip);
            lvAddIn.Items.Remove(lvAddIn.SelectedItems[0]);
            _serviceCollection.Services.Remove(aip.Name);
            _serviceList.Remove(aip.Name);
            if (_lastService != aip.Name)
            {
                _eventList.Clear();
                _functionList.Clear();
                _lastService = string.Empty;
            }

            _servicesModified = true;
            btnSave.Enabled = true;
        }

        private void UpdateEditorParaResource(string p)
        {
            if (p != _lastParent)
            {
                _tsiList.Clear();
                _tsiList.Add("");
                UiElemParser uip = treeView.SelectedNode.Parent.Tag as UiElemParser;
                foreach (UiElemParser u in uip.UiElemParserList)
                {
                    if (u is CmdParser)
                    {
                        _tsiList.Add(u.Name);
                    }
                }
            }
        }

        private void UpdateEditorServiceResource(string value)
        {
            if (value != _lastService)
            {
                _eventList.Clear();
                _eventList.Add("");
                _functionList.Clear();
                _functionList.Add("");
                _injectorList.Clear();
                _injectorList.Add("");

                if (value == "")
                    return;

                try
                {
                    ServiceBase service = _serviceCollection.GetService(value);
                    if (service != null)
                    {
                        Type[] interfaces = service.GetType().GetInterfaces();
                        Type[] types = new Type[interfaces.Length + 1];
                        interfaces.CopyTo(types, 0);
                        types[interfaces.Length] = service.GetType();
                        foreach (Type type in types)
                        {
                            EventInfo[] eventInfos = type.GetEvents(BindingFlags.Instance | BindingFlags.Public);

                            foreach (EventInfo ei in eventInfos)
                            {
                                if (ei.EventHandlerType == typeof(UpdateUiElemHandler))
                                    _eventList.Add(ei.Name);
                            }

                            MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Instance | BindingFlags.Public);

                            foreach (MethodInfo mi in methodInfos)
                            {
                                object[] fas = mi.GetCustomAttributes(typeof(FunctionAttribute), true);
                                if (fas != null && fas.Length > 0)
                                {
                                    _functionList.Add(mi.ToString());
                                }

                                object[] ias = mi.GetCustomAttributes(typeof(InjectorAttribute), true);
                                if (ias != null && ias.Length > 0)
                                {
                                    _injectorList.Add(mi.ToString());
                                }
                            }
                        }
                    }
                }
                catch
                {

                }
            }
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
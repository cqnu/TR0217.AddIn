using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AddIn.Core;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace MyIE
{
    internal partial class FavoritesForm : DockContent
    {
        private MyIEService _myIE;
        private FavoritesAgent _favoritesAgent;
        private EditUrlForm _editUrlForm;

        internal TreeView TreeView
        {
            get { return treeView; }
        }

        internal FavoritesAgent FavoritesAgent
        {
            get { return _favoritesAgent; }
            set 
            {
                _favoritesAgent = value;
                this.InitialTreeView();
            }
        }

        private void InitialTreeView()
        {
            treeView.Nodes.Clear();
            this.InitialTreeView(_favoritesAgent.FavoritesDir, treeView.Nodes);
        }

        Dictionary<string, TreeNode> _nodeDic = new Dictionary<string, TreeNode>();

        public Dictionary<string, TreeNode> NodeDic
        {
            get { return _nodeDic; }
        }
        private void InitialTreeView(FavoritesDir _favoritesDir, TreeNodeCollection treeNodeCollection)
        {
            foreach (FavoritesDir dir in _favoritesDir.FavoritesDirList)
            {
                string[] strs = dir.Path.Split(Path.DirectorySeparatorChar);
                TreeNode node = this.CreateTreeNode(strs[strs.Length-1], "folder");
                node.Tag = dir;
                _nodeDic.Add(dir.Path, node);
                treeNodeCollection.Add(node);
                this.InitialTreeView(dir, node.Nodes);
            }

            foreach (UrlFile url in _favoritesDir.UrlFileList)
            {
                TreeNode node = this.CreateTreeNode(url.FileName, "page");
                node.Tag = url;
                treeNodeCollection.Add(node);
            }
        }

        internal TreeNode CreateTreeNode(string label, string key)
        {
            TreeNode node = new TreeNode(label);
            node.ContextMenuStrip = contextMenuStrip;
            node.ImageKey = key;
            node.SelectedImageKey = key;
            node.Name = key;

            return node;
        }


        public FavoritesForm(MyIEService myIE)
        {
            _myIE = myIE;
            InitializeComponent();
            _myIE.UpdateClose += new UpdateUiElemHandler(_myIE_UpdateClose);
            if(_myIE.CurrentPage != null)
                flag = true;
        }

        bool flag = false;
        void _myIE_UpdateClose(object sender, UpdateUiElemEventArgs e)
        {
            flag = e.Enabled;
        }

        private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Name == "page")
            {
                UrlFile urlFile = e.Node.Tag as UrlFile;
                _myIE.Go(urlFile.Site);
            }
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView.SelectedNode = e.Node;
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tsmiOpenInCurrentPage.Enabled = false;
            tsmiOpenInNewPage.Enabled = false;
            tsmiRepleaseSite.Enabled = false;
            tsmiCopySite.Enabled = false;
            tsbCopyUrl.Enabled = false;
            tsbEdit.Enabled = false;
            tsmiEidt.Enabled = false;

            if (e.Node.Name == "page")
            {
                if (flag)
                {
                    tsmiOpenInCurrentPage.Enabled = true;
                    tsmiRepleaseSite.Enabled = true;
                }
                tsmiOpenInNewPage.Enabled = true;                
                tsmiCopySite.Enabled = true;
                tsbCopyUrl.Enabled = true;
                tsbEdit.Enabled = true;
                tsmiEidt.Enabled = true;
            }
        }

        private void tsmiOpenInNewPage_Click(object sender, EventArgs e)
        {
            UrlFile urlFile = treeView.SelectedNode.Tag as UrlFile;
            _myIE.Go(urlFile.Site);
        }

        private void tsmiOpenInCurrentPage_Click(object sender, EventArgs e)
        {
            UrlFile urlFile = treeView.SelectedNode.Tag as UrlFile;
            _myIE.CurrentPage.Navigate(urlFile.Site);
        }

        private void tsmiOpenFolder_Click(object sender, EventArgs e)
        {
            string path;
            if (treeView.SelectedNode.Name == "page")
            {
                UrlFile urlFile = treeView.SelectedNode.Tag as UrlFile;
                path = Path.GetDirectoryName(urlFile.FullName);
            }
            else
            {
                FavoritesDir dir = treeView.SelectedNode.Tag as FavoritesDir;
                path = dir.Path;
            }
            System.Diagnostics.Process.Start("explorer.exe ", path);
        }

        private void tsmiRepleaseSite_Click(object sender, EventArgs e)
        {
            UrlFile urlFile = treeView.SelectedNode.Tag as UrlFile;
            urlFile.Site = _myIE.CurrentPage.Url;
            urlFile.ToFile();
        }

        private void tsmiCopySite_Click(object sender, EventArgs e)
        {
            UrlFile urlFile = treeView.SelectedNode.Tag as UrlFile;
            Clipboard.SetDataObject(urlFile.Site, true);
        }

        private void tsmiNewFolder2_Click(object sender, EventArgs e)
        {
            string dirPath;
            TreeNodeCollection nc;
            if (treeView.SelectedNode.Name == "page")
            {
                UrlFile urlFile = treeView.SelectedNode.Tag as UrlFile;
                dirPath = Path.GetDirectoryName(urlFile.FullName);
                if (treeView.SelectedNode.Parent == null)
                    nc = treeView.Nodes;
                else
                    nc = treeView.SelectedNode.Parent.Nodes;
            }
            else
            {
                FavoritesDir dir1 = treeView.SelectedNode.Tag as FavoritesDir;
                dirPath = dir1.Path;
                nc = treeView.SelectedNode.Nodes;
            }

            try
            {
                string dirName = FavoritesAgent.GetAcceptableFileName(dirPath, "新建文件夹");

                string path = dirPath + Path.DirectorySeparatorChar + dirName;
                Directory.CreateDirectory(path);
                FavoritesDir dir = new FavoritesDir();
                dir.Path = path;
                TreeNode node = this.CreateTreeNode(dirName, "folder");
                node.Tag = dir;
                _nodeDic.Add(dir.Path, node);
                nc.Insert(0, node);
                treeView.SelectedNode = node;
            }
            catch(Exception ex)
            {
                MessageBox.Show("未能新建文件夹！请确认是否权限读写目录。");
                MyIEService.MyIELogger.Error("未能新建文件夹！请确认是否权限读写目录。",ex);
            }
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeView.SelectedNode.Name == "page")
                {
                    UrlFile urlFile = treeView.SelectedNode.Tag as UrlFile;
                    File.Delete(urlFile.FullName);
                    this.DeleteToolStripItem(urlFile.ToolStripItems);
                }
                else
                {
                    FavoritesDir dir = treeView.SelectedNode.Tag as FavoritesDir;
                    Directory.Delete(dir.Path);
                    this.DeleteToolStripItem(dir.ToolStripItems);
                }

                treeView.SelectedNode.Remove();
            }
            catch(Exception ex)
            {
                MessageBox.Show("未能删除！请检查读写属性。");
                MyIEService.MyIELogger.Error("未能删除！请检查读写属性。",ex);
            }
        }

        private void DeleteToolStripItem(List<ToolStripItem> list)
        {
            foreach (ToolStripItem tsi in list)
            {
                tsi.Visible = false;
            }
            list.Clear();
        }

        private void tsmiRename_Click(object sender, EventArgs e)
        {
            treeView.LabelEdit = true;
            treeView.SelectedNode.BeginEdit();
        }

        private void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (String.IsNullOrEmpty(e.Label))
                e.CancelEdit = true;
            else
            {
                try
                {
                    if (treeView.SelectedNode.Name == "page")
                    {

                        UrlFile urlFile = treeView.SelectedNode.Tag as UrlFile;
                        string src = urlFile.FullName;
                        urlFile.FileName = e.Label;
                        File.Move(src, urlFile.FullName);
                        this.RenameLabelToolStripItem(urlFile.ToolStripItems,e.Label,40);
                    }
                    else
                    {
                        FavoritesDir dir = treeView.SelectedNode.Tag as FavoritesDir;
                        int i = dir.Path.LastIndexOf(Path.DirectorySeparatorChar);
                        string path = dir.Path.Substring(0, i + 1);
                        path += e.Label;
                        Directory.Move(dir.Path, path);
                        dir.Path = path;
                        this.RenameLabelToolStripItem(dir.ToolStripItems, e.Label, 30);
                    }
                }
                catch(Exception ex)
                {
                    e.CancelEdit = true;
                    MessageBox.Show("重命名失败！请检查读写属性。");
                    MyIEService.MyIELogger.Error("重命名失败！请检查读写属性。", ex);
                }
            }
        }

        private void RenameLabelToolStripItem(List<ToolStripItem> list, string p, int len)
        {
            foreach (ToolStripItem tsi in list)
            {
                tsi.Text = _myIE.CutString(p, len);
            }
        }

        private void tsmiEidt_Click(object sender, EventArgs e)
        {
            UrlFile urlFile = treeView.SelectedNode.Tag as UrlFile;
            if (_editUrlForm == null)
                _editUrlForm = new EditUrlForm(_favoritesAgent);

            _editUrlForm.UrlFile = urlFile;

            if (_editUrlForm.ShowDialog() == DialogResult.OK)
            {
                if (_editUrlForm.NameChanged || _editUrlForm.PathChanged || _editUrlForm.SiteChanged)
                {
                    try
                    {
                        File.Delete(_editUrlForm.OriginalFileName);
                    }
                    catch { }
                    urlFile.ToFile();
                    _editUrlForm.FavoritesDir.UrlFileList.Add(urlFile);
                }

                if (_editUrlForm.PathChanged)
                {
                    TreeNode node = treeView.SelectedNode;
                    node.Text = urlFile.FileName;
                    string s = Path.GetDirectoryName(urlFile.FullName);
                    this.DeleteToolStripItem(urlFile.ToolStripItems);
                    if (s == System.Environment.GetFolderPath(Environment.SpecialFolder.Favorites))
                    {
                        node.Remove();
                        treeView.Nodes.Add(node);
                        _favoritesAgent.FavoritesDir.UrlFileList.Remove(urlFile);
                        _myIE.FavoritesMenu.DropDownItems.Add(_myIE.CreateToolStripMenuItem(urlFile));
                        _myIE.FavoritesStrip.Items.Add(_myIE.CreateToolStripButton(urlFile));
                    }
                    else
                    {
                        if (node.Parent == null)
                            _favoritesAgent.FavoritesDir.UrlFileList.Remove(urlFile);
                        else
                            (node.Parent.Tag as FavoritesDir).UrlFileList.Remove(urlFile);
                        node.Remove();
                        TreeNode n = _nodeDic[s];
                        FavoritesDir fdir = n.Tag as FavoritesDir;
                        foreach(ToolStripItem tsi in fdir.ToolStripItems)
                        {
                            ToolStripDropDownItem tsddi = tsi as ToolStripDropDownItem;
                            if (tsddi != null)
                                tsddi.DropDownItems.Add(_myIE.CreateToolStripMenuItem(urlFile));
                        }
                        n.Nodes.Add(node);
                    }
                }
                else if (_editUrlForm.NameChanged)
                {
                    treeView.SelectedNode.Text = urlFile.FileName;
                    this.RenameLabelToolStripItem(urlFile.ToolStripItems, urlFile.FileName, 30);
                }
            }
            
        }

        private void tsmiNewSite_Click(object sender, EventArgs e)
        {
            if (_editUrlForm == null)
                _editUrlForm = new EditUrlForm(_favoritesAgent);
            _editUrlForm.UrlFile = new UrlFile();

            if (_editUrlForm.ShowDialog() == DialogResult.OK)
            {
                _editUrlForm.UrlFile.ToFile();

                TreeNode node = this.CreateTreeNode(_editUrlForm.UrlFile.FileName, "page");
                node.Tag = _editUrlForm.UrlFile;
                TreeNodeCollection tc;
                FavoritesDir fdir = _editUrlForm.FavoritesDir;
                string s = Path.GetDirectoryName(_editUrlForm.UrlFile.FullName);
                if (s == System.Environment.GetFolderPath(Environment.SpecialFolder.Favorites))
                {
                    tc = treeView.Nodes;
                    _myIE.FavoritesMenu.DropDownItems.Add(_myIE.CreateToolStripMenuItem(_editUrlForm.UrlFile));
                    _myIE.FavoritesStrip.Items.Add(_myIE.CreateToolStripButton(_editUrlForm.UrlFile));
                }
                else
                {
                    tc = _nodeDic[s].Nodes;
                    foreach (ToolStripItem tsi in fdir.ToolStripItems)
                    {
                        ToolStripDropDownItem tsddi = tsi as ToolStripDropDownItem;
                        if (tsddi != null)
                            tsddi.DropDownItems.Add(_myIE.CreateToolStripMenuItem(_editUrlForm.UrlFile));
                    }
                }

                fdir.UrlFileList.Add(_editUrlForm.UrlFile);
                tc.Add(node);
            }
        }
    }
}
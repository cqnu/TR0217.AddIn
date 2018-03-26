using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TR0217.ControlEx;
using System.IO;

namespace MyIE
{
    internal partial class EditUrlForm : Form
    {
        private FavoritesAgent _favoritesAgent;
        private UrlFile _urlFile;
        private bool _nameChanged = false;
        private bool _siteChanged = false;
        private bool _pathChanged = false;
        private string _originalFileName;

        public string OriginalFileName
        {
            get { return _originalFileName; }
        }

        private FavoritesDir _favoritesDir;

        internal FavoritesDir FavoritesDir
        {
            get { return _favoritesDir; }
        }

        public bool PathChanged
        {
            get { return _pathChanged; }
        }

        public bool SiteChanged
        {
            get { return _siteChanged; }
        }

        public bool NameChanged
        {
            get { return _nameChanged; }
        }

        internal UrlFile UrlFile
        {
            get { return _urlFile; }
            set 
            { 
                _urlFile = value;
                if (_urlFile != null)
                {
                    _originalFileName = _urlFile.FullName;
                    txtName.Text = _urlFile.FileName;
                    mtxtUrl.Text = _urlFile.Site;
                    string path;
                    try
                    {
                        path = Path.GetDirectoryName(_urlFile.FullName);
                    }
                    catch
                    {
                        path = System.Environment.GetFolderPath(Environment.SpecialFolder.Favorites);
                    }
                    this.SetSelectedItem(path);
                    _nameChanged = false;
                    _siteChanged = false;
                    _pathChanged = false;
                }
            }
        }

        private void SetSelectedItem(string path)
        {
            foreach (Object obj in cboiFolder.Items)
            {
                ComboBoxImageItem cboi = obj as ComboBoxImageItem;
                if (cboi != null)
                {
                    if ((cboi.Tag as FavoritesDir).Path== path)
                        cboiFolder.SelectedItem = cboi;
                }
            }
        }

        public EditUrlForm(FavoritesAgent favoritesAgent)
        {
            _favoritesAgent = favoritesAgent;
            InitializeComponent();
            ComboBoxImageItem item = new ComboBoxImageItem();
            //string path = System.Environment.GetFolderPath(Environment.SpecialFolder.Favorites);
            item.Text = "收藏夹";
            item.Level = 0;
            item.Tag = _favoritesAgent.FavoritesDir;
            cboiFolder.Items.Add(item);
            //this.ProcessFavoriates(path);
            this.ProcessFavoriates(_favoritesAgent.FavoritesDir);
            cboiFolder.SelectedIndex = 0;
            _favoritesDir = favoritesAgent.FavoritesDir;
        }

        int level = 0;
        private void ProcessFavoriates(FavoritesDir fdir)
        {
            level++;
            foreach (FavoritesDir dir in fdir.FavoritesDirList)
            {
                ComboBoxImageItem item = new ComboBoxImageItem();
                int i = dir.Path.LastIndexOf(Path.DirectorySeparatorChar);
                item.Text = dir.Path.Substring(i + 1);
                item.Level = level;
                item.Tag = dir;
                cboiFolder.Items.Add(item);
                this.ProcessFavoriates(dir);
            }
            level--;
        }
        private void ProcessFavoriates(string path)
        {
            level++;
            foreach (string dir in Directory.GetDirectories(path))
            {
                ComboBoxImageItem item = new ComboBoxImageItem();
                int i = dir.LastIndexOf(Path.DirectorySeparatorChar);
                item.Text = dir.Substring(i+1);
                item.Level = level;
                item.Tag = dir;
                cboiFolder.Items.Add(item);
                this.ProcessFavoriates(dir);
            }
            level--;
        }

        bool flag = false;

        private void btnOK_Click(object sender, EventArgs e)
        {
            _favoritesDir = (cboiFolder.SelectedItem as ComboBoxImageItem).Tag as FavoritesDir;
            if (String.IsNullOrEmpty(txtName.Text)
                || String.IsNullOrEmpty(mtxtUrl.Text))
            {
                flag = true;
                return;
            }
            string fileName = txtName.Text;
            if(!fileName.EndsWith(".url",true,null))
                fileName +=".url";

            if (_pathChanged)
                _urlFile.FullName = _favoritesDir.Path + Path.DirectorySeparatorChar + fileName;

            else if (_nameChanged)
                _urlFile.FileName = fileName;

            if(_siteChanged)
                _urlFile.Site = mtxtUrl.Text;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            _nameChanged = true;
        }

        private void mtxtUrl_TextChanged(object sender, EventArgs e)
        {
            _siteChanged = true;
        }

        private void cboiFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            _pathChanged = true;
        }

        private void EditUrlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flag)
            {
                MessageBox.Show("请输入合理的名称和地址！");
                e.Cancel = true;
            }
        }
    }
}
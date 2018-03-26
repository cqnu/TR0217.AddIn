using System;
using System.Collections.Generic;
using System.Text;
using AddIn.Core;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using mshtml;
using System.Xml;
using System.Threading;
using MyIE.Properties;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using AddIn.UiInterface;

namespace MyIE
{
    public class MyIEService:ServiceBase, IMyIE
    {
        //private ContextMenuStrip _contextMenuStripClient = null;
        private ContextMenuStrip _contextMenuStripCaption = null;

        internal static log4net.ILog MyIELogger = log4net.LogManager.GetLogger("MyIE.MyIEService");
        private FavoritesAgent _favoritesAgent;
        private FavoritesForm _favoritesForm;
        private ToolStrip _favoritesStrip;
        private HistoryUrlList _historyUrlList;
        private System.Threading.Timer _timer;

        internal ToolStrip FavoritesStrip
        {
            get { return _favoritesStrip; }
        }
        private ToolStripMenuItem _favoritesMenu;

        internal ToolStripMenuItem FavoritesMenu
        {
            get { return _favoritesMenu; }
        }
        private PageForm _currentPage;
        private string _currentSearchEngine;
        private List<PageForm> _visiblePageList;

        private IServiceCollection _services;
        private IUiExService _uiService = null;
        private const int MaxHistoryPageCount = 15;

        //private LeakStack<PageForm> _hidePageLeakStack;
        private LeakStack<string> _closedUrlLeakStack;

        private ImageList _imageList;

        internal ImageList ImageList
        {
            get { return _imageList; }
            set { _imageList = value; }
        }
        private OpenFileDialog _openFileDialog = null;
        private SaveFileDialog _saveFileDialog = null;

        public event UpdateUiElemHandler UpdateSite;
        public event UpdateUiElemHandler UpdateGoBack;
        public event UpdateUiElemHandler UpdateGoForward;
        public event UpdateUiElemHandler UpdateClose;
        public event UpdateUiElemHandler UpdateSearchEngine;
        public event UpdateUiElemHandler UpdateProgress;
        public event UpdateUiElemHandler UpdateRestore;
        public event UpdateUiElemHandler UpdateComplete;
        public event UpdateUiElemHandler UpdateStatus;
        public event UpdateUiElemHandler UpdateSearchKeyword;

        public PageForm CurrentPage
        {
            get { return _currentPage; }
        }

        public MyIEService()
        {
            _currentPage = null;
            _historyUrlList = new HistoryUrlList(Application.StartupPath + @"\AddIns\MyIE\HistoryUrlList.html");
            _timer = new System.Threading.Timer(SaveHistoryUrlList,null,30000,30000);
            _currentSearchEngine = "http://www.baidu.com/s?wd=";
            _visiblePageList = new List<PageForm>(MaxHistoryPageCount);
            //_hidePageLeakStack = new LeakStack<PageForm>(MaxHistoryPageCount);
            _closedUrlLeakStack = new LeakStack<string>(MaxHistoryPageCount);
            _imageList = new ImageList();

            _imageList.Images.Add(Resources.folderClosed);
            _imageList.Images.Add(Resources.page);

            //_hidePageLeakStack.OnPoped += new EventHandler(OnPoped);
            //_hidePageLeakStack.OnPushed += new EventHandler(OnPushed);
            //_hidePageLeakStack.OnLeak += new LeakStack<PageForm>.LeakHandler(OnLeak);

            _closedUrlLeakStack.OnPoped += new EventHandler(OnPoped);
            _closedUrlLeakStack.OnPushed += new EventHandler(OnPushed);
            FavoritesAgent.OnAddFavoritesItem += new ProcessFavoritesHandler(FavoritesAgent_OnProcessFavoritesMenu);
            FavoritesAgent.OnAddFavoritesItem += new ProcessFavoritesHandler(FavoritesAgent_OnProcessFavoritesStrip);

            AppFrame.AfterLoadMainForm += new LoadMainFormHandler(AppFrame_AfterLoadMainForm);
        }


        internal ToolStripButton CreateToolStripButton(UrlFile urlFile)
        {
            ToolStripButton l = new ToolStripButton(CutString(urlFile.FileName, 40));
            l.Image = _imageList.Images[1];
            l.Click += new EventHandler(mi_Click);
            l.Tag = urlFile;
            urlFile.ToolStripItems.Add(l);
            return l;
        }

        internal ToolStripMenuItem CreateToolStripMenuItem(UrlFile urlFile)
        {
            ToolStripMenuItem mi = new ToolStripMenuItem(CutString(urlFile.FileName, 40));
            mi.Image = _imageList.Images[1];
            mi.Click += new EventHandler(mi_Click);
            mi.Tag = urlFile;
            urlFile.ToolStripItems.Add(mi);

            return mi;
        }

        internal ToolStripMenuItem CreateToolStripMenuItem(FavoritesDir fdir)
        {
            ToolStripMenuItem mi = new ToolStripMenuItem(CutString(Path.GetFileNameWithoutExtension(fdir.Path), 30));
            mi.Image = _imageList.Images[0];
            mi.Tag = fdir;
            fdir.ToolStripItems.Add(mi);

            return mi;
        }

        internal ToolStripDropDownButton CreateToolStripDropDownButton(FavoritesDir fdir)
        {
            ToolStripDropDownButton ddb = new ToolStripDropDownButton(CutString(Path.GetFileNameWithoutExtension(fdir.Path), 30));
            ddb.Image = _imageList.Images[0];
            ddb.Tag = fdir;
            fdir.ToolStripItems.Add(ddb);

            return ddb;
        }

        ToolStripMenuItem _tsmi;
        int _level1 = 0;
        void FavoritesAgent_OnProcessFavoritesMenu(object sender, FavoritesEventArgs e)
        {
            try
            {
                ToolStripMenuItem mi = null;

                if (e.UrlFile != null)
                {
                    mi = this.CreateToolStripMenuItem(e.UrlFile);
                }
                else
                {
                    mi = this.CreateToolStripMenuItem(e.FavoritesDir);
                }

                this.ResetTsmi(e.Level, _level1);
                _tsmi.DropDownItems.Add(mi);
                _tsmi = mi;
                _level1 = e.Level;
            }
            catch (System.Exception ex)
            {
                MyIELogger.Fatal("生成收藏夹菜单失败！", ex);
            }
        }

        private void ResetTsmi(int level0, int level1)
        {
            int l = level0;
            while (l <= level1)
            {
                _tsmi = _tsmi.OwnerItem as ToolStripMenuItem;
                l++;
            }
        }

        ToolStripItem _tsi;
        int _level2 = 0;
        void FavoritesAgent_OnProcessFavoritesStrip(object sender, FavoritesEventArgs e)
        {
            try
            {
                ToolStripItem tsi = null;
                ToolStripItemCollection tsic = null;

                if (e.Level == 1)
                {
                    if (e.UrlFile != null)
                    {
                        tsi = this.CreateToolStripButton(e.UrlFile);
                    }
                    else
                    {
                        tsi = this.CreateToolStripDropDownButton(e.FavoritesDir);
                    }
                    tsic = _favoritesStrip.Items;
                }
                else
                {
                    this.ResetTsi(e.Level, _level2);
                    if (e.UrlFile != null)
                    {
                        tsi = this.CreateToolStripMenuItem(e.UrlFile);
                    }
                    else
                    {
                        tsi = this.CreateToolStripMenuItem(e.FavoritesDir);
                    }

                    tsic = (_tsi as  ToolStripDropDownItem).DropDownItems;
                }

                tsic.Add(tsi);
                _tsi = tsi;

                _level2 = e.Level;
            }
            catch (System.Exception ex)
            {
                MyIELogger.Fatal("生成收藏夹工具条失败！", ex);
            }
        }

        private void ResetTsi(int level0, int level1)
        {
            int l = level0 == 1 ? 2 : level0;
            while (l <= level1)
            {
                _tsi = _tsi.OwnerItem as ToolStripItem;
                l++;
            }
        }

        void mi_Click(object sender, EventArgs e)
        {
            UrlFile url = (sender as ToolStripItem).Tag as UrlFile;
            Go(url.Site);
        }

        void AppFrame_AfterLoadMainForm(LoadMainFormEventArgs e)
        {
            _services = AppFrame.ServiceCollection;
            _uiService = (IUiExService)_services.GetService<IUiExService>();
            _uiService.MainForm.WindowState = FormWindowState.Maximized;

            _favoritesMenu = _uiService.GetToolStripItem("MenuStrip/收藏(&B)/") as ToolStripMenuItem;
            _favoritesMenu.DropDownItems.Add(new ToolStripSeparator());
            _tsmi = _favoritesMenu;
            _favoritesStrip = _uiService.GetToolStrip("tspFavorites");
            _contextMenuStripCaption = _uiService.GetContextMenuStrip("cmsPageForm");

            if (_favoritesMenu == null)
                MyIELogger.Error("获取收藏菜单栏失败！请用界面配置工具查看是否存在路径为“MenuStrip/收藏(&B)/”的菜单项。");

            if (_favoritesStrip == null)
                MyIELogger.Error("获取收藏工具条失败！请用界面配置工具查看是否存在名称为“ts1”的工具条。");

            _favoritesAgent = new FavoritesAgent();


            if (e.Args != null && e.Args.Length > 0)
                Go(e.Args[0]);
            else GoHome();

            _uiService.MainForm.TopLevel = true;
            _uiService.MainForm.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
        }

        private void SaveHistoryUrlList(object state)
        {
            try
            {
                _historyUrlList.Save();
            }
            catch(Exception e)
            {
                MessageBox.Show("记录访问历史失败！请检查插件日志获取详细信息！");
                MyIELogger.Fatal("记录访问历史失败！",e);
            }
        }

        //程序退出时保存浏览历史
        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _timer.Dispose();
            _historyUrlList.Save();
        }

        private void OnPoped(object sender, EventArgs e)
        {
            if (_closedUrlLeakStack.Count == 0)
            {
                if (UpdateRestore != null)
                {
                    UpdateUiElemEventArgs arg = new UpdateUiElemEventArgs();
                    arg.Enabled = false;
                    UpdateRestore(this, arg);
                }
            }
        }

        private void OnPushed(object sender, EventArgs e)
        {
            if (UpdateRestore != null)
            {
                UpdateUiElemEventArgs arg = new UpdateUiElemEventArgs();
                arg.Enabled = true;
                UpdateRestore(this, arg);
            }
        }

        private void OnLeak(object sender,LeakStack<PageForm>.LeakEventArgs e)
        {
            e.LeakItem.Dispose();
        }

        private bool _annal = true;
        public void Go(string url)
        {
            PageForm page = new PageForm();
            page.TabPageContextMenuStrip = _contextMenuStripCaption;
            page._annal = _annal;
            _currentPage = page;
            page.Load += new EventHandler(page_Load);
            page.Activated += new EventHandler(page_Activated);
            //page.Deactivate += new EventHandler(page_Deactivate);

            page.FormClosing += new FormClosingEventHandler(page_FormClosing);

            page.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(page_DocumentCompleted);
            page.ProgressChanged += new WebBrowserProgressChangedEventHandler(page_ProgressChanged);
            page.StatusTextChanged += new EventHandler(page_StatusTextChanged);
            page.CanGoBackChanged += new EventHandler(page_CanGoBackChanged);
            page.CanGoForwardChanged += new EventHandler(page_CanGoForwardChanged);
            page.Navigated += new WebBrowserNavigatedEventHandler(page_Navigated);
            page.WebBrowser.NewWindowEx += new NewWindowExEventHandler(WebBrowser_NewWindowEx);
            page.WebBrowser.SelectedTextChanged += new EventHandler(WebBrowser_SelectedTextChanged);

            _uiService.ShowDocForm(page);

            if (string.IsNullOrEmpty(url))
                page.WebBrowser.GoHome();
            else
                page.Navigate(url);

            page.Text = CutString(page.Url, 20);

            UpdateUiElemEventArgs arg = new UpdateUiElemEventArgs();


            if (UpdateClose != null)
            {
                UpdateClose(this, arg);
            }


            if (UpdateSite != null)
            {
                arg.Text = page.Url;
                arg.Maximum = 20;
                arg.Value = page.Url;
                UpdateSite(this, arg);
            }
        }

        public void GoHome()
        {
            _annal = false;
            Go(null);
            _annal = true;
        }

        void page_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            PageForm page = sender as PageForm;

            if (!page.IsActivated)
                return;

            UpdateUiElemEventArgs arg = new UpdateUiElemEventArgs();
            if (UpdateSite != null)
            {
                try
                {
                    arg.Enabled = true;
                    arg.Text = _currentPage.Url;
                    arg.Visible = true;
                    arg.Count = -1;
                    UpdateSite(this, arg);
                }
                catch
                {
                }
            }
        }


        void page_CanGoForwardChanged(object sender, EventArgs e)
        {
            PageForm page = sender as PageForm;

            if (!page.IsActivated)
                return;

            UpdateUiElemEventArgs arg = new UpdateUiElemEventArgs();
            arg.Enabled = page.WebBrowser.CanGoForward;
            if (UpdateGoForward != null)
                UpdateGoForward(this, arg);

        }

        void page_CanGoBackChanged(object sender, EventArgs e)
        {
            PageForm page = sender as PageForm;

            if (!page.IsActivated)
                return;

            UpdateUiElemEventArgs arg = new UpdateUiElemEventArgs();

            arg.Enabled = page.WebBrowser.CanGoBack;
            if (UpdateGoBack != null)
                UpdateGoBack(this, arg);

        }

        void WebBrowser_SelectedTextChanged(object sender, EventArgs e)
        {
            if (UpdateSearchKeyword != null)
            {
                UpdateUiElemEventArgs arg = new UpdateUiElemEventArgs();
                arg.Text = (sender as WebBrowserEx).SelectedText;
                UpdateSearchKeyword(this, arg);
            }
        }

        void WebBrowser_NewWindowEx(object sender, NewWindowExEventArgs e)
        {
            e.Cancel = true;
            Go(e.Url.AbsoluteUri);
        }

        public void GoInCurrentPage(string url)
        {
            _currentPage.WebBrowser.Navigate(url);
        }

        public void CopyCurrentPage()
        {
            Go(_currentPage.Url);
        }

        void page_StatusTextChanged(object sender, EventArgs e)
        {
            PageForm page = sender as PageForm;

            if (!page.IsActivated)
                return;

            if (UpdateStatus != null)
            {
                UpdateUiElemEventArgs arg = new UpdateUiElemEventArgs();
                arg.Text = page.WebBrowser.StatusText;
                UpdateStatus(this, arg);
            }
        }

        void page_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            PageForm page = sender as PageForm;

            if (!page.IsActivated)
                return;

            if (UpdateProgress != null)
            {
                UpdateUiElemEventArgs arg = new UpdateUiElemEventArgs();
                arg.Count = page.Progress;
                arg.Maximum = 100;
                arg.Visible = page.Progress < 100;
                UpdateProgress(this, arg);
            }
        }

        void page_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            PageForm page = sender as PageForm;

            if (!page.IsActivated)
                return;

            UpdateUiElemEventArgs arg = new UpdateUiElemEventArgs();

            if (UpdateComplete != null)
            {
                arg.Enabled = true;
                arg.Visible = true;
                UpdateComplete(this, arg);
            }

            if (UpdateProgress != null)
            {
                arg.Count = page.Progress;
                arg.Maximum = 100;
                arg.Visible = false;
                UpdateProgress(this, arg);
            }
        }

        void page_FormClosing(object sender, FormClosingEventArgs e)
        {
            PageForm page = sender as PageForm;

            if(page._annal)
                _historyUrlList.Append(page.Url, page.Text);

            if (e.CloseReason == CloseReason.UserClosing)
            {
                //e.Cancel = true;
                //page.AxWebBrowser.Stop();
                //page.Hide();
                //_hidePageLeakStack.Push(page);
                _closedUrlLeakStack.Push(page.Url);
                _visiblePageList.Remove(page);
            }
        }

        void page_Deactivate(object sender, EventArgs e)
        {
            _currentPage = null;

            UpdateUiElemEventArgs arg = new UpdateUiElemEventArgs();
            arg.Enabled = false;

            if (UpdateGoBack != null)
            {
                UpdateGoBack(this, arg);
            }
            if (UpdateGoForward != null)
            {
                UpdateGoForward(this, arg);
            }

            if (UpdateClose != null)
            {
                UpdateClose(this, arg);
            }

            if (UpdateComplete != null)
            {
                UpdateComplete(this, arg);
            }

            if (UpdateProgress != null)
            {
                arg.Enabled = true;
                arg.Visible = false;
                UpdateProgress(this, arg);
            }
        }

        void page_Activated(object sender, EventArgs e)
        {
            _currentPage = sender as PageForm;
            UpdateUiElemEventArgs arg = new UpdateUiElemEventArgs();

            if (UpdateGoBack != null)
            {
                arg.Enabled = _currentPage.WebBrowser.CanGoBack;
                UpdateGoBack(this, arg);
            }
            if (UpdateGoForward != null)
            {
                arg.Enabled = _currentPage.WebBrowser.CanGoForward;
                UpdateGoForward(this, arg);
            }

            if (UpdateClose != null)
            {
                arg.Enabled = true;
                UpdateClose(this, arg);
            }

            if (UpdateComplete != null)
            {
                arg.Enabled = _currentPage.Complete;
                UpdateComplete(this, arg);
            }

            if (UpdateProgress != null)
            {
                arg.Enabled = true;
                arg.Maximum = 100;
                arg.Count = _currentPage.Progress;
                arg.Visible = _currentPage.Progress < 100;
                UpdateProgress(this, arg);
            }
            if (UpdateSite != null)
            {
                try
                {
                    arg.Enabled = true;
                    arg.Text = _currentPage.Url;
                    arg.Visible = true;
                    arg.Count = -1;
                    UpdateSite(this, arg);
                }
                catch
                {
                }
            }

        }

        void page_Load(object sender, EventArgs e)
        {
            _visiblePageList.Add(sender as PageForm);
        }

        public void GoBack()
        {
            _currentPage.WebBrowser.GoBack();
        }

        public void GoForward()
        {
            _currentPage.WebBrowser.GoForward();
        }

        public void Stop()
        {
            _currentPage.WebBrowser.Stop();
        }

        public void Refresh()
        {
            try
            {
                _currentPage.WebBrowser.Refresh();
            }
            catch
            { }
        }

        public void Search(string keyword)
        {
            Search(_currentSearchEngine, keyword);
        }

        private void Search(string url, string keyword)
        {
            if (keyword.Length < 1)
            {
                Go(url);
                return;
            }
            Go(url + System.Web.HttpUtility.UrlEncode(Encoding.Default.GetBytes(keyword)));
        }

        public void SetSearchEngine(string url, string name)
        {
            _currentSearchEngine = url;

            if (UpdateSearchEngine != null)
            {
                UpdateUiElemEventArgs arg = new UpdateUiElemEventArgs();
                arg.Text = name;
                arg.Enabled = false;
                UpdateSearchEngine(this, arg);
            }
        }

        public void SetEngineSearch(string key, string url, string name)
        {
            _currentSearchEngine = url;

            if (UpdateSearchEngine != null)
            {
                UpdateUiElemEventArgs arg = new UpdateUiElemEventArgs();
                arg.Text = name;
                arg.Enabled = false;
                UpdateSearchEngine(this, arg);
            }

            Search(key);
        }

        public void Restore()
        {
            //PageForm page = _hidePageLeakStack.Pop();
            //page.WebBrowser.Refresh();
            //_uiService.ShowDocForm(page);
            //_visiblePageList.Add(page);
            //_currentPage = page;
            //page.Select();

            Go(_closedUrlLeakStack.Pop());
        }

        public void CloseCurrentPage()
        {
            if (_currentPage != null)
                _currentPage.Close();
        }

        public void CloseAllOtherPages()
        {
            PageForm[] pageA = _visiblePageList.ToArray();
            for (int i = 0; i < pageA.Length; i++)
            {
                if (pageA[i] != _currentPage)
                    pageA[i].Close();
            }
        }

        public void CloseAllPages()
        {
            PageForm[] pageA = _visiblePageList.ToArray();
            for (int i = 0; i < pageA.Length; i++)
            {
                pageA[i].Close();
            }
        }

        public void OpenLoacal()
        {
            if (_openFileDialog == null)
            {
                _openFileDialog = new OpenFileDialog();
                _openFileDialog.Filter = "网页文件(*htm;*html;*mht;*.url)|*.htm;*.html;*.mht;*.url";
                _openFileDialog.Multiselect = false;
            }
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (_openFileDialog.FileName.EndsWith(".url", true, null))
                {
                    UrlFile urlFile = new UrlFile();
                    urlFile.FromFile(_openFileDialog.FileName);
                    Go(urlFile.Site);
                }
                else
                {
                    Go(_openFileDialog.FileName);
                }
            }
        }

        public void SaveCurrentPage()
        {
            _currentPage.WebBrowser.ShowSaveAsDialog();
        }

        public void SaveCurrentPageAsIamge()
        {
            if (_saveFileDialog == null)
            {
                _saveFileDialog = new SaveFileDialog();
                _saveFileDialog.AddExtension = true;
                _saveFileDialog.OverwritePrompt = true;
            }
            _saveFileDialog.Filter = "BMP文件(*.bmp)|*.bmp|jpg文件(*.jpg,*.jpeg)|*.jpg;*.jpeg";
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Size mySize = _currentPage.WebBrowser.Size; 
                Bitmap myPic = new Bitmap(mySize.Width, mySize.Height);
                Rectangle myRec = new Rectangle(0, 0, mySize.Width, mySize.Height);
                _currentPage.WebBrowser.DrawToBitmap(myPic, myRec);
                myPic.Save(_saveFileDialog.FileName);
            }
        }

        public void PageSetup()
        {
            _currentPage.WebBrowser.ShowPageSetupDialog();
        }

        public void PrintPreview()
        {
            _currentPage.WebBrowser.ShowPrintPreviewDialog();
        }

        public void Print()
        {
            _currentPage.WebBrowser.ShowPrintDialog();
        }

        public void PageProperties()
        {
            _currentPage.WebBrowser.ShowPropertiesDialog();
        }

        public void Zoom(int size)
        {
            string zoom = _currentPage.WebBrowser.Document.Body.Style;

            switch (size)
            {
                case 0:
                    zoom = "zoom:70%";
                    break;
                case 1:
                    zoom = "zoom:100%";
                    break;
                case 2:
                    zoom = "zoom:150%";
                    break;
                case 3:
                    zoom = "zoom:200%";
                    break;
                default:
                    zoom = "zoom:200%";
                    break;
            }

            _currentPage.WebBrowser.Document.Body.Style = zoom;
        }

        //FindForm _findForm;
        public void Find()
        {
            IntPtr vHandle = _currentPage.WebBrowser.Handle;
            vHandle = FindWindowEx(vHandle, IntPtr.Zero, "Shell Embedding", null);
            vHandle = FindWindowEx(vHandle, IntPtr.Zero, "Shell DocObject View", null);
            vHandle = FindWindowEx(vHandle, IntPtr.Zero, "Internet Explorer_Server", null);
            SendMessage(vHandle, WM_COMMAND, IDM_FIND, (int)vHandle);
            //if (_findForm == null)
            //    _findForm = new FindForm();
            //_findForm.WebBrowser = _currentPage.WebBrowser;
            //_findForm.Show(_uiService.MainForm);
        }

        //private bool Find(string text, bool forward, bool matchWholeWord, bool matchCase)
        //{
        //    bool success = false;
        //    if (_currentPage.WebBrowser.Document != null)
        //    {
        //        IHTMLDocument2 doc = _currentPage.WebBrowser.Document.DomDocument as IHTMLDocument2;
        //        IHTMLBodyElement body = doc.body as IHTMLBodyElement;

        //        if (body != null)
        //        {
        //            IHTMLTxtRange range;
        //            if (doc.selection != null)
        //            {
        //                range = doc.selection.createRange() as IHTMLTxtRange;
        //                if (forward)
        //                    range.moveStart("character", 1);
        //                else
        //                    range.moveEnd("character", -1);
        //            }
        //            else
        //                range = body.createTextRange();

        //            int flags = 0;
        //            if (matchWholeWord) flags += 2;
        //            if (matchCase) flags += 4;
        //            success = range.findText(text, forward ? 999999 : -999999, flags);

        //            if (success)
        //            {
        //                range.select();
        //                range.scrollIntoView(!forward);
        //            }
        //        }
        //    }

        //    return success;
        //}


        //public void Find(string keyword)
        //{
        //    this.Find(keyword, true, true, false);
        //}

        public void InternetOption()
        {
            try
            {
                System.Diagnostics.Process.Start("Rundll32.exe", "shell32.dll,Control_RunDLL InetCpl.cpl,,0");
            }
            catch
            {
            }
        }

        private EditUrlForm _editUrlForm;
        public void AddFavorite()
        {
            //IntPtr vHandle = _currentPage.WebBrowser.Handle;
            //vHandle = FindWindowEx(vHandle, IntPtr.Zero, "Shell Embedding", null);
            //vHandle = FindWindowEx(vHandle, IntPtr.Zero, "Shell DocObject View", null);
            //vHandle = FindWindowEx(vHandle, IntPtr.Zero, "Internet Explorer_Server", null);
            //SendMessage(vHandle, WM_COMMAND, IDM_ADDFAVORITES, (int)vHandle);

            if (_editUrlForm == null)
            {
                _editUrlForm = new EditUrlForm(_favoritesAgent);
                _editUrlForm.Text = "添加收藏";
            }
            UrlFile urlFile = new UrlFile(_currentPage.Url, _currentPage.WebBrowser.DocumentTitle);
            _editUrlForm.UrlFile = urlFile;
            
            if (_editUrlForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    urlFile.ToFile();
                    _editUrlForm.FavoritesDir.UrlFileList.Add(urlFile);
                    string s = Path.GetDirectoryName(urlFile.FullName);
                    if (s == System.Environment.GetFolderPath(Environment.SpecialFolder.Favorites))
                    {
                        //_favoritesAgent.FavoritesDir.UrlFileList.Add(urlFile);
                        _favoritesMenu.DropDownItems.Add(this.CreateToolStripMenuItem(urlFile));
                        _favoritesStrip.Items.Add(this.CreateToolStripButton(urlFile));
                        if (_favoritesForm != null)
                        {
                            TreeNode node = _favoritesForm.CreateTreeNode(_editUrlForm.UrlFile.FileName, "page");
                            node.Tag = urlFile;
                            _favoritesForm.TreeView.Nodes.Add(node);
                        }
                    }
                    else
                    {
                        FavoritesDir fdir = _editUrlForm.FavoritesDir;
                        if (_favoritesForm != null)
                        {
                            TreeNode node = _favoritesForm.CreateTreeNode(_editUrlForm.UrlFile.FileName, "page");
                            node.Tag = urlFile;
                            TreeNode n = _favoritesForm.NodeDic[s];
                            n.Nodes.Add(node);
                        }
                        foreach (ToolStripItem tsi in fdir.ToolStripItems)
                        {
                            ToolStripDropDownItem tsddi = tsi as ToolStripDropDownItem;
                            if (tsddi != null)
                                tsddi.DropDownItems.Add(this.CreateToolStripMenuItem(urlFile));
                        }
                    }
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public void OrganizeFavorites()
        {
            if (_favoritesForm == null)
            {
                _favoritesForm = new FavoritesForm(this);
                _favoritesForm.FavoritesAgent = _favoritesAgent;
                _uiService.ShowToolWin(_favoritesForm, DockStyle.Left);
            }
            else
                _favoritesForm.Show();           

        }

        public void OpenFavoritesPath()
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe ", System.Environment.GetFolderPath(Environment.SpecialFolder.Favorites));
            }
            catch
            {
            }
        }

        [DllImport("User32.DLL")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam,
            int lParam);
        [DllImport("User32.DLL")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent,
            IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        //private int IDM_ADDFAVORITES = 2261;
        private int IDM_FIND = 67;
        private int IDM_VIEWSOURCE = 2139;
        private uint WM_COMMAND = 0x0111;

        public void ViewSource()
        {
            IntPtr vHandle = _currentPage.WebBrowser.Handle;
            vHandle = FindWindowEx(vHandle, IntPtr.Zero, "Shell Embedding", null);
            vHandle = FindWindowEx(vHandle, IntPtr.Zero, "Shell DocObject View", null);
            vHandle = FindWindowEx(vHandle, IntPtr.Zero, "Internet Explorer_Server", null);
            SendMessage(vHandle, WM_COMMAND, IDM_VIEWSOURCE, (int)vHandle);
        }

        public void ViewHistory()
        {
            _annal = false;
            this.Go(Application.StartupPath + @"\AddIns\MyIE\HistoryUrlList.html");
            _annal = true;
        }

        public void DeleteHistory()
        {
            _historyUrlList.Clear();
            try
            {
                IntPtr vHandle;
                IEP.INTERNET_CACHE_ENTRY_INFO vInternetCacheEntryInfo = new IEP.INTERNET_CACHE_ENTRY_INFO();
                int vFirstCacheEntryInfoBufferSize = 0;

                IEP.FindFirstUrlCacheEntryEx(null, 0, (int)IEP.CacheEntry.URLHISTORY_CACHE_ENTRY, 0, (IntPtr)null,
                    ref vFirstCacheEntryInfoBufferSize, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

                IntPtr vBuffer = Marshal.AllocHGlobal((int)vFirstCacheEntryInfoBufferSize);
                vHandle = IEP.FindFirstUrlCacheEntryEx(null, 0, (int)IEP.CacheEntry.URLHISTORY_CACHE_ENTRY, 0,
                   vBuffer, ref vFirstCacheEntryInfoBufferSize,
                   IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

                while (vHandle != IntPtr.Zero)
                {
                    Marshal.PtrToStructure(vBuffer, vInternetCacheEntryInfo);

                    IEP.DeleteUrlCacheEntry(vInternetCacheEntryInfo.lpszLocalFileName);

                    Marshal.FreeCoTaskMem(vBuffer);
                    IEP.FindNextUrlCacheEntryEx(vHandle, (IntPtr)null, ref vFirstCacheEntryInfoBufferSize,
                        IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

                    vBuffer = Marshal.AllocHGlobal((int)vFirstCacheEntryInfoBufferSize);

                    if (!IEP.FindNextUrlCacheEntryEx(vHandle, vBuffer, ref vFirstCacheEntryInfoBufferSize,
                        IntPtr.Zero, IntPtr.Zero, IntPtr.Zero))
                        break;
                }
                Marshal.FreeCoTaskMem(vBuffer);

                MessageBox.Show("成功清理浏览历史记录！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch { }
        }

        public void ViewHelp()
        {
            _annal = false;
            this.Go(Application.StartupPath + @"\AddIns\MyIE\Help\help.mht");
            _annal = true;
        }

        SettingForm _settingForm = null;
        public override void Config()
        {
            if (_settingForm == null)
                _settingForm = new SettingForm();
            _settingForm.ShowDialog(_uiService.MainForm);
        }


        public void SetDefault()
        {
            SetDefaultBrowser(Application.ExecutablePath);
        }

        internal class Script
        {
            public string language;
            public string code;

            public void FormFile(string fileName)
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string s = sr.ReadToEnd();
                    int i1 = s.IndexOf("language=\"");
                    int i2 = s.IndexOf("\">");
                    int i3 = s.LastIndexOf("</");

                    language = s.Substring(i1+10, i2 - i1 - 10);
                    code = s.Substring(i2 + 2, i3 - i2 - 2);
                    //System.Xml.XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(s);
                    //XmlElement elem = doc.ChildNodes[1] as XmlElement;
                    //language = elem.GetAttribute("language");
                    //code = elem.InnerText;
                }
            }
        }

        private Dictionary<string, Script> _scriptDic = new Dictionary<string, Script>(5);

        public void ExecScript(string fileName)
        {
            try
            {
                Script script;
                try
                {
                    script = _scriptDic[fileName];
                }
                catch
                {
                    if (string.IsNullOrEmpty(fileName))
                    {
                        MessageBox.Show("请配置正确的脚本信息！");
                        return;
                    }
                    script = new Script();
                    script.FormFile(fileName);
                    _scriptDic.Add(fileName, script);
                }
                IHTMLDocument2 doc = _currentPage.WebBrowser.Document.DomDocument as IHTMLDocument2;
                IHTMLWindow2 win = doc.parentWindow;
                win.execScript(script.code, script.language);
            }
            catch { }
        }

        internal string CutString(string s, int byteCount)
        {
            if (Encoding.Default.GetByteCount(s) < byteCount)
            {
                return s;
            }
            else
            {
                byte[] bytes = Encoding.Default.GetBytes(s);
                string subStr = Encoding.Default.GetString(bytes, 0, byteCount - 3);
                char ch = subStr[subStr.Length - 1];
                if (subStr[subStr.Length - 1] != s[subStr.Length - 1])
                {
                    return subStr.Substring(0, subStr.Length - 1) + "...";
                }
                else
                {
                    return subStr + "...";
                }
            }
        }



        /// <summary>
        /// 设置自定义浏览器为默认浏览器
        /// </summary>
        /// <param name="browserExePath"></param>
        /// <returns></returns>
        internal static bool SetDefaultBrowser(string browserExePath)
        {
            string mainKey = @"http\shell\open\command";
            string nameKey = @"http\shell\open\ddeexec\Application";
            bool result = false;

            try
            {
                string value = string.Format("\"{0}\" \"%1\"", browserExePath);
                RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(mainKey, true);
                regKey.SetValue("", value);
                regKey.Close();

                FileInfo fileInfo = new FileInfo(browserExePath);
                string fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
                regKey = Registry.ClassesRoot.OpenSubKey(nameKey, true);
                regKey.SetValue("", fileName);
                regKey.Close();

                result = true;
            }
            catch (Exception ex)
            {
                MyIELogger.Error("设置默认浏览器出错！", ex);
            }

            return result;
        }

        /// <summary>
        /// 恢复为默认浏览器
        /// </summary>
        /// <returns></returns>
        internal static bool ResetDefaultBrowser()
        {
            string mainKey = @"http\shell\open\command";
            string nameKey = @"http\shell\open\ddeexec\Application";
            string IEPath = @"C:\Program Files\Internet Explorer\iexplore.exe";
            bool result = false;

            try
            {
                string value = string.Format("\"{0}\" -- \"%1\"", IEPath);
                RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(mainKey, true);
                regKey.SetValue("", value);
                regKey.Close();

                regKey = Registry.ClassesRoot.OpenSubKey(nameKey, true);
                regKey.SetValue("", "IExplore");
                regKey.Close();

                result = true;
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// 保存默认浏览器设置
        /// </summary>
        /// <returns></returns>
        internal static bool SaveDefaultBrowserSetting()
        {
            string mainKey = @"http\shell\open\command";
            string nameKey = @"http\shell\open\ddeexec\Application";
            bool result = false;

            try
            {
                RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(mainKey, true);
                string path = regKey.GetValue("").ToString();
                regKey.Close();

                regKey = Registry.ClassesRoot.OpenSubKey(nameKey, true);
                string name  = regKey.GetValue("").ToString();
                regKey.Close();

                result = true;
            }
            catch
            {
            }

            return result;
        }
    }
}

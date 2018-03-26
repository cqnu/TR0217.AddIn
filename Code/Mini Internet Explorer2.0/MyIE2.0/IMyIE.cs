using System;
using System.Windows.Forms;
namespace MyIE
{
    interface IMyIE
    {
        //打开一个页面
        void Go(string url);
        //打开主页
        void GoHome();
        //在当前页面中打开一个网址
        void GoInCurrentPage(string url);
        //弹出对话框打开本地的网址链接活着网页文件
        void OpenLoacal();
        //在当前页面中进行后退操作，如未做特殊说明后面的操作都是针对当前页面进行的
        void GoBack();
        //前进
        void GoForward();
        //刷新
        void Refresh();
        //停止
        void Stop();
        //弹出对话框在页面中查找关键字
        void Find();
        //设置页面字体的尺寸
        void Zoom(int size);

        //关闭当前页面
        void CloseCurrentPage();
        //关闭所有非当前页面
        void CloseAllOtherPages();
        //关闭所有页面
        void CloseAllPages();
        //恢复最近关闭的页面
        void Restore();

        //弹出Internet选项对话框
        void InternetOption();
        //察看当前页面的属性
        void PageProperties();

        //将当前浏览的网址添加到收藏夹
        void AddFavorite();
        //打开整理收藏夹工具窗口
        void OrganizeFavorites();
        //打开收藏夹目录
        void OpenFavoritesPath();

        //在新标签中打开当前页面的网址
        void CopyCurrentPage();
        //察看当前页面的源代码
        void ViewSource();     

        //打印需要的页面设置
        void PageSetup(); 
        //打印预览
        void PrintPreview();
        //打印页面
        void Print();

        //弹出对话框保存当前页面
        void SaveCurrentPage();
        //保存当前页面为图片
        void SaveCurrentPageAsIamge();

        //使用默认搜索引擎搜索关键字
        void Search(string keyword);
        //设置默认搜索引擎并搜索关键字，url为搜索引擎的调用网址，name为搜索引擎的名称
        void SetEngineSearch(string key, string url, string name);
        //设置默认搜索引擎
        void SetSearchEngine(string url, string name);

        //查看历史记录
        void ViewHistory();
        //删除历史记录
        void DeleteHistory();

        //在当前页面中执行一段脚本，filename为脚本的全路径。
        //解除右键菜单，清除页面上的飞行广告，
        //让页面变成某种适合阅读的颜色等功能都可以通过在当前页面上执行一段脚本来完成
        void ExecScript(string fileName); 

        //弹出关于对话框
        void About();

        //查看帮助文档
        void ViewHelp();

        //设置为默认浏览器
        void SetDefault();

        //注入收藏夹菜单栏
        void InjectFavoritesMenu(ToolStripMenuItem tsmi);

        //注入收藏夹工具条
        void InjectFavoritesStrip(ToolStrip ts);

        //注入页面窗口标题的上下文菜单
        void InjectCaptionContextMenuStrip(ContextMenuStrip cms);

        //获取当前页面窗体，当然这不是作为用户功能提供的，
        //而是提供来开发扩展此浏览器的插件用的，通过PageForm可以获得WebBrowser控件
        PageForm CurrentPage { get; }

        //用于页面关闭后维护界面逻辑的事件
        event AddIn.Core.UpdateUiElemHandler UpdateClose;
        //用于页面下载完成后更新界面逻辑
        event AddIn.Core.UpdateUiElemHandler UpdateComplete;
        //用于确认是否可以进行后退操作，以更新完成后退功能的界面元素的状态
        event AddIn.Core.UpdateUiElemHandler UpdateGoBack;
        //用于确认是否可以进行前进操作，以更新完成前进功能的界面元素的状态
        event AddIn.Core.UpdateUiElemHandler UpdateGoForward;
        //用于更新页面的下载进度
        event AddIn.Core.UpdateUiElemHandler UpdateProgress;
        //用于确认是否可以进行恢复最近关闭的页面操作
        event AddIn.Core.UpdateUiElemHandler UpdateRestore;
        //用于默认搜索引擎发生变化时，更新用于指明默认搜索引擎的界面元素
        event AddIn.Core.UpdateUiElemHandler UpdateSearchEngine;
        //用于更新网址输入栏中显示的当前页面的网址
        event AddIn.Core.UpdateUiElemHandler UpdateSite;
        //用于维护指示当前页面的状态的界面元素
        event AddIn.Core.UpdateUiElemHandler UpdateStatus;
    }
}

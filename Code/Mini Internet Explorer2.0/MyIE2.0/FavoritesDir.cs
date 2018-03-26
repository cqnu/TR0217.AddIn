using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace MyIE
{
    internal class UrlFile
    {

        string _fullName;
        string _site;
        List<ToolStripItem> _toolStripItems = new List<ToolStripItem>(2);

        public List<ToolStripItem> ToolStripItems
        {
            get { return _toolStripItems; }
        }


        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }


        public string FileName
        {
            get { return Path.GetFileNameWithoutExtension(_fullName); }
            set 
            {
                string fileName = value;
                if (!fileName.EndsWith(".url", true, null))
                    fileName += ".url";
                _fullName = Path.GetDirectoryName(_fullName) + Path.DirectorySeparatorChar + fileName;
            }
        }


        public string Site
        {
            get { return _site; }
            set { _site = value; }
        }

        public UrlFile()
        {
            _site = "about:blank";
            _fullName = System.Environment.GetFolderPath(Environment.SpecialFolder.Favorites)
                + Path.DirectorySeparatorChar
                + FavoritesAgent.GetAcceptableFileName(System.Environment.GetFolderPath(Environment.SpecialFolder.Favorites), "新建收藏")
                + ".url";
        }

        public UrlFile(string site, string name)
        {
            char[] chars = Path.GetInvalidFileNameChars();
            string name1 = name;
            if (name1.Length > 70)
                name1 = name.Substring(0, 70);
            char[] fileName =  name1.ToCharArray();
            string s = new string(chars);
            for (int i = 0; i < fileName.Length; i++)
            {
                if(s.Contains(fileName[i].ToString()))
                    fileName[i] = ' ';
            }
            _site = site;
            _fullName = System.Environment.GetFolderPath(Environment.SpecialFolder.Favorites)
                + Path.DirectorySeparatorChar
                + FavoritesAgent.GetAcceptableFileName(System.Environment.GetFolderPath(Environment.SpecialFolder.Favorites), new string(fileName));
            if (_fullName.Length > 256)
                _fullName = _fullName.Substring(0, 256) + ".url";
        }

        public void FromFile(string fullName)
        {
            _fullName = fullName;
            string content = Encoding.Default.GetString(ReadFile(fullName));
            int start = content.IndexOf("URL=");
            if (start >= 0)
            {
                string url = string.Empty;

                start += 4;
                int end = content.IndexOfAny(new char[] { '\r', '\n' }, start);
                if (end >= start)
                {
                    url = content.Substring(start, end - start);
                }
                else
                {
                    url = content.Substring(start);
                }

                _site = url;
            }
        }

        public void ToFile(string fullName)
        {
            string content = "[InternetShortcut]" + System.Environment.NewLine
                       + "URL=" + _site;
            using (StreamWriter writer = new StreamWriter(fullName, false))
            {
                writer.Write(content);
                writer.Close();
            }
        }

        public void ToFile()
        {
            this.ToFile(_fullName);
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <return>字节数组</return>
        public static byte[] ReadFile(string fullName)
        {
            FileStream s = new FileStream(fullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            try
            {
                int n = (int)s.Length; // 文件长度
                byte[] content = new byte[n];
                s.Read(content, 0, n);
                return content;
            }
            finally
            {
                s.Close();
            }
        }
    }


    internal class FavoritesDir
    {
        string _path = System.Environment.GetFolderPath(Environment.SpecialFolder.Favorites);
        private List<UrlFile> _urlFileList = new List<UrlFile>();
        private List<FavoritesDir> _favoritesDirList = new List<FavoritesDir>();
        List<ToolStripItem> _toolStripItems = new List<ToolStripItem>(2);

        public List<ToolStripItem> ToolStripItems
        {
            get { return _toolStripItems; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        internal List<UrlFile> UrlFileList
        {
            get { return _urlFileList; }
        }
        
        internal List<FavoritesDir> FavoritesDirList
        {
            get { return _favoritesDirList; }
        }
    }

    internal class FavoritesEventArgs : EventArgs
    {
        //当前收藏夹目录相当于根收藏夹目录的层次，收藏夹根目录的直接下层为1
        int _level;

        FavoritesDir _favoritesDir;//不为null表示当前处理的是文件夹

        UrlFile _urlFile;

        internal FavoritesEventArgs(int level, FavoritesDir dir, UrlFile file)
        {
            _level = level;
            _favoritesDir = dir;
            _urlFile = file;
        }

        internal UrlFile UrlFile
        {
            get { return _urlFile; }
        }

        internal FavoritesDir FavoritesDir
        {
            get { return _favoritesDir; }
        }

        internal int Level
        {
            get { return _level; }
        }
    }

    internal delegate void ProcessFavoritesHandler(object sender, FavoritesEventArgs e);

    internal class FavoritesAgent
    {
        internal static event ProcessFavoritesHandler OnAddFavoritesItem;
        FavoritesDir _favoritesDir = new FavoritesDir();

        public static string GetAcceptableFileName(string path, string name)
        {
            int i = 0;
            string pathEx = path + Path.DirectorySeparatorChar;
            string destName = name;
            do
            {
                i++;
                destName = name + i.ToString();
            }
            while (Directory.Exists(pathEx + destName));

            return destName;
        }

        internal FavoritesDir FavoritesDir
        {
            get { return _favoritesDir; }
        }

        internal FavoritesAgent()
        {
            _favoritesDir.Path = System.Environment.GetFolderPath(Environment.SpecialFolder.Favorites);
        }

        int _level = 0;
        public void ProcessFavoritesDir(FavoritesDir favoritesDir)
        {
            if (favoritesDir == null)
            {
                favoritesDir = _favoritesDir;
            }

            _level++;

            foreach (string dir in Directory.GetDirectories(favoritesDir.Path))
            {
                FavoritesDir fDir = new FavoritesDir();
                fDir.Path = dir;
                favoritesDir.FavoritesDirList.Add(fDir);
                if (FavoritesAgent.OnAddFavoritesItem != null)
                {
                    FavoritesEventArgs arg = new FavoritesEventArgs(_level, fDir, null);
                    FavoritesAgent.OnAddFavoritesItem(this, arg);
                }
                this.ProcessFavoritesDir(fDir);
            }

            foreach (string file in Directory.GetFiles(favoritesDir.Path))
            {
                if (file.EndsWith(".url", true, null))
                {
                    UrlFile urlFile = new UrlFile();
                    urlFile.FromFile(file);
                    favoritesDir.UrlFileList.Add(urlFile);
                    if (FavoritesAgent.OnAddFavoritesItem != null)
                    {
                        FavoritesEventArgs arg = new FavoritesEventArgs(_level, null, urlFile);
                        FavoritesAgent.OnAddFavoritesItem(this, arg);
                    }
                }
            }
            _level--;
        }
    }
}

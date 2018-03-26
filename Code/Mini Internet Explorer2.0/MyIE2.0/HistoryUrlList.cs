using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace MyIE
{
    internal class HistoryUrlList
    {
        System.Xml.XmlDocument _historyUrlDoc;
        string  _docPath;
        int _historyLength = 7;
        XmlElement _liDateElem;
        XmlElement _ulElem;
        string _today;

        private bool _dirty = false;

        public int HistoryLength
        {
            get { return _historyLength; }
            set { _historyLength = value; }
        }

        public HistoryUrlList(string path)
        {
            _docPath = path;
            _today = DateTime.Today.ToShortDateString();

            try
            {
                _historyUrlDoc = new System.Xml.XmlDocument();
                _historyUrlDoc.Load(path);
                XmlElement tlUlElem = (XmlElement)_historyUrlDoc.DocumentElement.ChildNodes[1];
                if (tlUlElem.ChildNodes.Count >= 2)
                {

                    _liDateElem = (XmlElement)tlUlElem.ChildNodes[0];
                    _ulElem = (XmlElement)tlUlElem.ChildNodes[1];
                }
                this.InitialAppend();                
            }
            catch
            {
                this.InitialHistorFile();
            } 
        }

        //
        private void InitialAppend()
        {
            XmlElement tlUlElem = (XmlElement)_historyUrlDoc.DocumentElement.ChildNodes[1];

            if (_liDateElem == null || _liDateElem.InnerText != _today)
            {
                _dirty = true;
                _liDateElem = _historyUrlDoc.CreateElement("LI");
                _liDateElem.InnerText = DateTime.Today.ToShortDateString();
                _ulElem = _historyUrlDoc.CreateElement("UL");
                _ulElem.InnerText = "   ";
                tlUlElem.InsertBefore(_ulElem, tlUlElem.ChildNodes[0]);
                tlUlElem.InsertBefore(_liDateElem, _ulElem);

            }
            if (tlUlElem.ChildNodes.Count > _historyLength * 2)
            {
                _dirty = true;
                for (int i = _historyLength * 2; i < tlUlElem.ChildNodes.Count; i++)
                    tlUlElem.RemoveChild(tlUlElem.ChildNodes[tlUlElem.ChildNodes.Count - 1]);
            }
        }


        private void InitialHistorFile()
        {
            _dirty = true;
            _historyUrlDoc = new System.Xml.XmlDocument();
            XmlDeclaration xmldec = _historyUrlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement htmlElem = _historyUrlDoc.CreateElement("HTML");

            XmlElement headElem = _historyUrlDoc.CreateElement("HEAD");
            XmlElement titleElem = _historyUrlDoc.CreateElement("TITLE");
            titleElem.InnerText = "浏览历史";
            headElem.AppendChild(titleElem);

            XmlElement tlUlElem = _historyUrlDoc.CreateElement("UL");
            _liDateElem = _historyUrlDoc.CreateElement("LI");
            _liDateElem.InnerText = DateTime.Today.ToShortDateString();
            _ulElem = _historyUrlDoc.CreateElement("UL");
            _ulElem.InnerText = "   ";

            tlUlElem.AppendChild(_liDateElem);
            tlUlElem.AppendChild(_ulElem);
 
            htmlElem.AppendChild(headElem);
            htmlElem.AppendChild(tlUlElem);

            _historyUrlDoc.AppendChild(xmldec);
            _historyUrlDoc.AppendChild(htmlElem);

            this.Save();
        }

        public void Append(string url, string title)
        {
            _dirty = true;
            this.InitialAppend();
            XmlElement historyLiElem = _historyUrlDoc.CreateElement("LI");
            XmlElement linkElem = _historyUrlDoc.CreateElement("A");

            linkElem.InnerText = title;
            linkElem.SetAttribute("HREF", url);
            linkElem.SetAttribute("TARGET", "_blank");
            historyLiElem.AppendChild(linkElem);
            lock (_historyUrlDoc)
            {
                _ulElem.AppendChild(historyLiElem);
            }
        }

        public void Save()
        {
            if (_dirty)
            {
                _dirty = false;
                lock (_historyUrlDoc)
                {
                    _historyUrlDoc.Save(_docPath);
                }
            }
        }

        public void Clear()
        {
            this.InitialHistorFile();
        }
    }
}

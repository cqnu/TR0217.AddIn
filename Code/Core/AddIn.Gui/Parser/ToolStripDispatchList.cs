using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace AddIn.Gui.Parser
{
    class ToolStripDispatchList : IList
    {
        MyToolStripContainer _toolStripContainer;

        public ToolStripDispatchList(MyToolStripContainer tsc)
        {
            _toolStripContainer = tsc;
        }
        #region IList 成员

        public int Add(object value)
        {
            ToolStripWrapper ts = value as ToolStripWrapper;            
            if (ts != null)
            {
                ToolStripPanel tsp = null;
                switch (ts.Location)
                {
                    case ToolStripLocation.Left:
                        tsp = _toolStripContainer.LeftToolStripPanel;
                        break;
                    case ToolStripLocation.Right:
                        tsp = _toolStripContainer.RightToolStripPanel;
                        break;
                    case ToolStripLocation.Bottom:
                        tsp = _toolStripContainer.BottomToolStripPanel;
                        break;
                    default:
                        tsp = _toolStripContainer.TopToolStripPanel;
                        break;
                }
                if (ts.Joined)
                {
                    if (tsp.Controls.Count == 0)
                        tsp.Join(ts.ToolStrip, tsp.Rows.Length);
                    else
                    {
                        ToolStripPanelRow row = tsp.Rows[tsp.Rows.Length - 1];
                        ToolStrip toolStrip = row.Controls[row.Controls.Length - 1] as ToolStrip;
                        tsp.Join(ts.ToolStrip,
                            toolStrip.Bounds.Right,
                            toolStrip.Bounds.Top);
                    }
                }
                else
                {
                    tsp.Join(ts.ToolStrip, tsp.Rows.Length);
                }
            }

            return 0;
        }

        public void Clear()
        {
            _toolStripContainer.LeftToolStripPanel.Controls.Clear();
            _toolStripContainer.RightToolStripPanel.Controls.Clear();
            _toolStripContainer.BottomToolStripPanel.Controls.Clear();
            _toolStripContainer.TopToolStripPanel.Controls.Clear();
        }

        public bool Contains(object value)
        {
            ToolStripWrapper ts = value as ToolStripWrapper;
            if (ts != null)
            {
                return _toolStripContainer.LeftToolStripPanel.Contains(ts.ToolStrip)
                    || _toolStripContainer.RightToolStripPanel.Contains(ts.ToolStrip)
                    || _toolStripContainer.BottomToolStripPanel.Contains(ts.ToolStrip)
                    || _toolStripContainer.TopToolStripPanel.Contains(ts.ToolStrip);
            }

            return false;
        }

        public int IndexOf(object value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Insert(int index, object value)
        {
            this.Add(value);
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Remove(object value)
        {
            ToolStripWrapper ts = value as ToolStripWrapper;
            if (ts != null)
            {
                if (_toolStripContainer.LeftToolStripPanel.Contains(ts.ToolStrip))
                    _toolStripContainer.LeftToolStripPanel.Controls.Remove(ts.ToolStrip);
                else if (_toolStripContainer.RightToolStripPanel.Contains(ts.ToolStrip))
                    _toolStripContainer.RightToolStripPanel.Controls.Remove(ts.ToolStrip);
                else if (_toolStripContainer.BottomToolStripPanel.Contains(ts.ToolStrip))
                    _toolStripContainer.BottomToolStripPanel.Controls.Remove(ts.ToolStrip);
                else if (_toolStripContainer.TopToolStripPanel.Contains(ts.ToolStrip))
                    _toolStripContainer.TopToolStripPanel.Controls.Remove(ts.ToolStrip);
            }
        }

        public void RemoveAt(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object this[int index]
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion

        #region ICollection 成员

        public void CopyTo(Array array, int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Count
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool IsSynchronized
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public object SyncRoot
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IEnumerable 成员

        public IEnumerator GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}

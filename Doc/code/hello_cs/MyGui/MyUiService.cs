using System;
using System.Collections.Generic;
using System.Text;
using AddIn.Core;

namespace MyGui
{
    public class MyUiService : ServiceBase,IUiService
    {
        Form1 _mainForm;
        #region IUiService 成员

        public override void Config()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Execute(string exe, string parameter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Exexute(string exe)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Exit()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public System.Windows.Forms.ContextMenuStrip GetContextMenuStrip(string str, bool byform)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public System.Windows.Forms.StatusStrip GetStatusStrip(string str)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public System.Windows.Forms.ToolStrip GetToolStrip(string str, bool byname)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public System.Windows.Forms.ToolStripItem GetToolStripItem(string path)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public System.Windows.Forms.Form LoadMainForm()
        {
            return _mainForm = new Form1();
        }

        public System.Windows.Forms.Form MainForm
        {
            get { return _mainForm; }
        }

        public void ModifyAddIns()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetStatusStripVisible(bool visible, string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetToolStripVisible(bool visible, string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ShowDocForm(System.Windows.Forms.Form docForm)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ShowToolWin(System.Windows.Forms.Form toolWin, System.Windows.Forms.DockStyle dockStyle)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}

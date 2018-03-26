using System;
using System.Collections.Generic;
using System.Text;
using AddIn.Core;
using System.Windows.Forms;

namespace AddIn.UiInterface
{
    public interface IUiExService : IUiService
    {
        ContextMenuStrip GetContextMenuStrip(string name);
        ToolStrip GetToolStrip(string name);
        ToolStripItem GetToolStripItem(string path);
        void SetStatusStripVisible(bool visible);
        void SetToolStripVisible(bool visible, string name);
        void ShowDocForm(Form docForm);
        void ShowToolWin(Form toolWin, DockStyle dockStyle);
        void ShowInfoMsg(string msg);
        void ShowInfoMsg(string msg, string caption);
        void ShowWarningMsg(string msg);
        void ShowWarningMsg(string msg, string caption);
        void ShowErrorMsg(string msg);
        void ShowErrorMsg(string msg, string caption);
        DialogResult PromptYesNo(string msg);
        DialogResult PromptYesNo(string msg, string caption);
        DialogResult PromptYesNoCancel(string msg);
        DialogResult PromptYesNoCancel(string msg, string caption);
        DialogResult PromptOKCancel(string msg);
        DialogResult PromptOKCancel(string msg, string caption);
        DialogResult PromptRetryCancel(string msg);
        DialogResult PromptRetryCancel(string msg, string caption);
        DialogResult PromptAbortRetryIgnore(string msg);
        DialogResult PromptAbortRetryIgnore(string msg, string caption);
    }
}

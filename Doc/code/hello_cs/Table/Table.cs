using System;
using System.Collections.Generic;
using System.Text;
using AddIn.Core;
using System.Windows.Forms;

namespace Table
{
    public class Table : ServiceBase
    {
        private IUiService _uiService;
        private TableForm _tableForm = null;

        public Table()
        {
            AppFrame.AfterLoadMainForm += new LoadMainFormHandler(AppFrame_AfterLoadMainForm);
        }

        void AppFrame_AfterLoadMainForm(LoadMainFormEventArgs e)
        {
            _uiService = e.UiService;
        }

        public void ShowTable()
        {
            if (_tableForm == null)
            {
                _tableForm = new TableForm();
            }
            _uiService.ShowToolWin(_tableForm, DockStyle.Bottom);
        }

    }
}

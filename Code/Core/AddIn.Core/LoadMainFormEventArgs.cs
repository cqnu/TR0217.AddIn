using System;
using System.Text;
using System.Windows.Forms;

namespace AddIn.Core
{
    public class LoadMainFormEventArgs:EventArgs
    {
        private Form _mainForm;
        private IUiService _uiService;
        private string[] _args;

        public string[] Args
        {
            get { return _args; }
        }

        public IUiService UiService
        {
            get { return _uiService; }
        }

        public Form MainForm
        {
            get { return _mainForm; }
        }

        public LoadMainFormEventArgs(Form form, IUiService ui, string[] args)
        {
            _mainForm = form;
            _uiService = ui;
            _args = args;
        }

    }
}

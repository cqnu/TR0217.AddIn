using System;
using System.Collections.Generic;
using System.Text;

namespace AddIn.Core
{
    public abstract class ServiceBase
    {
        private IUiService _uiService = null;
        internal AddInParser _addInParser = null;

        public ServiceBase()
        {
            _uiService = AppFrame.ServiceCollection.GetService<IUiService>();
        }

        public IUiService UiService
        {
            get { return _uiService; }
        }

        public virtual void Config()
        {
            return;
        }

        public virtual void About()
        {
            try
            {
                AboutForm frm = new AboutForm();
                frm.Text = this._addInParser.Name;
                frm.Author = this._addInParser.Author;
                frm.Version = this._addInParser.Version;
                frm.Url = this._addInParser.Url;
                frm.Copyright = this._addInParser.Copyright;
                frm.Description = this._addInParser.Description;
                frm.ShowDialog();
            }
            catch { }
        }
    }
}

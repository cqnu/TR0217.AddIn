using System;
using System.Windows.Forms;

namespace AddIn.Core
{
    public interface IUiService
    {
        void InitialUiServiceInfo(AddInParser ap);
        void Execute(string exe,string parameter);
        void Exexute(string exe);
        void Exit();
        void Config();
        Form LoadMainForm();
        Form MainForm { get; }
        void ModifyAddIns();
    }
}

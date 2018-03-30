using System;
using System.Windows.Forms;

namespace AddIn.Core
{
    public interface IUiService
    {
        void InitialUiServiceInfo(AddInParser ap);
        [Function]
        void Execute(string exe,string parameter);
        [Function]
        void Exexute(string exe);
        [Function]
        void Exit();
        [Function]
        void Config();
        Form LoadMainForm();
        Form MainForm { get; }
        bool IsMainFormLoaded { get; }
        void ModifyAddIns();
    }
}

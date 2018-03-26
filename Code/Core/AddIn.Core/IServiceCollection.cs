using System;
namespace AddIn.Core
{
    public interface IServiceCollection
    {
        System.Collections.Generic.List<AddInParser> AddInParserList { get; }
        System.Collections.Generic.List<AddInParser> BaseServiceParserList { get; }
        event LoadAddInHandler AfterLoadOneAddIn;
        event LoadAddInHandler BeforLoadeOneAddIn;
        ServiceBase GetService(string name);
        T GetService<T>() where T : class;
        void SaveAddInConfig(string configPath);
        void SaveBaseServiceConfig(string bsConfigPath);
        void SaveAddInConfig();
        void SaveBaseServiceConfig();
        System.Collections.Generic.Dictionary<string, ServiceBase> Services { get; }
    }
}

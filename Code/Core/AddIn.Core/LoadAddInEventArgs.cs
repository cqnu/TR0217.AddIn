using System;
using System.Text;

namespace AddIn.Core
{
    public class LoadAddInEventArgs:EventArgs
    {
        private AddInParser _addInParser;

        private IServiceCollection _serviceCollection;

        public IServiceCollection ServiceCollection
        {
            get { return _serviceCollection; }
        }

        public AddInParser AddInParser
        {
            get { return _addInParser; }
        }

        public LoadAddInEventArgs(AddInParser addInParser, IServiceCollection sc)
        {
            _addInParser = addInParser;
            _serviceCollection = sc;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace AddIn.Gui
{
    internal class FunctionConverter : MethodConverter
    {
        public override TypeConverter.StandardValuesCollection
        GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new List<string>(AddInModifyForm._functionList));
        }
    }
}

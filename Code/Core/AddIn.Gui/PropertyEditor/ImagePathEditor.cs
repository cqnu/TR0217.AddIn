using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Reflection;

namespace AddIn.Gui
{
    internal class ImagePathEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            Type type = context.Instance.GetType();
            PropertyInfo properInfo = type.GetProperty(context.PropertyDescriptor.Name,BindingFlags.Instance|BindingFlags.GetProperty|BindingFlags.Public);
            string path = properInfo.GetValue(context.Instance, null).ToString();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            if(string.IsNullOrEmpty(path))
                ofd.InitialDirectory = Application.StartupPath;
            else
                ofd.InitialDirectory = path;

            ofd.FileName = "*.png";
            ofd.Filter = "PNG file(*.png)|*.png|Icon file(*.ico)|*.ico|All files(*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                path = ofd.FileName;
                if (path.Contains(Application.StartupPath))
                {
                    path = path.Replace(Application.StartupPath, ".");
                }
            }

            return path;
        }
    }
}

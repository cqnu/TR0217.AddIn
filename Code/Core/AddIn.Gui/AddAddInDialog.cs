using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AddIn.Core;
using System.Reflection;


namespace AddIn.Gui
{
    internal partial class AddAddInDialog : Form
    {
        private AddInParser _addInParser;
        private Assembly assembly = null;

        public bool IsModify
        {

            set 
            {
                txtPath.Enabled = !value;
                btnBrowse.Enabled = !value;
                cboName.Enabled = !value;
                cboIsBaseService.Enabled = !value;
            }
        }


        public AddAddInDialog(AddInParser addInParser)
        {
            InitializeComponent();
            _addInParser = addInParser;
        }

        private void AddAddInDialog_Load(object sender, EventArgs e)
        {
            cboLazyLoad.SelectedIndex = 1;
            if (_addInParser != null)
            {
                if (_addInParser.Path != string.Empty)
                {
                    try
                    {
                        assembly = Assembly.LoadFrom(_addInParser.Path);
                        Type[] types = assembly.GetExportedTypes();
                        cboName.Items.Clear();
                        foreach (Type type in types)
                        {
                            if (type.BaseType == typeof(ServiceBase))
                            {
                                cboName.Items.Add(type.FullName);
                            }
                        }
                        cboName.Text = _addInParser.Name;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                txtAuthor.Text = _addInParser.Author;
                txtVersion.Text = _addInParser.Version;
                txtPath.Text = _addInParser.Path;
                cboName.SelectedValue = _addInParser.Name;
                txtCopyRight.Text = _addInParser.Copyright;
                txtUrl.Text = _addInParser.Url;
                cboLazyLoad.Text = _addInParser.Lazyload.ToString();
                cboIsBaseService.Text = _addInParser.IsBaseService.ToString();
                txtDescription.Text = _addInParser.Description;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog.FileName;
                if (path.Contains(Application.StartupPath))
                {
                    path = path.Replace(Application.StartupPath, ".");
                }
                txtPath.Text = path;
                SetCboName(openFileDialog.FileName);
            }
        }

        private void SetCboName(String path)
        {
            try
            {
                assembly = Assembly.LoadFrom(path);
                Type[] types = assembly.GetExportedTypes();
                cboName.Items.Clear();
                foreach (Type type in types)
                {
                    if (type.BaseType == typeof(ServiceBase))
                    {
                        cboName.Items.Add(type.FullName);
                    }
                }

                if (cboName.Items.Count > 0)
                {
                    cboName.SelectedIndex = 0;
                    _addInParser.Valid = true;
                    _addInParser.Assembly = assembly;
                }
                else
                {
                    _addInParser.Valid = false;
                    _addInParser.Assembly = null;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _addInParser.Valid = false;
                _addInParser.Assembly = assembly = null;
                return;
            }
        }

        private void txtPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetCboName(txtPath.Text);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _addInParser.Author = txtAuthor.Text;
            _addInParser.Version = txtVersion.Text;
            _addInParser.Path = txtPath.Text;
            _addInParser.Name = cboName.Text;
            _addInParser.Lazyload = cboLazyLoad.Text == "true" ? true : false;
            _addInParser.IsBaseService = cboIsBaseService.Text == "true" ? true : false;
            _addInParser.Url = txtUrl.Text;
            _addInParser.Copyright = txtCopyRight.Text;
            _addInParser.Description = txtDescription.Text;
        }
    }
}
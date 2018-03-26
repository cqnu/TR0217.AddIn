using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AddIn.Core;

namespace AddIn.Gui
{
    public partial class ConfigForm : Form
    {
        public ConfigForm(IServiceCollection sc)
        {
            InitializeComponent();
            foreach (KeyValuePair<string, ServiceBase> kvp in sc.Services)
                lstService.Items.Add(kvp.Value);
        }

        private void lstService_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstService.SelectedItem == null)
            {
                btnConfig.Enabled = false;
                btnAbout.Enabled = false;
            }
            else
            {
                btnAbout.Enabled = true;
                btnConfig.Enabled = !(lstService.SelectedItem is IUiService);
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            (lstService.SelectedItem as ServiceBase).Config();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            (lstService.SelectedItem as ServiceBase).About();
        }

        private void lstService_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstService.SelectedItem != null)
                (lstService.SelectedItem as ServiceBase).About();
        }
    }
}
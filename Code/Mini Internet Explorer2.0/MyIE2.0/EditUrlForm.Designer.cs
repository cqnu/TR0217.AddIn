namespace MyIE
{
    partial class EditUrlForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditUrlForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.mtxtUrl = new System.Windows.Forms.MaskedTextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cboiFolder = new TR0217.ControlEx.ComboBoxImage();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "名称:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "网址:";
            // 
            // mtxtUrl
            // 
            this.mtxtUrl.Location = new System.Drawing.Point(70, 56);
            this.mtxtUrl.Name = "mtxtUrl";
            this.mtxtUrl.Size = new System.Drawing.Size(271, 21);
            this.mtxtUrl.TabIndex = 2;
            this.mtxtUrl.TextChanged += new System.EventHandler(this.mtxtUrl_TextChanged);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(70, 24);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(271, 21);
            this.txtName.TabIndex = 3;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(162, 128);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(254, 128);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "文件夹:";
            // 
            // cboiFolder
            // 
            this.cboiFolder.DefaultImage = ((System.Drawing.Image)(resources.GetObject("cboiFolder.DefaultImage")));
            this.cboiFolder.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboiFolder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboiFolder.EmptyTextTip = null;
            this.cboiFolder.FormattingEnabled = true;
            this.cboiFolder.LevelAlign = 10;
            this.cboiFolder.Location = new System.Drawing.Point(70, 87);
            this.cboiFolder.Name = "cboiFolder";
            this.cboiFolder.SeparatorColor = System.Drawing.Color.Black;
            this.cboiFolder.SeparatorMargin = 2;
            this.cboiFolder.SeparatorStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.cboiFolder.SeparatorWidth = 1;
            this.cboiFolder.Size = new System.Drawing.Size(271, 22);
            this.cboiFolder.TabIndex = 6;
            this.cboiFolder.SelectedIndexChanged += new System.EventHandler(this.cboiFolder_SelectedIndexChanged);
            // 
            // EditUrlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 164);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboiFolder);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.mtxtUrl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "EditUrlForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " 编辑收藏";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditUrlForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox mtxtUrl;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private TR0217.ControlEx.ComboBoxImage cboiFolder;
        private System.Windows.Forms.Label label3;
    }
}
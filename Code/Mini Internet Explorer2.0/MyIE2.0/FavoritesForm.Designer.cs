namespace MyIE
{
    partial class FavoritesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FavoritesForm));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tssbNew = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiNewSite = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbCopyUrl = new System.Windows.Forms.ToolStripButton();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiOpenInNewPage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenInCurrentPage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiRename = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCopySite = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRepleaseSite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewFolder2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEidt = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssbNew,
            this.tsbCopyUrl,
            this.tsbEdit,
            this.tsbDelete});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(268, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // tssbNew
            // 
            this.tssbNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNewSite,
            this.tsmiNewFolder});
            this.tssbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbNew.Name = "tssbNew";
            this.tssbNew.Size = new System.Drawing.Size(42, 22);
            this.tssbNew.Text = "新建";
            // 
            // tsmiNewSite
            // 
            this.tsmiNewSite.Image = ((System.Drawing.Image)(resources.GetObject("tsmiNewSite.Image")));
            this.tsmiNewSite.Name = "tsmiNewSite";
            this.tsmiNewSite.Size = new System.Drawing.Size(152, 22);
            this.tsmiNewSite.Text = "网址";
            this.tsmiNewSite.Click += new System.EventHandler(this.tsmiNewSite_Click);
            // 
            // tsmiNewFolder
            // 
            this.tsmiNewFolder.Image = ((System.Drawing.Image)(resources.GetObject("tsmiNewFolder.Image")));
            this.tsmiNewFolder.Name = "tsmiNewFolder";
            this.tsmiNewFolder.Size = new System.Drawing.Size(152, 22);
            this.tsmiNewFolder.Text = "文件夹";
            this.tsmiNewFolder.Click += new System.EventHandler(this.tsmiNewFolder2_Click);
            // 
            // tsbCopyUrl
            // 
            this.tsbCopyUrl.Enabled = false;
            this.tsbCopyUrl.Image = ((System.Drawing.Image)(resources.GetObject("tsbCopyUrl.Image")));
            this.tsbCopyUrl.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCopyUrl.Name = "tsbCopyUrl";
            this.tsbCopyUrl.Size = new System.Drawing.Size(73, 22);
            this.tsbCopyUrl.Text = "复制网址";
            this.tsbCopyUrl.Click += new System.EventHandler(this.tsmiCopySite_Click);
            // 
            // tsbEdit
            // 
            this.tsbEdit.Enabled = false;
            this.tsbEdit.Image = ((System.Drawing.Image)(resources.GetObject("tsbEdit.Image")));
            this.tsbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEdit.Name = "tsbEdit";
            this.tsbEdit.Size = new System.Drawing.Size(49, 22);
            this.tsbEdit.Text = "编辑";
            this.tsbEdit.Click += new System.EventHandler(this.tsmiEidt_Click);
            // 
            // tsbDelete
            // 
            this.tsbDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsbDelete.Image")));
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(49, 22);
            this.tsbDelete.Text = "删除";
            this.tsbDelete.Click += new System.EventHandler(this.tsmiDelete_Click);
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.Location = new System.Drawing.Point(0, 25);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(268, 332);
            this.treeView.TabIndex = 1;
            this.treeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseDoubleClick);
            this.treeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView_AfterLabelEdit);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "page");
            this.imageList.Images.SetKeyName(1, "folder");
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOpenInNewPage,
            this.tsmiOpenInCurrentPage,
            this.toolStripSeparator1,
            this.tsmiRename,
            this.tsmiCopySite,
            this.tsmiRepleaseSite,
            this.toolStripSeparator3,
            this.tsmiOpenFolder,
            this.tsmiNewFolder2,
            this.toolStripSeparator2,
            this.tsmiDelete,
            this.tsmiEidt});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(167, 220);
            // 
            // tsmiOpenInNewPage
            // 
            this.tsmiOpenInNewPage.Name = "tsmiOpenInNewPage";
            this.tsmiOpenInNewPage.Size = new System.Drawing.Size(166, 22);
            this.tsmiOpenInNewPage.Text = "在新标签中打开";
            this.tsmiOpenInNewPage.Click += new System.EventHandler(this.tsmiOpenInNewPage_Click);
            // 
            // tsmiOpenInCurrentPage
            // 
            this.tsmiOpenInCurrentPage.Name = "tsmiOpenInCurrentPage";
            this.tsmiOpenInCurrentPage.Size = new System.Drawing.Size(166, 22);
            this.tsmiOpenInCurrentPage.Text = "在当前标签中打开";
            this.tsmiOpenInCurrentPage.Click += new System.EventHandler(this.tsmiOpenInCurrentPage_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(163, 6);
            // 
            // tsmiRename
            // 
            this.tsmiRename.Name = "tsmiRename";
            this.tsmiRename.Size = new System.Drawing.Size(166, 22);
            this.tsmiRename.Text = "重命名";
            this.tsmiRename.Click += new System.EventHandler(this.tsmiRename_Click);
            // 
            // tsmiCopySite
            // 
            this.tsmiCopySite.Name = "tsmiCopySite";
            this.tsmiCopySite.Size = new System.Drawing.Size(166, 22);
            this.tsmiCopySite.Text = "复制网址";
            this.tsmiCopySite.Click += new System.EventHandler(this.tsmiCopySite_Click);
            // 
            // tsmiRepleaseSite
            // 
            this.tsmiRepleaseSite.Name = "tsmiRepleaseSite";
            this.tsmiRepleaseSite.Size = new System.Drawing.Size(166, 22);
            this.tsmiRepleaseSite.Text = "替换为当前网址";
            this.tsmiRepleaseSite.Click += new System.EventHandler(this.tsmiRepleaseSite_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(163, 6);
            // 
            // tsmiOpenFolder
            // 
            this.tsmiOpenFolder.Name = "tsmiOpenFolder";
            this.tsmiOpenFolder.Size = new System.Drawing.Size(166, 22);
            this.tsmiOpenFolder.Text = "打开文件夹";
            this.tsmiOpenFolder.Click += new System.EventHandler(this.tsmiOpenFolder_Click);
            // 
            // tsmiNewFolder2
            // 
            this.tsmiNewFolder2.Name = "tsmiNewFolder2";
            this.tsmiNewFolder2.Size = new System.Drawing.Size(166, 22);
            this.tsmiNewFolder2.Text = "新建文件夹";
            this.tsmiNewFolder2.Click += new System.EventHandler(this.tsmiNewFolder2_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(163, 6);
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Name = "tsmiDelete";
            this.tsmiDelete.Size = new System.Drawing.Size(166, 22);
            this.tsmiDelete.Text = "删除";
            this.tsmiDelete.Click += new System.EventHandler(this.tsmiDelete_Click);
            // 
            // tsmiEidt
            // 
            this.tsmiEidt.Name = "tsmiEidt";
            this.tsmiEidt.Size = new System.Drawing.Size(166, 22);
            this.tsmiEidt.Text = "编辑";
            this.tsmiEidt.Click += new System.EventHandler(this.tsmiEidt_Click);
            // 
            // FavoritesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 357);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.toolStrip);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)));
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FavoritesForm";
            this.Text = "收藏夹";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.ToolStripDropDownButton tssbNew;
        private System.Windows.Forms.ToolStripButton tsbCopyUrl;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewSite;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewFolder;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenInNewPage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopySite;
        private System.Windows.Forms.ToolStripMenuItem tsmiRepleaseSite;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewFolder2;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenFolder;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenInCurrentPage;
        private System.Windows.Forms.ToolStripMenuItem tsmiRename;
        private System.Windows.Forms.ToolStripButton tsbEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiEidt;
    }
}
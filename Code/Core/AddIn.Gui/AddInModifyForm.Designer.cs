namespace AddIn.Gui
{
    partial class AddInModifyForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddInModifyForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpUi = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.tpRegist = new System.Windows.Forms.TabPage();
            this.btnDeflate = new System.Windows.Forms.Button();
            this.btnModifySelected = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            this.lvAddIn = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAuthor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chCopyRight = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chUrl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chIsBaseService = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLazyLoad = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cmsSingleItem = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCreateToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreateComboBoxItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreateComboBoxImageItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreateContextMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSingle = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiSingleCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSingleCut = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSinglePaste = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSingleDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsCreateCommand = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCreateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreateButton = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreateDropDownButton = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreateSplitButton = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreateComboBox = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreateComboBoxImage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreateTextBox = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreateLabel = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreateStatusLabel = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreateSeparator = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreateProgressBar = new System.Windows.Forms.ToolStripMenuItem();
            this.tssCreate = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiCreateCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreateCut = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreatePaste = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreateDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tabControl1.SuspendLayout();
            this.tpUi.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tpRegist.SuspendLayout();
            this.cmsSingleItem.SuspendLayout();
            this.cmsCreateCommand.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl1.Controls.Add(this.tpUi);
            this.tabControl1.Controls.Add(this.tpRegist);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(723, 349);
            this.tabControl1.TabIndex = 2;
            // 
            // tpUi
            // 
            this.tpUi.Controls.Add(this.splitContainer2);
            this.tpUi.Location = new System.Drawing.Point(4, 25);
            this.tpUi.Name = "tpUi";
            this.tpUi.Padding = new System.Windows.Forms.Padding(3);
            this.tpUi.Size = new System.Drawing.Size(715, 320);
            this.tpUi.TabIndex = 1;
            this.tpUi.Text = "Config UI";
            this.tpUi.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.treeView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.propertyGrid);
            this.splitContainer2.Size = new System.Drawing.Size(709, 314);
            this.splitContainer2.SplitterDistance = 226;
            this.splitContainer2.TabIndex = 1;
            // 
            // treeView
            // 
            this.treeView.AllowDrop = true;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HideSelection = false;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(226, 314);
            this.treeView.TabIndex = 0;
            this.treeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
            this.treeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeSelect);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            this.treeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_DragDrop);
            this.treeView.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView_DragOver);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imageList.Images.SetKeyName(0, "Button");
            this.imageList.Images.SetKeyName(1, "ComboBox");
            this.imageList.Images.SetKeyName(2, "ComboBoxImage");
            this.imageList.Images.SetKeyName(3, "Label");
            this.imageList.Images.SetKeyName(4, "MenuItem");
            this.imageList.Images.SetKeyName(5, "MenuStrip");
            this.imageList.Images.SetKeyName(6, "ProgressBar");
            this.imageList.Images.SetKeyName(7, "Separator");
            this.imageList.Images.SetKeyName(8, "SplitButton");
            this.imageList.Images.SetKeyName(9, "StatusStrip");
            this.imageList.Images.SetKeyName(10, "TextBox");
            this.imageList.Images.SetKeyName(11, "ToolStrip");
            this.imageList.Images.SetKeyName(12, "System");
            this.imageList.Images.SetKeyName(13, "ToolStrips");
            this.imageList.Images.SetKeyName(14, "StatusStrips");
            this.imageList.Images.SetKeyName(15, "MenuStrips");
            this.imageList.Images.SetKeyName(16, "ContextMenuStrip");
            this.imageList.Images.SetKeyName(17, "ContextMenuStrips");
            this.imageList.Images.SetKeyName(18, "ToolStripContainer");
            this.imageList.Images.SetKeyName(19, "ComboBoxItem");
            this.imageList.Images.SetKeyName(20, "ComboBoxImageItem");
            this.imageList.Images.SetKeyName(21, "copy");
            this.imageList.Images.SetKeyName(22, "cut");
            this.imageList.Images.SetKeyName(23, "delete");
            this.imageList.Images.SetKeyName(24, "paste");
            this.imageList.Images.SetKeyName(25, "valid");
            this.imageList.Images.SetKeyName(26, "invalid");
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(479, 314);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // tpRegist
            // 
            this.tpRegist.Controls.Add(this.btnDeflate);
            this.tpRegist.Controls.Add(this.btnModifySelected);
            this.tpRegist.Controls.Add(this.btnRemoveAll);
            this.tpRegist.Controls.Add(this.btnAdd);
            this.tpRegist.Controls.Add(this.btnRemoveSelected);
            this.tpRegist.Controls.Add(this.lvAddIn);
            this.tpRegist.Location = new System.Drawing.Point(4, 25);
            this.tpRegist.Name = "tpRegist";
            this.tpRegist.Padding = new System.Windows.Forms.Padding(3);
            this.tpRegist.Size = new System.Drawing.Size(715, 296);
            this.tpRegist.TabIndex = 0;
            this.tpRegist.Text = "Regist AddIns";
            this.tpRegist.UseVisualStyleBackColor = true;
            // 
            // btnDeflate
            // 
            this.btnDeflate.Location = new System.Drawing.Point(533, 6);
            this.btnDeflate.Name = "btnDeflate";
            this.btnDeflate.Size = new System.Drawing.Size(108, 23);
            this.btnDeflate.TabIndex = 49;
            this.btnDeflate.Text = "Deflate";
            this.btnDeflate.UseVisualStyleBackColor = true;
            this.btnDeflate.Click += new System.EventHandler(this.btnDeflate_Click);
            // 
            // btnModifySelected
            // 
            this.btnModifySelected.Enabled = false;
            this.btnModifySelected.Location = new System.Drawing.Point(132, 6);
            this.btnModifySelected.Name = "btnModifySelected";
            this.btnModifySelected.Size = new System.Drawing.Size(108, 23);
            this.btnModifySelected.TabIndex = 48;
            this.btnModifySelected.Text = "Modify Selected";
            this.btnModifySelected.UseVisualStyleBackColor = true;
            this.btnModifySelected.Click += new System.EventHandler(this.btnModifySelected_Click);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Location = new System.Drawing.Point(266, 6);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(108, 23);
            this.btnRemoveAll.TabIndex = 45;
            this.btnRemoveAll.Text = "Remove All";
            this.btnRemoveAll.UseVisualStyleBackColor = true;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(3, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(108, 23);
            this.btnAdd.TabIndex = 43;
            this.btnAdd.Text = "Add New";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.Enabled = false;
            this.btnRemoveSelected.Location = new System.Drawing.Point(401, 6);
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(108, 23);
            this.btnRemoveSelected.TabIndex = 42;
            this.btnRemoveSelected.Text = "Remove Selected";
            this.btnRemoveSelected.UseVisualStyleBackColor = true;
            this.btnRemoveSelected.Click += new System.EventHandler(this.btnRemoveSelected_Click);
            // 
            // lvAddIn
            // 
            this.lvAddIn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvAddIn.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chAuthor,
            this.chVersion,
            this.chCopyRight,
            this.chUrl,
            this.chPath,
            this.chIsBaseService,
            this.chLazyLoad,
            this.chDescription});
            this.lvAddIn.FullRowSelect = true;
            this.lvAddIn.GridLines = true;
            this.lvAddIn.Location = new System.Drawing.Point(3, 35);
            this.lvAddIn.Name = "lvAddIn";
            this.lvAddIn.Size = new System.Drawing.Size(709, 258);
            this.lvAddIn.SmallImageList = this.imageList;
            this.lvAddIn.TabIndex = 0;
            this.lvAddIn.UseCompatibleStateImageBehavior = false;
            this.lvAddIn.View = System.Windows.Forms.View.Details;
            this.lvAddIn.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvAddIn_ItemSelectionChanged);
            this.lvAddIn.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvAddIn_MouseDoubleClick);
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 94;
            // 
            // chAuthor
            // 
            this.chAuthor.Text = "Author";
            this.chAuthor.Width = 93;
            // 
            // chVersion
            // 
            this.chVersion.Text = "Version";
            this.chVersion.Width = 70;
            // 
            // chCopyRight
            // 
            this.chCopyRight.Text = "CopyRight";
            this.chCopyRight.Width = 96;
            // 
            // chUrl
            // 
            this.chUrl.Text = "Url";
            this.chUrl.Width = 80;
            // 
            // chPath
            // 
            this.chPath.Text = "Path";
            this.chPath.Width = 103;
            // 
            // chIsBaseService
            // 
            this.chIsBaseService.Text = "IsBaseService";
            // 
            // chLazyLoad
            // 
            this.chLazyLoad.Text = "LazyLoad";
            this.chLazyLoad.Width = 98;
            // 
            // chDescription
            // 
            this.chDescription.Text = "Description";
            this.chDescription.Width = 130;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(636, 361);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(542, 361);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(445, 361);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cmsSingleItem
            // 
            this.cmsSingleItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCreateToolStrip,
            this.tsmiCreateComboBoxItem,
            this.tsmiCreateComboBoxImageItem,
            this.tsmiCreateContextMenuStrip,
            this.tssSingle,
            this.tsmiSingleCopy,
            this.tsmiSingleCut,
            this.tsmiSinglePaste,
            this.tsmiSingleDelete});
            this.cmsSingleItem.Name = "cmsSingleItem";
            this.cmsSingleItem.Size = new System.Drawing.Size(235, 186);
            // 
            // tsmiCreateToolStrip
            // 
            this.tsmiCreateToolStrip.Name = "tsmiCreateToolStrip";
            this.tsmiCreateToolStrip.Size = new System.Drawing.Size(234, 22);
            this.tsmiCreateToolStrip.Text = "New ToolStrip";
            this.tsmiCreateToolStrip.Click += new System.EventHandler(this.tsmiCreate_Click);
            // 
            // tsmiCreateComboBoxItem
            // 
            this.tsmiCreateComboBoxItem.Name = "tsmiCreateComboBoxItem";
            this.tsmiCreateComboBoxItem.Size = new System.Drawing.Size(234, 22);
            this.tsmiCreateComboBoxItem.Text = "New ComboBoxItem";
            this.tsmiCreateComboBoxItem.Click += new System.EventHandler(this.tsmiCreate_Click);
            // 
            // tsmiCreateComboBoxImageItem
            // 
            this.tsmiCreateComboBoxImageItem.Name = "tsmiCreateComboBoxImageItem";
            this.tsmiCreateComboBoxImageItem.Size = new System.Drawing.Size(234, 22);
            this.tsmiCreateComboBoxImageItem.Text = "New ComboBoxImageItem";
            this.tsmiCreateComboBoxImageItem.Click += new System.EventHandler(this.tsmiCreate_Click);
            // 
            // tsmiCreateContextMenuStrip
            // 
            this.tsmiCreateContextMenuStrip.Name = "tsmiCreateContextMenuStrip";
            this.tsmiCreateContextMenuStrip.Size = new System.Drawing.Size(234, 22);
            this.tsmiCreateContextMenuStrip.Text = "New ContextMenuStrip";
            this.tsmiCreateContextMenuStrip.Click += new System.EventHandler(this.tsmiCreate_Click);
            // 
            // tssSingle
            // 
            this.tssSingle.Name = "tssSingle";
            this.tssSingle.Size = new System.Drawing.Size(231, 6);
            // 
            // tsmiSingleCopy
            // 
            this.tsmiSingleCopy.Name = "tsmiSingleCopy";
            this.tsmiSingleCopy.Size = new System.Drawing.Size(234, 22);
            this.tsmiSingleCopy.Text = "Copy";
            this.tsmiSingleCopy.Click += new System.EventHandler(this.tsmiCopy_Click);
            // 
            // tsmiSingleCut
            // 
            this.tsmiSingleCut.Name = "tsmiSingleCut";
            this.tsmiSingleCut.Size = new System.Drawing.Size(234, 22);
            this.tsmiSingleCut.Text = "Cut";
            this.tsmiSingleCut.Click += new System.EventHandler(this.tsmiCut_Click);
            // 
            // tsmiSinglePaste
            // 
            this.tsmiSinglePaste.Enabled = false;
            this.tsmiSinglePaste.Name = "tsmiSinglePaste";
            this.tsmiSinglePaste.Size = new System.Drawing.Size(234, 22);
            this.tsmiSinglePaste.Text = "Paste";
            this.tsmiSinglePaste.Click += new System.EventHandler(this.tsmiPaste_Click);
            // 
            // tsmiSingleDelete
            // 
            this.tsmiSingleDelete.Name = "tsmiSingleDelete";
            this.tsmiSingleDelete.Size = new System.Drawing.Size(234, 22);
            this.tsmiSingleDelete.Text = "Delete";
            this.tsmiSingleDelete.Click += new System.EventHandler(this.tsmiDelete_Click);
            // 
            // cmsCreateCommand
            // 
            this.cmsCreateCommand.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCreateMenuItem,
            this.tsmiCreateButton,
            this.tsmiCreateDropDownButton,
            this.tsmiCreateSplitButton,
            this.tsmiCreateComboBox,
            this.tsmiCreateComboBoxImage,
            this.tsmiCreateTextBox,
            this.tsmiCreateLabel,
            this.tsmiCreateStatusLabel,
            this.tsmiCreateSeparator,
            this.tsmiCreateProgressBar,
            this.tssCreate,
            this.tsmiCreateCopy,
            this.tsmiCreateCut,
            this.tsmiCreatePaste,
            this.tsmiCreateDelete});
            this.cmsCreateCommand.Name = "cmsSingleItem";
            this.cmsCreateCommand.Size = new System.Drawing.Size(209, 340);
            // 
            // tsmiCreateMenuItem
            // 
            this.tsmiCreateMenuItem.Name = "tsmiCreateMenuItem";
            this.tsmiCreateMenuItem.Size = new System.Drawing.Size(208, 22);
            this.tsmiCreateMenuItem.Text = "New MenuItem";
            this.tsmiCreateMenuItem.Click += new System.EventHandler(this.tsmiCreate_Click);
            // 
            // tsmiCreateButton
            // 
            this.tsmiCreateButton.Name = "tsmiCreateButton";
            this.tsmiCreateButton.Size = new System.Drawing.Size(208, 22);
            this.tsmiCreateButton.Text = "New Button";
            this.tsmiCreateButton.Click += new System.EventHandler(this.tsmiCreate_Click);
            // 
            // tsmiCreateDropDownButton
            // 
            this.tsmiCreateDropDownButton.Name = "tsmiCreateDropDownButton";
            this.tsmiCreateDropDownButton.Size = new System.Drawing.Size(208, 22);
            this.tsmiCreateDropDownButton.Text = "New DropDownButton";
            this.tsmiCreateDropDownButton.Click += new System.EventHandler(this.tsmiCreate_Click);
            // 
            // tsmiCreateSplitButton
            // 
            this.tsmiCreateSplitButton.Name = "tsmiCreateSplitButton";
            this.tsmiCreateSplitButton.Size = new System.Drawing.Size(208, 22);
            this.tsmiCreateSplitButton.Text = "New SplitButton";
            this.tsmiCreateSplitButton.Click += new System.EventHandler(this.tsmiCreate_Click);
            // 
            // tsmiCreateComboBox
            // 
            this.tsmiCreateComboBox.Name = "tsmiCreateComboBox";
            this.tsmiCreateComboBox.Size = new System.Drawing.Size(208, 22);
            this.tsmiCreateComboBox.Text = "New ComboBox";
            this.tsmiCreateComboBox.Click += new System.EventHandler(this.tsmiCreate_Click);
            // 
            // tsmiCreateComboBoxImage
            // 
            this.tsmiCreateComboBoxImage.Name = "tsmiCreateComboBoxImage";
            this.tsmiCreateComboBoxImage.Size = new System.Drawing.Size(208, 22);
            this.tsmiCreateComboBoxImage.Text = "New ComboBoxImage";
            this.tsmiCreateComboBoxImage.Visible = false;
            this.tsmiCreateComboBoxImage.Click += new System.EventHandler(this.tsmiCreate_Click);
            // 
            // tsmiCreateTextBox
            // 
            this.tsmiCreateTextBox.Name = "tsmiCreateTextBox";
            this.tsmiCreateTextBox.Size = new System.Drawing.Size(208, 22);
            this.tsmiCreateTextBox.Text = "New TextBox";
            this.tsmiCreateTextBox.Click += new System.EventHandler(this.tsmiCreate_Click);
            // 
            // tsmiCreateLabel
            // 
            this.tsmiCreateLabel.Name = "tsmiCreateLabel";
            this.tsmiCreateLabel.Size = new System.Drawing.Size(208, 22);
            this.tsmiCreateLabel.Text = "New Label";
            this.tsmiCreateLabel.Click += new System.EventHandler(this.tsmiCreate_Click);
            // 
            // tsmiCreateStatusLabel
            // 
            this.tsmiCreateStatusLabel.Name = "tsmiCreateStatusLabel";
            this.tsmiCreateStatusLabel.Size = new System.Drawing.Size(208, 22);
            this.tsmiCreateStatusLabel.Text = "New StatusLabel";
            this.tsmiCreateStatusLabel.Click += new System.EventHandler(this.tsmiCreate_Click);
            // 
            // tsmiCreateSeparator
            // 
            this.tsmiCreateSeparator.Name = "tsmiCreateSeparator";
            this.tsmiCreateSeparator.Size = new System.Drawing.Size(208, 22);
            this.tsmiCreateSeparator.Text = "New Separator";
            this.tsmiCreateSeparator.Click += new System.EventHandler(this.tsmiCreate_Click);
            // 
            // tsmiCreateProgressBar
            // 
            this.tsmiCreateProgressBar.Name = "tsmiCreateProgressBar";
            this.tsmiCreateProgressBar.Size = new System.Drawing.Size(208, 22);
            this.tsmiCreateProgressBar.Text = "New ProgressBar";
            this.tsmiCreateProgressBar.Click += new System.EventHandler(this.tsmiCreate_Click);
            // 
            // tssCreate
            // 
            this.tssCreate.Name = "tssCreate";
            this.tssCreate.Size = new System.Drawing.Size(205, 6);
            // 
            // tsmiCreateCopy
            // 
            this.tsmiCreateCopy.Name = "tsmiCreateCopy";
            this.tsmiCreateCopy.Size = new System.Drawing.Size(208, 22);
            this.tsmiCreateCopy.Text = "Copy";
            this.tsmiCreateCopy.Click += new System.EventHandler(this.tsmiCopy_Click);
            // 
            // tsmiCreateCut
            // 
            this.tsmiCreateCut.Name = "tsmiCreateCut";
            this.tsmiCreateCut.Size = new System.Drawing.Size(208, 22);
            this.tsmiCreateCut.Text = "Cut";
            this.tsmiCreateCut.Click += new System.EventHandler(this.tsmiCut_Click);
            // 
            // tsmiCreatePaste
            // 
            this.tsmiCreatePaste.Enabled = false;
            this.tsmiCreatePaste.Name = "tsmiCreatePaste";
            this.tsmiCreatePaste.Size = new System.Drawing.Size(208, 22);
            this.tsmiCreatePaste.Text = "Paste";
            this.tsmiCreatePaste.Click += new System.EventHandler(this.tsmiPaste_Click);
            // 
            // tsmiCreateDelete
            // 
            this.tsmiCreateDelete.Name = "tsmiCreateDelete";
            this.tsmiCreateDelete.Size = new System.Drawing.Size(208, 22);
            this.tsmiCreateDelete.Text = "Delete";
            this.tsmiCreateDelete.Click += new System.EventHandler(this.tsmiDelete_Click);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tabControl1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.btnSave);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.btnCancel);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.btnOK);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(723, 396);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(723, 421);
            this.toolStripContainer1.TabIndex = 7;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(723, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            this.menuStrip1.ItemAdded += new System.Windows.Forms.ToolStripItemEventHandler(this.menuStrip1_ItemAdded);
            this.menuStrip1.ItemRemoved += new System.Windows.Forms.ToolStripItemEventHandler(this.menuStrip1_ItemRemoved);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 421);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(723, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // AddInModifyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 443);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AddInModifyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Custom Work Environment";
            this.tabControl1.ResumeLayout(false);
            this.tpUi.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.tpRegist.ResumeLayout(false);
            this.cmsSingleItem.ResumeLayout(false);
            this.cmsCreateCommand.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpUi;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TabPage tpRegist;
        private System.Windows.Forms.Button btnDeflate;
        private System.Windows.Forms.Button btnModifySelected;
        private System.Windows.Forms.Button btnRemoveAll;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemoveSelected;
        private System.Windows.Forms.ListView lvAddIn;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenuStrip cmsSingleItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateContextMenuStrip;
        private System.Windows.Forms.ToolStripSeparator tssSingle;
        private System.Windows.Forms.ToolStripMenuItem tsmiSingleCopy;
        private System.Windows.Forms.ToolStripMenuItem tsmiSingleCut;
        private System.Windows.Forms.ToolStripMenuItem tsmiSinglePaste;
        private System.Windows.Forms.ToolStripMenuItem tsmiSingleDelete;
        private System.Windows.Forms.ContextMenuStrip cmsCreateCommand;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateButton;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateComboBox;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateTextBox;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateLabel;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateSeparator;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateSplitButton;
        private System.Windows.Forms.ToolStripSeparator tssCreate;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreatePaste;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateDelete;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateCut;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateCopy;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateComboBoxImage;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateProgressBar;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateToolStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateComboBoxItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateComboBoxImageItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreateMenuItem;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chAuthor;
        private System.Windows.Forms.ColumnHeader chCopyRight;
        private System.Windows.Forms.ColumnHeader chUrl;
        private System.Windows.Forms.ColumnHeader chPath;
        private System.Windows.Forms.ColumnHeader chIsBaseService;
        private System.Windows.Forms.ColumnHeader chLazyLoad;
        private System.Windows.Forms.ColumnHeader chDescription;
        private System.Windows.Forms.ColumnHeader chVersion;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        
    }
}
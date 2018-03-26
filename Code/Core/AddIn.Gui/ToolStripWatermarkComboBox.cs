using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design;

namespace AddIn.Gui
{
    [ToolboxBitmap(typeof(ToolStripComboBox))]
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    public class ToolStripWatermarkComboBox : ToolStripControlHost
    {
        public ToolStripWatermarkComboBox()
            : base(CreateControlInstance())
        {
            base.AutoSize = false;
            (base.Control as WatermarkComboBox).SelectedIndexChanged += ToolStripComboBoxEx_SelectedIndexChanged;
        }

        [Browsable(false)]
        public WatermarkComboBox WatermarkComboBox
        {
            get { return base.Control as WatermarkComboBox; }
        }

        void ToolStripComboBoxEx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, e);
        }

        public ToolStripWatermarkComboBox(string name)
            :this()
        {
            base.Name = name;
        }

        [Browsable(true)]
        [Category("apparent")]
        [Description("The Text show in the editbox when the ComboBox is not focused and its DropDownStyle is not DropDownList and its Text is empty.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string EmptyTextTip
        {
            get { return (base.Control as WatermarkComboBox).EmptyTextTip; }
            set { (base.Control as WatermarkComboBox).EmptyTextTip = value; }
        }

        [Browsable(true)]
        [Category("apparent")]
        [Description("The color of the EmptyTextTip.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(typeof(Color), "LightGray")]
        public Color EmptyTextTipColor
        {
            get { return (base.Control as WatermarkComboBox).EmptyTextTipColor; }
            set { (base.Control as WatermarkComboBox).EmptyTextTipColor = value; }
        }

        public AutoCompleteSource AutoCompleteSource
        {
            get { return (base.Control as WatermarkComboBox).AutoCompleteSource; }
            set { (base.Control as WatermarkComboBox).AutoCompleteSource = value; }
        }

        public AutoCompleteMode AutoCompleteMode
        {
            get { return (base.Control as WatermarkComboBox).AutoCompleteMode; }
            set { (base.Control as WatermarkComboBox).AutoCompleteMode = value; }
        }

        public ComboBoxStyle DropDownStyle
        {
            get { return (base.Control as WatermarkComboBox).DropDownStyle; }
            set { (base.Control as WatermarkComboBox).DropDownStyle = value; }
        }

        public int DropDownWidth
        {
            get { return (base.Control as WatermarkComboBox).DropDownWidth; }
            set { (base.Control as WatermarkComboBox).DropDownWidth = value; }
        }

        public int DropDownHeight
        {
            get { return (base.Control as WatermarkComboBox).DropDownHeight; }
            set { (base.Control as WatermarkComboBox).DropDownHeight = value; }
        }

        public int MaxDropDownItems
        {
            get { return (base.Control as WatermarkComboBox).MaxDropDownItems; }
            set { (base.Control as WatermarkComboBox).MaxDropDownItems = value; }
        }

        public int SelectedIndex
        {
            get { return (base.Control as WatermarkComboBox).SelectedIndex; }
            set { (base.Control as WatermarkComboBox).SelectedIndex = value; }
        }

        public object SelectedItem
        {
            get { return (base.Control as WatermarkComboBox).SelectedItem; }
            set { (base.Control as WatermarkComboBox).SelectedItem = value; }
        }

        public ComboBox.ObjectCollection Items
        {
            get { return (base.Control as WatermarkComboBox).Items; }
        }

        private static Control CreateControlInstance()
        {
            WatermarkComboBox watermarkComboBox = new WatermarkComboBox();

            return watermarkComboBox;
        }

        public event EventHandler SelectedIndexChanged;
    }
}

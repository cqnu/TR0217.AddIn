using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design;

namespace AddIn.Gui
{
    [ToolboxBitmap(typeof(ToolStripTextBox))]
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    public class ToolStripWatermarkTextBox : ToolStripControlHost
    {
        public ToolStripWatermarkTextBox()
            : base(CreateControlInstance())
        {
            this.AutoSize = false;
            this.Size = new Size(100, 22);
        }

        public ToolStripWatermarkTextBox(string name)
            : this()
        {
            base.Name = name;
        }

        public WatermarkTextBox WatermarkTextBox
        {
            get { return base.Control as WatermarkTextBox; }
        }


        [Browsable(true)]
        [Category("apparent")]
        [Description("The Text show in the editbox when the ComboBox is not focused and its DropDownStyle is not DropDownList and its Text is empty.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string EmptyTextTip
        {
            get { return (base.Control as WatermarkTextBox).EmptyTextTip; }
            set { (base.Control as WatermarkTextBox).EmptyTextTip = value; }
        }

        [Browsable(true)]
        [Category("apparent")]
        [Description("The color of the EmptyTextTip.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(typeof(Color), "DarkGray")]
        public Color EmptyTextTipColor
        {
            get { return (base.Control as WatermarkTextBox).EmptyTextTipColor; }
            set { (base.Control as WatermarkTextBox).EmptyTextTipColor = value; }
        }

        private static Control CreateControlInstance()
        {
            WatermarkTextBox watermarkTextBox = new WatermarkTextBox();
            watermarkTextBox.Size = new Size(100, 22);

            return watermarkTextBox;
        }
    }
}

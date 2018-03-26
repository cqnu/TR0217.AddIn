using System;
using System.Collections.Generic;
using System.Text;

namespace AddIn.Core
{
    public delegate void UpdateUiElemHandler(object sender, UpdateUiElemEventArgs e);
 
    public class UpdateUiElemEventArgs : EventArgs
    {
        private bool _checked;
        private bool _enabled;
        private bool _visible;
        private int _count;
        private int _maximum;
        private string _text;
        private object _value;

        /// <summary>
        /// used for setting uiElem's checked state after an event happened.
        /// </summary>
        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; }
        }

        /// <summary>
        /// used for setting uiElem's enabled state after an event happened.
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        /// <summary>
        /// used for setting uiElem's visible state after an event happened.
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        /// <summary>
        /// used for setting uiElem's progress or ratio number after an evnet happened.
        /// this value usually be used to update ProgressBar's progress value.
        /// surely, it can also be used to update TrackBar's value and ComboBox's selected index.
        /// if this be used for setting ComboBox's selected index, it will be the priority.
        /// please keep in mind that change of container control's selected index or value 
        /// will raise an event lead to the function registed to that control to be invoked.
        /// </summary>
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        /// <summary>
        /// used for calculating uiElem's max progress value or track value.
        /// </summary>
        public int Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }

        /// <summary>
        /// used for setting uiElem's text after an evnet happened.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// used for setting ComboBox's selected value or other container control's selected value.
        /// this field is prepared for some usercontrol, for instance, ComboBox that contain the items
        /// that have a image at the begining.
        /// </summary>
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public UpdateUiElemEventArgs()
        {
            _checked = false;
            _enabled = true;
            _visible = true;
            _count = -10;
            _maximum = 100;
            _text = string.Empty;
        }
    } 
}

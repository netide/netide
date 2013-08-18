using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Util.Forms
{
    public class UserControl : System.Windows.Forms.UserControl
    {
        private readonly FormHelper _fixer;

        internal bool IsFixed { get; set; }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public new AutoScaleMode AutoScaleMode
        {
            get { return base.AutoScaleMode; }
            set
            {
                // This value is set by the designer. To not have to manually change the
                // defaults set by the designer, it's silently ignored here at runtime.

                if (_fixer.InDesignMode)
                    base.AutoScaleMode = value;
            }
        }

        public UserControl()
        {
            _fixer = new FormHelper(this)
            {
                EnableBoundsTracking = false
            };
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
                _fixer.InitializeForm();

            base.OnVisibleChanged(e);
        }

        protected override void SetVisibleCore(bool value)
        {
            _fixer.InitializeForm();

            base.SetVisibleCore(value);
        }
    }
}

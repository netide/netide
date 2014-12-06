namespace NetIde.Core.TextEditor
{
    partial class FindControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._elementControl = new GdiPresentation.ElementControl();
            this.SuspendLayout();
            // 
            // _elementControl
            // 
            this._elementControl.BackColor = System.Drawing.SystemColors.Control;
            this._elementControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._elementControl.Location = new System.Drawing.Point(1, 0);
            this._elementControl.Name = "_elementControl";
            this._elementControl.ResizeTarget = null;
            this._elementControl.ShowHorizontalScrollBar = GdiPresentation.ScrollBarVisibility.Hidden;
            this._elementControl.ShowVerticalScrollBar = GdiPresentation.ScrollBarVisibility.Hidden;
            this._elementControl.Size = new System.Drawing.Size(311, 200);
            this._elementControl.TabIndex = 0;
            // 
            // FindControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this._elementControl);
            this.Name = "FindControl";
            this.Padding = new System.Windows.Forms.Padding(1, 0, 0, 1);
            this.Size = new System.Drawing.Size(312, 201);
            this.ResumeLayout(false);

        }

        #endregion

        private GdiPresentation.ElementControl _elementControl;
    }
}

namespace NetIde.Core.PackageManagement
{
    partial class PackageDetailsControl
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
            this._moreInformationLink = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // _moreInformationLink
            // 
            this._moreInformationLink.AutoSize = true;
            this._moreInformationLink.Location = new System.Drawing.Point(7, 7);
            this._moreInformationLink.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._moreInformationLink.Name = "_moreInformationLink";
            this._moreInformationLink.Size = new System.Drawing.Size(86, 13);
            this._moreInformationLink.TabIndex = 24;
            this._moreInformationLink.TabStop = true;
            this._moreInformationLink.Text = "More Information";
            this._moreInformationLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._moreInformationLink_LinkClicked);
            // 
            // PackageDetailsControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._moreInformationLink);
            this.Name = "PackageDetailsControl";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Size = new System.Drawing.Size(250, 400);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel _moreInformationLink;

    }
}

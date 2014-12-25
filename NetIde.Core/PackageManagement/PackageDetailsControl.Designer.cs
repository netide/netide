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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageDetailsControl));
            this._moreInformationLink = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // _moreInformationLink
            // 
            resources.ApplyResources(this._moreInformationLink, "_moreInformationLink");
            this._moreInformationLink.Name = "_moreInformationLink";
            this._moreInformationLink.TabStop = true;
            this._moreInformationLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._moreInformationLink_LinkClicked);
            // 
            // PackageDetailsControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._moreInformationLink);
            this.Name = "PackageDetailsControl";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel _moreInformationLink;

    }
}

namespace NetIde.Util.Forms
{
    partial class BrowserControl
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
            this._browseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _browseButton
            // 
            this._browseButton.Location = new System.Drawing.Point(281, 3);
            this._browseButton.Name = "_browseButton";
            this._browseButton.Size = new System.Drawing.Size(25, 23);
            this._browseButton.TabIndex = 4;
            this._browseButton.Text = "...";
            this._browseButton.UseVisualStyleBackColor = true;
            this._browseButton.Click += new System.EventHandler(this._browseButton_Click);
            // 
            // BrowserControl
            // 
            this.Controls.Add(this._browseButton);
            this.Name = "BrowserControl";
            this.Size = new System.Drawing.Size(309, 29);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _browseButton;
    }
}

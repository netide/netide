using System.Text;
using System.Collections.Generic;
using System;

namespace NetIde.Util.Forms
{
    partial class FileBrowser
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
            this._path = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _path
            // 
            this._path.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this._path.Location = new System.Drawing.Point(0, 1);
            this._path.Name = "_path";
            this._path.Size = new System.Drawing.Size(173, 20);
            this._path.TabIndex = 0;
            this._path.TextChanged += new System.EventHandler(this._path_TextChanged);
            // 
            // FileBrowser
            // 
            this.Controls.Add(this._path);
            this.Name = "FileBrowser";
            this.Controls.SetChildIndex(this._path, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _path;
    }
}

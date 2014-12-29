using System.Text;
using System.Collections.Generic;
using System;

namespace NetIde.Util.Forms
{
    partial class MruFileBrowser
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
            this._path = new NetIde.Util.Forms.MruComboBox();
            this.SuspendLayout();
            // 
            // _path
            // 
            this._path.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this._path.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._path.FormattingEnabled = true;
            this._path.Location = new System.Drawing.Point(0, 0);
            this._path.Name = "_path";
            this._path.Size = new System.Drawing.Size(121, 21);
            this._path.TabIndex = 0;
            this._path.LoadHistory += new NetIde.Util.Forms.MruHistoryEventHandler(this._path_LoadHistory);
            this._path.SaveHistory += new NetIde.Util.Forms.MruHistoryEventHandler(this._path_SaveHistory);
            this._path.TextChanged += new System.EventHandler(this._path_TextChanged);
            // 
            // MruFileBrowser
            // 
            this.Controls.Add(this._path);
            this.Name = "MruFileBrowser";
            this.Controls.SetChildIndex(this._path, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MruComboBox _path;

    }
}

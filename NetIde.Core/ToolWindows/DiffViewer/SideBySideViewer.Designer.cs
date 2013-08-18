namespace NetIde.Core.ToolWindows.DiffViewer
{
    partial class SideBySideViewer
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.themedPanel1 = new NetIde.Util.Forms.ThemedPanel();
            this._leftEditor = new ICSharpCode.TextEditor.TextEditorControl();
            this._leftDetails = new NetIde.Core.ToolWindows.DiffViewer.StreamDetailsControl();
            this.themedPanel2 = new NetIde.Util.Forms.ThemedPanel();
            this._rightEditor = new ICSharpCode.TextEditor.TextEditorControl();
            this._rightDetails = new NetIde.Core.ToolWindows.DiffViewer.StreamDetailsControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.themedPanel1.SuspendLayout();
            this.themedPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.themedPanel1);
            this.splitContainer1.Panel1.Controls.Add(this._leftDetails);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.themedPanel2);
            this.splitContainer1.Panel2.Controls.Add(this._rightDetails);
            this.splitContainer1.Size = new System.Drawing.Size(500, 500);
            this.splitContainer1.SplitterDistance = 247;
            this.splitContainer1.TabIndex = 1;
            // 
            // themedPanel1
            // 
            this.themedPanel1.Controls.Add(this._leftEditor);
            this.themedPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.themedPanel1.Location = new System.Drawing.Point(0, 25);
            this.themedPanel1.Name = "themedPanel1";
            this.themedPanel1.Size = new System.Drawing.Size(247, 475);
            this.themedPanel1.TabIndex = 2;
            // 
            // _leftEditor
            // 
            this._leftEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this._leftEditor.IsReadOnly = false;
            this._leftEditor.Location = new System.Drawing.Point(1, 1);
            this._leftEditor.Name = "_leftEditor";
            this._leftEditor.ShowLineNumbers = false;
            this._leftEditor.Size = new System.Drawing.Size(245, 473);
            this._leftEditor.TabIndex = 1;
            // 
            // _leftDetails
            // 
            this._leftDetails.CanOverflow = false;
            this._leftDetails.ContentType = "";
            this._leftDetails.Encoding = null;
            this._leftDetails.FileSize = null;
            this._leftDetails.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._leftDetails.HaveBom = false;
            this._leftDetails.LastWriteTime = null;
            this._leftDetails.LineTermination = null;
            this._leftDetails.Location = new System.Drawing.Point(0, 0);
            this._leftDetails.Name = "_leftDetails";
            this._leftDetails.Size = new System.Drawing.Size(247, 25);
            this._leftDetails.TabIndex = 3;
            this._leftDetails.ContentTypeSelected += new System.EventHandler(this._leftDetails_ContentTypeSelected);
            // 
            // themedPanel2
            // 
            this.themedPanel2.Controls.Add(this._rightEditor);
            this.themedPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.themedPanel2.Location = new System.Drawing.Point(0, 25);
            this.themedPanel2.Name = "themedPanel2";
            this.themedPanel2.Size = new System.Drawing.Size(249, 475);
            this.themedPanel2.TabIndex = 2;
            // 
            // _rightEditor
            // 
            this._rightEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rightEditor.IsReadOnly = false;
            this._rightEditor.Location = new System.Drawing.Point(1, 1);
            this._rightEditor.Name = "_rightEditor";
            this._rightEditor.ShowLineNumbers = false;
            this._rightEditor.Size = new System.Drawing.Size(247, 473);
            this._rightEditor.TabIndex = 1;
            // 
            // _rightDetails
            // 
            this._rightDetails.CanOverflow = false;
            this._rightDetails.ContentType = "";
            this._rightDetails.Encoding = null;
            this._rightDetails.FileSize = null;
            this._rightDetails.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._rightDetails.HaveBom = false;
            this._rightDetails.LastWriteTime = null;
            this._rightDetails.LineTermination = null;
            this._rightDetails.Location = new System.Drawing.Point(0, 0);
            this._rightDetails.Name = "_rightDetails";
            this._rightDetails.Size = new System.Drawing.Size(249, 25);
            this._rightDetails.TabIndex = 3;
            this._rightDetails.ContentTypeSelected += new System.EventHandler(this._rightDetails_ContentTypeSelected);
            // 
            // SideBySideViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "SideBySideViewer";
            this.Size = new System.Drawing.Size(500, 500);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.themedPanel1.ResumeLayout(false);
            this.themedPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Util.Forms.ThemedPanel themedPanel1;
        private ICSharpCode.TextEditor.TextEditorControl _leftEditor;
        private Util.Forms.ThemedPanel themedPanel2;
        private ICSharpCode.TextEditor.TextEditorControl _rightEditor;
        private StreamDetailsControl _leftDetails;
        private StreamDetailsControl _rightDetails;
    }
}

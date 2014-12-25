namespace NetIde.Core.ToolWindows.DiffViewer
{
    partial class UnifiedViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnifiedViewer));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._leftDetails = new NetIde.Core.ToolWindows.DiffViewer.StreamDetailsControl();
            this._rightDetails = new NetIde.Core.ToolWindows.DiffViewer.StreamDetailsControl();
            this.themedPanel1 = new NetIde.Util.Forms.ThemedPanel();
            this._editor = new NetIde.Core.TextEditor.TextEditorControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.themedPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this._leftDetails, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._rightDetails, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.themedPanel1, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // _leftDetails
            // 
            this._leftDetails.CanOverflow = false;
            this._leftDetails.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            resources.ApplyResources(this._leftDetails, "_leftDetails");
            this._leftDetails.Name = "_leftDetails";
            // 
            // _rightDetails
            // 
            this._rightDetails.CanOverflow = false;
            this._rightDetails.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            resources.ApplyResources(this._rightDetails, "_rightDetails");
            this._rightDetails.Name = "_rightDetails";
            // 
            // themedPanel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.themedPanel1, 3);
            this.themedPanel1.Controls.Add(this._editor);
            resources.ApplyResources(this.themedPanel1, "themedPanel1");
            this.themedPanel1.Name = "themedPanel1";
            // 
            // _editor
            // 
            resources.ApplyResources(this._editor, "_editor");
            this._editor.IsReadOnly = false;
            this._editor.Name = "_editor";
            this._editor.ShowLineNumbers = false;
            // 
            // UnifiedViewer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UnifiedViewer";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.themedPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private StreamDetailsControl _leftDetails;
        private StreamDetailsControl _rightDetails;
        private Util.Forms.ThemedPanel themedPanel1;
        private NetIde.Core.TextEditor.TextEditorControl _editor;
    }
}

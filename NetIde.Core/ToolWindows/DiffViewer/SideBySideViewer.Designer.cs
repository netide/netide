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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SideBySideViewer));
            this.themedPanel1 = new NetIde.Util.Forms.ThemedPanel();
            this._leftEditor = new NetIde.Core.TextEditor.TextEditorControl();
            this.themedPanel2 = new NetIde.Util.Forms.ThemedPanel();
            this._rightEditor = new NetIde.Core.TextEditor.TextEditorControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._markerMap = new NetIde.Core.ToolWindows.DiffViewer.DiffMarkerMapControl();
            this._leftDetails = new NetIde.Core.ToolWindows.DiffViewer.StreamDetailsControl();
            this._rightDetails = new NetIde.Core.ToolWindows.DiffViewer.StreamDetailsControl();
            this._editor = new NetIde.Core.ToolWindows.DiffViewer.DiffEditorControl();
            this.themedPanel1.SuspendLayout();
            this.themedPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // themedPanel1
            // 
            this.themedPanel1.Controls.Add(this._leftEditor);
            resources.ApplyResources(this.themedPanel1, "themedPanel1");
            this.themedPanel1.Name = "themedPanel1";
            // 
            // _leftEditor
            // 
            resources.ApplyResources(this._leftEditor, "_leftEditor");
            this._leftEditor.EnableFolding = false;
            this._leftEditor.IsReadOnly = false;
            this._leftEditor.Name = "_leftEditor";
            this._leftEditor.ShowLineNumbers = false;
            // 
            // themedPanel2
            // 
            this.themedPanel2.Controls.Add(this._rightEditor);
            resources.ApplyResources(this.themedPanel2, "themedPanel2");
            this.themedPanel2.Name = "themedPanel2";
            // 
            // _rightEditor
            // 
            resources.ApplyResources(this._rightEditor, "_rightEditor");
            this._rightEditor.EnableFolding = false;
            this._rightEditor.IsReadOnly = false;
            this._rightEditor.Name = "_rightEditor";
            this._rightEditor.ShowLineNumbers = false;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.themedPanel2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.themedPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._markerMap, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this._leftDetails, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._rightDetails, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this._editor, 1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // _markerMap
            // 
            resources.ApplyResources(this._markerMap, "_markerMap");
            this._markerMap.Name = "_markerMap";
            this._markerMap.TabStop = false;
            this._markerMap.LineClicked += new NetIde.Core.ToolWindows.DiffViewer.DiffLineClickedEventHandler(this._markerMap_LineClicked);
            // 
            // _leftDetails
            // 
            this._leftDetails.CanOverflow = false;
            this._leftDetails.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            resources.ApplyResources(this._leftDetails, "_leftDetails");
            this._leftDetails.Name = "_leftDetails";
            this._leftDetails.ContentTypeSelected += new System.EventHandler(this._leftDetails_ContentTypeSelected);
            // 
            // _rightDetails
            // 
            this._rightDetails.CanOverflow = false;
            this._rightDetails.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            resources.ApplyResources(this._rightDetails, "_rightDetails");
            this._rightDetails.Name = "_rightDetails";
            this._rightDetails.ContentTypeSelected += new System.EventHandler(this._rightDetails_ContentTypeSelected);
            // 
            // _editor
            // 
            resources.ApplyResources(this._editor, "_editor");
            this._editor.Name = "_editor";
            this._editor.TabStop = false;
            this._editor.ButtonClick += new NetIde.Core.ToolWindows.DiffViewer.DiffEditorButtonEventHandler(this._editor_ButtonClick);
            // 
            // SideBySideViewer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SideBySideViewer";
            this.themedPanel1.ResumeLayout(false);
            this.themedPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Util.Forms.ThemedPanel themedPanel1;
        private NetIde.Core.TextEditor.TextEditorControl _leftEditor;
        private Util.Forms.ThemedPanel themedPanel2;
        private NetIde.Core.TextEditor.TextEditorControl _rightEditor;
        private StreamDetailsControl _leftDetails;
        private StreamDetailsControl _rightDetails;
        private DiffMarkerMapControl _markerMap;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DiffEditorControl _editor;
    }
}

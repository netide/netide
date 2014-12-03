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
            this.themedPanel1 = new NetIde.Util.Forms.ThemedPanel();
            this._leftEditor = new NetIde.Core.TextEditorControl();
            this.themedPanel2 = new NetIde.Util.Forms.ThemedPanel();
            this._rightEditor = new NetIde.Core.TextEditorControl();
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
            this.themedPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.themedPanel1.Location = new System.Drawing.Point(0, 25);
            this.themedPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.themedPanel1.Name = "themedPanel1";
            this.themedPanel1.Size = new System.Drawing.Size(240, 475);
            this.themedPanel1.TabIndex = 2;
            // 
            // _leftEditor
            // 
            this._leftEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this._leftEditor.EnableFolding = false;
            this._leftEditor.IsReadOnly = false;
            this._leftEditor.Location = new System.Drawing.Point(1, 1);
            this._leftEditor.Name = "_leftEditor";
            this._leftEditor.ShowLineNumbers = false;
            this._leftEditor.Size = new System.Drawing.Size(238, 473);
            this._leftEditor.TabIndex = 1;
            // 
            // themedPanel2
            // 
            this.themedPanel2.Controls.Add(this._rightEditor);
            this.themedPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.themedPanel2.Location = new System.Drawing.Point(244, 25);
            this.themedPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.themedPanel2.Name = "themedPanel2";
            this.themedPanel2.Size = new System.Drawing.Size(240, 475);
            this.themedPanel2.TabIndex = 4;
            // 
            // _rightEditor
            // 
            this._rightEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rightEditor.EnableFolding = false;
            this._rightEditor.IsReadOnly = false;
            this._rightEditor.Location = new System.Drawing.Point(1, 1);
            this._rightEditor.Name = "_rightEditor";
            this._rightEditor.ShowLineNumbers = false;
            this._rightEditor.Size = new System.Drawing.Size(238, 473);
            this._rightEditor.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.Controls.Add(this.themedPanel2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.themedPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._markerMap, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this._leftDetails, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._rightDetails, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this._editor, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(500, 500);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _markerMap
            // 
            this._markerMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this._markerMap.Location = new System.Drawing.Point(484, 25);
            this._markerMap.Margin = new System.Windows.Forms.Padding(0);
            this._markerMap.Name = "_markerMap";
            this._markerMap.Size = new System.Drawing.Size(16, 475);
            this._markerMap.TabIndex = 5;
            this._markerMap.TabStop = false;
            this._markerMap.Text = "sideBySideMarkerIndexControl1";
            this._markerMap.LineClicked += new NetIde.Core.ToolWindows.DiffViewer.DiffLineClickedEventHandler(this._markerMap_LineClicked);
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
            this._leftDetails.Size = new System.Drawing.Size(240, 25);
            this._leftDetails.TabIndex = 0;
            this._leftDetails.ContentTypeSelected += new System.EventHandler(this._leftDetails_ContentTypeSelected);
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
            this._rightDetails.Location = new System.Drawing.Point(244, 0);
            this._rightDetails.Name = "_rightDetails";
            this._rightDetails.Size = new System.Drawing.Size(240, 25);
            this._rightDetails.TabIndex = 1;
            this._rightDetails.ContentTypeSelected += new System.EventHandler(this._rightDetails_ContentTypeSelected);
            // 
            // _editor
            // 
            this._editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this._editor.Location = new System.Drawing.Point(240, 25);
            this._editor.Margin = new System.Windows.Forms.Padding(0);
            this._editor.Name = "_editor";
            this._editor.Size = new System.Drawing.Size(4, 475);
            this._editor.TabIndex = 3;
            this._editor.TabStop = false;
            this._editor.ButtonClick += new NetIde.Core.ToolWindows.DiffViewer.DiffEditorButtonEventHandler(this._editor_ButtonClick);
            // 
            // SideBySideViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SideBySideViewer";
            this.Size = new System.Drawing.Size(500, 500);
            this.themedPanel1.ResumeLayout(false);
            this.themedPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Util.Forms.ThemedPanel themedPanel1;
        private NetIde.Core.TextEditorControl _leftEditor;
        private Util.Forms.ThemedPanel themedPanel2;
        private NetIde.Core.TextEditorControl _rightEditor;
        private StreamDetailsControl _leftDetails;
        private StreamDetailsControl _rightDetails;
        private DiffMarkerMapControl _markerMap;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DiffEditorControl _editor;
    }
}

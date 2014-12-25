namespace NetIde.Core.ToolWindows.DiffViewer
{
    partial class DiffViewerControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiffViewerControl));
            this._textViewer = new NetIde.Core.ToolWindows.DiffViewer.TextViewer();
            this._summaryViewer = new NetIde.Core.ToolWindows.DiffViewer.SummaryViewer();
            this._imageViewer = new NetIde.Core.ToolWindows.DiffViewer.ImageViewer();
            this.SuspendLayout();
            // 
            // _textViewer
            // 
            resources.ApplyResources(this._textViewer, "_textViewer");
            this._textViewer.Name = "_textViewer";
            this._textViewer.UnifiedDiffChanged += new System.EventHandler(this._textViewer_UnifiedDiffChanged);
            this._textViewer.IgnoreWhitespaceChanged += new System.EventHandler(this._textViewer_IgnoreWhitespaceChanged);
            // 
            // _summaryViewer
            // 
            resources.ApplyResources(this._summaryViewer, "_summaryViewer");
            this._summaryViewer.Name = "_summaryViewer";
            // 
            // _imageViewer
            // 
            resources.ApplyResources(this._imageViewer, "_imageViewer");
            this._imageViewer.Name = "_imageViewer";
            // 
            // DiffViewerControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._textViewer);
            this.Controls.Add(this._summaryViewer);
            this.Controls.Add(this._imageViewer);
            this.Name = "DiffViewerControl";
            this.ResumeLayout(false);

        }

        #endregion

        private ImageViewer _imageViewer;
        private SummaryViewer _summaryViewer;
        private TextViewer _textViewer;


    }
}

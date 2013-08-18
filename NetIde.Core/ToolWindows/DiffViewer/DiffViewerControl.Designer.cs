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
            this._textViewer = new NetIde.Core.ToolWindows.DiffViewer.TextViewer();
            this._summaryViewer = new NetIde.Core.ToolWindows.DiffViewer.SummaryViewer();
            this._imageViewer = new NetIde.Core.ToolWindows.DiffViewer.ImageViewer();
            this.SuspendLayout();
            // 
            // _textViewer
            // 
            this._textViewer.Location = new System.Drawing.Point(222, 44);
            this._textViewer.Name = "_textViewer";
            this._textViewer.Size = new System.Drawing.Size(191, 162);
            this._textViewer.TabIndex = 4;
            this._textViewer.UnifiedDiff = true;
            this._textViewer.UnifiedDiffChanged += new System.EventHandler(this._textViewer_UnifiedDiffChanged);
            // 
            // _summaryViewer
            // 
            this._summaryViewer.Location = new System.Drawing.Point(97, 277);
            this._summaryViewer.Name = "_summaryViewer";
            this._summaryViewer.Size = new System.Drawing.Size(166, 149);
            this._summaryViewer.TabIndex = 3;
            // 
            // _imageViewer
            // 
            this._imageViewer.Location = new System.Drawing.Point(16, 27);
            this._imageViewer.Name = "_imageViewer";
            this._imageViewer.Size = new System.Drawing.Size(159, 140);
            this._imageViewer.TabIndex = 0;
            // 
            // DiffViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._textViewer);
            this.Controls.Add(this._summaryViewer);
            this.Controls.Add(this._imageViewer);
            this.Name = "DiffViewerControl";
            this.Size = new System.Drawing.Size(608, 534);
            this.ResumeLayout(false);

        }

        #endregion

        private ImageViewer _imageViewer;
        private SummaryViewer _summaryViewer;
        private TextViewer _textViewer;


    }
}

namespace NetIde.Core.ToolWindows.DiffViewer
{
    partial class TextViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextViewer));
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._unified = new System.Windows.Forms.ToolStripButton();
            this._sideBySide = new System.Windows.Forms.ToolStripButton();
            this._ignoreWhitespace = new System.Windows.Forms.ToolStripButton();
            this._unifiedViewer = new NetIde.Core.ToolWindows.DiffViewer.UnifiedViewer();
            this._sideBySideViewer = new NetIde.Core.ToolWindows.DiffViewer.SideBySideViewer();
            this._container = new System.Windows.Forms.Panel();
            this._toolStrip.SuspendLayout();
            this._container.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            resources.ApplyResources(this._toolStrip, "_toolStrip");
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._unified,
            this._sideBySide,
            this._ignoreWhitespace});
            this._toolStrip.Name = "_toolStrip";
            // 
            // _unified
            // 
            this._unified.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this._unified, "_unified");
            this._unified.Name = "_unified";
            this._unified.Click += new System.EventHandler(this._unified_Click);
            // 
            // _sideBySide
            // 
            this._sideBySide.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this._sideBySide, "_sideBySide");
            this._sideBySide.Name = "_sideBySide";
            this._sideBySide.Click += new System.EventHandler(this._sideBySide_Click);
            // 
            // _ignoreWhitespace
            // 
            this._ignoreWhitespace.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._ignoreWhitespace.CheckOnClick = true;
            this._ignoreWhitespace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._ignoreWhitespace.Image = global::NetIde.Core.NeutralResources.IgnoreWhitespace;
            resources.ApplyResources(this._ignoreWhitespace, "_ignoreWhitespace");
            this._ignoreWhitespace.Name = "_ignoreWhitespace";
            this._ignoreWhitespace.CheckedChanged += new System.EventHandler(this._ignoreWhitespace_CheckedChanged);
            // 
            // _unifiedViewer
            // 
            this._unifiedViewer.Context = 3;
            resources.ApplyResources(this._unifiedViewer, "_unifiedViewer");
            this._unifiedViewer.Name = "_unifiedViewer";
            // 
            // _sideBySideViewer
            // 
            resources.ApplyResources(this._sideBySideViewer, "_sideBySideViewer");
            this._sideBySideViewer.Name = "_sideBySideViewer";
            this._sideBySideViewer.LeftUpdated += new System.EventHandler(this._sideBySideViewer_LeftUpdated);
            this._sideBySideViewer.RightUpdated += new System.EventHandler(this._sideBySideViewer_RightUpdated);
            this._sideBySideViewer.LeftUpdating += new System.ComponentModel.CancelEventHandler(this._sideBySideViewer_LeftUpdating);
            this._sideBySideViewer.RightUpdating += new System.ComponentModel.CancelEventHandler(this._sideBySideViewer_RightUpdating);
            // 
            // _container
            // 
            this._container.Controls.Add(this._sideBySideViewer);
            this._container.Controls.Add(this._unifiedViewer);
            resources.ApplyResources(this._container, "_container");
            this._container.Name = "_container";
            // 
            // TextViewer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._container);
            this.Controls.Add(this._toolStrip);
            this.Name = "TextViewer";
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this._container.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _unified;
        private System.Windows.Forms.ToolStripButton _sideBySide;
        private UnifiedViewer _unifiedViewer;
        private SideBySideViewer _sideBySideViewer;
        private System.Windows.Forms.Panel _container;
        private System.Windows.Forms.ToolStripButton _ignoreWhitespace;
    }
}

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
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._unified,
            this._sideBySide,
            this._ignoreWhitespace});
            this._toolStrip.Location = new System.Drawing.Point(0, 527);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(608, 25);
            this._toolStrip.TabIndex = 1;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _unified
            // 
            this._unified.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._unified.Image = ((System.Drawing.Image)(resources.GetObject("_unified.Image")));
            this._unified.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._unified.Name = "_unified";
            this._unified.Size = new System.Drawing.Size(49, 22);
            this._unified.Text = "Unified";
            this._unified.Click += new System.EventHandler(this._unified_Click);
            // 
            // _sideBySide
            // 
            this._sideBySide.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._sideBySide.Image = ((System.Drawing.Image)(resources.GetObject("_sideBySide.Image")));
            this._sideBySide.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._sideBySide.Name = "_sideBySide";
            this._sideBySide.Size = new System.Drawing.Size(73, 22);
            this._sideBySide.Text = "Side by side";
            this._sideBySide.Click += new System.EventHandler(this._sideBySide_Click);
            // 
            // _ignoreWhitespace
            // 
            this._ignoreWhitespace.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._ignoreWhitespace.CheckOnClick = true;
            this._ignoreWhitespace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._ignoreWhitespace.Image = global::NetIde.Core.NeutralResources.IgnoreWhitespace;
            this._ignoreWhitespace.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._ignoreWhitespace.Name = "_ignoreWhitespace";
            this._ignoreWhitespace.Size = new System.Drawing.Size(23, 22);
            this._ignoreWhitespace.Text = "Ignore Whitespace";
            this._ignoreWhitespace.CheckedChanged += new System.EventHandler(this._ignoreWhitespace_CheckedChanged);
            // 
            // _unifiedViewer
            // 
            this._unifiedViewer.Context = 3;
            this._unifiedViewer.Location = new System.Drawing.Point(42, 31);
            this._unifiedViewer.Name = "_unifiedViewer";
            this._unifiedViewer.Size = new System.Drawing.Size(273, 248);
            this._unifiedViewer.TabIndex = 2;
            // 
            // _sideBySideViewer
            // 
            this._sideBySideViewer.Location = new System.Drawing.Point(295, 268);
            this._sideBySideViewer.Name = "_sideBySideViewer";
            this._sideBySideViewer.Size = new System.Drawing.Size(269, 215);
            this._sideBySideViewer.TabIndex = 3;
            this._sideBySideViewer.LeftUpdated += new System.EventHandler(this._sideBySideViewer_LeftUpdated);
            this._sideBySideViewer.RightUpdated += new System.EventHandler(this._sideBySideViewer_RightUpdated);
            this._sideBySideViewer.LeftUpdating += new System.ComponentModel.CancelEventHandler(this._sideBySideViewer_LeftUpdating);
            this._sideBySideViewer.RightUpdating += new System.ComponentModel.CancelEventHandler(this._sideBySideViewer_RightUpdating);
            // 
            // _container
            // 
            this._container.Controls.Add(this._sideBySideViewer);
            this._container.Controls.Add(this._unifiedViewer);
            this._container.Dock = System.Windows.Forms.DockStyle.Fill;
            this._container.Location = new System.Drawing.Point(0, 0);
            this._container.Name = "_container";
            this._container.Size = new System.Drawing.Size(608, 527);
            this._container.TabIndex = 4;
            // 
            // TextViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._container);
            this.Controls.Add(this._toolStrip);
            this.Name = "TextViewer";
            this.Size = new System.Drawing.Size(608, 552);
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

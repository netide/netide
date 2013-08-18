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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._unified = new System.Windows.Forms.ToolStripButton();
            this._sideBySide = new System.Windows.Forms.ToolStripButton();
            this._unifiedViewer = new NetIde.Core.ToolWindows.DiffViewer.UnifiedViewer();
            this._sideBySideViewer = new NetIde.Core.ToolWindows.DiffViewer.SideBySideViewer();
            this._container = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            this._container.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._unified,
            this._sideBySide});
            this.toolStrip1.Location = new System.Drawing.Point(0, 527);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(608, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
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
            // _unifiedViewer
            // 
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
            this.Controls.Add(this.toolStrip1);
            this.Name = "TextViewer";
            this.Size = new System.Drawing.Size(608, 552);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this._container.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton _unified;
        private System.Windows.Forms.ToolStripButton _sideBySide;
        private UnifiedViewer _unifiedViewer;
        private SideBySideViewer _sideBySideViewer;
        private System.Windows.Forms.Panel _container;
    }
}

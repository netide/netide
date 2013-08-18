namespace NetIde
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._dockPanel = new NetIde.Support.DockPanel();
            this._toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this._menuStrip = new System.Windows.Forms.MenuStrip();
            this._toolStripContainer.ContentPanel.SuspendLayout();
            this._toolStripContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // _dockPanel
            // 
            this._dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this._dockPanel.Location = new System.Drawing.Point(0, 0);
            this._dockPanel.Name = "_dockPanel";
            this._dockPanel.Size = new System.Drawing.Size(855, 595);
            this._dockPanel.TabIndex = 0;
            this._dockPanel.ActiveDocumentChanged += new System.EventHandler(this._dockPanel_ActiveDocumentChanged);
            // 
            // _toolStripContainer
            // 
            // 
            // _toolStripContainer.ContentPanel
            // 
            this._toolStripContainer.ContentPanel.Controls.Add(this._dockPanel);
            this._toolStripContainer.ContentPanel.Size = new System.Drawing.Size(855, 595);
            this._toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._toolStripContainer.Location = new System.Drawing.Point(0, 24);
            this._toolStripContainer.Name = "_toolStripContainer";
            this._toolStripContainer.Size = new System.Drawing.Size(855, 620);
            this._toolStripContainer.TabIndex = 1;
            this._toolStripContainer.Text = "toolStripContainer1";
            // 
            // _menuStrip
            // 
            this._menuStrip.Location = new System.Drawing.Point(0, 0);
            this._menuStrip.Name = "_menuStrip";
            this._menuStrip.Size = new System.Drawing.Size(855, 24);
            this._menuStrip.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 644);
            this.Controls.Add(this._toolStripContainer);
            this.Controls.Add(this._menuStrip);
            this.Name = "MainForm";
            this.Text = "Net IDE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this._toolStripContainer.ContentPanel.ResumeLayout(false);
            this._toolStripContainer.ResumeLayout(false);
            this._toolStripContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NetIde.Support.DockPanel _dockPanel;
        private System.Windows.Forms.ToolStripContainer _toolStripContainer;
        private System.Windows.Forms.MenuStrip _menuStrip;

    }
}


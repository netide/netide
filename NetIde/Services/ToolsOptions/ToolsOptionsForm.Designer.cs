namespace NetIde.Services.ToolsOptions
{
    partial class ToolsOptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolsOptionsForm));
            this.formFooter1 = new NetIde.Util.Forms.FormFooter();
            this._cancelButton = new System.Windows.Forms.Button();
            this._acceptButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._treeView = new System.Windows.Forms.TreeView();
            this.pageContainer1 = new NetIde.Services.ToolsOptions.PageContainer();
            this._pageHost = new NetIde.Services.ToolsOptions.PageHost();
            this.formFooter1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pageContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFooter1
            // 
            this.formFooter1.Controls.Add(this._cancelButton);
            this.formFooter1.Controls.Add(this._acceptButton);
            resources.ApplyResources(this.formFooter1, "formFooter1");
            this.formFooter1.Name = "formFooter1";
            this.formFooter1.Style = NetIde.Util.Forms.FormFooterStyle.Dialog;
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this._cancelButton, "_cancelButton");
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _acceptButton
            // 
            resources.ApplyResources(this._acceptButton, "_acceptButton");
            this._acceptButton.Name = "_acceptButton";
            this._acceptButton.UseVisualStyleBackColor = true;
            this._acceptButton.Click += new System.EventHandler(this._acceptButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this._treeView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pageContainer1, 2, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // _treeView
            // 
            resources.ApplyResources(this._treeView, "_treeView");
            this._treeView.HideSelection = false;
            this._treeView.Name = "_treeView";
            this._treeView.ShowLines = false;
            this._treeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this._treeView_BeforeSelect);
            // 
            // pageContainer1
            // 
            this.pageContainer1.Controls.Add(this._pageHost);
            resources.ApplyResources(this.pageContainer1, "pageContainer1");
            this.pageContainer1.Name = "pageContainer1";
            // 
            // _pageHost
            // 
            resources.ApplyResources(this._pageHost, "_pageHost");
            this._pageHost.Name = "_pageHost";
            // 
            // ToolsOptionsForm
            // 
            this.AcceptButton = this._acceptButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.formFooter1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "ToolsOptionsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ToolsOptionsForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ToolsOptionsForm_FormClosed);
            this.Load += new System.EventHandler(this.ToolsOptionsDialog_Load);
            this.formFooter1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pageContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Util.Forms.FormFooter formFooter1;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _acceptButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TreeView _treeView;
        private PageContainer pageContainer1;
        private PageHost _pageHost;
    }
}
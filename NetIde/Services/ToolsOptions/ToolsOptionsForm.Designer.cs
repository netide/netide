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
            this.formFooter1.Location = new System.Drawing.Point(0, 365);
            this.formFooter1.Name = "formFooter1";
            this.formFooter1.Size = new System.Drawing.Size(741, 37);
            this.formFooter1.Style = NetIde.Util.Forms.FormFooterStyle.Dialog;
            this.formFooter1.TabIndex = 1;
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._cancelButton.Location = new System.Drawing.Point(655, 3);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 1;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _acceptButton
            // 
            this._acceptButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._acceptButton.Location = new System.Drawing.Point(574, 3);
            this._acceptButton.Name = "_acceptButton";
            this._acceptButton.Size = new System.Drawing.Size(75, 23);
            this._acceptButton.TabIndex = 0;
            this._acceptButton.Text = "OK";
            this._acceptButton.UseVisualStyleBackColor = true;
            this._acceptButton.Click += new System.EventHandler(this._acceptButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(9);
            this.panel1.Size = new System.Drawing.Size(741, 365);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 248F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this._treeView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pageContainer1, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 9);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(723, 347);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _treeView
            // 
            this._treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._treeView.HideSelection = false;
            this._treeView.Location = new System.Drawing.Point(3, 3);
            this._treeView.Name = "_treeView";
            this._treeView.Size = new System.Drawing.Size(242, 341);
            this._treeView.TabIndex = 0;
            this._treeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this._treeView_BeforeSelect);
            // 
            // pageContainer1
            // 
            this.pageContainer1.Controls.Add(this._pageHost);
            this.pageContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageContainer1.Location = new System.Drawing.Point(263, 3);
            this.pageContainer1.Name = "pageContainer1";
            this.pageContainer1.Size = new System.Drawing.Size(457, 341);
            this.pageContainer1.TabIndex = 1;
            // 
            // _pageHost
            // 
            this._pageHost.AcceptsArrows = true;
            this._pageHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pageHost.Location = new System.Drawing.Point(0, 0);
            this._pageHost.Name = "_pageHost";
            this._pageHost.Page = null;
            this._pageHost.Size = new System.Drawing.Size(457, 335);
            this._pageHost.TabIndex = 0;
            this._pageHost.Text = "pageHost1";
            // 
            // ToolsOptionsForm
            // 
            this.AcceptButton = this._acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(741, 402);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.formFooter1);
            this.Name = "ToolsOptionsForm";
            this.Text = "Options";
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
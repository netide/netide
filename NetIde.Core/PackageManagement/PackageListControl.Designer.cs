namespace NetIde.Core.PackageManagement
{
    partial class PackageListControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._pager = new System.Windows.Forms.FlowLayoutPanel();
            this._leftButton = new NetIde.Core.PackageManagement.PackagePageButton();
            this._page1Button = new NetIde.Core.PackageManagement.PackagePageButton();
            this._page2Button = new NetIde.Core.PackageManagement.PackagePageButton();
            this._page3Button = new NetIde.Core.PackageManagement.PackagePageButton();
            this._page4Button = new NetIde.Core.PackageManagement.PackagePageButton();
            this._page5Button = new NetIde.Core.PackageManagement.PackagePageButton();
            this._rightButton = new NetIde.Core.PackageManagement.PackagePageButton();
            this._toolbar = new System.Windows.Forms.TableLayoutPanel();
            this._packageStability = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this._sortBy = new System.Windows.Forms.ComboBox();
            this._container = new System.Windows.Forms.Panel();
            this._packages = new NetIde.Core.PackageManagement.PackageListContainer();
            this._loadingControl = new NetIde.Core.PackageManagement.LoadingControl();
            this._restartPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this._restartButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this._pager.SuspendLayout();
            this._toolbar.SuspendLayout();
            this._container.SuspendLayout();
            this._restartPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this._pager, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this._toolbar, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._container, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this._restartPanel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(471, 401);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _pager
            // 
            this._pager.AutoSize = true;
            this._pager.Controls.Add(this._leftButton);
            this._pager.Controls.Add(this._page1Button);
            this._pager.Controls.Add(this._page2Button);
            this._pager.Controls.Add(this._page3Button);
            this._pager.Controls.Add(this._page4Button);
            this._pager.Controls.Add(this._page5Button);
            this._pager.Controls.Add(this._rightButton);
            this._pager.Location = new System.Drawing.Point(179, 376);
            this._pager.Margin = new System.Windows.Forms.Padding(0);
            this._pager.Name = "_pager";
            this._pager.Size = new System.Drawing.Size(113, 25);
            this._pager.TabIndex = 1;
            this._pager.WrapContents = false;
            // 
            // _leftButton
            // 
            this._leftButton.Image = global::NetIde.Core.NeutralResources.PageLeft;
            this._leftButton.Location = new System.Drawing.Point(0, 5);
            this._leftButton.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this._leftButton.Name = "_leftButton";
            this._leftButton.Size = new System.Drawing.Size(14, 15);
            this._leftButton.TabIndex = 0;
            this._leftButton.Click += new System.EventHandler(this._leftButton_Click);
            // 
            // _page1Button
            // 
            this._page1Button.Image = null;
            this._page1Button.Location = new System.Drawing.Point(14, 5);
            this._page1Button.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this._page1Button.Name = "_page1Button";
            this._page1Button.Size = new System.Drawing.Size(17, 15);
            this._page1Button.TabIndex = 2;
            this._page1Button.Text = "1";
            this._page1Button.Click += new System.EventHandler(this._pageButton_Click);
            // 
            // _page2Button
            // 
            this._page2Button.Image = null;
            this._page2Button.Location = new System.Drawing.Point(31, 5);
            this._page2Button.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this._page2Button.Name = "_page2Button";
            this._page2Button.Size = new System.Drawing.Size(17, 15);
            this._page2Button.TabIndex = 3;
            this._page2Button.Text = "2";
            this._page2Button.Click += new System.EventHandler(this._pageButton_Click);
            // 
            // _page3Button
            // 
            this._page3Button.Image = null;
            this._page3Button.Location = new System.Drawing.Point(48, 5);
            this._page3Button.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this._page3Button.Name = "_page3Button";
            this._page3Button.Size = new System.Drawing.Size(17, 15);
            this._page3Button.TabIndex = 4;
            this._page3Button.Text = "3";
            this._page3Button.Click += new System.EventHandler(this._pageButton_Click);
            // 
            // _page4Button
            // 
            this._page4Button.Image = null;
            this._page4Button.Location = new System.Drawing.Point(65, 5);
            this._page4Button.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this._page4Button.Name = "_page4Button";
            this._page4Button.Size = new System.Drawing.Size(17, 15);
            this._page4Button.TabIndex = 5;
            this._page4Button.Text = "4";
            this._page4Button.Click += new System.EventHandler(this._pageButton_Click);
            // 
            // _page5Button
            // 
            this._page5Button.Image = null;
            this._page5Button.Location = new System.Drawing.Point(82, 5);
            this._page5Button.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this._page5Button.Name = "_page5Button";
            this._page5Button.Size = new System.Drawing.Size(17, 15);
            this._page5Button.TabIndex = 6;
            this._page5Button.Text = "5";
            this._page5Button.Click += new System.EventHandler(this._pageButton_Click);
            // 
            // _rightButton
            // 
            this._rightButton.Image = global::NetIde.Core.NeutralResources.PageRight;
            this._rightButton.Location = new System.Drawing.Point(99, 5);
            this._rightButton.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this._rightButton.Name = "_rightButton";
            this._rightButton.Size = new System.Drawing.Size(14, 15);
            this._rightButton.TabIndex = 1;
            this._rightButton.Click += new System.EventHandler(this._rightButton_Click);
            // 
            // _toolbar
            // 
            this._toolbar.AutoSize = true;
            this._toolbar.BackColor = System.Drawing.SystemColors.Control;
            this._toolbar.ColumnCount = 4;
            this.tableLayoutPanel1.SetColumnSpan(this._toolbar, 3);
            this._toolbar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this._toolbar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._toolbar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this._toolbar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._toolbar.Controls.Add(this._packageStability, 0, 0);
            this._toolbar.Controls.Add(this.label1, 1, 0);
            this._toolbar.Controls.Add(this._sortBy, 2, 0);
            this._toolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this._toolbar.Location = new System.Drawing.Point(0, 35);
            this._toolbar.Margin = new System.Windows.Forms.Padding(0);
            this._toolbar.Name = "_toolbar";
            this._toolbar.RowCount = 1;
            this._toolbar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._toolbar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this._toolbar.Size = new System.Drawing.Size(471, 27);
            this._toolbar.TabIndex = 2;
            // 
            // _packageStability
            // 
            this._packageStability.Dock = System.Windows.Forms.DockStyle.Fill;
            this._packageStability.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._packageStability.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._packageStability.FormattingEnabled = true;
            this._packageStability.Location = new System.Drawing.Point(3, 3);
            this._packageStability.Name = "_packageStability";
            this._packageStability.Size = new System.Drawing.Size(144, 21);
            this._packageStability.TabIndex = 0;
            this._packageStability.SelectedIndexChanged += new System.EventHandler(this._packageStability_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(153, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 27);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sort by:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _sortBy
            // 
            this._sortBy.Dock = System.Windows.Forms.DockStyle.Fill;
            this._sortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._sortBy.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._sortBy.FormattingEnabled = true;
            this._sortBy.Location = new System.Drawing.Point(202, 3);
            this._sortBy.Name = "_sortBy";
            this._sortBy.Size = new System.Drawing.Size(144, 21);
            this._sortBy.TabIndex = 2;
            this._sortBy.SelectedIndexChanged += new System.EventHandler(this._sortBy_SelectedIndexChanged);
            // 
            // _container
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._container, 3);
            this._container.Controls.Add(this._packages);
            this._container.Controls.Add(this._loadingControl);
            this._container.Dock = System.Windows.Forms.DockStyle.Fill;
            this._container.Location = new System.Drawing.Point(0, 62);
            this._container.Margin = new System.Windows.Forms.Padding(0);
            this._container.Name = "_container";
            this._container.Size = new System.Drawing.Size(471, 314);
            this._container.TabIndex = 3;
            // 
            // _packages
            // 
            this._packages.AutoScroll = true;
            this._packages.Location = new System.Drawing.Point(53, 67);
            this._packages.Name = "_packages";
            this._packages.Size = new System.Drawing.Size(94, 104);
            this._packages.TabIndex = 2;
            this._packages.Paint += new System.Windows.Forms.PaintEventHandler(this._packages_Paint);
            // 
            // _loadingControl
            // 
            this._loadingControl.BackColor = System.Drawing.SystemColors.Window;
            this._loadingControl.Location = new System.Drawing.Point(199, 85);
            this._loadingControl.Margin = new System.Windows.Forms.Padding(0);
            this._loadingControl.Name = "_loadingControl";
            this._loadingControl.Size = new System.Drawing.Size(226, 184);
            this._loadingControl.TabIndex = 1;
            // 
            // _restartPanel
            // 
            this._restartPanel.AutoSize = true;
            this._restartPanel.BackColor = System.Drawing.SystemColors.Info;
            this._restartPanel.ColumnCount = 2;
            this.tableLayoutPanel1.SetColumnSpan(this._restartPanel, 3);
            this._restartPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._restartPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._restartPanel.Controls.Add(this.label2, 0, 0);
            this._restartPanel.Controls.Add(this._restartButton, 1, 0);
            this._restartPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._restartPanel.Location = new System.Drawing.Point(0, 0);
            this._restartPanel.Margin = new System.Windows.Forms.Padding(0);
            this._restartPanel.Name = "_restartPanel";
            this._restartPanel.RowCount = 1;
            this._restartPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._restartPanel.Size = new System.Drawing.Size(471, 35);
            this._restartPanel.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(6, 6);
            this.label2.Margin = new System.Windows.Forms.Padding(6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(339, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "You must restart the application in order for the changes to take effect.";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _restartButton
            // 
            this._restartButton.Location = new System.Drawing.Point(368, 6);
            this._restartButton.Margin = new System.Windows.Forms.Padding(6);
            this._restartButton.Name = "_restartButton";
            this._restartButton.Size = new System.Drawing.Size(97, 23);
            this._restartButton.TabIndex = 1;
            this._restartButton.Text = "Restart Now";
            this._restartButton.UseVisualStyleBackColor = true;
            this._restartButton.Click += new System.EventHandler(this._restartButton_Click);
            // 
            // PackageListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PackageListControl";
            this.Size = new System.Drawing.Size(471, 401);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this._pager.ResumeLayout(false);
            this._pager.PerformLayout();
            this._toolbar.ResumeLayout(false);
            this._toolbar.PerformLayout();
            this._container.ResumeLayout(false);
            this._restartPanel.ResumeLayout(false);
            this._restartPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel _pager;
        private System.Windows.Forms.TableLayoutPanel _toolbar;
        private System.Windows.Forms.ComboBox _packageStability;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox _sortBy;
        private PackagePageButton _leftButton;
        private PackagePageButton _page1Button;
        private PackagePageButton _page2Button;
        private PackagePageButton _page3Button;
        private PackagePageButton _page4Button;
        private PackagePageButton _page5Button;
        private PackagePageButton _rightButton;
        private System.Windows.Forms.Panel _container;
        private LoadingControl _loadingControl;
        private PackageListContainer _packages;
        private System.Windows.Forms.TableLayoutPanel _restartPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button _restartButton;
    }
}

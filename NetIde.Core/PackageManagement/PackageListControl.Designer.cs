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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageListControl));
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
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this._pager, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this._toolbar, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._container, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this._restartPanel, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // _pager
            // 
            resources.ApplyResources(this._pager, "_pager");
            this._pager.Controls.Add(this._leftButton);
            this._pager.Controls.Add(this._page1Button);
            this._pager.Controls.Add(this._page2Button);
            this._pager.Controls.Add(this._page3Button);
            this._pager.Controls.Add(this._page4Button);
            this._pager.Controls.Add(this._page5Button);
            this._pager.Controls.Add(this._rightButton);
            this._pager.Name = "_pager";
            // 
            // _leftButton
            // 
            this._leftButton.Image = global::NetIde.Core.NeutralResources.PageLeft;
            resources.ApplyResources(this._leftButton, "_leftButton");
            this._leftButton.Name = "_leftButton";
            this._leftButton.Click += new System.EventHandler(this._leftButton_Click);
            // 
            // _page1Button
            // 
            this._page1Button.Image = null;
            resources.ApplyResources(this._page1Button, "_page1Button");
            this._page1Button.Name = "_page1Button";
            this._page1Button.Click += new System.EventHandler(this._pageButton_Click);
            // 
            // _page2Button
            // 
            this._page2Button.Image = null;
            resources.ApplyResources(this._page2Button, "_page2Button");
            this._page2Button.Name = "_page2Button";
            this._page2Button.Click += new System.EventHandler(this._pageButton_Click);
            // 
            // _page3Button
            // 
            this._page3Button.Image = null;
            resources.ApplyResources(this._page3Button, "_page3Button");
            this._page3Button.Name = "_page3Button";
            this._page3Button.Click += new System.EventHandler(this._pageButton_Click);
            // 
            // _page4Button
            // 
            this._page4Button.Image = null;
            resources.ApplyResources(this._page4Button, "_page4Button");
            this._page4Button.Name = "_page4Button";
            this._page4Button.Click += new System.EventHandler(this._pageButton_Click);
            // 
            // _page5Button
            // 
            this._page5Button.Image = null;
            resources.ApplyResources(this._page5Button, "_page5Button");
            this._page5Button.Name = "_page5Button";
            this._page5Button.Click += new System.EventHandler(this._pageButton_Click);
            // 
            // _rightButton
            // 
            this._rightButton.Image = global::NetIde.Core.NeutralResources.PageRight;
            resources.ApplyResources(this._rightButton, "_rightButton");
            this._rightButton.Name = "_rightButton";
            this._rightButton.Click += new System.EventHandler(this._rightButton_Click);
            // 
            // _toolbar
            // 
            resources.ApplyResources(this._toolbar, "_toolbar");
            this._toolbar.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.SetColumnSpan(this._toolbar, 3);
            this._toolbar.Controls.Add(this._packageStability, 0, 0);
            this._toolbar.Controls.Add(this.label1, 1, 0);
            this._toolbar.Controls.Add(this._sortBy, 2, 0);
            this._toolbar.Name = "_toolbar";
            // 
            // _packageStability
            // 
            resources.ApplyResources(this._packageStability, "_packageStability");
            this._packageStability.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._packageStability.FormattingEnabled = true;
            this._packageStability.Name = "_packageStability";
            this._packageStability.SelectedIndexChanged += new System.EventHandler(this._packageStability_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // _sortBy
            // 
            resources.ApplyResources(this._sortBy, "_sortBy");
            this._sortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._sortBy.FormattingEnabled = true;
            this._sortBy.Name = "_sortBy";
            this._sortBy.SelectedIndexChanged += new System.EventHandler(this._sortBy_SelectedIndexChanged);
            // 
            // _container
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._container, 3);
            this._container.Controls.Add(this._packages);
            this._container.Controls.Add(this._loadingControl);
            resources.ApplyResources(this._container, "_container");
            this._container.Name = "_container";
            // 
            // _packages
            // 
            resources.ApplyResources(this._packages, "_packages");
            this._packages.Name = "_packages";
            this._packages.Paint += new System.Windows.Forms.PaintEventHandler(this._packages_Paint);
            // 
            // _loadingControl
            // 
            this._loadingControl.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this._loadingControl, "_loadingControl");
            this._loadingControl.Name = "_loadingControl";
            // 
            // _restartPanel
            // 
            resources.ApplyResources(this._restartPanel, "_restartPanel");
            this._restartPanel.BackColor = System.Drawing.SystemColors.Info;
            this.tableLayoutPanel1.SetColumnSpan(this._restartPanel, 3);
            this._restartPanel.Controls.Add(this.label2, 0, 0);
            this._restartPanel.Controls.Add(this._restartButton, 1, 0);
            this._restartPanel.Name = "_restartPanel";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // _restartButton
            // 
            resources.ApplyResources(this._restartButton, "_restartButton");
            this._restartButton.Name = "_restartButton";
            this._restartButton.UseVisualStyleBackColor = true;
            this._restartButton.Click += new System.EventHandler(this._restartButton_Click);
            // 
            // PackageListControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PackageListControl";
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

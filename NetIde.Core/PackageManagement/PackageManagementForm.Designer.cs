namespace NetIde.Core.PackageManagement
{
    partial class PackageManagementForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageManagementForm));
            this.formFooter1 = new NetIde.Util.Forms.FormFooter();
            this._cancelButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._packageDetails = new NetIde.Core.PackageManagement.PackageDetailsControl();
            this._selector = new System.Windows.Forms.ListBox();
            this._container = new System.Windows.Forms.Panel();
            this._packageList = new NetIde.Core.PackageManagement.PackageListControl();
            this.formFooter1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this._container.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFooter1
            // 
            this.formFooter1.BackColor = System.Drawing.SystemColors.Window;
            this.formFooter1.Controls.Add(this._cancelButton);
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
            this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this._packageDetails, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this._selector, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._container, 1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // _packageDetails
            // 
            resources.ApplyResources(this._packageDetails, "_packageDetails");
            this._packageDetails.Name = "_packageDetails";
            this._packageDetails.Package = null;
            // 
            // _selector
            // 
            this._selector.BackColor = System.Drawing.SystemColors.Control;
            this._selector.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this._selector, "_selector");
            this._selector.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this._selector.FormattingEnabled = true;
            this._selector.Name = "_selector";
            this._selector.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this._selector_DrawItem);
            this._selector.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this._selector_MeasureItem);
            this._selector.SelectedIndexChanged += new System.EventHandler(this._selector_SelectedIndexChanged);
            // 
            // _container
            // 
            this._container.Controls.Add(this._packageList);
            resources.ApplyResources(this._container, "_container");
            this._container.Name = "_container";
            // 
            // _packageList
            // 
            this._packageList.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this._packageList, "_packageList");
            this._packageList.Loading = true;
            this._packageList.Name = "_packageList";
            this._packageList.NoResultsText = null;
            this._packageList.RestartPending = false;
            this._packageList.SelectedPage = 0;
            this._packageList.ShowToolbar = true;
            this._packageList.SelectedPackageChanged += new System.EventHandler(this._packageList_SelectedPackageChanged);
            this._packageList.QueryParametersChanged += new System.EventHandler(this._packageList_QueryParametersChanged);
            this._packageList.PackageButtonClick += new NetIde.Core.PackageManagement.PackageControlButtonEventHandler(this._packageList_PackageButtonClick);
            this._packageList.RestartClick += new System.EventHandler(this._packageList_RestartClick);
            // 
            // PackageManagementForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.formFooter1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PackageManagementForm";
            this.Load += new System.EventHandler(this.PackageManagementForm_Load);
            this.formFooter1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this._container.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Util.Forms.FormFooter formFooter1;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private PackageDetailsControl _packageDetails;
        private System.Windows.Forms.ListBox _selector;
        private System.Windows.Forms.Panel _container;
        private PackageListControl _packageList;
    }
}
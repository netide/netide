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
            this.formFooter1.Location = new System.Drawing.Point(0, 365);
            this.formFooter1.Name = "formFooter1";
            this.formFooter1.Size = new System.Drawing.Size(784, 46);
            this.formFooter1.Style = NetIde.Util.Forms.FormFooterStyle.Dialog;
            this.formFooter1.TabIndex = 1;
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(698, 12);
            this._cancelButton.Margin = new System.Windows.Forms.Padding(3, 12, 3, 3);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 0;
            this._cancelButton.Text = "&Close";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.Controls.Add(this._packageDetails, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this._selector, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._container, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 365);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _packageDetails
            // 
            this._packageDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this._packageDetails.Location = new System.Drawing.Point(534, 0);
            this._packageDetails.Margin = new System.Windows.Forms.Padding(0);
            this._packageDetails.Name = "_packageDetails";
            this._packageDetails.Package = null;
            this._packageDetails.Padding = new System.Windows.Forms.Padding(6);
            this._packageDetails.Size = new System.Drawing.Size(250, 365);
            this._packageDetails.TabIndex = 2;
            // 
            // _selector
            // 
            this._selector.BackColor = System.Drawing.SystemColors.Control;
            this._selector.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._selector.Dock = System.Windows.Forms.DockStyle.Fill;
            this._selector.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this._selector.FormattingEnabled = true;
            this._selector.IntegralHeight = false;
            this._selector.Location = new System.Drawing.Point(0, 0);
            this._selector.Margin = new System.Windows.Forms.Padding(0);
            this._selector.Name = "_selector";
            this._selector.Size = new System.Drawing.Size(180, 365);
            this._selector.TabIndex = 0;
            this._selector.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this._selector_DrawItem);
            this._selector.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this._selector_MeasureItem);
            this._selector.SelectedIndexChanged += new System.EventHandler(this._selector_SelectedIndexChanged);
            // 
            // _container
            // 
            this._container.Controls.Add(this._packageList);
            this._container.Dock = System.Windows.Forms.DockStyle.Fill;
            this._container.Location = new System.Drawing.Point(180, 0);
            this._container.Margin = new System.Windows.Forms.Padding(0);
            this._container.Name = "_container";
            this._container.Size = new System.Drawing.Size(354, 365);
            this._container.TabIndex = 1;
            // 
            // _packageList
            // 
            this._packageList.BackColor = System.Drawing.SystemColors.Window;
            this._packageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._packageList.Loading = true;
            this._packageList.Location = new System.Drawing.Point(0, 0);
            this._packageList.Margin = new System.Windows.Forms.Padding(0);
            this._packageList.Name = "_packageList";
            this._packageList.NoResultsText = null;
            this._packageList.RestartPending = false;
            this._packageList.SelectedPage = 0;
            this._packageList.ShowToolbar = true;
            this._packageList.Size = new System.Drawing.Size(354, 365);
            this._packageList.TabIndex = 1;
            this._packageList.SelectedPackageChanged += new System.EventHandler(this._packageList_SelectedPackageChanged);
            this._packageList.QueryParametersChanged += new System.EventHandler(this._packageList_QueryParametersChanged);
            this._packageList.PackageButtonClick += new NetIde.Core.PackageManagement.PackageControlButtonEventHandler(this._packageList_PackageButtonClick);
            this._packageList.RestartClick += new System.EventHandler(this._packageList_RestartClick);
            // 
            // PackageManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(784, 411);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.formFooter1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 450);
            this.Name = "PackageManagementForm";
            this.Text = "Extensions and Updates";
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
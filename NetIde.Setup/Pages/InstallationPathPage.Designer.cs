namespace NetIde.Setup.Pages
{
    partial class InstallationPathPage
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._introduction = new System.Windows.Forms.Label();
            this._destination = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this._targetPath = new System.Windows.Forms.TextBox();
            this._browse = new System.Windows.Forms.Button();
            this._formHeader = new NetIde.Util.Forms.FormHeader();
            this.formFooter1 = new NetIde.Setup.FormFooter();
            this._cancelButton = new System.Windows.Forms.Button();
            this._acceptButton = new System.Windows.Forms.Button();
            this._previousButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this._destination.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.formFooter1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 47);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(12);
            this.panel1.Size = new System.Drawing.Size(484, 262);
            this.panel1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this._introduction, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._destination, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(460, 238);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _introduction
            // 
            this._introduction.AutoSize = true;
            this._introduction.Location = new System.Drawing.Point(3, 0);
            this._introduction.Name = "_introduction";
            this._introduction.Size = new System.Drawing.Size(428, 26);
            this._introduction.TabIndex = 0;
            this._introduction.Text = "Setup will install {0} in the following folder. To install in a different folder," +
    " click Browse and select another folder. Click Next to continue.";
            // 
            // _destination
            // 
            this._destination.AutoSize = true;
            this._destination.Controls.Add(this.tableLayoutPanel2);
            this._destination.Dock = System.Windows.Forms.DockStyle.Fill;
            this._destination.Location = new System.Drawing.Point(3, 162);
            this._destination.Margin = new System.Windows.Forms.Padding(3, 14, 3, 14);
            this._destination.Name = "_destination";
            this._destination.Padding = new System.Windows.Forms.Padding(12, 8, 12, 12);
            this._destination.Size = new System.Drawing.Size(454, 62);
            this._destination.TabIndex = 1;
            this._destination.TabStop = false;
            this._destination.Text = "&Destination Folder";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this._targetPath, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this._browse, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(12, 21);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(430, 29);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // _targetPath
            // 
            this._targetPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this._targetPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this._targetPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this._targetPath.Location = new System.Drawing.Point(3, 4);
            this._targetPath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._targetPath.Name = "_targetPath";
            this._targetPath.Size = new System.Drawing.Size(332, 20);
            this._targetPath.TabIndex = 0;
            this._targetPath.TextChanged += new System.EventHandler(this._targetPath_TextChanged);
            // 
            // _browse
            // 
            this._browse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._browse.Location = new System.Drawing.Point(352, 3);
            this._browse.Margin = new System.Windows.Forms.Padding(14, 3, 3, 3);
            this._browse.Name = "_browse";
            this._browse.Size = new System.Drawing.Size(75, 23);
            this._browse.TabIndex = 1;
            this._browse.Text = "Browse&...";
            this._browse.UseVisualStyleBackColor = true;
            this._browse.Click += new System.EventHandler(this._browse_Click);
            // 
            // _formHeader
            // 
            this._formHeader.Location = new System.Drawing.Point(0, 0);
            this._formHeader.Name = "_formHeader";
            this._formHeader.Size = new System.Drawing.Size(484, 47);
            this._formHeader.SubText = "Choose the folder in which to install {0}.";
            this._formHeader.TabIndex = 3;
            this._formHeader.Text = "Choose Install Location";
            // 
            // formFooter1
            // 
            this.formFooter1.Controls.Add(this._cancelButton);
            this.formFooter1.Controls.Add(this._acceptButton);
            this.formFooter1.Controls.Add(this._previousButton);
            this.formFooter1.Location = new System.Drawing.Point(0, 309);
            this.formFooter1.Name = "formFooter1";
            this.formFooter1.Size = new System.Drawing.Size(484, 53);
            this.formFooter1.TabIndex = 4;
            // 
            // _cancelButton
            // 
            this._cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._cancelButton.Location = new System.Drawing.Point(397, 20);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 2;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
            // 
            // _acceptButton
            // 
            this._acceptButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._acceptButton.Location = new System.Drawing.Point(316, 20);
            this._acceptButton.Name = "_acceptButton";
            this._acceptButton.Size = new System.Drawing.Size(75, 23);
            this._acceptButton.TabIndex = 1;
            this._acceptButton.Text = "&Next >";
            this._acceptButton.UseVisualStyleBackColor = true;
            this._acceptButton.Click += new System.EventHandler(this._acceptButton_Click);
            // 
            // _previousButton
            // 
            this._previousButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._previousButton.Location = new System.Drawing.Point(235, 20);
            this._previousButton.Name = "_previousButton";
            this._previousButton.Size = new System.Drawing.Size(75, 23);
            this._previousButton.TabIndex = 0;
            this._previousButton.Text = "< &Previous";
            this._previousButton.UseVisualStyleBackColor = true;
            this._previousButton.Click += new System.EventHandler(this._previousButton_Click);
            // 
            // InstallationPathPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.formFooter1);
            this.Controls.Add(this._formHeader);
            this.Name = "InstallationPathPage";
            this.Size = new System.Drawing.Size(484, 362);
            this.Load += new System.EventHandler(this.InstallationPathPage_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this._destination.ResumeLayout(false);
            this._destination.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.formFooter1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label _introduction;
        private System.Windows.Forms.GroupBox _destination;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox _targetPath;
        private System.Windows.Forms.Button _browse;
        private Util.Forms.FormHeader _formHeader;
        private FormFooter formFooter1;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _acceptButton;
        private System.Windows.Forms.Button _previousButton;
    }
}

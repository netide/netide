namespace NetIde.Setup.Pages
{
    partial class StartMenuPage
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
            this._container = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this._startMenuFolder = new System.Windows.Forms.TextBox();
            this._startMenuFolders = new System.Windows.Forms.ListBox();
            this._createStartMenu = new System.Windows.Forms.CheckBox();
            this._createOnDesktop = new System.Windows.Forms.CheckBox();
            this.formFooter1 = new NetIde.Setup.FormFooter();
            this._cancelButton = new System.Windows.Forms.Button();
            this._acceptButton = new System.Windows.Forms.Button();
            this._previousButton = new System.Windows.Forms.Button();
            this._formHeader = new NetIde.Util.Forms.FormHeader();
            this.panel1.SuspendLayout();
            this._container.SuspendLayout();
            this.formFooter1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._container);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 47);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(12);
            this.panel1.Size = new System.Drawing.Size(484, 262);
            this.panel1.TabIndex = 1;
            // 
            // _container
            // 
            this._container.ColumnCount = 1;
            this._container.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._container.Controls.Add(this.label1, 0, 0);
            this._container.Controls.Add(this._startMenuFolder, 0, 1);
            this._container.Controls.Add(this._startMenuFolders, 0, 2);
            this._container.Controls.Add(this._createStartMenu, 0, 3);
            this._container.Controls.Add(this._createOnDesktop, 0, 4);
            this._container.Dock = System.Windows.Forms.DockStyle.Fill;
            this._container.Location = new System.Drawing.Point(12, 12);
            this._container.Name = "_container";
            this._container.RowCount = 5;
            this._container.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._container.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._container.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._container.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._container.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._container.Size = new System.Drawing.Size(460, 238);
            this._container.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(452, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Select the Start Menu folder in which you would like to create the program\'s sho" +
    "rtcut. You can also enter a name to create a new folder.";
            // 
            // _startMenuFolder
            // 
            this._startMenuFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this._startMenuFolder.Location = new System.Drawing.Point(3, 47);
            this._startMenuFolder.Name = "_startMenuFolder";
            this._startMenuFolder.Size = new System.Drawing.Size(454, 20);
            this._startMenuFolder.TabIndex = 1;
            this._startMenuFolder.TextChanged += new System.EventHandler(this._startMenuFolder_TextChanged);
            // 
            // _startMenuFolders
            // 
            this._startMenuFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this._startMenuFolders.FormattingEnabled = true;
            this._startMenuFolders.IntegralHeight = false;
            this._startMenuFolders.Location = new System.Drawing.Point(3, 73);
            this._startMenuFolders.Name = "_startMenuFolders";
            this._startMenuFolders.Size = new System.Drawing.Size(454, 114);
            this._startMenuFolders.TabIndex = 2;
            // 
            // _createStartMenu
            // 
            this._createStartMenu.AutoSize = true;
            this._createStartMenu.Checked = true;
            this._createStartMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this._createStartMenu.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._createStartMenu.Location = new System.Drawing.Point(3, 193);
            this._createStartMenu.Name = "_createStartMenu";
            this._createStartMenu.Size = new System.Drawing.Size(164, 18);
            this._createStartMenu.TabIndex = 3;
            this._createStartMenu.Text = "&Create Start Menu shortcuts";
            this._createStartMenu.UseVisualStyleBackColor = true;
            this._createStartMenu.CheckedChanged += new System.EventHandler(this._createStartMenu_CheckedChanged);
            // 
            // _createOnDesktop
            // 
            this._createOnDesktop.AutoSize = true;
            this._createOnDesktop.Checked = true;
            this._createOnDesktop.CheckState = System.Windows.Forms.CheckState.Checked;
            this._createOnDesktop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._createOnDesktop.Location = new System.Drawing.Point(3, 217);
            this._createOnDesktop.Name = "_createOnDesktop";
            this._createOnDesktop.Size = new System.Drawing.Size(180, 18);
            this._createOnDesktop.TabIndex = 4;
            this._createOnDesktop.Text = "Create shortcut on the &Desktop";
            this._createOnDesktop.UseVisualStyleBackColor = true;
            this._createOnDesktop.CheckedChanged += new System.EventHandler(this._createOnDesktop_CheckedChanged);
            // 
            // formFooter1
            // 
            this.formFooter1.Controls.Add(this._cancelButton);
            this.formFooter1.Controls.Add(this._acceptButton);
            this.formFooter1.Controls.Add(this._previousButton);
            this.formFooter1.Location = new System.Drawing.Point(0, 309);
            this.formFooter1.Name = "formFooter1";
            this.formFooter1.Size = new System.Drawing.Size(484, 53);
            this.formFooter1.TabIndex = 2;
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
            // _formHeader
            // 
            this._formHeader.Location = new System.Drawing.Point(0, 0);
            this._formHeader.Name = "_formHeader";
            this._formHeader.Size = new System.Drawing.Size(484, 47);
            this._formHeader.SubText = "Choose a Start Menu folder for the {0} shortcuts.";
            this._formHeader.TabIndex = 0;
            this._formHeader.Text = "Choose Start Menu Folder";
            // 
            // StartMenuPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.formFooter1);
            this.Controls.Add(this._formHeader);
            this.Name = "StartMenuPage";
            this.Size = new System.Drawing.Size(484, 362);
            this.Load += new System.EventHandler(this.StartMenuPage_Load);
            this.panel1.ResumeLayout(false);
            this._container.ResumeLayout(false);
            this._container.PerformLayout();
            this.formFooter1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel _container;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _startMenuFolder;
        private System.Windows.Forms.ListBox _startMenuFolders;
        private System.Windows.Forms.CheckBox _createStartMenu;
        private System.Windows.Forms.CheckBox _createOnDesktop;
        private Util.Forms.FormHeader _formHeader;
        private FormFooter formFooter1;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _acceptButton;
        private System.Windows.Forms.Button _previousButton;
    }
}

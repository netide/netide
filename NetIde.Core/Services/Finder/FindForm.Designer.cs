namespace NetIde.Core.Services.Finder
{
    partial class FindForm
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
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._modeFind = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._modeFindReplace = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._lookInBrowser = new NetIde.Util.Forms.BrowserControl();
            this._lookIn = new System.Windows.Forms.ComboBox();
            this._keepOpen = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this._replaceWithLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._includeSubFolders = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this._matchCase = new System.Windows.Forms.CheckBox();
            this._matchWholeWord = new System.Windows.Forms.CheckBox();
            this._useRegularExpressions = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this._lookInFileTypes = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this._replaceFindNext = new System.Windows.Forms.Button();
            this._replace = new System.Windows.Forms.Button();
            this._findAll = new System.Windows.Forms.Button();
            this._replaceAll = new System.Windows.Forms.Button();
            this._findPrevious = new System.Windows.Forms.Button();
            this._skipFile = new System.Windows.Forms.Button();
            this._findNext = new System.Windows.Forms.Button();
            this._findWhat = new System.Windows.Forms.ComboBox();
            this._replaceWith = new System.Windows.Forms.ComboBox();
            this._clientArea = new System.Windows.Forms.Panel();
            this._toolStrip.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this._lookInBrowser.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this._clientArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._modeFind,
            this.toolStripSeparator1,
            this._modeFindReplace});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(405, 25);
            this._toolStrip.TabIndex = 0;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _modeFind
            // 
            this._modeFind.AutoToolTip = false;
            this._modeFind.Image = global::NetIde.Core.NiResources.Find;
            this._modeFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._modeFind.Name = "_modeFind";
            this._modeFind.Size = new System.Drawing.Size(50, 22);
            this._modeFind.Text = "&Find";
            this._modeFind.ToolTipText = "Switch to Find";
            this._modeFind.Click += new System.EventHandler(this._modeFind_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // _modeFindReplace
            // 
            this._modeFindReplace.AutoToolTip = false;
            this._modeFindReplace.Image = global::NetIde.Core.NiResources.Replace;
            this._modeFindReplace.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._modeFindReplace.Name = "_modeFindReplace";
            this._modeFindReplace.Size = new System.Drawing.Size(117, 22);
            this._modeFindReplace.Text = "Find and &Replace";
            this._modeFindReplace.ToolTipText = "Switch to Find and Replace";
            this._modeFindReplace.Click += new System.EventHandler(this._modeFindReplace_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this._lookInBrowser, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this._keepOpen, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._replaceWithLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this._includeSubFolders, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this._findWhat, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._replaceWith, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(399, 451);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _lookInBrowser
            // 
            this._lookInBrowser.Controls.Add(this._lookIn);
            this._lookInBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lookInBrowser.Location = new System.Drawing.Point(3, 114);
            this._lookInBrowser.Name = "_lookInBrowser";
            this._lookInBrowser.Size = new System.Drawing.Size(393, 21);
            this._lookInBrowser.TabIndex = 5;
            this._lookInBrowser.Browse += new System.EventHandler(this._lookInBrowser_Browse);
            // 
            // _lookIn
            // 
            this._lookIn.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this._lookIn.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this._lookIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lookIn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._lookIn.FormattingEnabled = true;
            this._lookIn.Location = new System.Drawing.Point(0, 0);
            this._lookIn.Name = "_lookIn";
            this._lookIn.Size = new System.Drawing.Size(365, 21);
            this._lookIn.TabIndex = 6;
            this._lookIn.TextChanged += new System.EventHandler(this._lookIn_TextChanged);
            // 
            // _keepOpen
            // 
            this._keepOpen.AutoSize = true;
            this._keepOpen.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._keepOpen.Location = new System.Drawing.Point(3, 314);
            this._keepOpen.Name = "_keepOpen";
            this._keepOpen.Size = new System.Drawing.Size(228, 18);
            this._keepOpen.TabIndex = 8;
            this._keepOpen.Text = "Keep &modified files open after Replace All";
            this._keepOpen.UseVisualStyleBackColor = true;
            this._keepOpen.CheckedChanged += new System.EventHandler(this._keepOpen_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fi&nd what:";
            // 
            // _replaceWithLabel
            // 
            this._replaceWithLabel.AutoSize = true;
            this._replaceWithLabel.Location = new System.Drawing.Point(3, 49);
            this._replaceWithLabel.Margin = new System.Windows.Forms.Padding(3);
            this._replaceWithLabel.Name = "_replaceWithLabel";
            this._replaceWithLabel.Size = new System.Drawing.Size(72, 13);
            this._replaceWithLabel.TabIndex = 2;
            this._replaceWithLabel.Text = "Re&place with:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 95);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "&Look in:";
            // 
            // _includeSubFolders
            // 
            this._includeSubFolders.AutoSize = true;
            this._includeSubFolders.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._includeSubFolders.Location = new System.Drawing.Point(3, 141);
            this._includeSubFolders.Name = "_includeSubFolders";
            this._includeSubFolders.Size = new System.Drawing.Size(121, 18);
            this._includeSubFolders.TabIndex = 6;
            this._includeSubFolders.Text = "Include su&b-folders";
            this._includeSubFolders.UseVisualStyleBackColor = true;
            this._includeSubFolders.CheckedChanged += new System.EventHandler(this._includeSubFolders_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.tableLayoutPanel3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 165);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(8, 4, 8, 8);
            this.groupBox1.Size = new System.Drawing.Size(393, 143);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Find &options";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this._matchCase, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this._matchWholeWord, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this._useRegularExpressions, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this._lookInFileTypes, 0, 4);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(8, 17);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(377, 118);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // _matchCase
            // 
            this._matchCase.AutoSize = true;
            this._matchCase.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._matchCase.Location = new System.Drawing.Point(3, 3);
            this._matchCase.Name = "_matchCase";
            this._matchCase.Size = new System.Drawing.Size(88, 18);
            this._matchCase.TabIndex = 0;
            this._matchCase.Text = "Match &case";
            this._matchCase.UseVisualStyleBackColor = true;
            this._matchCase.CheckedChanged += new System.EventHandler(this._matchCase_CheckedChanged);
            // 
            // _matchWholeWord
            // 
            this._matchWholeWord.AutoSize = true;
            this._matchWholeWord.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._matchWholeWord.Location = new System.Drawing.Point(3, 27);
            this._matchWholeWord.Name = "_matchWholeWord";
            this._matchWholeWord.Size = new System.Drawing.Size(119, 18);
            this._matchWholeWord.TabIndex = 1;
            this._matchWholeWord.Text = "Match &whole word";
            this._matchWholeWord.UseVisualStyleBackColor = true;
            this._matchWholeWord.CheckedChanged += new System.EventHandler(this._matchWholeWord_CheckedChanged);
            // 
            // _useRegularExpressions
            // 
            this._useRegularExpressions.AutoSize = true;
            this._useRegularExpressions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._useRegularExpressions.Location = new System.Drawing.Point(3, 51);
            this._useRegularExpressions.Name = "_useRegularExpressions";
            this._useRegularExpressions.Size = new System.Drawing.Size(150, 18);
            this._useRegularExpressions.TabIndex = 2;
            this._useRegularExpressions.Text = "Us&e Regular Expressions";
            this._useRegularExpressions.UseVisualStyleBackColor = true;
            this._useRegularExpressions.CheckedChanged += new System.EventHandler(this._useRegularExpressions_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 75);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Look at these file &types:";
            // 
            // _lookInFileTypes
            // 
            this._lookInFileTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lookInFileTypes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._lookInFileTypes.FormattingEnabled = true;
            this._lookInFileTypes.Location = new System.Drawing.Point(3, 94);
            this._lookInFileTypes.Name = "_lookInFileTypes";
            this._lookInFileTypes.Size = new System.Drawing.Size(371, 21);
            this._lookInFileTypes.TabIndex = 4;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this._replaceFindNext, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this._replace, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this._findAll, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this._replaceAll, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this._findPrevious, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this._skipFile, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this._findNext, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(187, 335);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(212, 116);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // _replaceFindNext
            // 
            this._replaceFindNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._replaceFindNext.Location = new System.Drawing.Point(3, 3);
            this._replaceFindNext.Name = "_replaceFindNext";
            this._replaceFindNext.Size = new System.Drawing.Size(100, 23);
            this._replaceFindNext.TabIndex = 0;
            this._replaceFindNext.Text = "&Find Next";
            this._replaceFindNext.UseVisualStyleBackColor = true;
            this._replaceFindNext.Click += new System.EventHandler(this._replaceFindNext_Click);
            // 
            // _replace
            // 
            this._replace.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._replace.Location = new System.Drawing.Point(109, 3);
            this._replace.Name = "_replace";
            this._replace.Size = new System.Drawing.Size(100, 23);
            this._replace.TabIndex = 1;
            this._replace.Text = "&Replace";
            this._replace.UseVisualStyleBackColor = true;
            this._replace.Click += new System.EventHandler(this._replace_Click);
            // 
            // _findAll
            // 
            this._findAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._findAll.Location = new System.Drawing.Point(109, 90);
            this._findAll.Name = "_findAll";
            this._findAll.Size = new System.Drawing.Size(100, 23);
            this._findAll.TabIndex = 4;
            this._findAll.Text = "Find &All";
            this._findAll.UseVisualStyleBackColor = true;
            this._findAll.Click += new System.EventHandler(this._findAll_Click);
            // 
            // _replaceAll
            // 
            this._replaceAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._replaceAll.Location = new System.Drawing.Point(109, 61);
            this._replaceAll.Name = "_replaceAll";
            this._replaceAll.Size = new System.Drawing.Size(100, 23);
            this._replaceAll.TabIndex = 3;
            this._replaceAll.Text = "Replace &All";
            this._replaceAll.UseVisualStyleBackColor = true;
            this._replaceAll.Click += new System.EventHandler(this._replaceAll_Click);
            // 
            // _findPrevious
            // 
            this._findPrevious.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._findPrevious.Location = new System.Drawing.Point(3, 32);
            this._findPrevious.Name = "_findPrevious";
            this._findPrevious.Size = new System.Drawing.Size(100, 23);
            this._findPrevious.TabIndex = 5;
            this._findPrevious.Text = "Find &Previous";
            this._findPrevious.UseVisualStyleBackColor = true;
            this._findPrevious.Click += new System.EventHandler(this._findPrevious_Click);
            // 
            // _skipFile
            // 
            this._skipFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._skipFile.Location = new System.Drawing.Point(3, 61);
            this._skipFile.Name = "_skipFile";
            this._skipFile.Size = new System.Drawing.Size(100, 23);
            this._skipFile.TabIndex = 2;
            this._skipFile.Text = "Sk&ip File";
            this._skipFile.UseVisualStyleBackColor = true;
            this._skipFile.Click += new System.EventHandler(this._skipFile_Click);
            // 
            // _findNext
            // 
            this._findNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._findNext.Location = new System.Drawing.Point(109, 32);
            this._findNext.Name = "_findNext";
            this._findNext.Size = new System.Drawing.Size(100, 23);
            this._findNext.TabIndex = 6;
            this._findNext.Text = "&Find Next";
            this._findNext.UseVisualStyleBackColor = true;
            this._findNext.Click += new System.EventHandler(this._findNext_Click);
            // 
            // _findWhat
            // 
            this._findWhat.Dock = System.Windows.Forms.DockStyle.Fill;
            this._findWhat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._findWhat.FormattingEnabled = true;
            this._findWhat.Location = new System.Drawing.Point(3, 22);
            this._findWhat.Name = "_findWhat";
            this._findWhat.Size = new System.Drawing.Size(393, 21);
            this._findWhat.TabIndex = 1;
            // 
            // _replaceWith
            // 
            this._replaceWith.Dock = System.Windows.Forms.DockStyle.Fill;
            this._replaceWith.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._replaceWith.FormattingEnabled = true;
            this._replaceWith.Location = new System.Drawing.Point(3, 68);
            this._replaceWith.Name = "_replaceWith";
            this._replaceWith.Size = new System.Drawing.Size(393, 21);
            this._replaceWith.TabIndex = 3;
            // 
            // _clientArea
            // 
            this._clientArea.AutoSize = true;
            this._clientArea.Controls.Add(this.tableLayoutPanel1);
            this._clientArea.Dock = System.Windows.Forms.DockStyle.Top;
            this._clientArea.Location = new System.Drawing.Point(0, 25);
            this._clientArea.Name = "_clientArea";
            this._clientArea.Padding = new System.Windows.Forms.Padding(3);
            this._clientArea.Size = new System.Drawing.Size(405, 457);
            this._clientArea.TabIndex = 2;
            // 
            // FindForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 506);
            this.Controls.Add(this._clientArea);
            this.Controls.Add(this._toolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FindForm";
            this.Text = "Find and Replace";
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this._lookInBrowser.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this._clientArea.ResumeLayout(false);
            this._clientArea.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _modeFind;
        private System.Windows.Forms.ToolStripButton _modeFindReplace;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox _keepOpen;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label _replaceWithLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox _includeSubFolders;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.CheckBox _matchCase;
        private System.Windows.Forms.CheckBox _matchWholeWord;
        private System.Windows.Forms.CheckBox _useRegularExpressions;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox _lookInFileTypes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button _replaceFindNext;
        private System.Windows.Forms.Button _replace;
        private System.Windows.Forms.Button _skipFile;
        private System.Windows.Forms.Button _replaceAll;
        private System.Windows.Forms.Panel _clientArea;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button _findAll;
        private System.Windows.Forms.ComboBox _findWhat;
        private System.Windows.Forms.ComboBox _replaceWith;
        private System.Windows.Forms.Button _findPrevious;
        private System.Windows.Forms.Button _findNext;
        private Util.Forms.BrowserControl _lookInBrowser;
        private System.Windows.Forms.ComboBox _lookIn;
    }
}
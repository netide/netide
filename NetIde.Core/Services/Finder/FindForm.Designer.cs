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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindForm));
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
            resources.ApplyResources(this._toolStrip, "_toolStrip");
            this._toolStrip.Name = "_toolStrip";
            // 
            // _modeFind
            // 
            this._modeFind.AutoToolTip = false;
            this._modeFind.Image = global::NetIde.Core.NeutralResources.Find;
            resources.ApplyResources(this._modeFind, "_modeFind");
            this._modeFind.Name = "_modeFind";
            this._modeFind.Click += new System.EventHandler(this._modeFind_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // _modeFindReplace
            // 
            this._modeFindReplace.AutoToolTip = false;
            this._modeFindReplace.Image = global::NetIde.Core.NeutralResources.Replace;
            resources.ApplyResources(this._modeFindReplace, "_modeFindReplace");
            this._modeFindReplace.Name = "_modeFindReplace";
            this._modeFindReplace.Click += new System.EventHandler(this._modeFindReplace_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
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
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // _lookInBrowser
            // 
            this._lookInBrowser.Controls.Add(this._lookIn);
            resources.ApplyResources(this._lookInBrowser, "_lookInBrowser");
            this._lookInBrowser.Name = "_lookInBrowser";
            this._lookInBrowser.Browse += new System.EventHandler(this._lookInBrowser_Browse);
            // 
            // _lookIn
            // 
            this._lookIn.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this._lookIn.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            resources.ApplyResources(this._lookIn, "_lookIn");
            this._lookIn.FormattingEnabled = true;
            this._lookIn.Name = "_lookIn";
            this._lookIn.TextChanged += new System.EventHandler(this._lookIn_TextChanged);
            // 
            // _keepOpen
            // 
            resources.ApplyResources(this._keepOpen, "_keepOpen");
            this._keepOpen.Name = "_keepOpen";
            this._keepOpen.UseVisualStyleBackColor = true;
            this._keepOpen.CheckedChanged += new System.EventHandler(this._keepOpen_CheckedChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // _replaceWithLabel
            // 
            resources.ApplyResources(this._replaceWithLabel, "_replaceWithLabel");
            this._replaceWithLabel.Name = "_replaceWithLabel";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // _includeSubFolders
            // 
            resources.ApplyResources(this._includeSubFolders, "_includeSubFolders");
            this._includeSubFolders.Name = "_includeSubFolders";
            this._includeSubFolders.UseVisualStyleBackColor = true;
            this._includeSubFolders.CheckedChanged += new System.EventHandler(this._includeSubFolders_CheckedChanged);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.tableLayoutPanel3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this._matchCase, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this._matchWholeWord, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this._useRegularExpressions, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this._lookInFileTypes, 0, 4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // _matchCase
            // 
            resources.ApplyResources(this._matchCase, "_matchCase");
            this._matchCase.Name = "_matchCase";
            this._matchCase.UseVisualStyleBackColor = true;
            this._matchCase.CheckedChanged += new System.EventHandler(this._matchCase_CheckedChanged);
            // 
            // _matchWholeWord
            // 
            resources.ApplyResources(this._matchWholeWord, "_matchWholeWord");
            this._matchWholeWord.Name = "_matchWholeWord";
            this._matchWholeWord.UseVisualStyleBackColor = true;
            this._matchWholeWord.CheckedChanged += new System.EventHandler(this._matchWholeWord_CheckedChanged);
            // 
            // _useRegularExpressions
            // 
            resources.ApplyResources(this._useRegularExpressions, "_useRegularExpressions");
            this._useRegularExpressions.Name = "_useRegularExpressions";
            this._useRegularExpressions.UseVisualStyleBackColor = true;
            this._useRegularExpressions.CheckedChanged += new System.EventHandler(this._useRegularExpressions_CheckedChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // _lookInFileTypes
            // 
            resources.ApplyResources(this._lookInFileTypes, "_lookInFileTypes");
            this._lookInFileTypes.FormattingEnabled = true;
            this._lookInFileTypes.Name = "_lookInFileTypes";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this._replaceFindNext, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this._replace, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this._findAll, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this._replaceAll, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this._findPrevious, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this._skipFile, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this._findNext, 1, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // _replaceFindNext
            // 
            resources.ApplyResources(this._replaceFindNext, "_replaceFindNext");
            this._replaceFindNext.Name = "_replaceFindNext";
            this._replaceFindNext.UseVisualStyleBackColor = true;
            this._replaceFindNext.Click += new System.EventHandler(this._replaceFindNext_Click);
            // 
            // _replace
            // 
            resources.ApplyResources(this._replace, "_replace");
            this._replace.Name = "_replace";
            this._replace.UseVisualStyleBackColor = true;
            this._replace.Click += new System.EventHandler(this._replace_Click);
            // 
            // _findAll
            // 
            resources.ApplyResources(this._findAll, "_findAll");
            this._findAll.Name = "_findAll";
            this._findAll.UseVisualStyleBackColor = true;
            this._findAll.Click += new System.EventHandler(this._findAll_Click);
            // 
            // _replaceAll
            // 
            resources.ApplyResources(this._replaceAll, "_replaceAll");
            this._replaceAll.Name = "_replaceAll";
            this._replaceAll.UseVisualStyleBackColor = true;
            this._replaceAll.Click += new System.EventHandler(this._replaceAll_Click);
            // 
            // _findPrevious
            // 
            resources.ApplyResources(this._findPrevious, "_findPrevious");
            this._findPrevious.Name = "_findPrevious";
            this._findPrevious.UseVisualStyleBackColor = true;
            this._findPrevious.Click += new System.EventHandler(this._findPrevious_Click);
            // 
            // _skipFile
            // 
            resources.ApplyResources(this._skipFile, "_skipFile");
            this._skipFile.Name = "_skipFile";
            this._skipFile.UseVisualStyleBackColor = true;
            this._skipFile.Click += new System.EventHandler(this._skipFile_Click);
            // 
            // _findNext
            // 
            resources.ApplyResources(this._findNext, "_findNext");
            this._findNext.Name = "_findNext";
            this._findNext.UseVisualStyleBackColor = true;
            this._findNext.Click += new System.EventHandler(this._findNext_Click);
            // 
            // _findWhat
            // 
            resources.ApplyResources(this._findWhat, "_findWhat");
            this._findWhat.FormattingEnabled = true;
            this._findWhat.Name = "_findWhat";
            // 
            // _replaceWith
            // 
            resources.ApplyResources(this._replaceWith, "_replaceWith");
            this._replaceWith.FormattingEnabled = true;
            this._replaceWith.Name = "_replaceWith";
            // 
            // _clientArea
            // 
            resources.ApplyResources(this._clientArea, "_clientArea");
            this._clientArea.Controls.Add(this.tableLayoutPanel1);
            this._clientArea.Name = "_clientArea";
            // 
            // FindForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._clientArea);
            this.Controls.Add(this._toolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FindForm";
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
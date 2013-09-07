namespace NetIde.VisualStudio.Wizard
{
    partial class SigningStep
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
            this.formHeader1 = new NetIde.Util.Forms.FormHeader();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._generateNew = new System.Windows.Forms.RadioButton();
            this._useExisting = new System.Windows.Forms.RadioButton();
            this._keyFileContainer = new System.Windows.Forms.TableLayoutPanel();
            this._keyFile = new NetIde.Util.Forms.FileBrowser();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this._keyFileContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // formHeader1
            // 
            this.formHeader1.Location = new System.Drawing.Point(0, 0);
            this.formHeader1.Name = "formHeader1";
            this.formHeader1.Size = new System.Drawing.Size(554, 47);
            this.formHeader1.SubText = "Choose below how you want to sign your package.";
            this.formHeader1.TabIndex = 1;
            this.formHeader1.Text = "Sign Your Package";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 47);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(9);
            this.panel1.Size = new System.Drawing.Size(554, 399);
            this.panel1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this._generateNew, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._useExisting, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._keyFileContainer, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 9);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(536, 381);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _generateNew
            // 
            this._generateNew.AutoSize = true;
            this._generateNew.Checked = true;
            this.tableLayoutPanel1.SetColumnSpan(this._generateNew, 2);
            this._generateNew.Location = new System.Drawing.Point(3, 3);
            this._generateNew.Name = "_generateNew";
            this._generateNew.Size = new System.Drawing.Size(235, 17);
            this._generateNew.TabIndex = 0;
            this._generateNew.TabStop = true;
            this._generateNew.Text = "Generate a new key file to sign the assembly";
            this._generateNew.UseVisualStyleBackColor = true;
            // 
            // _useExisting
            // 
            this._useExisting.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this._useExisting, 2);
            this._useExisting.Location = new System.Drawing.Point(3, 26);
            this._useExisting.Name = "_useExisting";
            this._useExisting.Size = new System.Drawing.Size(243, 17);
            this._useExisting.TabIndex = 1;
            this._useExisting.TabStop = true;
            this._useExisting.Text = "Use the following key file to sign the assembly:";
            this._useExisting.UseVisualStyleBackColor = true;
            this._useExisting.CheckedChanged += new System.EventHandler(this._useExisting_CheckedChanged);
            // 
            // _keyFileContainer
            // 
            this._keyFileContainer.AutoSize = true;
            this._keyFileContainer.ColumnCount = 1;
            this._keyFileContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._keyFileContainer.Controls.Add(this._keyFile, 0, 0);
            this._keyFileContainer.Controls.Add(this.label1, 0, 1);
            this._keyFileContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._keyFileContainer.Location = new System.Drawing.Point(23, 49);
            this._keyFileContainer.Name = "_keyFileContainer";
            this._keyFileContainer.RowCount = 2;
            this._keyFileContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._keyFileContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._keyFileContainer.Size = new System.Drawing.Size(510, 45);
            this._keyFileContainer.TabIndex = 2;
            // 
            // _keyFile
            // 
            this._keyFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this._keyFile.Filter = "Key File (*.snk)|*.snk|All Files (*.*)|*.*";
            this._keyFile.Location = new System.Drawing.Point(3, 3);
            this._keyFile.Name = "_keyFile";
            this._keyFile.Path = null;
            this._keyFile.Size = new System.Drawing.Size(504, 20);
            this._keyFile.TabIndex = 1;
            this._keyFile.PathChanged += new System.EventHandler(this._keyFile_PathChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(302, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "The file will be copied to the Key.snk and added to the project.";
            // 
            // SigningStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.formHeader1);
            this.Name = "SigningStep";
            this.Size = new System.Drawing.Size(554, 446);
            this.Load += new System.EventHandler(this.SigningStep_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this._keyFileContainer.ResumeLayout(false);
            this._keyFileContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Util.Forms.FormHeader formHeader1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton _generateNew;
        private System.Windows.Forms.RadioButton _useExisting;
        private System.Windows.Forms.TableLayoutPanel _keyFileContainer;
        private Util.Forms.FileBrowser _keyFile;
        private System.Windows.Forms.Label label1;
    }
}

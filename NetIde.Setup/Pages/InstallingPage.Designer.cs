namespace NetIde.Setup.Pages
{
    partial class InstallingPage
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
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this._showDetails = new System.Windows.Forms.Button();
            this._progressListBox = new System.Windows.Forms.ListBox();
            this._progressLabel = new NetIde.Util.Forms.PathLabel();
            this.formFooter1 = new NetIde.Setup.FormFooter();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this._formHeader = new NetIde.Util.Forms.FormHeader();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this._progressBar, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._showDetails, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this._progressListBox, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this._progressLabel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(460, 238);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _progressBar
            // 
            this._progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this._progressBar.Location = new System.Drawing.Point(3, 22);
            this._progressBar.Maximum = 1000;
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(454, 18);
            this._progressBar.TabIndex = 1;
            // 
            // _showDetails
            // 
            this._showDetails.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._showDetails.Location = new System.Drawing.Point(3, 46);
            this._showDetails.Name = "_showDetails";
            this._showDetails.Size = new System.Drawing.Size(98, 23);
            this._showDetails.TabIndex = 2;
            this._showDetails.Text = "&Show details";
            this._showDetails.UseVisualStyleBackColor = true;
            this._showDetails.Click += new System.EventHandler(this._showDetails_Click);
            // 
            // _progressListBox
            // 
            this._progressListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._progressListBox.FormattingEnabled = true;
            this._progressListBox.IntegralHeight = false;
            this._progressListBox.Location = new System.Drawing.Point(3, 75);
            this._progressListBox.Name = "_progressListBox";
            this._progressListBox.Size = new System.Drawing.Size(454, 160);
            this._progressListBox.TabIndex = 3;
            // 
            // _progressLabel
            // 
            this._progressLabel.AutoSize = true;
            this._progressLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._progressLabel.Location = new System.Drawing.Point(3, 3);
            this._progressLabel.Name = "_progressLabel";
            this._progressLabel.Size = new System.Drawing.Size(454, 13);
            this._progressLabel.TabIndex = 4;
            // 
            // formFooter1
            // 
            this.formFooter1.Controls.Add(this.button1);
            this.formFooter1.Controls.Add(this.button2);
            this.formFooter1.Location = new System.Drawing.Point(0, 309);
            this.formFooter1.Name = "formFooter1";
            this.formFooter1.Size = new System.Drawing.Size(484, 53);
            this.formFooter1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(397, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(316, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "&Next >";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // _formHeader
            // 
            this._formHeader.Location = new System.Drawing.Point(0, 0);
            this._formHeader.Name = "_formHeader";
            this._formHeader.Size = new System.Drawing.Size(484, 47);
            this._formHeader.SubText = "<<mode sub text>>";
            this._formHeader.TabIndex = 0;
            this._formHeader.Text = "<<mode>>";
            // 
            // InstallingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.formFooter1);
            this.Controls.Add(this._formHeader);
            this.Name = "InstallingPage";
            this.Size = new System.Drawing.Size(484, 362);
            this.Load += new System.EventHandler(this.InstallingPage_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.formFooter1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ProgressBar _progressBar;
        private System.Windows.Forms.Button _showDetails;
        private System.Windows.Forms.ListBox _progressListBox;
        private Util.Forms.FormHeader _formHeader;
        private FormFooter formFooter1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private Util.Forms.PathLabel _progressLabel;
    }
}

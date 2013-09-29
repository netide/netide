namespace NetIde.Setup.Pages
{
    partial class LicensePage
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
            this.label1 = new System.Windows.Forms.Label();
            this._license = new System.Windows.Forms.TextBox();
            this._agree = new System.Windows.Forms.Label();
            this._formHeader = new NetIde.Util.Forms.FormHeader();
            this.formFooter1 = new NetIde.Setup.FormFooter();
            this._cancelButton = new System.Windows.Forms.Button();
            this._acceptButton = new System.Windows.Forms.Button();
            this._previousButton = new System.Windows.Forms.Button();
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
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._license, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._agree, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(460, 238);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(248, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Press Page Down to see the rest of the &agreement.";
            // 
            // _license
            // 
            this._license.BackColor = System.Drawing.SystemColors.Window;
            this._license.Dock = System.Windows.Forms.DockStyle.Fill;
            this._license.Location = new System.Drawing.Point(3, 28);
            this._license.Multiline = true;
            this._license.Name = "_license";
            this._license.ReadOnly = true;
            this._license.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._license.Size = new System.Drawing.Size(454, 181);
            this._license.TabIndex = 1;
            // 
            // _agree
            // 
            this._agree.AutoSize = true;
            this._agree.Location = new System.Drawing.Point(3, 212);
            this._agree.Name = "_agree";
            this._agree.Size = new System.Drawing.Size(426, 26);
            this._agree.TabIndex = 2;
            this._agree.Text = "If you accept the terms of the agreement, click I Agree to continue. You must acc" +
    "ept the agreement to install {0}.";
            // 
            // _formHeader
            // 
            this._formHeader.Location = new System.Drawing.Point(0, 0);
            this._formHeader.Name = "_formHeader";
            this._formHeader.Size = new System.Drawing.Size(484, 47);
            this._formHeader.SubText = "Please review the license terms before installing {0}.";
            this._formHeader.TabIndex = 0;
            this._formHeader.Text = "License Agreement";
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
            this._acceptButton.Text = "I &Agree";
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
            // LicensePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.formFooter1);
            this.Controls.Add(this._formHeader);
            this.Name = "LicensePage";
            this.Size = new System.Drawing.Size(484, 362);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _license;
        private System.Windows.Forms.Label _agree;
        private Util.Forms.FormHeader _formHeader;
        private FormFooter formFooter1;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _acceptButton;
        private System.Windows.Forms.Button _previousButton;
    }
}

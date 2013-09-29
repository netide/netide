namespace NetIde.Setup.Pages
{
    partial class ConfirmPage
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
            this._message = new System.Windows.Forms.Label();
            this._packages = new System.Windows.Forms.ListBox();
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
            this._container.Controls.Add(this._message, 0, 0);
            this._container.Controls.Add(this._packages, 0, 1);
            this._container.Dock = System.Windows.Forms.DockStyle.Fill;
            this._container.Location = new System.Drawing.Point(12, 12);
            this._container.Name = "_container";
            this._container.RowCount = 2;
            this._container.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._container.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._container.Size = new System.Drawing.Size(460, 238);
            this._container.TabIndex = 1;
            // 
            // _message
            // 
            this._message.AutoSize = true;
            this._message.Location = new System.Drawing.Point(3, 0);
            this._message.Margin = new System.Windows.Forms.Padding(3, 0, 3, 18);
            this._message.Name = "_message";
            this._message.Size = new System.Drawing.Size(97, 13);
            this._message.TabIndex = 0;
            this._message.Text = "<<mode sub text>>";
            // 
            // _packages
            // 
            this._packages.Dock = System.Windows.Forms.DockStyle.Fill;
            this._packages.FormattingEnabled = true;
            this._packages.IntegralHeight = false;
            this._packages.Location = new System.Drawing.Point(3, 34);
            this._packages.Name = "_packages";
            this._packages.Size = new System.Drawing.Size(454, 201);
            this._packages.TabIndex = 2;
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
            this._acceptButton.Text = "&Install";
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
            this._formHeader.SubText = "<<mode sub text>>";
            this._formHeader.TabIndex = 0;
            this._formHeader.Text = "Confirm Components";
            // 
            // ConfirmPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.formFooter1);
            this.Controls.Add(this._formHeader);
            this.Name = "ConfirmPage";
            this.Size = new System.Drawing.Size(484, 362);
            this.Load += new System.EventHandler(this.ConfirmPage_Load);
            this.panel1.ResumeLayout(false);
            this._container.ResumeLayout(false);
            this._container.PerformLayout();
            this.formFooter1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Util.Forms.FormHeader _formHeader;
        private FormFooter formFooter1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _acceptButton;
        private System.Windows.Forms.Button _previousButton;
        private System.Windows.Forms.TableLayoutPanel _container;
        private System.Windows.Forms.Label _message;
        private System.Windows.Forms.ListBox _packages;
    }
}

namespace NetIde.VisualStudio.Wizard
{
    partial class WizardForm
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
            this._clientArea = new System.Windows.Forms.Panel();
            this.formFooter1 = new NetIde.Util.Forms.FormFooter();
            this._cancelButton = new System.Windows.Forms.Button();
            this._finishButton = new System.Windows.Forms.Button();
            this._nextButton = new System.Windows.Forms.Button();
            this._previousButton = new System.Windows.Forms.Button();
            this.formFooter1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _clientArea
            // 
            this._clientArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this._clientArea.Location = new System.Drawing.Point(0, 0);
            this._clientArea.Name = "_clientArea";
            this._clientArea.Size = new System.Drawing.Size(542, 369);
            this._clientArea.TabIndex = 0;
            // 
            // formFooter1
            // 
            this.formFooter1.Controls.Add(this._cancelButton);
            this.formFooter1.Controls.Add(this._finishButton);
            this.formFooter1.Controls.Add(this._nextButton);
            this.formFooter1.Controls.Add(this._previousButton);
            this.formFooter1.Location = new System.Drawing.Point(0, 369);
            this.formFooter1.Name = "formFooter1";
            this.formFooter1.Size = new System.Drawing.Size(542, 45);
            this.formFooter1.TabIndex = 1;
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._cancelButton.Location = new System.Drawing.Point(446, 11);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 3;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _finishButton
            // 
            this._finishButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._finishButton.Location = new System.Drawing.Point(365, 11);
            this._finishButton.Name = "_finishButton";
            this._finishButton.Size = new System.Drawing.Size(75, 23);
            this._finishButton.TabIndex = 2;
            this._finishButton.Text = "&Finish";
            this._finishButton.UseVisualStyleBackColor = true;
            this._finishButton.Click += new System.EventHandler(this._finishButton_Click);
            // 
            // _nextButton
            // 
            this._nextButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._nextButton.Location = new System.Drawing.Point(284, 11);
            this._nextButton.Name = "_nextButton";
            this._nextButton.Size = new System.Drawing.Size(75, 23);
            this._nextButton.TabIndex = 1;
            this._nextButton.Text = "&Next >";
            this._nextButton.UseVisualStyleBackColor = true;
            this._nextButton.Click += new System.EventHandler(this._nextButton_Click);
            // 
            // _previousButton
            // 
            this._previousButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._previousButton.Location = new System.Drawing.Point(203, 11);
            this._previousButton.Name = "_previousButton";
            this._previousButton.Size = new System.Drawing.Size(75, 23);
            this._previousButton.TabIndex = 0;
            this._previousButton.Text = "< &Previous";
            this._previousButton.UseVisualStyleBackColor = true;
            this._previousButton.Click += new System.EventHandler(this._previousButton_Click);
            // 
            // WizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(542, 414);
            this.Controls.Add(this._clientArea);
            this.Controls.Add(this.formFooter1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "WizardForm";
            this.Text = "Net IDE Package Wizard";
            this.formFooter1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel _clientArea;
        private Util.Forms.FormFooter formFooter1;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _finishButton;
        private System.Windows.Forms.Button _nextButton;
        private System.Windows.Forms.Button _previousButton;
    }
}
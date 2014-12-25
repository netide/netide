namespace NetIde.Core.TextEditor
{
    partial class GoToLineForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GoToLineForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._lineNumberLabel = new System.Windows.Forms.Label();
            this._lineNumber = new NetIde.Util.Forms.NumericBox();
            this.formFooter1 = new NetIde.Util.Forms.FormFooter();
            this._cancelButton = new System.Windows.Forms.Button();
            this._acceptButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.formFooter1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this._lineNumberLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._lineNumber, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // _lineNumberLabel
            // 
            resources.ApplyResources(this._lineNumberLabel, "_lineNumberLabel");
            this._lineNumberLabel.Name = "_lineNumberLabel";
            // 
            // _lineNumber
            // 
            resources.ApplyResources(this._lineNumber, "_lineNumber");
            this._lineNumber.Name = "_lineNumber";
            this._lineNumber.NumberScale = 0;
            this._lineNumber.ValueChanged += new System.EventHandler(this._lineNumber_ValueChanged);
            // 
            // formFooter1
            // 
            this.formFooter1.Controls.Add(this._cancelButton);
            this.formFooter1.Controls.Add(this._acceptButton);
            resources.ApplyResources(this.formFooter1, "formFooter1");
            this.formFooter1.Name = "formFooter1";
            this.formFooter1.Style = NetIde.Util.Forms.FormFooterStyle.Dialog;
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this._cancelButton, "_cancelButton");
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _acceptButton
            // 
            resources.ApplyResources(this._acceptButton, "_acceptButton");
            this._acceptButton.Name = "_acceptButton";
            this._acceptButton.UseVisualStyleBackColor = true;
            this._acceptButton.Click += new System.EventHandler(this._acceptButton_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Name = "panel1";
            // 
            // GoToLineForm
            // 
            this.AcceptButton = this._acceptButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.formFooter1);
            this.Name = "GoToLineForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.formFooter1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Util.Forms.FormFooter formFooter1;
        private System.Windows.Forms.Label _lineNumberLabel;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _acceptButton;
        private System.Windows.Forms.Panel panel1;
        private Util.Forms.NumericBox _lineNumber;
    }
}
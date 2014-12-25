namespace NetIde.Services.Shell
{
    partial class SaveHierarchiesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveHierarchiesForm));
            this.formFooter1 = new NetIde.Util.Forms.FormFooter();
            this._cancel = new System.Windows.Forms.Button();
            this._no = new System.Windows.Forms.Button();
            this._yes = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this._listBox = new System.Windows.Forms.ListBox();
            this.formFooter1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // formFooter1
            // 
            this.formFooter1.Controls.Add(this._cancel);
            this.formFooter1.Controls.Add(this._no);
            this.formFooter1.Controls.Add(this._yes);
            resources.ApplyResources(this.formFooter1, "formFooter1");
            this.formFooter1.Name = "formFooter1";
            // 
            // _cancel
            // 
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this._cancel, "_cancel");
            this._cancel.Name = "_cancel";
            this._cancel.UseVisualStyleBackColor = true;
            this._cancel.Click += new System.EventHandler(this._cancel_Click);
            // 
            // _no
            // 
            resources.ApplyResources(this._no, "_no");
            this._no.Name = "_no";
            this._no.UseVisualStyleBackColor = true;
            this._no.Click += new System.EventHandler(this._no_Click);
            // 
            // _yes
            // 
            resources.ApplyResources(this._yes, "_yes");
            this._yes.Name = "_yes";
            this._yes.UseVisualStyleBackColor = true;
            this._yes.Click += new System.EventHandler(this._yes_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._listBox, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // _listBox
            // 
            resources.ApplyResources(this._listBox, "_listBox");
            this._listBox.FormattingEnabled = true;
            this._listBox.Name = "_listBox";
            // 
            // SaveHierarchiesForm
            // 
            this.AcceptButton = this._yes;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancel;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.formFooter1);
            this.Name = "SaveHierarchiesForm";
            this.formFooter1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Util.Forms.FormFooter formFooter1;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.Button _no;
        private System.Windows.Forms.Button _yes;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox _listBox;
    }
}
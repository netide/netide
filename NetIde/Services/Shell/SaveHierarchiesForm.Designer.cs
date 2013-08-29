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
            this.formFooter1 = new NetIde.Util.Forms.FormFooter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._cancel = new System.Windows.Forms.Button();
            this._no = new System.Windows.Forms.Button();
            this._yes = new System.Windows.Forms.Button();
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
            this.formFooter1.Location = new System.Drawing.Point(0, 280);
            this.formFooter1.Name = "formFooter1";
            this.formFooter1.Size = new System.Drawing.Size(524, 45);
            this.formFooter1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(9);
            this.panel1.Size = new System.Drawing.Size(524, 280);
            this.panel1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._listBox, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 9);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(506, 262);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _cancel
            // 
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(428, 11);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 23);
            this._cancel.TabIndex = 0;
            this._cancel.Text = "&Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            this._cancel.Click += new System.EventHandler(this._cancel_Click);
            // 
            // _no
            // 
            this._no.Location = new System.Drawing.Point(347, 11);
            this._no.Name = "_no";
            this._no.Size = new System.Drawing.Size(75, 23);
            this._no.TabIndex = 1;
            this._no.Text = "&No";
            this._no.UseVisualStyleBackColor = true;
            this._no.Click += new System.EventHandler(this._no_Click);
            // 
            // _yes
            // 
            this._yes.Location = new System.Drawing.Point(266, 11);
            this._yes.Name = "_yes";
            this._yes.Size = new System.Drawing.Size(75, 23);
            this._yes.TabIndex = 2;
            this._yes.Text = "&Yes";
            this._yes.UseVisualStyleBackColor = true;
            this._yes.Click += new System.EventHandler(this._yes_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(500, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Save changes to the following items?";
            // 
            // _listBox
            // 
            this._listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listBox.FormattingEnabled = true;
            this._listBox.Location = new System.Drawing.Point(3, 22);
            this._listBox.Name = "_listBox";
            this._listBox.Size = new System.Drawing.Size(500, 237);
            this._listBox.TabIndex = 1;
            // 
            // SaveHierarchiesForm
            // 
            this.AcceptButton = this._yes;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(524, 325);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.formFooter1);
            this.Name = "SaveHierarchiesForm";
            this.Text = "Save";
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
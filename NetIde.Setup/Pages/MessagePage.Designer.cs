namespace NetIde.Setup.Pages
{
    partial class MessagePage
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
            this.formFooter1 = new NetIde.Setup.FormFooter();
            this._acceptButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._pictureBox = new System.Windows.Forms.PictureBox();
            this._message = new System.Windows.Forms.Label();
            this.formFooter1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // formFooter1
            // 
            this.formFooter1.Controls.Add(this._acceptButton);
            this.formFooter1.Location = new System.Drawing.Point(0, 309);
            this.formFooter1.Name = "formFooter1";
            this.formFooter1.Size = new System.Drawing.Size(484, 53);
            this.formFooter1.TabIndex = 1;
            // 
            // _acceptButton
            // 
            this._acceptButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._acceptButton.Location = new System.Drawing.Point(397, 20);
            this._acceptButton.Name = "_acceptButton";
            this._acceptButton.Size = new System.Drawing.Size(75, 23);
            this._acceptButton.TabIndex = 0;
            this._acceptButton.Text = "&Finish";
            this._acceptButton.UseVisualStyleBackColor = true;
            this._acceptButton.Click += new System.EventHandler(this._acceptButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Controls.Add(this._pictureBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._message, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(484, 309);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _pictureBox
            // 
            this._pictureBox.Location = new System.Drawing.Point(63, 142);
            this._pictureBox.Name = "_pictureBox";
            this._pictureBox.Size = new System.Drawing.Size(24, 24);
            this._pictureBox.TabIndex = 0;
            this._pictureBox.TabStop = false;
            // 
            // _message
            // 
            this._message.AutoSize = true;
            this._message.Location = new System.Drawing.Point(93, 142);
            this._message.Margin = new System.Windows.Forms.Padding(3);
            this._message.Name = "_message";
            this._message.Size = new System.Drawing.Size(21, 13);
            this._message.TabIndex = 1;
            this._message.Text = "{0}";
            // 
            // MessagePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.formFooter1);
            this.Name = "MessagePage";
            this.Size = new System.Drawing.Size(484, 362);
            this.formFooter1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FormFooter formFooter1;
        private System.Windows.Forms.Button _acceptButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox _pictureBox;
        private System.Windows.Forms.Label _message;
    }
}

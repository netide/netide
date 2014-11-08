namespace NetIde.Services.WaitDialog
{
    partial class WaitDialogForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._message = new System.Windows.Forms.Label();
            this._progressText = new System.Windows.Forms.Label();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this._cancelButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this._caption = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(469, 132);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.Controls.Add(this._message, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._progressText, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this._progressBar, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this._cancelButton, 2, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 34);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(469, 98);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // _message
            // 
            this._message.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this._message, 2);
            this._message.Dock = System.Windows.Forms.DockStyle.Fill;
            this._message.Location = new System.Drawing.Point(11, 15);
            this._message.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this._message.Name = "_message";
            this._message.Size = new System.Drawing.Size(447, 13);
            this._message.TabIndex = 0;
            this._message.Text = "<<message>>";
            // 
            // _progressText
            // 
            this._progressText.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this._progressText, 2);
            this._progressText.Dock = System.Windows.Forms.DockStyle.Fill;
            this._progressText.Location = new System.Drawing.Point(11, 40);
            this._progressText.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this._progressText.Name = "_progressText";
            this._progressText.Size = new System.Drawing.Size(447, 13);
            this._progressText.TabIndex = 1;
            this._progressText.Text = "<<progress text>>";
            // 
            // _progressBar
            // 
            this._progressBar.Dock = System.Windows.Forms.DockStyle.Top;
            this._progressBar.Location = new System.Drawing.Point(11, 65);
            this._progressBar.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this._progressBar.MarqueeAnimationSpeed = 20;
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(338, 16);
            this._progressBar.TabIndex = 2;
            // 
            // _cancelButton
            // 
            this._cancelButton.Location = new System.Drawing.Point(358, 62);
            this._cancelButton.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(100, 23);
            this._cancelButton.TabIndex = 3;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this._caption);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.panel2.Size = new System.Drawing.Size(469, 34);
            this.panel2.TabIndex = 0;
            // 
            // _caption
            // 
            this._caption.Dock = System.Windows.Forms.DockStyle.Fill;
            this._caption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._caption.Location = new System.Drawing.Point(10, 0);
            this._caption.Name = "_caption";
            this._caption.Size = new System.Drawing.Size(449, 34);
            this._caption.TabIndex = 1;
            this._caption.Text = "<<caption>>";
            this._caption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WaitDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ClientSize = new System.Drawing.Size(471, 134);
            this.CloseButtonEnabled = false;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WaitDialogForm";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label _message;
        private System.Windows.Forms.Label _progressText;
        private System.Windows.Forms.ProgressBar _progressBar;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label _caption;
    }
}
namespace NetIde.Services.JobManager
{
    partial class JobProgressControl
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
            this._titleLabel = new System.Windows.Forms.Label();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this._statusLabel = new System.Windows.Forms.Label();
            this._clientArea = new System.Windows.Forms.TableLayoutPanel();
            this._clientArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // _titleLabel
            // 
            this._titleLabel.AutoEllipsis = true;
            this._titleLabel.AutoSize = true;
            this._titleLabel.Location = new System.Drawing.Point(3, 0);
            this._titleLabel.Name = "_titleLabel";
            this._titleLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this._titleLabel.Size = new System.Drawing.Size(61, 17);
            this._titleLabel.TabIndex = 0;
            this._titleLabel.Text = "Initializing...";
            // 
            // _progressBar
            // 
            this._progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._progressBar.Location = new System.Drawing.Point(3, 20);
            this._progressBar.Maximum = 1000;
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(312, 15);
            this._progressBar.TabIndex = 1;
            // 
            // _statusLabel
            // 
            this._statusLabel.AutoEllipsis = true;
            this._statusLabel.AutoSize = true;
            this._statusLabel.Location = new System.Drawing.Point(3, 38);
            this._statusLabel.Name = "_statusLabel";
            this._statusLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this._statusLabel.Size = new System.Drawing.Size(61, 17);
            this._statusLabel.TabIndex = 0;
            this._statusLabel.Text = "Initializing...";
            // 
            // _clientArea
            // 
            this._clientArea.AutoSize = true;
            this._clientArea.ColumnCount = 1;
            this._clientArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._clientArea.Controls.Add(this._titleLabel, 0, 0);
            this._clientArea.Controls.Add(this._statusLabel, 0, 2);
            this._clientArea.Controls.Add(this._progressBar, 0, 1);
            this._clientArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this._clientArea.Location = new System.Drawing.Point(4, 2);
            this._clientArea.Name = "_clientArea";
            this._clientArea.RowCount = 3;
            this._clientArea.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._clientArea.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._clientArea.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._clientArea.Size = new System.Drawing.Size(318, 55);
            this._clientArea.TabIndex = 2;
            // 
            // BackgroundJobProgressControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this._clientArea);
            this.Name = "BackgroundJobProgressControl";
            this.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Size = new System.Drawing.Size(326, 59);
            this.Load += new System.EventHandler(this.BackgroundJobProgressControl_Load);
            this._clientArea.ResumeLayout(false);
            this._clientArea.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _titleLabel;
        private System.Windows.Forms.ProgressBar _progressBar;
        private System.Windows.Forms.Label _statusLabel;
        private System.Windows.Forms.TableLayoutPanel _clientArea;
    }
}

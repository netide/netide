namespace NetIde.Services.JobManager
{
    partial class JobForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobForm));
            this._informationPictureBox = new System.Windows.Forms.PictureBox();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this._acceptButton = new System.Windows.Forms.Button();
            this._progressItemsOuterPanel = new NetIde.Util.Forms.ThemedPanel();
            this._progressItemsPanel = new System.Windows.Forms.Panel();
            this._inProgressLabel = new System.Windows.Forms.Label();
            this._detailsButton = new System.Windows.Forms.Button();
            this._clientArea = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this._statusTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this._informationPictureBox)).BeginInit();
            this._progressItemsOuterPanel.SuspendLayout();
            this._clientArea.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _informationPictureBox
            // 
            this._informationPictureBox.Image = global::NetIde.NeutralResources.Information;
            this._informationPictureBox.Location = new System.Drawing.Point(3, 3);
            this._informationPictureBox.Name = "_informationPictureBox";
            this._informationPictureBox.Size = new System.Drawing.Size(32, 32);
            this._informationPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this._informationPictureBox.TabIndex = 0;
            this._informationPictureBox.TabStop = false;
            // 
            // _progressBar
            // 
            this._progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._clientArea.SetColumnSpan(this._progressBar, 2);
            this._progressBar.Location = new System.Drawing.Point(3, 43);
            this._progressBar.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(439, 19);
            this._progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this._progressBar.TabIndex = 1;
            // 
            // _acceptButton
            // 
            this._acceptButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._acceptButton.Location = new System.Drawing.Point(3, 3);
            this._acceptButton.Name = "_acceptButton";
            this._acceptButton.Size = new System.Drawing.Size(75, 23);
            this._acceptButton.TabIndex = 3;
            this._acceptButton.Text = "Close";
            this._acceptButton.UseVisualStyleBackColor = true;
            // 
            // _progressItemsOuterPanel
            // 
            this._clientArea.SetColumnSpan(this._progressItemsOuterPanel, 2);
            this._progressItemsOuterPanel.Controls.Add(this._progressItemsPanel);
            this._progressItemsOuterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._progressItemsOuterPanel.Location = new System.Drawing.Point(3, 93);
            this._progressItemsOuterPanel.Name = "_progressItemsOuterPanel";
            this._progressItemsOuterPanel.Size = new System.Drawing.Size(439, 115);
            this._progressItemsOuterPanel.TabIndex = 4;
            // 
            // _progressItemsPanel
            // 
            this._progressItemsPanel.AutoScroll = true;
            this._progressItemsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._progressItemsPanel.Location = new System.Drawing.Point(0, 0);
            this._progressItemsPanel.Name = "_progressItemsPanel";
            this._progressItemsPanel.Size = new System.Drawing.Size(437, 113);
            this._progressItemsPanel.TabIndex = 0;
            // 
            // _inProgressLabel
            // 
            this._inProgressLabel.AutoSize = true;
            this._inProgressLabel.Location = new System.Drawing.Point(41, 3);
            this._inProgressLabel.Margin = new System.Windows.Forms.Padding(3);
            this._inProgressLabel.Name = "_inProgressLabel";
            this._inProgressLabel.Size = new System.Drawing.Size(116, 13);
            this._inProgressLabel.TabIndex = 6;
            this._inProgressLabel.Text = "Operation in progress...";
            // 
            // _detailsButton
            // 
            this._detailsButton.Location = new System.Drawing.Point(84, 3);
            this._detailsButton.Name = "_detailsButton";
            this._detailsButton.Size = new System.Drawing.Size(88, 23);
            this._detailsButton.TabIndex = 7;
            this._detailsButton.UseVisualStyleBackColor = true;
            this._detailsButton.Click += new System.EventHandler(this._detailsButton_Click);
            // 
            // _clientArea
            // 
            this._clientArea.ColumnCount = 2;
            this._clientArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._clientArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._clientArea.Controls.Add(this._informationPictureBox, 0, 0);
            this._clientArea.Controls.Add(this._inProgressLabel, 1, 0);
            this._clientArea.Controls.Add(this._progressItemsOuterPanel, 0, 3);
            this._clientArea.Controls.Add(this._progressBar, 0, 1);
            this._clientArea.Controls.Add(this.flowLayoutPanel1, 1, 4);
            this._clientArea.Controls.Add(this._statusTextBox, 0, 2);
            this._clientArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this._clientArea.Location = new System.Drawing.Point(10, 10);
            this._clientArea.Name = "_clientArea";
            this._clientArea.RowCount = 5;
            this._clientArea.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._clientArea.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._clientArea.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._clientArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._clientArea.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._clientArea.Size = new System.Drawing.Size(445, 240);
            this._clientArea.TabIndex = 8;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this._acceptButton);
            this.flowLayoutPanel1.Controls.Add(this._detailsButton);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(270, 211);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(175, 29);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // _statusTextBox
            // 
            this._statusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._statusTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._clientArea.SetColumnSpan(this._statusTextBox, 2);
            this._statusTextBox.Location = new System.Drawing.Point(3, 72);
            this._statusTextBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this._statusTextBox.Name = "_statusTextBox";
            this._statusTextBox.ReadOnly = true;
            this._statusTextBox.Size = new System.Drawing.Size(439, 13);
            this._statusTextBox.TabIndex = 8;
            this._statusTextBox.TabStop = false;
            // 
            // JobForm
            // 
            this.AcceptButton = this._acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._acceptButton;
            this.ClientSize = new System.Drawing.Size(465, 260);
            this.Controls.Add(this._clientArea);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JobForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Processing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.JobForm_FormClosing);
            this.Load += new System.EventHandler(this.JobForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this._informationPictureBox)).EndInit();
            this._progressItemsOuterPanel.ResumeLayout(false);
            this._clientArea.ResumeLayout(false);
            this._clientArea.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox _informationPictureBox;
        private System.Windows.Forms.ProgressBar _progressBar;
        private System.Windows.Forms.Button _acceptButton;
        private NetIde.Util.Forms.ThemedPanel _progressItemsOuterPanel;
        private System.Windows.Forms.Panel _progressItemsPanel;
        private System.Windows.Forms.Label _inProgressLabel;
        private System.Windows.Forms.Button _detailsButton;
        private System.Windows.Forms.TableLayoutPanel _clientArea;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TextBox _statusTextBox;
    }
}
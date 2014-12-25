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
            resources.ApplyResources(this._informationPictureBox, "_informationPictureBox");
            this._informationPictureBox.Name = "_informationPictureBox";
            this._informationPictureBox.TabStop = false;
            // 
            // _progressBar
            // 
            resources.ApplyResources(this._progressBar, "_progressBar");
            this._clientArea.SetColumnSpan(this._progressBar, 2);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // _acceptButton
            // 
            this._acceptButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this._acceptButton, "_acceptButton");
            this._acceptButton.Name = "_acceptButton";
            this._acceptButton.UseVisualStyleBackColor = true;
            // 
            // _progressItemsOuterPanel
            // 
            this._clientArea.SetColumnSpan(this._progressItemsOuterPanel, 2);
            this._progressItemsOuterPanel.Controls.Add(this._progressItemsPanel);
            resources.ApplyResources(this._progressItemsOuterPanel, "_progressItemsOuterPanel");
            this._progressItemsOuterPanel.Name = "_progressItemsOuterPanel";
            // 
            // _progressItemsPanel
            // 
            resources.ApplyResources(this._progressItemsPanel, "_progressItemsPanel");
            this._progressItemsPanel.Name = "_progressItemsPanel";
            // 
            // _inProgressLabel
            // 
            resources.ApplyResources(this._inProgressLabel, "_inProgressLabel");
            this._inProgressLabel.Name = "_inProgressLabel";
            // 
            // _detailsButton
            // 
            resources.ApplyResources(this._detailsButton, "_detailsButton");
            this._detailsButton.Name = "_detailsButton";
            this._detailsButton.UseVisualStyleBackColor = true;
            this._detailsButton.Click += new System.EventHandler(this._detailsButton_Click);
            // 
            // _clientArea
            // 
            resources.ApplyResources(this._clientArea, "_clientArea");
            this._clientArea.Controls.Add(this._informationPictureBox, 0, 0);
            this._clientArea.Controls.Add(this._inProgressLabel, 1, 0);
            this._clientArea.Controls.Add(this._progressItemsOuterPanel, 0, 3);
            this._clientArea.Controls.Add(this._progressBar, 0, 1);
            this._clientArea.Controls.Add(this.flowLayoutPanel1, 1, 4);
            this._clientArea.Controls.Add(this._statusTextBox, 0, 2);
            this._clientArea.Name = "_clientArea";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this._acceptButton);
            this.flowLayoutPanel1.Controls.Add(this._detailsButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // _statusTextBox
            // 
            resources.ApplyResources(this._statusTextBox, "_statusTextBox");
            this._statusTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._clientArea.SetColumnSpan(this._statusTextBox, 2);
            this._statusTextBox.Name = "_statusTextBox";
            this._statusTextBox.ReadOnly = true;
            this._statusTextBox.TabStop = false;
            // 
            // JobForm
            // 
            this.AcceptButton = this._acceptButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._acceptButton;
            this.Controls.Add(this._clientArea);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JobForm";
            this.ShowInTaskbar = false;
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
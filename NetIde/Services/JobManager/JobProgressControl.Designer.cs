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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobProgressControl));
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
            resources.ApplyResources(this._titleLabel, "_titleLabel");
            this._titleLabel.Name = "_titleLabel";
            // 
            // _progressBar
            // 
            resources.ApplyResources(this._progressBar, "_progressBar");
            this._progressBar.Maximum = 1000;
            this._progressBar.Name = "_progressBar";
            // 
            // _statusLabel
            // 
            this._statusLabel.AutoEllipsis = true;
            resources.ApplyResources(this._statusLabel, "_statusLabel");
            this._statusLabel.Name = "_statusLabel";
            // 
            // _clientArea
            // 
            resources.ApplyResources(this._clientArea, "_clientArea");
            this._clientArea.Controls.Add(this._titleLabel, 0, 0);
            this._clientArea.Controls.Add(this._statusLabel, 0, 2);
            this._clientArea.Controls.Add(this._progressBar, 0, 1);
            this._clientArea.Name = "_clientArea";
            // 
            // JobProgressControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this._clientArea);
            this.Name = "JobProgressControl";
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

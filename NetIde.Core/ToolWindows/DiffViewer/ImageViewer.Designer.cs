namespace NetIde.Core.ToolWindows.DiffViewer
{
    partial class ImageViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageViewer));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._leftImageContainer = new NetIde.Util.Forms.ThemedPanel();
            this._leftImage = new System.Windows.Forms.PictureBox();
            this._leftDetails = new NetIde.Core.ToolWindows.DiffViewer.StreamDetailsControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this._rightImageContainer = new NetIde.Util.Forms.ThemedPanel();
            this._rightImage = new System.Windows.Forms.PictureBox();
            this._rightDetails = new NetIde.Core.ToolWindows.DiffViewer.StreamDetailsControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this._leftImageContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._leftImage)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this._rightImageContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._rightImage)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel1.Controls.Add(this._leftDetails);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer1.Panel2.Controls.Add(this._rightDetails);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this._leftImageContainer, 1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // _leftImageContainer
            // 
            resources.ApplyResources(this._leftImageContainer, "_leftImageContainer");
            this._leftImageContainer.Controls.Add(this._leftImage);
            this._leftImageContainer.Name = "_leftImageContainer";
            // 
            // _leftImage
            // 
            resources.ApplyResources(this._leftImage, "_leftImage");
            this._leftImage.Name = "_leftImage";
            this._leftImage.TabStop = false;
            // 
            // _leftDetails
            // 
            this._leftDetails.CanOverflow = false;
            this._leftDetails.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            resources.ApplyResources(this._leftDetails, "_leftDetails");
            this._leftDetails.Name = "_leftDetails";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this._rightImageContainer, 1, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // _rightImageContainer
            // 
            resources.ApplyResources(this._rightImageContainer, "_rightImageContainer");
            this._rightImageContainer.Controls.Add(this._rightImage);
            this._rightImageContainer.Name = "_rightImageContainer";
            // 
            // _rightImage
            // 
            resources.ApplyResources(this._rightImage, "_rightImage");
            this._rightImage.Name = "_rightImage";
            this._rightImage.TabStop = false;
            // 
            // _rightDetails
            // 
            this._rightDetails.CanOverflow = false;
            this._rightDetails.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            resources.ApplyResources(this._rightDetails, "_rightDetails");
            this._rightDetails.Name = "_rightDetails";
            // 
            // ImageViewer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ImageViewer";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this._leftImageContainer.ResumeLayout(false);
            this._leftImageContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._leftImage)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this._rightImageContainer.ResumeLayout(false);
            this._rightImageContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._rightImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Util.Forms.ThemedPanel _leftImageContainer;
        private System.Windows.Forms.PictureBox _leftImage;
        private StreamDetailsControl _leftDetails;
        private Util.Forms.ThemedPanel _rightImageContainer;
        private System.Windows.Forms.PictureBox _rightImage;
        private StreamDetailsControl _rightDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}

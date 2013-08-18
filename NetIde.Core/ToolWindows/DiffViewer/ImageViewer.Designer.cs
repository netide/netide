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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._leftImageContainer = new NetIde.Util.Forms.ThemedPanel();
            this._leftImage = new System.Windows.Forms.PictureBox();
            this._rightImageContainer = new NetIde.Util.Forms.ThemedPanel();
            this._rightImage = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this._leftDetails = new NetIde.Core.ToolWindows.DiffViewer.StreamDetailsControl();
            this._rightDetails = new NetIde.Core.ToolWindows.DiffViewer.StreamDetailsControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this._leftImageContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._leftImage)).BeginInit();
            this._rightImageContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._rightImage)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
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
            this.splitContainer1.Size = new System.Drawing.Size(603, 536);
            this.splitContainer1.SplitterDistance = 297;
            this.splitContainer1.TabIndex = 0;
            // 
            // _leftImageContainer
            // 
            this._leftImageContainer.AutoSize = true;
            this._leftImageContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._leftImageContainer.Controls.Add(this._leftImage);
            this._leftImageContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._leftImageContainer.Location = new System.Drawing.Point(147, 254);
            this._leftImageContainer.Name = "_leftImageContainer";
            this._leftImageContainer.Size = new System.Drawing.Size(2, 2);
            this._leftImageContainer.TabIndex = 1;
            // 
            // _leftImage
            // 
            this._leftImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this._leftImage.Location = new System.Drawing.Point(1, 1);
            this._leftImage.Name = "_leftImage";
            this._leftImage.Size = new System.Drawing.Size(0, 0);
            this._leftImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this._leftImage.TabIndex = 0;
            this._leftImage.TabStop = false;
            // 
            // _rightImageContainer
            // 
            this._rightImageContainer.AutoSize = true;
            this._rightImageContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._rightImageContainer.Controls.Add(this._rightImage);
            this._rightImageContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rightImageContainer.Location = new System.Drawing.Point(150, 254);
            this._rightImageContainer.Name = "_rightImageContainer";
            this._rightImageContainer.Size = new System.Drawing.Size(2, 2);
            this._rightImageContainer.TabIndex = 1;
            // 
            // _rightImage
            // 
            this._rightImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rightImage.Location = new System.Drawing.Point(1, 1);
            this._rightImage.Name = "_rightImage";
            this._rightImage.Size = new System.Drawing.Size(0, 0);
            this._rightImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this._rightImage.TabIndex = 0;
            this._rightImage.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this._leftImageContainer, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(297, 511);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this._rightImageContainer, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(302, 511);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // _leftDetails
            // 
            this._leftDetails.CanOverflow = false;
            this._leftDetails.ContentType = "";
            this._leftDetails.Encoding = null;
            this._leftDetails.FileSize = null;
            this._leftDetails.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._leftDetails.HaveBom = false;
            this._leftDetails.LastWriteTime = null;
            this._leftDetails.LineTermination = null;
            this._leftDetails.Location = new System.Drawing.Point(0, 0);
            this._leftDetails.Name = "_leftDetails";
            this._leftDetails.Size = new System.Drawing.Size(297, 25);
            this._leftDetails.TabIndex = 0;
            // 
            // _rightDetails
            // 
            this._rightDetails.CanOverflow = false;
            this._rightDetails.ContentType = "";
            this._rightDetails.Encoding = null;
            this._rightDetails.FileSize = null;
            this._rightDetails.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._rightDetails.HaveBom = false;
            this._rightDetails.LastWriteTime = null;
            this._rightDetails.LineTermination = null;
            this._rightDetails.Location = new System.Drawing.Point(0, 0);
            this._rightDetails.Name = "_rightDetails";
            this._rightDetails.Size = new System.Drawing.Size(302, 25);
            this._rightDetails.TabIndex = 0;
            // 
            // ImageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ImageViewer";
            this.Size = new System.Drawing.Size(603, 536);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this._leftImageContainer.ResumeLayout(false);
            this._leftImageContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._leftImage)).EndInit();
            this._rightImageContainer.ResumeLayout(false);
            this._rightImageContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._rightImage)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
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

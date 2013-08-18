namespace NetIde.Core.ToolWindows.DiffViewer
{
    partial class SummaryViewer
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._leftDetails = new NetIde.Core.ToolWindows.DiffViewer.StreamDetailsControl();
            this._rightDetails = new NetIde.Core.ToolWindows.DiffViewer.StreamDetailsControl();
            this._summary = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this._leftDetails, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._rightDetails, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this._summary, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(500, 500);
            this.tableLayoutPanel1.TabIndex = 1;
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
            this._leftDetails.Size = new System.Drawing.Size(249, 25);
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
            this._rightDetails.Location = new System.Drawing.Point(251, 0);
            this._rightDetails.Name = "_rightDetails";
            this._rightDetails.Size = new System.Drawing.Size(249, 25);
            this._rightDetails.TabIndex = 1;
            // 
            // _summary
            // 
            this._summary.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel1.SetColumnSpan(this._summary, 3);
            this._summary.Dock = System.Windows.Forms.DockStyle.Fill;
            this._summary.Location = new System.Drawing.Point(10, 35);
            this._summary.Margin = new System.Windows.Forms.Padding(10);
            this._summary.Multiline = true;
            this._summary.Name = "_summary";
            this._summary.ReadOnly = true;
            this._summary.Size = new System.Drawing.Size(480, 455);
            this._summary.TabIndex = 2;
            // 
            // SummaryViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SummaryViewer";
            this.Size = new System.Drawing.Size(500, 500);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private StreamDetailsControl _leftDetails;
        private StreamDetailsControl _rightDetails;
        private System.Windows.Forms.TextBox _summary;

    }
}

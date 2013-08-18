namespace NetIde.Core.PackageManagement
{
    partial class PackageControl
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
            this._panel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this._icon = new System.Windows.Forms.PictureBox();
            this._title = new System.Windows.Forms.Label();
            this._description = new System.Windows.Forms.Label();
            this._panel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._icon)).BeginInit();
            this.SuspendLayout();
            // 
            // _panel
            // 
            this._panel.AutoSize = true;
            this._panel.ColumnCount = 2;
            this._panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._panel.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this._panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panel.Location = new System.Drawing.Point(4, 4);
            this._panel.Name = "_panel";
            this._panel.RowCount = 2;
            this._panel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._panel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._panel.Size = new System.Drawing.Size(150, 58);
            this._panel.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this._icon, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this._title, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this._description, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this._panel.SetRowSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(144, 52);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // _icon
            // 
            this._icon.ErrorImage = null;
            this._icon.InitialImage = null;
            this._icon.Location = new System.Drawing.Point(3, 3);
            this._icon.Name = "_icon";
            this.tableLayoutPanel2.SetRowSpan(this._icon, 2);
            this._icon.Size = new System.Drawing.Size(32, 32);
            this._icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._icon.TabIndex = 0;
            this._icon.TabStop = false;
            // 
            // _title
            // 
            this._title.AutoEllipsis = true;
            this._title.AutoSize = true;
            this._title.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._title.Location = new System.Drawing.Point(41, 0);
            this._title.Margin = new System.Windows.Forms.Padding(3, 0, 3, 4);
            this._title.Name = "_title";
            this._title.Size = new System.Drawing.Size(0, 15);
            this._title.TabIndex = 1;
            // 
            // _description
            // 
            this._description.Dock = System.Windows.Forms.DockStyle.Fill;
            this._description.Location = new System.Drawing.Point(41, 19);
            this._description.Name = "_description";
            this._description.Size = new System.Drawing.Size(100, 33);
            this._description.TabIndex = 2;
            // 
            // PackageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this._panel);
            this.Name = "PackageControl";
            this.Padding = new System.Windows.Forms.Padding(4, 4, 20, 4);
            this.Size = new System.Drawing.Size(174, 66);
            this._panel.ResumeLayout(false);
            this._panel.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._icon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _panel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox _icon;
        private System.Windows.Forms.Label _title;
        private System.Windows.Forms.Label _description;
    }
}

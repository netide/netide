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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageControl));
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
            resources.ApplyResources(this._panel, "_panel");
            this._panel.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this._panel.Name = "_panel";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this._icon, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this._title, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this._description, 1, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this._panel.SetRowSpan(this.tableLayoutPanel2, 2);
            // 
            // _icon
            // 
            resources.ApplyResources(this._icon, "_icon");
            this._icon.Name = "_icon";
            this.tableLayoutPanel2.SetRowSpan(this._icon, 2);
            this._icon.TabStop = false;
            // 
            // _title
            // 
            this._title.AutoEllipsis = true;
            resources.ApplyResources(this._title, "_title");
            this._title.Name = "_title";
            // 
            // _description
            // 
            resources.ApplyResources(this._description, "_description");
            this._description.Name = "_description";
            // 
            // PackageControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._panel);
            this.Name = "PackageControl";
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

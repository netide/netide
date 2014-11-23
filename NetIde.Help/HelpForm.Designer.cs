namespace NetIde.Help
{
    partial class HelpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpForm));
            this._toopStrip1 = new System.Windows.Forms.ToolStrip();
            this._back = new System.Windows.Forms.ToolStripButton();
            this._forward = new System.Windows.Forms.ToolStripButton();
            this._refresh = new System.Windows.Forms.ToolStripButton();
            this._stop = new System.Windows.Forms.ToolStripButton();
            this._home = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._print = new System.Windows.Forms.ToolStripButton();
            this._toolStrip2 = new System.Windows.Forms.ToolStrip();
            this._search = new NetIde.Util.Forms.ToolStripSpringTextBox();
            this._find = new System.Windows.Forms.ToolStripButton();
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this._webBrowser = new System.Windows.Forms.WebBrowser();
            this._toopStrip1.SuspendLayout();
            this._toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toopStrip1
            // 
            this._toopStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toopStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._back,
            this._forward,
            this._refresh,
            this._stop,
            this._home,
            this.toolStripSeparator1,
            this._print});
            this._toopStrip1.Location = new System.Drawing.Point(0, 0);
            this._toopStrip1.Name = "_toopStrip1";
            this._toopStrip1.Size = new System.Drawing.Size(361, 25);
            this._toopStrip1.TabIndex = 0;
            this._toopStrip1.Text = "toolStrip1";
            // 
            // _back
            // 
            this._back.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._back.Image = global::NetIde.Help.NeutralResources.arrow_left_blue;
            this._back.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._back.Name = "_back";
            this._back.Size = new System.Drawing.Size(23, 22);
            this._back.Text = "&Back";
            this._back.Click += new System.EventHandler(this._back_Click);
            // 
            // _forward
            // 
            this._forward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._forward.Image = global::NetIde.Help.NeutralResources.arrow_right_blue;
            this._forward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._forward.Name = "_forward";
            this._forward.Size = new System.Drawing.Size(23, 22);
            this._forward.Text = "&Forward";
            this._forward.Click += new System.EventHandler(this._forward_Click);
            // 
            // _refresh
            // 
            this._refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._refresh.Image = global::NetIde.Help.NeutralResources.refresh;
            this._refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._refresh.Name = "_refresh";
            this._refresh.Size = new System.Drawing.Size(23, 22);
            this._refresh.Text = "&Refresh (F5)";
            this._refresh.Click += new System.EventHandler(this._refresh_Click);
            // 
            // _stop
            // 
            this._stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._stop.Image = global::NetIde.Help.NeutralResources.stop;
            this._stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._stop.Name = "_stop";
            this._stop.Size = new System.Drawing.Size(23, 22);
            this._stop.Text = "&Stop";
            this._stop.Click += new System.EventHandler(this._stop_Click);
            // 
            // _home
            // 
            this._home.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._home.Image = global::NetIde.Help.NeutralResources.home;
            this._home.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._home.Name = "_home";
            this._home.Size = new System.Drawing.Size(23, 22);
            this._home.Text = "&Home";
            this._home.Click += new System.EventHandler(this._home_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // _print
            // 
            this._print.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._print.Image = global::NetIde.Help.NeutralResources.printer;
            this._print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._print.Name = "_print";
            this._print.Size = new System.Drawing.Size(23, 22);
            this._print.Text = "&Print (Ctrl+P)";
            this._print.Click += new System.EventHandler(this._print_Click);
            // 
            // _toolStrip2
            // 
            this._toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._search,
            this._find});
            this._toolStrip2.Location = new System.Drawing.Point(0, 25);
            this._toolStrip2.Name = "_toolStrip2";
            this._toolStrip2.Size = new System.Drawing.Size(361, 25);
            this._toolStrip2.TabIndex = 1;
            this._toolStrip2.Text = "toolStrip2";
            // 
            // _search
            // 
            this._search.AcceptsReturn = true;
            this._search.Name = "_search";
            this._search.Size = new System.Drawing.Size(277, 25);
            this._search.Spring = true;
            this._search.KeyUp += new System.Windows.Forms.KeyEventHandler(this._search_KeyUp);
            // 
            // _find
            // 
            this._find.Image = global::NetIde.Help.NeutralResources.find;
            this._find.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._find.Name = "_find";
            this._find.Size = new System.Drawing.Size(50, 22);
            this._find.Text = "&Find";
            this._find.ToolTipText = "Find (Ctrl+F)";
            this._find.Click += new System.EventHandler(this._find_Click);
            // 
            // _statusStrip
            // 
            this._statusStrip.Location = new System.Drawing.Point(0, 499);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size(361, 22);
            this._statusStrip.TabIndex = 3;
            this._statusStrip.Text = "statusStrip1";
            // 
            // _webBrowser
            // 
            this._webBrowser.AllowWebBrowserDrop = false;
            this._webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._webBrowser.IsWebBrowserContextMenuEnabled = false;
            this._webBrowser.Location = new System.Drawing.Point(0, 50);
            this._webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this._webBrowser.Name = "_webBrowser";
            this._webBrowser.ScriptErrorsSuppressed = true;
            this._webBrowser.Size = new System.Drawing.Size(361, 449);
            this._webBrowser.TabIndex = 2;
            this._webBrowser.WebBrowserShortcutsEnabled = false;
            this._webBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this._webBrowser_Navigated);
            this._webBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this._webBrowser_Navigating);
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 521);
            this.Controls.Add(this._webBrowser);
            this.Controls.Add(this._statusStrip);
            this.Controls.Add(this._toolStrip2);
            this.Controls.Add(this._toopStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HelpForm";
            this.Text = "Help";
            this.BrowseButtonClick += new NetIde.Util.Forms.BrowseButtonEventHandler(this.HelpForm_BrowseButtonClick);
            this._toopStrip1.ResumeLayout(false);
            this._toopStrip1.PerformLayout();
            this._toolStrip2.ResumeLayout(false);
            this._toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toopStrip1;
        private System.Windows.Forms.ToolStripButton _back;
        private System.Windows.Forms.ToolStripButton _forward;
        private System.Windows.Forms.ToolStripButton _refresh;
        private System.Windows.Forms.ToolStripButton _home;
        private System.Windows.Forms.ToolStripButton _print;
        private System.Windows.Forms.ToolStrip _toolStrip2;
        private Util.Forms.ToolStripSpringTextBox _search;
        private System.Windows.Forms.ToolStripButton _find;
        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStripButton _stop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.WebBrowser _webBrowser;
    }
}
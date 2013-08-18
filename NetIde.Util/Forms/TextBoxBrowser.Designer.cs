namespace NetIde.Util.Forms
{
    partial class TextBoxBrowser
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
            this._textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _textBox
            // 
            this._textBox.Location = new System.Drawing.Point(0, 1);
            this._textBox.Name = "_textBox";
            this._textBox.Size = new System.Drawing.Size(275, 20);
            this._textBox.TabIndex = 2;
            this._textBox.TextChanged += new System.EventHandler(this._textBox_TextChanged);
            // 
            // TextBoxBrowser
            // 
            this.Controls.Add(this._textBox);
            this.Name = "TextBoxBrowser";
            this.Controls.SetChildIndex(this._textBox, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _textBox;

    }
}

namespace NetIde.Core.OptionPages.Environment
{
    partial class KeyboardOptionControl
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
            this.label1 = new System.Windows.Forms.Label();
            this._filter = new System.Windows.Forms.TextBox();
            this._commands = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._assignedShortcuts = new System.Windows.Forms.ComboBox();
            this._remove = new System.Windows.Forms.Button();
            this._assign = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this._shortcutsConflicts = new System.Windows.Forms.ComboBox();
            this._shortcut = new NetIde.Core.OptionPages.Environment.KeysTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._filter, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._commands, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this._assignedShortcuts, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this._remove, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this._assign, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this._shortcutsConflicts, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this._shortcut, 0, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(489, 345);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(483, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Show commands containing:";
            // 
            // _filter
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._filter, 2);
            this._filter.Dock = System.Windows.Forms.DockStyle.Fill;
            this._filter.Location = new System.Drawing.Point(3, 16);
            this._filter.Name = "_filter";
            this._filter.Size = new System.Drawing.Size(483, 20);
            this._filter.TabIndex = 1;
            this._filter.TextChanged += new System.EventHandler(this._filter_TextChanged);
            // 
            // _commands
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._commands, 2);
            this._commands.Dock = System.Windows.Forms.DockStyle.Fill;
            this._commands.FormattingEnabled = true;
            this._commands.Location = new System.Drawing.Point(3, 42);
            this._commands.Name = "_commands";
            this._commands.Size = new System.Drawing.Size(483, 149);
            this._commands.TabIndex = 2;
            this._commands.SelectedIndexChanged += new System.EventHandler(this._commands_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 2);
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 200);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(483, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Shor&tcut for selected command:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label3, 2);
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 251);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(483, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "&Press shortcut keys:";
            // 
            // _assignedShortcuts
            // 
            this._assignedShortcuts.Dock = System.Windows.Forms.DockStyle.Fill;
            this._assignedShortcuts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._assignedShortcuts.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._assignedShortcuts.FormattingEnabled = true;
            this._assignedShortcuts.Location = new System.Drawing.Point(3, 219);
            this._assignedShortcuts.Name = "_assignedShortcuts";
            this._assignedShortcuts.Size = new System.Drawing.Size(377, 21);
            this._assignedShortcuts.TabIndex = 4;
            this._assignedShortcuts.SelectedIndexChanged += new System.EventHandler(this._assignedShortcuts_SelectedIndexChanged);
            // 
            // _remove
            // 
            this._remove.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._remove.Location = new System.Drawing.Point(386, 219);
            this._remove.Name = "_remove";
            this._remove.Size = new System.Drawing.Size(100, 23);
            this._remove.TabIndex = 5;
            this._remove.Text = "Remove";
            this._remove.UseVisualStyleBackColor = true;
            this._remove.Click += new System.EventHandler(this._remove_Click);
            // 
            // _assign
            // 
            this._assign.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._assign.Location = new System.Drawing.Point(386, 270);
            this._assign.Name = "_assign";
            this._assign.Size = new System.Drawing.Size(100, 23);
            this._assign.TabIndex = 8;
            this._assign.Text = "A&ssign";
            this._assign.UseVisualStyleBackColor = true;
            this._assign.Click += new System.EventHandler(this._assign_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label4, 2);
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 302);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(483, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Shortcut c&urrently used by:";
            // 
            // _shortcutsConflicts
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._shortcutsConflicts, 2);
            this._shortcutsConflicts.Dock = System.Windows.Forms.DockStyle.Fill;
            this._shortcutsConflicts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._shortcutsConflicts.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._shortcutsConflicts.FormattingEnabled = true;
            this._shortcutsConflicts.Location = new System.Drawing.Point(3, 321);
            this._shortcutsConflicts.Name = "_shortcutsConflicts";
            this._shortcutsConflicts.Size = new System.Drawing.Size(483, 21);
            this._shortcutsConflicts.TabIndex = 10;
            // 
            // _shortcut
            // 
            this._shortcut.Dock = System.Windows.Forms.DockStyle.Fill;
            this._shortcut.Keys = System.Windows.Forms.Keys.None;
            this._shortcut.Location = new System.Drawing.Point(3, 270);
            this._shortcut.Name = "_shortcut";
            this._shortcut.Size = new System.Drawing.Size(377, 20);
            this._shortcut.TabIndex = 7;
            this._shortcut.KeysChanged += new System.EventHandler(this._shortcut_KeysChanged);
            // 
            // KeyboardOptionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "KeyboardOptionControl";
            this.Size = new System.Drawing.Size(489, 345);
            this.Apply += new System.EventHandler(this.FontsControl_Apply);
            this.Load += new System.EventHandler(this.FontsControl_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _filter;
        private System.Windows.Forms.ListBox _commands;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox _assignedShortcuts;
        private System.Windows.Forms.Button _remove;
        private System.Windows.Forms.Button _assign;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox _shortcutsConflicts;
        private KeysTextBox _shortcut;

    }
}

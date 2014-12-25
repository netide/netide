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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeyboardOptionControl));
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
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
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
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Name = "label1";
            // 
            // _filter
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._filter, 2);
            resources.ApplyResources(this._filter, "_filter");
            this._filter.Name = "_filter";
            this._filter.TextChanged += new System.EventHandler(this._filter_TextChanged);
            // 
            // _commands
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._commands, 2);
            resources.ApplyResources(this._commands, "_commands");
            this._commands.FormattingEnabled = true;
            this._commands.Name = "_commands";
            this._commands.SelectedIndexChanged += new System.EventHandler(this._commands_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 2);
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.tableLayoutPanel1.SetColumnSpan(this.label3, 2);
            this.label3.Name = "label3";
            // 
            // _assignedShortcuts
            // 
            resources.ApplyResources(this._assignedShortcuts, "_assignedShortcuts");
            this._assignedShortcuts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._assignedShortcuts.FormattingEnabled = true;
            this._assignedShortcuts.Name = "_assignedShortcuts";
            this._assignedShortcuts.SelectedIndexChanged += new System.EventHandler(this._assignedShortcuts_SelectedIndexChanged);
            // 
            // _remove
            // 
            resources.ApplyResources(this._remove, "_remove");
            this._remove.Name = "_remove";
            this._remove.UseVisualStyleBackColor = true;
            this._remove.Click += new System.EventHandler(this._remove_Click);
            // 
            // _assign
            // 
            resources.ApplyResources(this._assign, "_assign");
            this._assign.Name = "_assign";
            this._assign.UseVisualStyleBackColor = true;
            this._assign.Click += new System.EventHandler(this._assign_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.tableLayoutPanel1.SetColumnSpan(this.label4, 2);
            this.label4.Name = "label4";
            // 
            // _shortcutsConflicts
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._shortcutsConflicts, 2);
            resources.ApplyResources(this._shortcutsConflicts, "_shortcutsConflicts");
            this._shortcutsConflicts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._shortcutsConflicts.FormattingEnabled = true;
            this._shortcutsConflicts.Name = "_shortcutsConflicts";
            // 
            // _shortcut
            // 
            resources.ApplyResources(this._shortcut, "_shortcut");
            this._shortcut.Keys = System.Windows.Forms.Keys.None;
            this._shortcut.Name = "_shortcut";
            this._shortcut.KeysChanged += new System.EventHandler(this._shortcut_KeysChanged);
            // 
            // KeyboardOptionControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "KeyboardOptionControl";
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

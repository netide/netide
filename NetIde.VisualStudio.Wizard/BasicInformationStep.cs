﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetIde.VisualStudio.Wizard
{
    [WizardStep(WizardStep.BasicInformation)]
    internal partial class BasicInformationStep : WizardStepControl
    {
        public BasicInformationStep()
        {
            InitializeComponent();
        }

        private void BasicInformationStep_Load(object sender, EventArgs e)
        {
            SetIconImage();

            _companyName.Text = Configuration.ReplacementsDictionary.GetValueOrDefault(ReplacementVariables.PackageCompany, "Company");
            _packageName.Text = Configuration.ReplacementsDictionary.GetValueOrDefault(ReplacementVariables.PackageName, "Core");
            _packageTitle.Text = Configuration.ReplacementsDictionary.GetValueOrDefault(ReplacementVariables.PackageTitle, Configuration.ReplacementsDictionary["$projectname$"]);
            _detailedInformation.Text = Configuration.ReplacementsDictionary.GetValueOrDefault(ReplacementVariables.PackageDescription, "Description for my package");
        }

        private void SetIconImage()
        {
            using (var stream = new MemoryStream(Configuration.MainIcon))
            using (var icon = new Icon(stream, new Size(32, 32)))
            {
                _icon.Image = icon.ToBitmap();
            }
        }

        private void _companyName_TextChanged(object sender, EventArgs e)
        {
            Configuration.ReplacementsDictionary[ReplacementVariables.PackageCompany] = _companyName.Text;
        }

        private void _packageName_TextChanged(object sender, EventArgs e)
        {
            Configuration.ReplacementsDictionary[ReplacementVariables.PackageName] = _packageName.Text;
        }

        private void _packageTitle_TextChanged(object sender, EventArgs e)
        {
            Configuration.ReplacementsDictionary[ReplacementVariables.PackageTitle] = _packageTitle.Text;
        }

        private void _detailedInformation_TextChanged(object sender, EventArgs e)
        {
            Configuration.ReplacementsDictionary[ReplacementVariables.PackageDescription] = _detailedInformation.Text;
        }

        private void _changeIcon_Click(object sender, EventArgs e)
        {
            using (var form = new OpenFileDialog())
            {
                form.Filter = "Icon (*.ico)|*.ico|All Files (*.*)|*.*";
                form.RestoreDirectory = true;
                form.Title = "Select Package Icon";

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        Configuration.MainIcon = File.ReadAllBytes(form.FileName);
                        SetIconImage();
                    }
                    catch
                    {
                        MessageBox.Show(
                            this,
                            "Cannot open icon",
                            FindForm().Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }
            }
        }

        public override bool CanNext()
        {
            return
                 !String.IsNullOrEmpty(Configuration.ReplacementsDictionary[ReplacementVariables.PackageCompany]) &&
                 !String.IsNullOrEmpty(Configuration.ReplacementsDictionary[ReplacementVariables.PackageName]) &&
                 !String.IsNullOrEmpty(Configuration.ReplacementsDictionary[ReplacementVariables.PackageTitle]) &&
                 !String.IsNullOrEmpty(Configuration.ReplacementsDictionary[ReplacementVariables.PackageDescription]);
        }
    }
}

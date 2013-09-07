using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources.Tools;
using System.Text;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.CSharp;
using Microsoft.VisualStudio.TemplateWizard;

namespace NetIde.VisualStudio.Wizard
{
    public class NetIdeWizard : BaseWizard
    {
        private WizardConfiguration _configuration;

        public override void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, Microsoft.VisualStudio.TemplateWizard.WizardRunKind runKind, object[] customParams)
        {
            _configuration = new WizardConfiguration(replacementsDictionary);

            using (var form = new WizardForm(_configuration))
            {
                if (form.ShowDialog() != DialogResult.OK)
                    throw new WizardBackoutException();
            }

            // A few variables are still missing.

            _configuration.ReplacementsDictionary["$rootnamespace$"] = _configuration.ReplacementsDictionary["$safeprojectname$"];
            _configuration.ReplacementsDictionary[ReplacementVariables.PackageClass] = _configuration.ReplacementsDictionary["$packagename$"] + "Package";
        }

        public override void RunFinished()
        {
            string destinationDirectory = _configuration.ReplacementsDictionary["$destinationdirectory$"];

            // Save the icon.

            File.WriteAllBytes(
                Path.Combine(destinationDirectory, "Resources", "MainIcon.ico"),
                _configuration.MainIcon
            );

            // Process the key file.

            string keyFile = Path.Combine(destinationDirectory, "Key.snk");

            if (_configuration.GenerateKeyFile)
                StrongKeyUtil.GenerateStrongKeyFile(keyFile);
            else
                File.Copy(_configuration.KeyFile, keyFile);

            // Generate the Designer.cs files for all resources.

            GenerateDesignerFiles(destinationDirectory);

            // And last, rename the package class.
        }

        public override void ProjectFinishedGenerating(Project project)
        {
            RenameFile(project, "EmptyPackageSample.cs", _configuration.ReplacementsDictionary[ReplacementVariables.PackageClass] + ".cs");
        }

        private void RenameFile(Project project, string source, string target)
        {
            var sourceItem = project.ProjectItems.Item(source);

            string sourcePath = sourceItem.FileNames[0];
            string targetPath = Path.Combine(Path.GetDirectoryName(sourcePath), target);

            sourceItem.Remove();

            File.Move(sourcePath, targetPath);

            var targetItem = project.ProjectItems.AddFromFile(targetPath);

            targetItem.Properties.Item("SubType").Value = sourceItem.Properties.Item("SubType").Value;
        }

        private void GenerateDesignerFiles(string destinationDirectory)
        {
            foreach (string path in Directory.GetFiles(destinationDirectory, "*.resx", SearchOption.AllDirectories))
            {
                string designerFileName = Path.Combine(
                    Path.GetDirectoryName(path),
                    Path.GetFileNameWithoutExtension(path) + ".Designer.cs"
                );

                if (!File.Exists(designerFileName) || new FileInfo(designerFileName).Length == 0)
                    GenerateResourceCodeBehind(destinationDirectory, path, designerFileName);
            }
        }

        private void GenerateResourceCodeBehind(string basePath, string resourceFileName, string targetFileName)
        {
            using (CurrentDirectorySetter.SetCurrentDirectory(basePath))
            using (var codeProvider = new CSharpCodeProvider())
            {
                string[] unmatchedElements;
                var code = StronglyTypedResourceBuilder.Create(
                    resourceFileName,
                    Path.GetFileNameWithoutExtension(resourceFileName),
                    _configuration.ReplacementsDictionary["$safeprojectname$"],
                    codeProvider,
                    true,
                    out unmatchedElements
                );

                using (var writer = new StreamWriter(targetFileName, false, Encoding.UTF8))
                {
                    codeProvider.GenerateCodeFromCompileUnit(
                        code,
                        writer,
                        new CodeGeneratorOptions()
                    );
                }
            }
        }
    }
}

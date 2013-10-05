using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources.Tools;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using EnvDTE;
using Microsoft.CSharp;
using Microsoft.VisualStudio.TemplateWizard;
using NuGet.VisualStudio;

namespace NetIde.VisualStudio.Wizard
{
    public class NetIdeWizard : BaseWizard
    {
        private const string AggregatePackageSource = "(Aggregate source)";

        private static readonly XNamespace VsTemplateNs = "http://schemas.microsoft.com/developer/vstemplate/2005";

        private readonly IVsPackageInstaller _installer;
        private WizardConfiguration _configuration;
        private List<PackageInformation> _packages;
        private DTE _dte;

        public NetIdeWizard()
        {
            _installer = Support.ServiceLocator.GetInstance<IVsPackageInstaller>();
            if (_installer == null)
                throw new InvalidOperationException("Could not resolve the IVsPackageInstaller service");
            _dte = Support.ServiceLocator.GetInstance<DTE>();
            if (_installer == null)
                throw new InvalidOperationException("Could not resolve the DTE service");
        }

        public override void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
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

            // Seed the replacements dictionary with encoded values.

            SeedEncodedValues();

            var vsTemplatePath = (string)customParams[0];

            using (var reader = XmlReader.Create(
                vsTemplatePath,
                new XmlReaderSettings
                {
                    XmlResolver = null,
                    DtdProcessing = DtdProcessing.Prohibit,
                    IgnoreWhitespace = false
                })
            ) {
                _packages = GetPackageConfiguration(XDocument.Load(reader));
            }
        }

        private List<PackageInformation> GetPackageConfiguration(XDocument document)
        {
            // Ignore XML namespaces since VS does not check them either when loading vstemplate files.
            var packagesElement = document.Root
                .Element(VsTemplateNs + "WizardData")
                .Element(VsTemplateNs + "packages");

            var result = new List<PackageInformation>();

            foreach (var package in packagesElement.Elements(VsTemplateNs + "package"))
            {
                result.Add(new PackageInformation(
                    package.Attribute("id").Value,
                    package.Attribute("version").Value
                ));
            }

            return result;
        }

        private void SeedEncodedValues()
        {
            var encoded = new Dictionary<string, string>();

            foreach (var variable in _configuration.ReplacementsDictionary)
            {
                if (
                    variable.Key.Length > 2 &&
                    variable.Key[0] == '$' &&
                    variable.Key[variable.Key.Length - 1] == '$' &&
                    variable.Key.IndexOf(':') == -1
                ) {
                    string name = variable.Key.Substring(1, variable.Key.Length - 2);

                    encoded.Add(String.Format("${0}:xml$", name), EncodeXml(variable.Value));
                    encoded.Add(String.Format("${0}:str$", name), EncodeString(variable.Value));
                }
            }

            foreach (var variable in encoded)
            {
                _configuration.ReplacementsDictionary[variable.Key] = variable.Value;
            }
        }

        private string EncodeXml(string value)
        {
            var sb = new StringBuilder();

            foreach (char c in value)
            {
                switch (c)
                {
                    case '<': sb.Append("&lt;"); break;
                    case '>': sb.Append("&gt;"); break;
                    case '&': sb.Append("&amp;"); break;
                    case '"': sb.Append("&quot;"); break;
                    default: sb.Append(c); break;
                }
            }

            return sb.ToString();
        }

        private string EncodeString(string value)
        {
            var sb = new StringBuilder();

            foreach (char c in value)
            {
                switch (c)
                {
                    case '\a': sb.Append("\\a"); break;
                    case '\b': sb.Append("\\b"); break;
                    case '\f': sb.Append("\\f"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    case '\v': sb.Append("\\v"); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '\0': sb.Append("\\0"); break;
                    case '"': sb.Append("\\\""); break;
                    default: sb.Append(c); break;
                }
            }

            return sb.ToString();
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
        }

        public override void ProjectFinishedGenerating(Project project)
        {
            RenameFile(
                project,
                "__PACKAGECLASS__.cs",
                _configuration.ReplacementsDictionary[ReplacementVariables.PackageClass] + ".cs"
            );
            RenameFile(
                project,
                "__CONTEXT__.Package.__PACKAGE__.nuspec",
                _configuration.ReplacementsDictionary[ReplacementVariables.PackageContext] +
                    ".Package." +
                    _configuration.ReplacementsDictionary[ReplacementVariables.PackageName] +
                    ".nuspec"
            );

            InstallNuGetPackages(project);
        }

        private void InstallNuGetPackages(Project project)
        {
            foreach (var package in _packages)
            {
                _dte.StatusBar.Text = String.Format(
                    "Installing NuGet package '{0}'...",
                    package.PackageId
                );

                _installer.InstallPackage(
                    AggregatePackageSource,
                    project,
                    package.PackageId,
                    package.Version,
                    false
                );
            }
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

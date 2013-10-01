param($InstallPath, $ToolsPath, $Package, $Project)

# InstallRoot maps to the Packages directory.

$InstallRoot = (Get-Item $InstallPath).Parent.FullName

# Update the debug configuration to point to the new debug host.

for ($i = 1; $i -le $Project.ConfigurationManager.Count; $i++)
{
  $Config = $Project.ConfigurationManager.Item($i, "")
  
  $Config.Properties.Item("StartProgram").Value = $InstallRoot + "\NetIde.Support." + $Package.Version + "\Tools\Runtime\NetIde.exe"
  $Config.Properties.Item("StartAction").Value = 1 # External program
  $Config.Properties.Item("StartArguments").Value = "/experimental"
}

# Install the NetIde target.

$TargetsFile = 'MSBuild\NetIde.targets'
$TargetsPath = $ToolsPath | Join-Path -ChildPath $TargetsFile

Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'

$MSBProject = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($Project.FullName) |
    Select-Object -First 1

$ProjectUri = New-Object -TypeName Uri -ArgumentList "file://$($Project.FullName)"
$TargetUri = New-Object -TypeName Uri -ArgumentList "file://$TargetsPath"

$RelativePath = $ProjectUri.MakeRelativeUri($TargetUri) -replace '/','\'

$ExistingImports = $MSBProject.Xml.Imports |
    Where-Object { $_.Project -like "*\$TargetsFile" }
if ($ExistingImports) {
    $ExistingImports | 
        ForEach-Object {
            $MSBProject.Xml.RemoveChild($_) | Out-Null
        }
}
$Import = $MSBProject.Xml.AddImport($RelativePath)
$Import.Condition = "Exists('" + $RelativePath + "')"
$Project.Save()

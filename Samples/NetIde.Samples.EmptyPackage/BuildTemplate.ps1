if ($Args.Length -eq 0) {
    $Args = @(
        "C:\Projects\NetIdeProjects\CodeMarker\NetIde\Samples\NetIde.Samples.EmptyPackage",
        "C:\Projects\NetIdeProjects\CodeMarker\NetIde\Samples\NetIde.Samples.EmptyPackage\bin\Debug\NetIdePackage.zip"
    )
}

$ProjectDir = (Get-Item $Args[0]).FullName + "\"
$Target = $Args[1]
$SolutionDir = (Get-Item $ProjectDir).Parent.Parent.FullName + "\"

# Load SharpZipLib

$SharpZipLib = $SolutionDir + "Libraries\SharpZipLib\ICSharpCode.SharpZipLib.dll"
[void][System.Reflection.Assembly]::LoadFile($SharpZipLib)

# Resolve the files to be included in the target

$IncludedFiles = @{ }

$IncludedFiles.Add("NetIdePackage.vstemplate", $False)

$TemplateNs = "http://schemas.microsoft.com/developer/vstemplate/2005"

[xml]$Template = Get-Content ($ProjectDir + "NetIdePackage.vstemplate")
$TemplateProject = $Template.VSTemplate.TemplateContent.Project

$IncludedFiles.Add($TemplateProject.File, $True)

function ResolveIncludedFiles($IncludedFiles, $Container, $Base = $Null)
{
    foreach ($Child in $Container.ChildNodes) {
        switch ($Child.LocalName) {
            "Folder" {
                $SubPath = $Child.Name
                if ($Base -ne $Null) {
                    $SubPath = $Base + "\" + $SubPath
                }

                ResolveIncludedFiles -IncludedFiles $IncludedFiles -Container $Child -Base $SubPath
            }
            "ProjectItem" {
                $SubPath = $Child.InnerText
                if ($Base -ne $Null) {
                    $SubPath = $Base + "\" + $SubPath
                }

                $Replace = $Child.ReplaceParameters -eq "true"

                $IncludedFiles.Add($SubPath, $Replace)
            }
        }
    }
}

ResolveIncludedFiles -IncludedFiles $IncludedFiles -Container $TemplateProject

# Logic for inserting replacement variables

function InsertReplacements($File, $Content)
{
    $Vars = $Null

    switch ($File) {
        "NetIde.Samples.EmptyPackage.csproj" {
            # For the project file, we need to remove the post build event that
            # calls this PowerShell script. We're using string replacement because
            # doing this through [xml] craps up the file format.

            $Before = $Content
            $Content = $Content -replace "(?m)`^\s*<PropertyGroup>[\r\n\s]*?<PostBuildEvent>.*?</PostBuildEvent>[\r\n\s]*?</PropertyGroup>\s*\r\n", ""
            if ($Before -eq $Content) {
                Write-Host Unable to remove the post build event from the project file
                exit 1
            }

            $Vars = @{
                "`{C017BE56-6C8E-48DF-BE45-644C488F8D97`}" = "`$guid1`$"
                "NetIde.Samples.EmptyPackage" = "`$safeprojectname:xml`$"
                "<TargetFrameworkVersion>v4.0</TargetFrameworkVersion>" = "<TargetFrameworkVersion>v`$targetframeworkversion`$</TargetFrameworkVersion>"
            }
        }
        "Properties\AssemblyInfo.cs" {
            $Vars = @{
                "Package Title" = "`$packagetitle:str`$"
                "Package Company" = "`$packagecompany:str`$"
            }
        }
        "BuildConfig.xml" {
            $Vars = @{
                "context=`"NetIdeEmptyPackageSample`"" = "context=`"`$packagecontext:xml`$`""
                "NetIdeEmptyPackageSample.Package.Core" = "`$packagecontext:xml`$.Package.`$packagename:xml`$"
            }
        }
        "EmptyPackageSample.cs" {
            $Vars = @{
                "586bf7e4-a5aa-4cb9-95e0-00a7227809ff" = "`$guid2`$"
                "EmptyPackageSample" = "`$packageclass`$"
                "NetIde.Samples.EmptyPackage" = "`$rootnamespace`$"
            }
        }
        "Labels.resx" {
            $Vars = @{
                "Package Description" = "`$packagedescription:xml`$"
            }
        }
        "NiContext.xml" {
            $Vars = @{
                "NetIdeEmptyPackageSample" = "`$packagecontext:xml`$"
            }
        }
        "NiPackage.manifest" {
            $Vars = @{
                "NetIde.Samples.EmptyPackage.EmptyPackageSample, NetIde.Samples.EmptyPackage" = "`$rootnamespace:xml`$.`$packageclass:xml`$, `$safeprojectname:xml`$"
            }
        }
        "Resources.cs" {
            $Vars = @{
                "NetIde.Samples.EmptyPackage" = "`$rootnamespace`$"
            }
        }
        "NetIdeEmptyPackageSample.Package.Core.nuspec" {
            $Vars = @{
                "NetIdeEmptyPackageSample.Package.Core" = "`$packagecontext:xml`$.Package.`$packagename:xml`$"
                "Package Title" = "`$packagetitle:xml`$"
                "Package Description" = "`$packagedescription:xml`$"
                "Package Company" = "`$packagecompany:xml`$"
            }
        }
    }

    if ($Vars -ne $Null) {
        foreach ($Key in $Vars.Keys) {
            $Content = $Content -replace $Key, $Vars[$Key]
        }
    }

    return $Content
}

# Build the ZIP file

$DummyFiles = @(
    "Key.snk",
    "Resources\MainIcon.ico"
)

$TargetStream = [System.IO.File]::Create($Target)
$ZipStream = New-Object ICSharpCode.SharpZipLib.Zip.ZipOutputStream $TargetStream
$Date = Get-Date
$Buffer = New-Object Byte[] 4096

foreach ($Key in $IncludedFiles.Keys)
{
    $ZipEntry = New-Object ICSharpCode.SharpZipLib.Zip.ZipEntry $Key
    $ZipEntry.DateTime = $Date

    $ZipStream.PutNextEntry($ZipEntry)

    if ($Key.EndsWith(".Designer.cs") -or $DummyFiles.Contains($Key)) {
        # We need to create a few dummy files because otherwise the project won't
        # be correctly generated.
        $Bytes = New-Object Byte[] 0
        $Source = New-Object System.IO.MemoryStream -ArgumentList @(, $Bytes )
    } elseif ($IncludedFiles[$Key]) {
        $Content = [System.IO.File]::ReadAllText($ProjectDir + $Key)

        $Content = InsertReplacements -File $Key -Content $Content

        $Bytes = [System.Text.Encoding]::UTF8.GetBytes($Content)
        $Source = New-Object System.IO.MemoryStream -ArgumentList @(, $Bytes )
    } else {
        $Source = [System.IO.File]::OpenRead($ProjectDir + $Key)
    }

    [ICSharpCode.SharpZipLib.Core.StreamUtils]::Copy($Source, $ZipStream, $Buffer)

    $Source.Dispose()
}

$ZipStream.Dispose()
$TargetStream.Dispose()

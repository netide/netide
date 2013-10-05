################################################################################
# SUPPORT
################################################################################

Function Get-Script-Directory
{
    $Scope = 1
    
    while ($True)
    {
        $Invoction = (Get-Variable MyInvocation -Scope $Scope).Value
        
        if ($Invoction.MyCommand.Path -Ne $Null)
        {
            Return Split-Path $Invoction.MyCommand.Path
        }
        
        $Scope = $Scope + 1
    }
}

Function Get-Config-Parameter([string]$Key)
{
    (Select-Xml -Path ($Global:Root + "\Build\Configuration\Configuration.xml") -XPath "//parameter[@key = '$Key']").Node.value
}

################################################################################
# ENTRY POINT
################################################################################

if ($Args.Length -eq 0) {
    $Args = @(
        "C:\Projects\NetIdeProjects\CodeMarker\NetIde\NetIde.ProjectTemplate",
        "C:\Projects\NetIdeProjects\CodeMarker\NetIde\NetIde.ProjectTemplate\bin\Debug\NetIdePackage.zip"
    )
}

$Global:Root = (Get-Item (Get-Script-Directory)).Parent.FullName + "\"

$ProjectDir = (Get-Item $Args[0]).FullName + "\"
$Target = $Args[1]

# Load SharpZipLib

$SharpZipLib = $Global:Root + "Libraries\SharpZipLib\ICSharpCode.SharpZipLib.dll"
[void][System.Reflection.Assembly]::LoadFile($SharpZipLib)

# Resolve the files to be included in the target

$IncludedFiles = @{ }

$IncludedFiles.Add("NetIdePackage.vstemplate", $True)
$IncludedFiles.Add("__TemplateIcon.ico", $False)

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
        "__PROJECT__.csproj" {
            # For the project file, we need to remove the post build event that
            # calls this PowerShell script. We're using string replacement because
            # doing this through [xml] craps up the file format.

            $Before = $Content
            $Content = $Content -replace "(?m)`^\s*<PropertyGroup>[\r\n\s]*?<PostBuildEvent>.*?</PostBuildEvent>[\r\n\s]*?</PropertyGroup>\s*\r\n", ""
            if ($Before -eq $Content) {
                Write-Host Unable to remove the post build event from the project file
                exit 1
            }

            $Before = $Content
            $Content = $Content -replace "<Import[^>]*\\packages\\[^>]*/>\r\n", ""
            if ($Before -eq $Content) {
                Write-Host Unable to remove the Net IDE MSBuild target
                exit 1
            }

            foreach ($Include in @( "NetIde Readme.html", "packages.config")) {
                $Before = $Content
                $Content = $Content -replace "(?m)`^\s*<(Content|None) Include=`"" + [System.Text.RegularExpressions.Regex]::Escape($Include) + "`" />\s*\r\n", ""
                if ($Before -eq $Content) {
                    Write-Host ("Unable to remove the " + $Include)
                    exit 1
                }
            }

            # Remove all NuGet references.

            $Offset = 0
            while ($True)
            {
                $End = $Content.IndexOf("</Reference>", $Offset)
                if ($End -eq -1) {
                    break
                }
                $End += "</Reference>".Length
                $Offset = $Content.LastIndexOf("<Reference", $End)

                $Reference = $Content.Substring($Offset, $End - $Offset)

                # If this is a NuGet reference, remove it. Otherwise jump just over
                # the closing tag to make the </Reference match the next reference.

                if ($Reference.IndexOf("\packages\") -eq -1) {
                    $Offset = $End + 2
                } else {
                    $Start = $Content.Substring(0, $Offset) -replace "[ \t]*$", ""
                    $End = $Content.Substring($End) -replace "^[ \t]*\r\n", ""
                    $Content = $Start + $End
                }
            }

            $Vars = @{
                "`{C017BE56-6C8E-48DF-BE45-644C488F8D97`}" = "`$guid1`$"
                "__PROJECT__" = "`$safeprojectname:xml`$"
                "__NAMESPACE__" = "`$rootnamespace:xml`$"
                "<TargetFrameworkVersion>v4.0</TargetFrameworkVersion>" = "<TargetFrameworkVersion>v`$targetframeworkversion`$</TargetFrameworkVersion>"
            }
        }
        "Properties\AssemblyInfo.cs" {
            $Vars = @{
                "__TITLE__" = "`$packagetitle:str`$"
                "__COMPANY__" = "`$packagecompany:str`$"
            }
        }
        "BuildConfig.xml" {
            $Vars = @{
                "__CONTEXT__" = "`$packagecontext:xml`$"
                "__PACKAGE__" = "`$packagename:xml`$"
            }
        }
        "__PACKAGECLASS__.cs" {
            $Vars = @{
                "586bf7e4-a5aa-4cb9-95e0-00a7227809ff" = "`$guid2`$"
                "__PACKAGECLASS__" = "`$packageclass`$"
                "__NAMESPACE__" = "`$rootnamespace`$"
            }
        }
        "Labels.resx" {
            $Vars = @{
                "__DESCRIPTION__" = "`$packagedescription:xml`$"
            }
        }
        "NiContext.xml" {
            $Vars = @{
                "__CONTEXT__" = "`$packagecontext:xml`$"
            }
        }
        "NiPackage.manifest" {
            $Vars = @{
                "__NAMESPACE__" = "`$rootnamespace:xml`$"
                "__PACKAGECLASS__" = "`$packageclass:xml`$"
                "__PROJECT__" = "`$safeprojectname:xml`$"
            }
        }
        "Resources.cs" {
            $Vars = @{
                "__NAMESPACE__" = "`$rootnamespace`$"
            }
        }
        "__CONTEXT__.Package.__PACKAGE__.nuspec" {
            $Vars = @{
                "__CONTEXT__" = "`$packagecontext:xml`$"
                "__PACKAGE__" = "`$packagename:xml`$"
                "__TITLE__" = "`$packagetitle:xml`$"
                "__DESCRIPTION__" = "`$packagedescription:xml`$"
                "__COMPANY__" = "`$packagecompany:xml`$"
            }
        }
        "NetIdePackage.vstemplate" {
            $Vars = @{
                "__VERSION__" = (Get-Config-Parameter "version")
            }
        }
    }

    if ($Vars -ne $Null) {
        foreach ($Key in $Vars.Keys) {
            $Before = $Content
            $Content = $Content -replace $Key, $Vars[$Key]

            if ($Before -eq $Content) {
                Write-Host ("Could not replace '" + $Key + "' in '" + $File + "'")
            }
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

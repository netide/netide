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

Function Console-Update-Status([string]$Status, [string]$ForegroundColor)
{
    $WindowWidth = (Get-Host).UI.RawUI.WindowSize.Width
    
    $Position = (Get-Host).UI.RawUI.CursorPosition
    
    $Position.X = $WindowWidth - ($Status.Length + 1)
    $Position.Y = $Position.Y - 1
    
    if ($Position.X -ge 0 -and $Position.Y -ge 0)
    {
        (Get-Host).UI.RawUI.CursorPosition = $Position
    }
    
    Write-Host $Status -ForegroundColor $ForegroundColor
}

Function Prepare-Directory([string]$Path)
{
    if (Test-Path -Path $Path)
    {
        Remove-Item -Recurse -Force $Path -ErrorAction SilentlyContinue
        
        if (Test-Path -Path $Path)
        {
            Write-Host "  Killing MSBuild..."
            
            # Lets see whether there is an MSBuild that is keeping hold of
            # our build tasks DLL.
            
            Stop-Process -ProcessName MSBuild -ErrorAction SilentlyContinue
            Stop-Process -ProcessName MSBuildTaskHost -ErrorAction SilentlyContinue
            
            # Second attempt. We need to give MSBuild time to shut down.
            
            for ($i = 0; $i -lt 5; $i++)
            {
                Remove-Item -Recurse -Force $Path -ErrorAction SilentlyContinue
                
                if (-not (Test-Path -Path $Path))
                {
                    break
                }
                
                Start-Sleep -m 400
            }
            
            if (Test-Path -Path $Path)
            {
                Console-Update-Status "[FAILED]" -ForegroundColor Red
                Exit 3
            }
        }
    }

    [void](New-Item -Type directory $Path)
}

Function Build-Solution([string]$Solution, [string]$Target = $Null, [string]$Configuration = "Release", [string]$Platform = "Mixed Platforms", [bool]$Skip = $False, [string]$ExtraProperties = $Null, [string]$SolutionDisplay = $Null)
{
    if ($Target -eq $Null -or $Target -eq "")
    {
        $Target = $Global:DefaultBuildTarget
    }
    if ($SolutionDisplay -eq "")
    {
        $SolutionDisplay = [System.IO.Path]::GetFileName($Solution)
    }
    
    $Status = "Building " + $SolutionDisplay + " ($Configuration"
    
    if ($Target -ne $DefaultBuildTarget)
    {
        $Status = $Status + ", " + $Target
    }
    
    $Status = $Status + ")"

    Write-Host $Status
    
    if ($Skip)
    {
        Console-Update-Status "[SKIPPED]" -ForegroundColor Yellow
    }
    else
    {
        $Properties = "Platform=`"$Platform`";Configuration=`"$Configuration`";NoWarn=`"1591,1587,1573,0436`""
        
        if ($ExtraProperties -ne $Null -and $ExtraProperties -ne "")
        {
            $Properties = $Properties + ";" + $ExtraProperties
        }
        
        # Perform the build. Note that warnings CS1591, CS1587 and CS1573 are the warnings for missing
        # XML documentation. Warning CS0436 is because we need to override a framework
        # class.
        
        $Arguments = "/nr:false /m /verbosity:quiet /nologo `"/t:$Target`" /p:$Properties `"$Solution`""
        
        $Process = Start-Process -NoNewWindow -Wait -FilePath $Global:MSBuild -ArgumentList $Arguments -PassThru
        
        if ($Process.ExitCode -eq 0)
        {
            Console-Update-Status "[OK]" -ForegroundColor Green
        }
        else
        {
            Console-Update-Status "[FAILED]" -ForegroundColor Red
            
            # If we get an error from a build, the chances that we'll be able
            # to continue are very dim. Because of this, we bail immediately.

            Exit 2
        }
    }
}

Function Find-MSBuild
{
    $MsBuild = $Null
    
    $FrameworkRegistryRoot = "HKLM:\Software\Wow6432Node\Microsoft\.NETFramework"
    
    if ((Test-Path $FrameworkRegistryRoot) -ne $True)
    {
        $FrameworkRegistryRoot = "HKLM:\Software\Microsoft\.NETFramework"
        
        if ((Test-Path $FrameworkRegistryRoot) -ne $True)
        {
            $FrameworkRegistryRoot = $Null
        }
    }
    
    if ($FrameworkRegistryRoot -ne $Null)
    {
        $FrameworkInstallRoot = (Get-ItemProperty $FrameworkRegistryRoot).InstallRoot
        
        if ($FrameworkInstallRoot -ne $Null)
        {
            $FrameworkVersionRoot = $FrameworkInstallRoot + "\v4.0.30319"
            
            if (Test-Path $FrameworkVersionRoot)
            {
                $MsBuildPath = $FrameworkVersionRoot + "\MSBuild.exe"
                
                if (Test-Path $MsBuildPath)
                {
                    $MsBuild = $MsBuildPath
                }
                else
                {
                    Write-Host `
                       ("MSBuild.exe could not be found in the .NET Framework " + `
                        "installation folder. Ensure the .NET Framework version 4.0 " + `
                        "is installed correctly and retry the build.") `
                        -ForegroundColor Red
                    
                    Exit 3
                }
            }
        }
    }
    
    if ($MsBuild -eq $Null)
    {
        Write-Host `
           ("Microsoft .NET framework not found; please install from " + `
            "http://www.microsoft.com/net/download. " + `
            "The build process cannot continue.") `
            -ForegroundColor Red
        
        Exit 3
    }
    
    Return $MsBuild
}

Function Get-Template([string]$Key)
{
    (Select-Xml -Path ($Global:Root + "\Build\Configuration\Configuration.xml") -XPath "//template[@key = '$Key']/text()").Node.Value
}

Function Get-Config-Parameter([string]$Key)
{
    (Select-Xml -Path ($Global:Root + "\Build\Configuration\Configuration.xml") -XPath "//parameter[@key = '$Key']").Node.value
}

Function AssemblyInfo-Write-All
{
    Write-Host "Writing AssemblyInfo.cs files"

    $Template = (Get-Template "AssemblyInfo.cs").Trim()
    $Template = $Template.Replace("`$company`$", (Get-Config-Parameter "company"))
    $Template = $Template.Replace("`$copyright`$", (Get-Config-Parameter "copyright"))

    $GlobalVersion = (Get-Config-Parameter "version")

    # Do all replacements we can do before hand

    $Projects = Select-Xml -Path ($Global:Root + "\Build\Configuration\Configuration.xml") -XPath "//project"

    foreach ($Project in $Projects)
    {
        $ProjectVersion = $Project.Node.version

        if ($ProjectVersion -eq $Null)
        {
            $ProjectVersion = $GlobalVersion
        }

        $ProjectTemplate = $Template.Replace("`$version`$", $ProjectVersion)

        AssemblyInfo-Write -Project $Project.Node.name -ClsCompliant $Project.Node.clsCompliant -Template $ProjectTemplate
    }
    
    Console-Update-Status "[OK]" -ForegroundColor Green
}

Function AssemblyInfo-Write([string]$Project, [string]$ClsCompliant, [string]$Template)
{
    if ($ClsCompliant -eq "")
    {
        $ClsCompliant = "true"
    }

    $ProjectName = $Project
    
    $Pos = $ProjectName.LastIndexOf("\")

    if ($Pos -ne -1)
    {
        $ProjectName = $ProjectName.Substring($Pos + 1)
    }
    
    $Template = $Template.Replace("`$name`$", $ProjectName)
    $Template = $Template.Replace("`$description`$", $ProjectName)
    $Template = $Template.Replace("`$cls-compliant`$", $ClsCompliant)
    
    $Template.Replace("`r", "").Replace("`n", "`r`n") | Out-File ($Global:Root + "\" + $Project + "\Properties\AssemblyInfo.cs")
}

################################################################################
# ILMERGE
################################################################################

Function ILMerge([string]$Primary, [string]$Source, [string]$Target)
{
    Write-Host ("Merging " + $Primary)

    $ILMerge = $Global:Root + "\Libraries\ILMerge\ILMerge.exe"
    
    $Libraries = $Null
    
    foreach ($File in (Get-ChildItem ($Source + "\*.dll")))
    {
        if ($Libraries -ne $Null)
        {
            $Libraries += " "
        }
        
        $Libraries += $File.Name
    }
    
    $Arguments = `
        "`"/out:" + $Target + "\" + $Primary + "`" " + `
        "`"/keyfile=" + $Global:Root + "\Support\Key.snk`" " + `
        "/v2 /ndebug " + `
        $Primary + " " + `
        $Libraries
    
    Start-Process `
        -NoNewWindow -Wait `
        -FilePath $ILMerge `
        -ArgumentList $Arguments `
        -WorkingDirectory $Source `
        -PassThru | Out-Null
    
    Console-Update-Status "[OK]" -ForegroundColor Green
}

################################################################################
# NUGET PACKAGES
################################################################################

Function Build-NuGet-Packages
{
    # Build the runtime NuGet package.

    foreach ($Item in (Get-ChildItem -Directory -Path $Global:Root))
    {
        $BasePath = $Item.FullName
        $NuSpec = $Null

        foreach ($ChildItem in (Get-ChildItem -Path $BasePath -Filter "*.nuspec"))
        {
            $NuSpec = $ChildItem.FullName
        }

        if ($NuSpec -ne $Null)
        {
            $IdNode = Select-Xml -Path $NuSpec -XPath "/rns:package/rns:metadata/rns:id" -Namespace @{ rns = "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd" }
            $PackageId = $IdNode.Node.InnerText

            # Don't build the unit test package.
            
            if (
                $PackageId -ne "NetIdeUnitTests.Package.Core"
            )
            {
                Build-NuGet-Package `
                    -NuSpec $NuSpec `
                    -Package $PackageId `
                    -TargetPath ($Global:Distrib + "\Packages") -BasePath ($BasePath + "\bin\" + $Global:Configuration)
            }
        }
    }

    # NuGet doesn't allow .nupkg files to be included in packages (go figure).
    # Instead, we copy the .nupkg file to a .zip file and include that.

    $CorePackage = $Null

    foreach ($Item in (Get-ChildItem -Path ($Global:Distrib + "\Packages") -Filter "NetIde.Package.Core.*.nupkg"))
    {
        $CorePackage = $Item.FullName
    }

    $CoreZipPackage = `
        [System.IO.Path]::GetDirectoryName($CorePackage) + "\" + `
        [System.IO.Path]::GetFileNameWithoutExtension($CorePackage) + ".zip"

    Copy-Item -Path $CorePackage -Destination $CoreZipPackage

    # Build the support package.

    Build-NuGet-Package `
        -NuSpec ($Global:Root + "\Build\Configuration\NuGet\NetIde.Support.nuspec") `
        -Package "NetIde.Support" `
        -TargetPath ($Global:Distrib + "\Packages") -BasePath ($Global:Root + "\NetIde\bin\" + $Global:Configuration)

    # Remove the zip file; it was just there to include it in the package.

    Remove-Item -Path $CoreZipPackage
}

Function Build-NuGet-Package([string]$NuSpec, [string]$Package, [string]$TargetPath, [string]$BasePath)
{
    Write-Host "Building NuGet $Package package"
    
    $Version = (Get-Config-Parameter "version")
    
    # Execute NuGet
    
    & ($Global:Root + "\Libraries\NuGet\NuGet.exe") `
        pack $NuSpec `
        -OutputDirectory $TargetPath `
        -BasePath $BasePath `
        -Version $Version `
        -NoPackageAnalysis | Out-Null
    
    Console-Update-Status "[OK]" -ForegroundColor Green
}

Function Publish-NuGet-Packages
{
    $NuGetSite = (Get-Config-Parameter "nuGetSite")

    if ($NuGetSite -eq $Null)
    {
        Write-Host "NuGet site parameter not configured"
        Return
    }

    foreach ($Item in (Get-ChildItem -Path ($Global:Distrib + "\Packages\*.nupkg")))
    {
        & ($Global:Root + "\Libraries\NuGet\NuGet.exe") `
            push ($Item.FullName) `
            -Source ($NuGetSite.TrimEnd("/") + "/package")
    }
}

################################################################################
## BUILD TASKS
################################################################################

Function Build-Tasks
{
    Prepare-Directory -Path ($Global:Distrib + "\MSBuild")

    Build-Solution -Solution ($Global:Root + "\BuildTasks.sln") -Configuration $Global:Configuration
    
    # Copy the required files from the MSBuild task.
    
    Copy-Item -Path ($Global:Root + "\NetIde.BuildTasks\bin\" + $Global:Configuration + "\*.dll") -Destination ($Global:Distrib + "\MSBuild")
    Copy-Item -Path ($Global:Root + "\NetIde.BuildTasks\bin\" + $Global:Configuration + "\*.exe") -Destination ($Global:Distrib + "\MSBuild")
    Copy-Item -Path ($Global:Root + "\NetIde.BuildTasks\bin\" + $Global:Configuration + "\NetIde.targets") -Destination ($Global:Distrib + "\MSBuild")
}

################################################################################
## SETUP
################################################################################

Function Setup
{
    Prepare-Directory -Path ($Global:Distrib + "\Setup")

    Build-Solution -Solution ($Global:Root + "\Setup.sln") -Configuration $Global:Configuration
    
    # Copy the required files from the setup baker.
    
    Copy-Item -Path ($Global:Root + "\NetIde.MakeSetup\bin\" + $Global:Configuration + "\nimakesetup.exe") -Destination ($Global:Distrib + "\Setup")
}

################################################################################
## USAGE
################################################################################

# Show usage.
Function Show-Usage
{
    Write-Host "Usage: [essentials|distrib|publish]"
}

################################################################################
# ENTRY POINT
################################################################################

$Global:Root = (Get-Item (Get-Script-Directory)).Parent.Parent.FullName
$Global:DefaultBuildTarget = "Build"
$Global:MSBuild = Find-MSBuild
$Global:Distrib = $Global:Root + "\Build\Distrib"
$Global:Configuration = "Debug"

if ($Args.Length -eq 0 -or -not ( @( "essentials", "distrib", "publish" ) -contains $Args[0] ))
{
    Show-Usage
    Exit
}

$Global:Mode = $Args[0]

AssemblyInfo-Write-All

Prepare-Directory -Path $Global:Distrib
Prepare-Directory -Path ($Global:Distrib + "\Packages")

Build-Tasks
Setup

if ($Global:Mode -eq "distrib" -or $Global:Mode -eq "publish")
{
    Build-Solution -Solution ($Global:Root + "\NetIde.sln") -Configuration $Global:Configuration
    
    Build-NuGet-Packages
    
    if ($Global:Mode -eq "publish")
    {
        Publish-NuGet-Packages
    }
}

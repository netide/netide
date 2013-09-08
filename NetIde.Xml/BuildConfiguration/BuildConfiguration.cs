using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.BuildConfiguration
{
    [XmlRoot("buildConfiguration", Namespace = Ns.BuildConfiguration)]
    public class BuildConfiguration
    {
        [XmlArray("buildNuGetPackages")]
        [XmlArrayItem("buildNuGetPackage")]
        public List<BuildNuGetPackage> BuildNuGetPackages { get; set; }

        [XmlArray("installPackages")]
        [XmlArrayItem("installPackage")]
        public List<InstallPackage> InstallPackages { get; set; }

        public BuildConfiguration()
        {
            BuildNuGetPackages = new List<BuildNuGetPackage>();
            InstallPackages = new List<InstallPackage>();
        }
    }
}

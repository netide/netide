using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.PackageMetadata
{
    [XmlRoot("metadata", Namespace = Ns.NuSpec)]
    public class PackageMetadata : IPackageId
    {
        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("version")]
        public string Version { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("authors")]
        public string Authors { get; set; }

        [XmlElement("licenseUrl")]
        public string LicenseUrl { get; set; }

        [XmlElement("projectUrl")]
        public string ProjectUrl { get; set; }

        [XmlElement("iconUrl")]
        public string IconUrl { get; set; }

        [XmlElement("requireLicenseAcceptance")]
        public bool RequireLicenseAcceptance { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("tags")]
        public string Tags { get; set; }

        [XmlElement("owners")]
        public string Owners { get; set; }

        [XmlArray("dependencies")]
        [XmlArrayItem("dependency")]
        public List<Dependency> Dependencies { get; set; }

        [XmlIgnore]
        public DateTime? Published { get; set; }

        [XmlIgnore]
        public string GalleryDetailsUrl { get; set; }

        [XmlIgnore]
        public int? DownloadCount { get; set; }

        [XmlIgnore]
        public string NuGetSite { get; set; }

        [XmlIgnore]
        public string PendingVersion { get; set; }

        [XmlIgnore]
        public PackageState State { get; set; }

        public PackageMetadata()
        {
            Dependencies = new List<Dependency>();
        }

        public override string ToString()
        {
            return String.Format("{0}, Version={1}, State={2}", Id, Version, State.ToString().Replace(", ", "|"));
        }
    }
}

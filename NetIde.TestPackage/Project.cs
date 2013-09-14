using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.TestPackage.Api;

namespace NetIde.TestPackage
{
    public class Project : NiProject
    {
        private int _rdtCookie;

        public void Load(string fileName, NiProjectCreateMode createMode)
        {
            this.SetPropertyEx(NiHierarchyProperty.Name, Path.GetFileNameWithoutExtension(fileName));

            // Load the project from the file system.

            LoadDirectory(this, Path.GetDirectoryName(fileName));

            // Associate a NiTextLines with the project to store the project
            // file in.

            object obj;
            ErrorUtil.ThrowOnFailure(((INiLocalRegistry)GetService(typeof(INiLocalRegistry))).CreateInstance(
                new Guid(NiConstants.TextLines),
                this,
                out obj
            ));

            var textLines = (INiTextLines)obj;

            // If we're creating a new project, create a file on disk and load that.

            if (createMode != NiProjectCreateMode.Open)
                new XDocument(new XElement((XNamespace)TPResources.ProjectNs + "testProject")).Save(fileName);

            ErrorUtil.ThrowOnFailure(textLines.LoadDocData(fileName));

            ErrorUtil.ThrowOnFailure(((INiRunningDocumentTable)GetService(typeof(INiRunningDocumentTable))).Register(
                fileName,
                this,
                textLines,
                out _rdtCookie
            ));
        }

        public override HResult Close()
        {
            try
            {
                // Remove the RDT registration.

                if (_rdtCookie != 0)
                {
                    ErrorUtil.ThrowOnFailure(((INiRunningDocumentTable)GetService(typeof(INiRunningDocumentTable))).Unregister(
                        _rdtCookie
                    ));

                    _rdtCookie = 0;
                }

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private void LoadDirectory(INiHierarchy parentItem, string path)
        {
            foreach (string subPath in Directory.GetDirectories(path))
            {
                if (new FileInfo(subPath).Attributes.HasFlag(FileAttributes.Hidden))
                    continue;

                var projectItem = new NiProjectItem();

                projectItem.SetPropertyEx(NiHierarchyProperty.ItemType, NiHierarchyType.Directory);
                projectItem.SetFileName(subPath);
                projectItem.SetPropertyEx(NiHierarchyProperty.SortPriority, -1);
                projectItem.SetPropertyEx(NiHierarchyProperty.Name, Path.GetFileName(subPath));
                projectItem.SetPropertyEx(NiHierarchyProperty.Parent, parentItem);

                LoadDirectory(projectItem, subPath);
            }

            foreach (string subPath in Directory.GetFiles(path))
            {
                if (
                    new FileInfo(subPath).Attributes.HasFlag(FileAttributes.Hidden) ||
                    String.Equals(Path.GetExtension(subPath), ".niproj", StringComparison.OrdinalIgnoreCase)
                )
                    continue;

                var projectItem = new NiProjectItem();

                projectItem.SetPropertyEx(NiHierarchyProperty.Name, Path.GetFileName(subPath));

                projectItem.SetPropertyEx(NiHierarchyProperty.ItemType, NiHierarchyType.File);
                projectItem.SetFileName(subPath);

                projectItem.SetPropertyEx(NiHierarchyProperty.Parent, parentItem);
            }
        }
    }
}

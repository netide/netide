using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetIde.Test.Support
{
    internal static class ProjectCreator
    {
        public const string ProjectFileName = "Project.niproj";

        public static Project CreateTestProject(bool createProjectFile)
        {
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            Directory.CreateDirectory(path);

            if (createProjectFile)
            {
                File.WriteAllText(
                    Path.Combine(path, ProjectFileName),
@"<?xml version=""1.0"" encoding=""utf-8""?>
<testProject xmlns=""https://github.com/pvginkel/NetIde/schemas/TestProject"" />"
                );
            }

            WriteFiles(path, "");

            for (int i = 1; i <= 3; i++)
            {
                string subPath = Path.Combine(path, "Folder" + i);

                Directory.CreateDirectory(subPath);

                WriteFiles(subPath, i.ToString());
            }

            return new Project(path, ProjectFileName);
        }

        private static void WriteFiles(string subPath, string prefix)
        {
            for (int i = 0; i < 3; i++)
            {
                string fileName = "File" + prefix + (char)('A' + i);

                using (var writer = File.CreateText(Path.Combine(subPath, fileName)))
                {
                    for (int j = 0; j < 3; j++)
                    {
                        writer.WriteLine((char)('A' + i) + prefix + (char)('a' + j));
                    }
                }
            }
        }
    }
}

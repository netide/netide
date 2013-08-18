using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetIde.Shell
{
    public class StreamInfo
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }

        public static StreamInfo FromFile(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            var fileInfo = new FileInfo(fileName);

            return new StreamInfo
            {
                Name = fileName,
                DisplayName = Path.GetFileName(fileName),
                CreationTime = fileInfo.CreationTime,
                LastWriteTime = fileInfo.LastWriteTime
            };
        }
    }
}

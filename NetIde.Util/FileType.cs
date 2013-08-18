using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetIde.Util
{
    public class FileType
    {
        private static readonly FileType BinaryFileType = new FileType(FileTypeType.Binary);

        public static FileType FromStream(Stream stream, string extension)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (extension == null)
                throw new ArgumentNullException("extension");

            FileType fileType = ImageFileType.FromExtension(extension);

            if (fileType != null)
                return fileType;

            fileType = TextFileType.FromStream(stream, extension);

            if (fileType != null)
                return fileType;

            return BinaryFileType;
        }

        public FileTypeType Type { get; private set; }

        protected FileType(FileTypeType type)
        {
            Type = type;
        }
    }
}

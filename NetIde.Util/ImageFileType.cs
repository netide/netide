using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace NetIde.Util
{
    public class ImageFileType : FileType
    {
        public static ImageFileType FromExtension(string extension)
        {
            if (extension == null)
                throw new ArgumentNullException("extension");

            foreach (var codecInfo in ImageCodecInfo.GetImageDecoders())
            {
                foreach (string codecExtension in codecInfo.FilenameExtension.Split(';'))
                {
                    if (codecExtension.Length > 0)
                    {
                        string toMatch = codecExtension.Trim();

                        if (toMatch[0] == '*')
                            toMatch = toMatch.Substring(1);

                        if (String.Equals(extension, toMatch, StringComparison.OrdinalIgnoreCase))
                            return new ImageFileType(codecInfo);
                    }
                }
            }

            return null;
        }

        public ImageCodecInfo CodecInfo { get; private set; }

        private ImageFileType(ImageCodecInfo codecInfo)
            : base(FileTypeType.Image)
        {
            if (codecInfo == null)
                throw new ArgumentNullException("codecInfo");

            CodecInfo = codecInfo;
        }

        public override string ToString()
        {
            return CodecInfo.CodecName;
        }
    }
}

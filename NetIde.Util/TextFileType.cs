using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetIde.Util
{
    public class TextFileType : FileType
    {
        private static readonly HashSet<byte> _validChars = BuildValidChars();

        private static HashSet<byte> BuildValidChars()
        {
            // Taken from http://stackoverflow.com/questions/898669/how-can-i-detect-if-a-file-is-binary-non-text-in-python

            var result = new HashSet<byte>();

            foreach (var c in new[] { 7, 8, 9, 10, 12, 13, 27 })
            {
                result.Add((byte)c);
            }

            for (int c = 0x20; c < 0x100; c++)
            {
                result.Add((byte)c);
            }

            return result;
        }

        public new static TextFileType FromStream(Stream stream, string extension)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (extension == null)
                throw new ArgumentNullException("extension");

            if (stream.Length == 0)
                return null;

            stream.Position = 0;

            var encoding = DetectEncoding(stream);

            int bomSize = (int)stream.Position;

            if (encoding != null)
                return new TextFileType(encoding, true, bomSize, DetectLineTermination(stream));

            encoding = GuessEncoding(stream);

            if (encoding != null)
                return new TextFileType(encoding, false, 0, DetectLineTermination(stream));

            return null;
        }

        private static Encoding DetectEncoding(Stream stream)
        {
            if (stream.Length < 2)
                return null;

            var byteBuffer = new byte[Math.Min(stream.Length, 4)];
            int byteLen = stream.Read(byteBuffer, 0, byteBuffer.Length);

            if (byteBuffer[0] == 0xFE && byteBuffer[1] == 0xFF)
            {
                // Big Endian Unicode

                stream.Position = 2;
                return new UnicodeEncoding(true, true);
            }
            else if (byteBuffer[0] == 0xFF && byteBuffer[1] == 0xFE)
            {
                // Little Endian Unicode, or possibly little endian UTF32

                if (byteLen < 4 || byteBuffer[2] != 0 || byteBuffer[3] != 0)
                {
                    stream.Position = 2;
                    return new UnicodeEncoding(false, true);
                }
                else
                {
                    stream.Position = 4;
                    return new UTF32Encoding(false, true);
                }
            }
            else if (byteLen >= 3 && byteBuffer[0] == 0xEF && byteBuffer[1] == 0xBB && byteBuffer[2] == 0xBF)
            {
                // UTF-8

                stream.Position = 3;
                return Encoding.UTF8;
            }
            else if (
                byteLen >= 4 && byteBuffer[0] == 0 && byteBuffer[1] == 0 &&
                byteBuffer[2] == 0xFE && byteBuffer[3] == 0xFF
            )
            {
                // Big Endian UTF32

                stream.Position = 4;
                return new UTF32Encoding(true, true);
            }

            stream.Position = 0;
            return null;
        }

        private static LineTermination DetectLineTermination(Stream stream)
        {
            int c;

            while ((c = stream.ReadByte()) != -1)
            {
                if (c == '\r')
                {
                    c = stream.ReadByte();

                    if (c != '\n')
                        return LineTermination.Mac;
                    return LineTermination.Pc;
                }
                else if (c == '\n')
                {
                    return LineTermination.Unix;
                }
            }

            // The default when we don't have any line ending.

            return
                PlatformUtil.IsUnix
                ? LineTermination.Unix
                : LineTermination.Pc;
        }

        private static Encoding GuessEncoding(Stream stream)
        {
            int printChars = 0;
            int nullChars = 0;
            int evenNullChars = 0;
            int oddNullChars = 0;

            stream.Position = 0;

            int c;
            while (stream.Position < 8000 && (c = stream.ReadByte()) != -1)
            {
                if (c == 0)
                {
                    nullChars++;

                    if (stream.Position % 2 == 0)
                        oddNullChars++;
                    else
                        evenNullChars++;
                }
                else if (_validChars.Contains((byte)c))
                {
                    printChars++;
                }
            }

            stream.Position = 0;

            double printRatio = (double)printChars / stream.Position;
            double nullRatio = (double)nullChars / stream.Position;
            double evenNullRatio = (double)evenNullChars / stream.Position;
            double oddNullRatio = (double)oddNullChars / stream.Position;

            // Could this be UTF-16?

            if (nullRatio > .4)
            {
                // Would expect more printable characters.

                if (printRatio < .4)
                    return null;

                // If either the even nulls or odd nulls predominate, it's
                // probably UTF-16.

                if (evenNullRatio > .4)
                    return new UnicodeEncoding(true, false);
                if (oddNullRatio > .4)
                    return new UnicodeEncoding(false, false);

                return null;
            }
            
            // If the file is almost exclusively printable characters, lets
            // treat it as ASCII.

            if (printRatio > .95)
                return Encoding.ASCII;

            return null;
        }

        public Encoding Encoding { get; private set; }

        public bool HaveBom { get; private set; }

        public LineTermination LineTermination { get; private set; }

        public int BomSize { get; private set; }

        private TextFileType(Encoding encoding, bool haveBom, int bomSize, LineTermination lineTermination)
            : base(FileTypeType.Text)
        {
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            Encoding = encoding;
            HaveBom = haveBom;
            BomSize = bomSize;
            LineTermination = lineTermination;
        }

        public override string ToString()
        {
            return Encoding.EncodingName + " " + LineTermination;
        }
    }
}

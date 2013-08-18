using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Util
{
    public class ImageListManager : IDisposable
    {
        private readonly Dictionary<int, int> _cache = new Dictionary<int, int>();
        private readonly Dictionary<Tuple<int, int>, int> _overlays = new Dictionary<Tuple<int, int>, int>();
        private bool _disposed;
        private IntPtr _handle;

        public ImageList ImageList { get; private set; }

        public ImageListManager()
        {
            ImageList = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(16, 16)
            };

            var fileInfo = new SHFILEINFO();

            _handle = SHGetFileInfo(
                ".txt",
                0,
                ref fileInfo,
                (uint)Marshal.SizeOf(fileInfo),
                SHGFI_USEFILEATTRIBUTES | SHGFI_SYSICONINDEX | SHGFI_SMALLICON
            );

            Debug.Assert(_handle != IntPtr.Zero);
        }

        public int GetIndexForFileName(string fileName)
        {
            return GetIndexForFileName(fileName, false);
        }

        public int GetIndexForDirectory(string path)
        {
            return GetIndexForFileName(path, true);
        }

        private int GetIndexForFileName(string fileName, bool directory)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            int index = AddShellIcon(fileName, directory);

            if (index == -1)
                return -1;

            int result;
            if (_cache.TryGetValue(index, out result))
                return result;

            result = ImageList.Images.Count;

            using (var icon = Icon.FromHandle(GetIcon(index)))
            {
                ImageList.Images.Add(new Icon(icon, new Size(16, 16)).ToBitmap());
            }

            _cache[index] = result;

            return result;
        }

        public int GetIndexForOverlay(int imageIndex, int overlayIndex)
        {
            var key = Tuple.Create(imageIndex, overlayIndex);
            int result;

            if (_overlays.TryGetValue(key, out result))
                return result;

            result = ImageList.Images.Count;

            var bitmap = new Bitmap(16, 16);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawImageUnscaled(ImageList.Images[imageIndex], 0, 0);
                graphics.DrawImageUnscaled(ImageList.Images[overlayIndex], 0, 0);
            }

            ImageList.Images.Add(bitmap);

            _overlays[key] = result;

            return result;
        }

        private int AddShellIcon(string path, bool directory)
        {
            var info = new SHFILEINFO();

            try
            {
                uint flags = 0;

                if (
                    (directory && !Directory.Exists(path)) ||
                    (!directory && !File.Exists(path))
                )
                    flags |= SHGFI_USEFILEATTRIBUTES;

                var imageList = SHGetFileInfo(
                    path,
                    directory ? FILE_ATTRIBUTE_DIRECTORY : 0,
                    ref info,
                    (uint)Marshal.SizeOf(info),
                    flags | SHGFI_SMALLICON | SHGFI_SYSICONINDEX
                );

                if (imageList == IntPtr.Zero)
                    return -1;

                Debug.Assert(imageList == _handle);

                return info.iIcon;
            }
            finally
            {
                if (info.hIcon != IntPtr.Zero)
                    DestroyIcon(info.hIcon);
            }
        }

        private IntPtr GetIcon(int index)
        {
            return ImageList_GetIcon(_handle, index, 0);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (ImageList != null)
                {
                    ImageList.Dispose();
                    ImageList = null;
                }

                _disposed = true;
            }
        }

        private const uint SHGFI_LARGEICON = 0x0;
        private const uint SHGFI_SMALLICON = 0x1;
        private const uint SHGFI_OPENICON = 0x2;
        private const uint SHGFI_SHELLICONSIZE = 0x4;
        private const uint SHGFI_USEFILEATTRIBUTES = 0x10;
        private const uint SHGFI_ADDOVERLAYS = 0x20;
        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_ATTRIBUTES = 0x800;
        private const uint SHGFI_SYSICONINDEX = 0x4000;

        private const uint FILE_ATTRIBUTE_DIRECTORY = 0x10;

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [DllImport("comctl32.dll")]
        private static extern IntPtr ImageList_GetIcon(IntPtr hImageList, int iconIndex, uint flags);

        [DllImport("user32.dll")]
        private static extern int DestroyIcon(IntPtr hIcon);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }
    }
}

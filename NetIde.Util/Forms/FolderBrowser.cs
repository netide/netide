using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Util.Forms
{
    public partial class FolderBrowser
    {
        private static readonly FolderBrowserMode _mode = DetectMode();

        private static FolderBrowserMode DetectMode()
        {
            if (PlatformUtil.IsMono)
                return FolderBrowserMode.Managed;
            else if (IsAtLeastVista())
                return FolderBrowserMode.Vista;
            else
                return FolderBrowserMode.Native;
        }

        private static bool IsAtLeastVista()
        {
            return
                Environment.OSVersion.Platform == PlatformID.Win32NT &&
                Environment.OSVersion.Version >= new Version(6, 0, 6000);
        }

        public FolderBrowserMode Mode { get { return _mode; } }

        public string Title { get; set; }

        public string SelectedPath { get; set; }

        public ISite Site { get; set; }

        public string Description { get; set; }

        public Environment.SpecialFolder RootFolder { get; set; }

        public bool ShowNewFolderButton { get; set; }

        public bool ShowEditBox { get; set; }

        public bool UseDescriptionForTitle { get; set; }

        public FolderBrowser()
        {
            ShowNewFolderButton = true;
            ShowEditBox = true;
        }

        public DialogResult ShowDialog()
        {
            return ShowDialog(null);
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            switch (_mode)
            {
                case FolderBrowserMode.Native: return ShowNativeDialog(owner);
                case FolderBrowserMode.Vista: return ShowVistaDialog(owner);
                default: return ShowManagedDialog(owner);
            }
        }

        private DialogResult ShowNativeDialog(IWin32Window owner)
        {
            var rootFolder = IntPtr.Zero;

            NativeMethods.SHGetSpecialFolderLocation(owner.Handle, (int)RootFolder, ref rootFolder);

            if (rootFolder == IntPtr.Zero)
            {
                NativeMethods.SHGetSpecialFolderLocation(owner.Handle, (int)Environment.SpecialFolder.Desktop, ref rootFolder);

                if (rootFolder == IntPtr.Zero)
                    throw new InvalidOperationException("Cannot resolve root folder");
            }

            uint options = NativeMethods.BIF_NEWDIALOGSTYLE;
            if (ShowEditBox)
                options |= NativeMethods.BIF_EDITBOX;
            if (!ShowNewFolderButton)
                options |= NativeMethods.BIF_NONEWFOLDERBUTTON;

            var result = IntPtr.Zero;
            var displayName = IntPtr.Zero;
            var selectedPath = IntPtr.Zero;

            try
            {
                var bi = new NativeMethods.BROWSEINFO();

                displayName = Marshal.AllocHGlobal(NativeMethods.MAX_PATH * Marshal.SystemDefaultCharSize);
                selectedPath = Marshal.AllocHGlobal(NativeMethods.MAX_PATH * Marshal.SystemDefaultCharSize);

                bi.pidlRoot = rootFolder;
                bi.hwndOwner = owner.Handle;
                bi.pszDisplayName = displayName;
                bi.lpszTitle = Description;
                bi.ulFlags = options;

                result = NativeMethods.SHBrowseForFolder(bi);

                if (result != IntPtr.Zero)
                {
                    NativeMethods.SHGetPathFromIDList(result, selectedPath);

                    SelectedPath = Marshal.PtrToStringAuto(selectedPath);

                    return DialogResult.OK;
                }

                return DialogResult.Cancel;
            }
            finally
            {
                NativeMethods.CoTaskMemFree(rootFolder);

                if (result != IntPtr.Zero)
                    NativeMethods.CoTaskMemFree(result);

                if (selectedPath != IntPtr.Zero)
                    Marshal.FreeHGlobal(selectedPath);

                if (displayName != IntPtr.Zero)
                    Marshal.FreeHGlobal(displayName);
            }
        }

        private DialogResult ShowVistaDialog(IWin32Window owner)
        {
            var dialog = new NativeMethods.FileOpenDialog();

            try
            {
                SetDialogProperties(dialog);

                int result = dialog.Show(owner == null ? IntPtr.Zero : owner.Handle);

                if (result < 0)
                {
                    if ((uint)result == (uint)NativeMethods.HRESULT.ERROR_CANCELLED)
                        return DialogResult.Cancel;
                    else
                        throw Marshal.GetExceptionForHR(result);
                }

                GetResult(dialog);

                return DialogResult.OK;
            }
            finally
            {
                Marshal.FinalReleaseComObject(dialog);
            }
        }

        private void SetDialogProperties(NativeMethods.IFileDialog dialog)
        {
            if (!String.IsNullOrEmpty(Description))
            {
                if (UseDescriptionForTitle)
                    dialog.SetTitle(Description);
                else
                    ((NativeMethods.IFileDialogCustomize)dialog).AddText(0, Description);
            }

            dialog.SetOptions(
                NativeMethods.FOS.FOS_PICKFOLDERS |
                NativeMethods.FOS.FOS_FORCEFILESYSTEM |
                NativeMethods.FOS.FOS_FILEMUSTEXIST
            );

            if (!String.IsNullOrEmpty(SelectedPath))
            {
                string parent = Path.GetDirectoryName(SelectedPath);

                if (parent == null || !Directory.Exists(parent))
                {
                    dialog.SetFileName(SelectedPath);
                }
                else
                {
                    string folder = Path.GetFileName(SelectedPath);

                    dialog.SetFolder(CreateItemFromParsingName(parent));
                    dialog.SetFileName(folder);
                }
            }
        }

        private NativeMethods.IShellItem CreateItemFromParsingName(string path)
        {
            object item;
            var guid = new Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe"); // IID_IShellItem
            Marshal.ThrowExceptionForHR(NativeMethods.SHCreateItemFromParsingName(
                path, IntPtr.Zero, ref guid, out item
            ));

            return (NativeMethods.IShellItem)item;
        }

        private void GetResult(NativeMethods.IFileDialog dialog)
        {
            NativeMethods.IShellItem item;
            dialog.GetResult(out item);

            string selectedPath;
            item.GetDisplayName(NativeMethods.SIGDN.SIGDN_FILESYSPATH, out selectedPath);

            SelectedPath = selectedPath;
        }

        private DialogResult ShowManagedDialog(IWin32Window owner)
        {
            using (var dialog = new FolderBrowserDialog
            {
                Description = Description,
                RootFolder = RootFolder,
                SelectedPath = SelectedPath,
                ShowNewFolderButton = ShowNewFolderButton
            })
            {
                var result = dialog.ShowDialog(owner);

                SelectedPath = dialog.SelectedPath;

                return result;
            }
        }
    }
}

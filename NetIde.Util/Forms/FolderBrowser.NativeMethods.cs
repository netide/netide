using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using NetIde.Util.Win32;

namespace NetIde.Util.Forms
{
    partial class FolderBrowser
    {
        /// <remarks>
        /// The contents of this class are not a Mono issue. The FolderBrowser
        /// class that uses these methods detects whether we're running Mono
        /// and ignores these methods when it is, reverting to the
        /// managed FolderBrowserDialog implementation.
        /// </remarks>
        private static class NativeMethods
        {
            [DllImport(ExternDll.Shell32, CharSet = CharSet.Unicode)]
            public static extern int SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IntPtr pbc, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

            [DllImport(ExternDll.Shell32)]
            public static extern IntPtr SHBrowseForFolder([In] BROWSEINFO lpbi);

            [DllImport(ExternDll.Shell32, SetLastError = true)]
            public static extern int SHGetSpecialFolderLocation(IntPtr hwndOwner, int nFolder, ref IntPtr ppidl);

            [DllImport(ExternDll.Shell32, CharSet = CharSet.Auto)]
            public static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);

            [DllImport(ExternDll.Ole32, SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true)]
            public extern static void CoTaskMemFree(IntPtr pv);

            public const uint BIF_EDITBOX = 0x0010;
            public const uint BIF_NEWDIALOGSTYLE = 0x0040;
            public const uint BIF_NONEWFOLDERBUTTON = 0x0200;

            public const int MAX_PATH = 260;

            [ComImport]
            [Guid("d57c7288-d4ad-4768-be02-9d969532d960")]
            [CoClass(typeof(FileOpenDialogRCW))]
            public interface FileOpenDialog : IFileOpenDialog
            {
            }

            [ComImport]
            [ClassInterface(ClassInterfaceType.None)]
            [TypeLibType(TypeLibTypeFlags.FCanCreate)]
            [Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")]
            public class FileOpenDialogRCW
            {
            }

            [ComImport]
            [Guid("b4db1657-70d7-485e-8e3e-6fcb5a5c1802")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IModalWindow
            {
                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), PreserveSig]
                int Show([In] IntPtr parent);
            }

            [ComImport]
            [Guid("42f85136-db7e-439c-85f1-e4075d135fc8")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IFileDialog : IModalWindow
            {
                // Defined on IModalWindow - repeated here due to requirements of COM interop layer
                // --------------------------------------------------------------------------------
                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), PreserveSig]
                new int Show([In] IntPtr parent);

                // IFileDialog-Specific interface members
                // --------------------------------------------------------------------------------
                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetFileTypes([In] uint cFileTypes, [In, MarshalAs(UnmanagedType.LPArray)] COMDLG_FILTERSPEC[] rgFilterSpec);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetFileTypeIndex([In] uint iFileType);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetFileTypeIndex(out uint piFileType);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void Advise([In, MarshalAs(UnmanagedType.Interface)] IFileDialogEvents pfde, out uint pdwCookie);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void Unadvise([In] uint dwCookie);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetOptions([In] FOS fos);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetOptions(out FOS pfos);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetDefaultFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetFolder([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetCurrentSelection([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetFileName([In, MarshalAs(UnmanagedType.LPWStr)] string pszName);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetTitle([In, MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetOkButtonLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszText);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetFileNameLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetResult([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void AddPlace([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, FDAP fdap);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetDefaultExtension([In, MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void Close([MarshalAs(UnmanagedType.Error)] int hr);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetClientGuid([In] ref Guid guid);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void ClearClientData();

                // Not supported:  IShellItemFilter is not defined, converting to IntPtr
                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);
            }

            [ComImport]
            [Guid("d57c7288-d4ad-4768-be02-9d969532d960")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IFileOpenDialog : IFileDialog
            {
                // Defined on IModalWindow - repeated here due to requirements of COM interop layer
                // --------------------------------------------------------------------------------
                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), PreserveSig]
                new int Show([In] IntPtr parent);

                // Defined on IFileDialog - repeated here due to requirements of COM interop layer
                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetFileTypes([In] uint cFileTypes, [In] ref COMDLG_FILTERSPEC rgFilterSpec);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void SetFileTypeIndex([In] uint iFileType);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void GetFileTypeIndex(out uint piFileType);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void Advise([In, MarshalAs(UnmanagedType.Interface)] IFileDialogEvents pfde, out uint pdwCookie);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void Unadvise([In] uint dwCookie);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void SetOptions([In] FOS fos);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void GetOptions(out FOS pfos);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void SetDefaultFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void SetFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void GetFolder([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void GetCurrentSelection([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void SetFileName([In, MarshalAs(UnmanagedType.LPWStr)] string pszName);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void SetTitle([In, MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void SetOkButtonLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszText);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void SetFileNameLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void GetResult([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void AddPlace([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, FDAP fdap);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void SetDefaultExtension([In, MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void Close([MarshalAs(UnmanagedType.Error)] int hr);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void SetClientGuid([In] ref Guid guid);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void ClearClientData();

                // Not supported:  IShellItemFilter is not defined, converting to IntPtr
                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                new void SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);

                // Defined by IFileOpenDialog
                // ---------------------------------------------------------------------------------
                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetResults([MarshalAs(UnmanagedType.Interface)] out IShellItemArray ppenum);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetSelectedItems([MarshalAs(UnmanagedType.Interface)] out IShellItemArray ppsai);
            }

            [ComImport]
            [Guid("973510DB-7D7F-452B-8975-74A85828D354")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IFileDialogEvents
            {
                // NOTE: some of these callbacks are cancelable - returning S_FALSE means that 
                // the dialog should not proceed (e.g. with closing, changing folder); to 
                // support this, we need to use the PreserveSig attribute to enable us to return
                // the proper HRESULT
                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), PreserveSig]
                HRESULT OnFileOk([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), PreserveSig]
                HRESULT OnFolderChanging([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd, [In, MarshalAs(UnmanagedType.Interface)] IShellItem psiFolder);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void OnFolderChange([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void OnSelectionChange([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void OnShareViolation([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd, [In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, out FDE_SHAREVIOLATION_RESPONSE pResponse);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void OnTypeChange([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void OnOverwrite([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd, [In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, out FDE_OVERWRITE_RESPONSE pResponse);
            }

            [ComImport]
            [Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IShellItem
            {
                // Not supported: IBindCtx
                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void BindToHandler([In, MarshalAs(UnmanagedType.Interface)] IntPtr pbc, [In] ref Guid bhid, [In] ref Guid riid, out IntPtr ppv);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetDisplayName([In] SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetAttributes([In] uint sfgaoMask, out uint psfgaoAttribs);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void Compare([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, [In] uint hint, out int piOrder);
            }

            [ComImport]
            [Guid("B63EA76D-1F85-456F-A19C-48159EFA858B")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IShellItemArray
            {
                // Not supported: IBindCtx
                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void BindToHandler([In, MarshalAs(UnmanagedType.Interface)] IntPtr pbc, [In] ref Guid rbhid, [In] ref Guid riid, out IntPtr ppvOut);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetPropertyStore([In] int Flags, [In] ref Guid riid, out IntPtr ppv);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetPropertyDescriptionList([In] ref PROPERTYKEY keyType, [In] ref Guid riid, out IntPtr ppv);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetAttributes([In] SIATTRIBFLAGS dwAttribFlags, [In] uint sfgaoMask, out uint psfgaoAttribs);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetCount(out uint pdwNumItems);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetItemAt([In] uint dwIndex, [MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

                // Not supported: IEnumShellItems (will use GetCount and GetItemAt instead)
                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void EnumItems([MarshalAs(UnmanagedType.Interface)] out IntPtr ppenumShellItems);
            }

            [ComImport]
            [Guid("38521333-6A87-46A7-AE10-0F16706816C3")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IKnownFolder
            {
                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetId(out Guid pkfid);

                // Not yet supported - adding to fill slot in vtable
                void spacer1();
                //[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                //void GetCategory(out mbtagKF_CATEGORY pCategory);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetShellItem([In] uint dwFlags, ref Guid riid, out IShellItem ppv);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetPath([In] uint dwFlags, [MarshalAs(UnmanagedType.LPWStr)] out string ppszPath);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetPath([In] uint dwFlags, [In, MarshalAs(UnmanagedType.LPWStr)] string pszPath);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetLocation([In] uint dwFlags, [Out, ComAliasName("Interop.wirePIDL")] IntPtr ppidl);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetFolderType(out Guid pftid);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetRedirectionCapabilities(out uint pCapabilities);

                // Not yet supported - adding to fill slot in vtable
                void spacer2();
                //[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                //void GetFolderDefinition(out tagKNOWNFOLDER_DEFINITION pKFD);
            }

            [ComImport]
            [Guid("44BEAAEC-24F4-4E90-B3F0-23D258FBB146")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IKnownFolderManager
            {
                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void FolderIdFromCsidl([In] int nCsidl, out Guid pfid);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void FolderIdToCsidl([In] ref Guid rfid, out int pnCsidl);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetFolderIds([Out] IntPtr ppKFId, [In, Out] ref uint pCount);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetFolder([In] ref Guid rfid, [MarshalAs(UnmanagedType.Interface)] out IKnownFolder ppkf);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetFolderByName([In, MarshalAs(UnmanagedType.LPWStr)] string pszCanonicalName, [MarshalAs(UnmanagedType.Interface)] out IKnownFolder ppkf);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void RegisterFolder([In] ref Guid rfid, [In] ref KNOWNFOLDER_DEFINITION pKFD);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void UnregisterFolder([In] ref Guid rfid);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void FindFolderFromPath([In, MarshalAs(UnmanagedType.LPWStr)] string pszPath, [In] FFFP_MODE mode, [MarshalAs(UnmanagedType.Interface)] out IKnownFolder ppkf);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void FindFolderFromIDList([In] IntPtr pidl, [MarshalAs(UnmanagedType.Interface)] out IKnownFolder ppkf);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void Redirect([In] ref Guid rfid, [In] IntPtr hwnd, [In] uint Flags, [In, MarshalAs(UnmanagedType.LPWStr)] string pszTargetPath, [In] uint cFolders, [In] ref Guid pExclusion, [MarshalAs(UnmanagedType.LPWStr)] out string ppszError);
            }

            [ComImport]
            [Guid("e6fdd21a-163f-4975-9c8c-a69f1ba37034")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IFileDialogCustomize
            {
                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void EnableOpenDropDown([In] int dwIDCtl);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void AddMenu([In] int dwIDCtl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void AddPushButton([In] int dwIDCtl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void AddComboBox([In] int dwIDCtl);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void AddRadioButtonList([In] int dwIDCtl);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void AddCheckButton([In] int dwIDCtl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel, [In] bool bChecked);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void AddEditBox([In] int dwIDCtl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszText);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void AddSeparator([In] int dwIDCtl);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void AddText([In] int dwIDCtl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszText);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetControlLabel([In] int dwIDCtl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetControlState([In] int dwIDCtl, [Out] out CDCONTROLSTATE pdwState);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetControlState([In] int dwIDCtl, [In] CDCONTROLSTATE dwState);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetEditBoxText([In] int dwIDCtl, [Out] IntPtr ppszText);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetEditBoxText([In] int dwIDCtl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszText);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetCheckButtonState([In] int dwIDCtl, [Out] out bool pbChecked);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetCheckButtonState([In] int dwIDCtl, [In] bool bChecked);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void AddControlItem([In] int dwIDCtl, [In] int dwIDItem, [In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void RemoveControlItem([In] int dwIDCtl, [In] int dwIDItem);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void RemoveAllControlItems([In] int dwIDCtl);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetControlItemState([In] int dwIDCtl, [In] int dwIDItem, [Out] out CDCONTROLSTATE pdwState);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetControlItemState([In] int dwIDCtl, [In] int dwIDItem, [In] CDCONTROLSTATE dwState);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetSelectedControlItem([In] int dwIDCtl, [Out] out int pdwIDItem);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetSelectedControlItem([In] int dwIDCtl, [In] int dwIDItem); // Not valid for OpenDropDown

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void StartVisualGroup([In] int dwIDCtl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void EndVisualGroup();

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void MakeProminent([In] int dwIDCtl);
            }

            [ComImport]
            [Guid("36116642-D713-4b97-9B83-7484A9D00433")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IFileDialogControlEvents
            {
                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void OnItemSelected([In, MarshalAs(UnmanagedType.Interface)] IFileDialogCustomize pfdc, [In] int dwIDCtl, [In] int dwIDItem);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void OnButtonClicked([In, MarshalAs(UnmanagedType.Interface)] IFileDialogCustomize pfdc, [In] int dwIDCtl);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void OnCheckButtonToggled([In, MarshalAs(UnmanagedType.Interface)] IFileDialogCustomize pfdc, [In] int dwIDCtl, [In] bool bChecked);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void OnControlActivating([In, MarshalAs(UnmanagedType.Interface)] IFileDialogCustomize pfdc, [In] int dwIDCtl);
            }

            [ComImport]
            [Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IPropertyStore
            {
                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetCount([Out] out uint cProps);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetAt([In] uint iProp, out PROPERTYKEY pkey);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void GetValue([In] ref PROPERTYKEY key, out object pv);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void SetValue([In] ref PROPERTYKEY key, [In] ref object pv);

                [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
                void Commit();
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
            public struct COMDLG_FILTERSPEC
            {
                [MarshalAs(UnmanagedType.LPWStr)]
                public string pszName;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string pszSpec;
            }

            [Flags]
            public enum FOS : uint
            {
                FOS_OVERWRITEPROMPT = 0x00000002,
                FOS_STRICTFILETYPES = 0x00000004,
                FOS_NOCHANGEDIR = 0x00000008,
                FOS_PICKFOLDERS = 0x00000020,
                FOS_FORCEFILESYSTEM = 0x00000040, // Ensure that items returned are filesystem items.
                FOS_ALLNONSTORAGEITEMS = 0x00000080, // Allow choosing items that have no storage.
                FOS_NOVALIDATE = 0x00000100,
                FOS_ALLOWMULTISELECT = 0x00000200,
                FOS_PATHMUSTEXIST = 0x00000800,
                FOS_FILEMUSTEXIST = 0x00001000,
                FOS_CREATEPROMPT = 0x00002000,
                FOS_SHAREAWARE = 0x00004000,
                FOS_NOREADONLYRETURN = 0x00008000,
                FOS_NOTESTFILECREATE = 0x00010000,
                FOS_HIDEMRUPLACES = 0x00020000,
                FOS_HIDEPINNEDPLACES = 0x00040000,
                FOS_NODEREFERENCELINKS = 0x00100000,
                FOS_DONTADDTORECENT = 0x02000000,
                FOS_FORCESHOWHIDDEN = 0x10000000,
                FOS_DEFAULTNOMINIMODE = 0x20000000
            }

            public enum FDAP
            {
                FDAP_BOTTOM = 0x00000000,
                FDAP_TOP = 0x00000001,
            }

            public enum HRESULT : long
            {
                S_FALSE = 0x0001,
                S_OK = 0x0000,
                E_INVALIDARG = 0x80070057,
                E_OUTOFMEMORY = 0x8007000E,
                ERROR_CANCELLED = 0x800704C7
            }

            public enum SIGDN : uint
            {
                SIGDN_NORMALDISPLAY = 0x00000000,           // SHGDN_NORMAL
                SIGDN_PARENTRELATIVEPARSING = 0x80018001,   // SHGDN_INFOLDER | SHGDN_FORPARSING
                SIGDN_DESKTOPABSOLUTEPARSING = 0x80028000,  // SHGDN_FORPARSING
                SIGDN_PARENTRELATIVEEDITING = 0x80031001,   // SHGDN_INFOLDER | SHGDN_FOREDITING
                SIGDN_DESKTOPABSOLUTEEDITING = 0x8004c000,  // SHGDN_FORPARSING | SHGDN_FORADDRESSBAR
                SIGDN_FILESYSPATH = 0x80058000,             // SHGDN_FORPARSING
                SIGDN_URL = 0x80068000,                     // SHGDN_FORPARSING
                SIGDN_PARENTRELATIVEFORADDRESSBAR = 0x8007c001,     // SHGDN_INFOLDER | SHGDN_FORPARSING | SHGDN_FORADDRESSBAR
                SIGDN_PARENTRELATIVE = 0x80080001           // SHGDN_INFOLDER
            }
            [StructLayout(LayoutKind.Sequential, Pack = 4)]
            public struct PROPERTYKEY
            {
                public Guid fmtid;
                public uint pid;
            }

            public enum SIATTRIBFLAGS
            {
                SIATTRIBFLAGS_AND = 0x00000001, // if multiple items and the attirbutes together.
                SIATTRIBFLAGS_OR = 0x00000002, // if multiple items or the attributes together.
                SIATTRIBFLAGS_APPCOMPAT = 0x00000003, // Call GetAttributes directly on the ShellFolder for multiple attributes
            }

            public enum FDE_SHAREVIOLATION_RESPONSE
            {
                FDESVR_DEFAULT = 0x00000000,
                FDESVR_ACCEPT = 0x00000001,
                FDESVR_REFUSE = 0x00000002
            }

            public enum FDE_OVERWRITE_RESPONSE
            {
                FDEOR_DEFAULT = 0x00000000,
                FDEOR_ACCEPT = 0x00000001,
                FDEOR_REFUSE = 0x00000002
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
            public struct KNOWNFOLDER_DEFINITION
            {
                public KF_CATEGORY category;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string pszName;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string pszCreator;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string pszDescription;
                public Guid fidParent;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string pszRelativePath;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string pszParsingName;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string pszToolTip;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string pszLocalizedName;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string pszIcon;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string pszSecurity;
                public uint dwAttributes;
                public KF_DEFINITION_FLAGS kfdFlags;
                public Guid ftidType;
            }

            public enum FFFP_MODE
            {
                FFFP_EXACTMATCH,
                FFFP_NEARESTPARENTMATCH
            }

            public enum KF_CATEGORY
            {
                KF_CATEGORY_VIRTUAL = 0x00000001,
                KF_CATEGORY_FIXED = 0x00000002,
                KF_CATEGORY_COMMON = 0x00000003,
                KF_CATEGORY_PERUSER = 0x00000004
            }

            [Flags]
            public enum KF_DEFINITION_FLAGS
            {
                KFDF_PERSONALIZE = 0x00000001,
                KFDF_LOCAL_REDIRECT_ONLY = 0x00000002,
                KFDF_ROAMABLE = 0x00000004,
            }

            public enum CDCONTROLSTATE
            {
                CDCS_INACTIVE = 0x00000000,
                CDCS_ENABLED = 0x00000001,
                CDCS_VISIBLE = 0x00000002
            }

#pragma warning disable 649

            public struct BROWSEINFO
            {
                public IntPtr hwndOwner;
                public IntPtr pidlRoot;
                public IntPtr pszDisplayName;
                public string lpszTitle;
                public uint ulFlags;
                public IntPtr lpfn;
                public IntPtr lParam;
                public int iImage;
            }

#pragma warning restore 649
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Core.PackageManagement;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Core
{
    partial class CorePackage
    {
        private void BuildCommands()
        {
            _commandMapper.Add(
                Shell.NiResources.File_OpenProject,
                p => ErrorUtil.ThrowOnFailure(_projectManager.OpenProjectViaDialog(null))
            );
            _commandMapper.Add(
                Shell.NiResources.File_NewProject,
                p => ErrorUtil.ThrowOnFailure(_projectManager.CreateProjectViaDialog(null))
            );
            _commandMapper.Add(
                Shell.NiResources.File_Close,
                p => CloseFile(),
                p => p.Status = _windowPaneSelection.ActiveDocument != null ? NiCommandStatus.Enabled : NiCommandStatus.Supported
            );
            _commandMapper.Add(
                Shell.NiResources.File_CloseProject,
                p => ErrorUtil.ThrowOnFailure(_projectManager.CloseProject()),
                p => p.Status = _projectManager.ActiveProject != null ? NiCommandStatus.Enabled : NiCommandStatus.Supported
            );
            _commandMapper.Add(
                Shell.NiResources.File_Save,
                p => SaveFile(),
                p => p.Status = CanSaveFile()
            );
            _commandMapper.Add(
                Shell.NiResources.File_SaveAs,
                p => { throw new NotImplementedException(); },
                p => p.Status = NiCommandStatus.Invisible
            );
            _commandMapper.Add(
                Shell.NiResources.File_SaveAll,
                p => ErrorUtil.ThrowOnFailure(_env.SaveAllDocuments(NiSaveAllMode.All, true)),
                p => p.Status = _projectManager.ActiveProject != null ? NiCommandStatus.Enabled : NiCommandStatus.Supported
            );
            _commandMapper.Add(
                Shell.NiResources.File_Exit,
                p => ErrorUtil.ThrowOnFailure(((INiEnv)GetService(typeof(INiEnv))).Quit())
            );
            _commandMapper.Add(
                Shell.NiResources.Tools_Options,
                p => ErrorUtil.ThrowOnFailure(((INiToolsOptions)GetService(typeof(INiToolsOptions))).Open())
            );
            _commandMapper.Add(
                Shell.NiResources.Tools_PackageManagement,
                p => OpenPackageManagementForm()
            );
            _commandMapper.Add(
                Shell.NiResources.Window_CloseAllDocuments,
                p => ErrorUtil.ThrowOnFailure(_env.CloseAllDocuments(NiSaveAllMode.VisibleOnly))
            );
        }

        private NiCommandStatus CanSaveFile()
        {
            if (_windowPaneSelection.ActiveDocument != null)
            {
                INiWindowFrame windowFrame;
                ErrorUtil.ThrowOnFailure(((INiShell)GetService(typeof(INiShell))).GetWindowFrameForWindowPane(
                    _windowPaneSelection.ActiveDocument,
                    out windowFrame
                ));

                Debug.Assert(windowFrame != null);

                if (windowFrame != null)
                {
                    var docData = (INiPersistDocData)windowFrame.GetPropertyEx(NiFrameProperty.DocData);

                    if (docData != null)
                    {
                        bool isDirty;
                        ErrorUtil.ThrowOnFailure(docData.IsDocDataDirty(out isDirty));

                        if (isDirty)
                            return NiCommandStatus.Enabled;
                    }
                }
            }

            return NiCommandStatus.Supported;
        }

        private void SaveFile()
        {
            INiWindowFrame windowFrame;
            ErrorUtil.ThrowOnFailure(((INiShell)GetService(typeof(INiShell))).GetWindowFrameForWindowPane(
                _windowPaneSelection.ActiveDocument,
                out windowFrame
            ));

            Debug.Assert(windowFrame != null);

            if (windowFrame != null)
            {
                var docData = (INiPersistDocData)windowFrame.GetPropertyEx(NiFrameProperty.DocData);

                if (docData != null)
                {
                    string document;
                    bool saved;
                    ErrorUtil.ThrowOnFailure(docData.SaveDocData(NiSaveMode.SilentSave, out document, out saved));
                }
            }
        }

        private void CloseFile()
        {
            INiWindowFrame windowFrame;
            ErrorUtil.ThrowOnFailure(((INiShell)GetService(typeof(INiShell))).GetWindowFrameForWindowPane(
                _windowPaneSelection.ActiveDocument,
                out windowFrame
            ));

            Debug.Assert(windowFrame != null);

            if (windowFrame != null)
                ErrorUtil.ThrowOnFailure(windowFrame.Close(NiFrameCloseMode.PromptSave));
        }

        private void OpenPackageManagementForm()
        {
            using (var form = new PackageManagementForm())
            {
                form.ShowDialog(this);
            }
        }
    }
}

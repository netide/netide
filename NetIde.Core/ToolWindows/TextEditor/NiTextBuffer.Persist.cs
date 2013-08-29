using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Core.ToolWindows.TextEditor
{
    partial class NiTextBuffer
    {
        private string _fileName;
        private IServiceProvider _serviceProvider;

        HResult INiObjectWithSite.SetSite(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            return HResult.OK;
        }

        HResult INiObjectWithSite.GetSite(out IServiceProvider serviceProvider)
        {
            serviceProvider = _serviceProvider;
            return HResult.OK;
        }

        HResult INiPersistDocData.IsDocDataDirty(out bool isDirty)
        {
            isDirty = _dirty;
            return HResult.OK;
        }

        HResult INiPersistDocData.LoadDocData(string document)
        {
            return ((INiPersistFile)this).Load(document);
        }

        HResult INiPersistDocData.SaveDocData(NiSaveMode mode, out string document, out bool saved)
        {
            return ((INiShell)_serviceProvider.GetService(typeof(INiShell))).SaveDocDataToFile(
                mode,
                this,
                _fileName,
                out document,
                out saved
            );
        }

        HResult INiPersistFile.GetFileName(out string fileName)
        {
            fileName = _fileName;
            return HResult.OK;
        }

        HResult INiPersistFile.IsDirty(out bool isDirty)
        {
            isDirty = _dirty;
            return HResult.OK;
        }

        HResult INiPersistFile.Load(string fileName)
        {
            try
            {
                if (fileName == null)
                    throw new ArgumentNullException("fileName");

                _fileName = fileName;

                InitializeContent(File.ReadAllText(fileName));

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        HResult INiPersistFile.Save(string fileName, bool remember)
        {
            try
            {
                if (fileName == null)
                    fileName = _fileName;
                if (fileName == null)
                    throw new ArgumentNullException("fileName");

                File.WriteAllText(fileName, Document.TextContent);

                if (remember)
                    _fileName = fileName;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
    }
}

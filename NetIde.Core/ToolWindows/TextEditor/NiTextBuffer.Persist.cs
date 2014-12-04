using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util;

namespace NetIde.Core.ToolWindows.TextEditor
{
    partial class NiTextBuffer
    {
        private string _fileName;
        private IServiceProvider _serviceProvider;
        private TextFileType _fileType;

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
                _dirty = false;

                string content;

                using (var stream = File.OpenRead(fileName))
                {
                    var fileType = FileType.FromStream(stream, Path.GetExtension(fileName));

                    stream.Position = 0;

                    var data = new byte[stream.Length];

                    stream.Read(data, 0, data.Length);

                    _fileType = fileType as TextFileType;

                    if (_fileType == null)
                        _fileType = new TextFileType(Encoding.Default, null, PlatformUtil.NativeLineTermination);

                    content = _fileType.Encoding.GetString(
                        data,
                        _fileType.BomSize,
                        data.Length - _fileType.BomSize
                    );
                }

                InitializeContent(content);

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

                if (_fileType == null)
                {
                    File.WriteAllText(fileName, Document.TextContent);
                }
                else
                {
                    using (var stream = File.Create(fileName))
                    {
                        if (_fileType.Bom != null)
                            stream.Write(_fileType.Bom, 0, _fileType.Bom.Length);

                        var data = _fileType.Encoding.GetBytes(Document.TextContent);

                        stream.Write(data, 0, data.Length);
                    }
                }

                if (remember)
                {
                    _dirty = false;
                    _fileName = fileName;
                }

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
    }
}

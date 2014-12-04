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
        private static readonly Encoding DefaultEncoding = new UTF8Encoding(true);
        private static readonly Encoding BigEndianUTF32 = new UTF8Encoding(true, false);

        private string _fileName;
        private IServiceProvider _serviceProvider;
        private Encoding _encoding = DefaultEncoding;

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

        public HResult GetCodePage(out int codePage, out bool emitPreamble)
        {
            codePage = 0;
            emitPreamble = false;

            try
            {
                codePage = _encoding.CodePage;
                emitPreamble = _encoding.GetPreamble().Length > 0;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult SetCodePage(int codePage, bool emitPreamble)
        {
            try
            {
                if (codePage == Encoding.UTF8.CodePage)
                    _encoding = new UTF8Encoding(emitPreamble);
                else if (codePage == Encoding.Unicode.CodePage)
                    _encoding = new UnicodeEncoding(false, emitPreamble);
                else if (codePage == Encoding.BigEndianUnicode.CodePage)
                    _encoding = new UnicodeEncoding(true, emitPreamble);
                else if (codePage == Encoding.UTF32.CodePage) // UTF32 is LE; there is no BE on Encoding
                    _encoding = new UTF32Encoding(false, emitPreamble);
                else if (codePage == BigEndianUTF32.CodePage)
                    _encoding = new UTF32Encoding(true, emitPreamble);
                else if (emitPreamble)
                    throw new ArgumentException("Cannot emit preamble for specified encoding");
                else
                    _encoding = Encoding.GetEncoding(codePage);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
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

                using (var stream = File.OpenRead(fileName))
                {
                    var fileType = FileType.FromStream(stream, Path.GetExtension(fileName)) as TextFileType;
                    if (fileType != null)
                        _encoding = fileType.Encoding;
                    else
                        _encoding = DefaultEncoding;
                }

                string content = File.ReadAllText(fileName, _encoding);

                ErrorUtil.ThrowOnFailure(InitializeContent(content));

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

                File.WriteAllText(
                    fileName,
                    Document.TextContent,
                    _encoding
                );

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

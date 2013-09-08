using System.Text;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;

namespace NetIde.VisualStudio.FileGenerators
{
    [ComVisible(true)]
    public abstract class BaseCodeGenerator : IVsSingleFileGenerator
    {
        protected string FileNameSpace { get; private set; }
        protected string InputFilePath { get; private set; }
        internal IVsGeneratorProgress CodeGeneratorProgress { get; private set; }

        int IVsSingleFileGenerator.DefaultExtension(out string pbstrDefaultExtension)
        {
            pbstrDefaultExtension = null;

            try
            {
                pbstrDefaultExtension = GetDefaultExtension();
                return VSConstants.S_OK;
            }
            catch
            {
                return VSConstants.E_FAIL;
            }
        }
        
        int IVsSingleFileGenerator.Generate(string wszInputFilePath, string bstrInputFileContents, string wszDefaultNamespace, IntPtr[] rgbOutputFileContents, out uint pcbOutput, IVsGeneratorProgress pGenerateProgress)
        {
            if (bstrInputFileContents == null)
                throw new ArgumentNullException("bstrInputFileContents");

            pcbOutput = 0;

            InputFilePath = wszInputFilePath;
            FileNameSpace = wszDefaultNamespace;
            CodeGeneratorProgress = pGenerateProgress;

            byte[] bytes = GenerateCode(bstrInputFileContents);
            if (bytes == null)
                return VSConstants.E_FAIL;

            int outputLength = bytes.Length;
            rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(outputLength);
            Marshal.Copy(bytes, 0, rgbOutputFileContents[0], outputLength);
            pcbOutput = (uint)outputLength;

            return VSConstants.S_OK;
        }

        protected abstract string GetDefaultExtension();

        protected abstract byte[] GenerateCode(string inputFileContent);

        protected virtual void GeneratorError(uint level, string message, uint line, uint column)
        {
            if (CodeGeneratorProgress != null)
                CodeGeneratorProgress.GeneratorError(0, level, message, line, column);
        }

        protected virtual void GeneratorWarning(uint level, string message, uint line, uint column)
        {
            if (CodeGeneratorProgress != null)
                CodeGeneratorProgress.GeneratorError(1, level, message, line, column);
        }
    }
}

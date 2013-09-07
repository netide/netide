using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NetIde.VisualStudio.Wizard
{
    internal static class StrongKeyUtil
    {
        [DllImport("mscoree.dll")]
        private extern static int StrongNameFreeBuffer(IntPtr pbMemory);
        [DllImport("mscoree.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int StrongNameKeyGen(IntPtr wszKeyContainer, uint dwFlags, out IntPtr KeyBlob, out uint KeyBlobSize);
        [DllImport("mscoree.dll", CharSet = CharSet.Unicode)]
        private static extern int StrongNameErrorInfo();

        public static void GenerateStrongKeyFile(string target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var buffer = IntPtr.Zero;

            try
            {
                uint buffSize;

                if (StrongNameKeyGen(IntPtr.Zero, 0, out buffer, out buffSize) != 0)
                    Marshal.ThrowExceptionForHR(StrongNameErrorInfo());

                if (buffer == IntPtr.Zero)
                    throw new InvalidOperationException("Generating strong key file failed");

                var keyBuffer = new byte[buffSize];

                Marshal.Copy(buffer, keyBuffer, 0, (int)buffSize);

                File.WriteAllBytes(target, keyBuffer);
            }
            finally
            {
                StrongNameFreeBuffer(buffer);
            } 
        }
    }
}
